/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.Adapter.Excel;
using BH.Engine.Adapter;
using BH.Engine.Base;
using BH.Engine.Reflection;
using BH.oM.Adapter;
using BH.oM.Adapters.Excel;
using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.OneClickLCA
{
    public partial class OneClickLCAAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Public Overrides                          ****/
        /***************************************************/

        public override List<object> Push(IEnumerable<object> objects, string tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            // Make sure there are objects to push
            if (objects == null || !objects.Any())
            {
                BH.Engine.Base.Compute.RecordError("No objects were provided for Push action.");
                return new List<object>();
            }
            objects = objects.Where(x => x != null).ToList();

            // Make sure the use has provide a report to save
            List<OneClickReport> reports = objects.OfType<OneClickReport>().ToList();
            if (reports.Count == 0)
            {
                BH.Engine.Base.Compute.RecordError("No OneClickReport were provided for Push action.");
                return new List<object>();
            }
            if (reports.Count > 1)
            {
                BH.Engine.Base.Compute.RecordWarning("More that one report was provided. Only the firt one will be pushed.");
            }

            // Extract the push config
            IPushConfig config = (IPushConfig)(object)actionConfig;
            if (config == null)
            {
                BH.Engine.Base.Compute.RecordWarning("No valid push config was provided. Only configs of type IPushConfig are supported");
            }

            // Create the Excel adapter and push the report
            ExcelAdapter excelAdapter = CreateAdapter(config as dynamic);
            switch (config.OutputFormat)
            {
                case OutputFormat.OneClickLCA:
                    BH.Engine.Base.Compute.RecordError("Saving the OneClickReport in the original OneClick format is currently not supported");
                    return new List<object>();
                default:
                    return CreateBHoMReport(excelAdapter, reports.First());
            }
            
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private ExcelAdapter CreateAdapter(FilePushConfig config)
        {
            return new ExcelAdapter(new FileSettings { FileName = config.FileName, Directory = config.Directory });
        }

        /***************************************************/

        private List<object> CreateBHoMReport(ExcelAdapter excelAdapter, OneClickReport report)
        {
            List<PushItem> pushItems1 = new List<PushItem>();
            List<PushItem> pushItems2 = new List<PushItem>();

            // Push the metadata
            List<object> metadata = new List<object>
            {
                new TableRow { Content = new List<object> { "Entity users", report.EntityUsers.Count > 0 ? report.EntityUsers.Aggregate((a,b) =>  a + ", " + b) : "" } },
                new TableRow { Content = new List<object> { "Project name", report.ProjectName } },
                new TableRow { Content = new List<object> { "Design name", report.DesignName } },
                new TableRow { Content = new List<object> { "Indicator name", report.Indicator } }
            };
            pushItems1.Add(new PushItem { Objects = metadata, Config = new ExcelPushConfig { Worksheet = "Report metadata" } });

            // Create the entries common table
            List<ReportEntry> entries = report.Entries;
            List<string> commonHeaders = new List<string> { "Resource", "Quantity", "QuantityUnit", "RICSCategory" };
            List<object> common = new List<object> { new TableRow { Content = commonHeaders.ToList<object>() } };
            common.AddRange(entries.Select(entry => new TableRow { Content = commonHeaders.Select(h => entry.PropertyValue(h)).ToList() }));
            CellAddress afterCommonCell = BH.Engine.Excel.Create.CellAddress("E1");

            // Push the entries without any data tied to an LCA module
            List<string> entriesHeaders = new List<string> { "Question", "Comment", "ServiceLife", "ResourceType", "Datasource", "YearsOfReplacement", "Thickness" };
            pushItems1.Add(new PushItem { Objects = common, Config = new ExcelPushConfig { Worksheet = "Entries" } });
            pushItems2.Add(new PushItem { Objects = entries.ToList<object>(), Config = new ExcelPushConfig { Worksheet = "Entries", ObjectProperties = entriesHeaders, StartingCell = afterCommonCell } });

            // Push the environmental metrics
            List<string> metricHeaders = new List<string> { "A1", "A2", "A3", "A1toA3", "A4", "A5", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "C1", "C2", "C3", "C4", "C1toC4", "D" };
            TableRow metricHeaderRow = new TableRow { Content = metricHeaders.ToList<object>() };
            List <Type> metricTypes = entries.SelectMany(x => x.EnvironmentalMetrics.Select(m => m.GetType())).Distinct().ToList();
            foreach (Type type in metricTypes)
            {
                string sheetName = type.Name.Length > 31 ? type.Name.Substring(0, 31) : type.Name;
                pushItems1.Add(new PushItem { Objects = common, Config = new ExcelPushConfig { Worksheet = sheetName } });
                pushItems2.Add(new PushItem
                {
                    Objects = new List<object> { metricHeaderRow }.Concat(entries.Select(entry => {
                        EnvironmentalMetric metric = entry.EnvironmentalMetrics.Where(x => x.GetType() == type).FirstOrDefault();
                        if (metric == null)
                            return new TableRow { Content = metricHeaders.Select(x => "").ToList<object>() };
                        else
                            return new TableRow { Content = metricHeaders.Select(m => (double)metric.PropertyValue(m)).Select(x => double.IsNaN(x) ? "" : x as object).ToList() };
                    })).ToList(),
                    Config = new ExcelPushConfig { Worksheet = sheetName, StartingCell = afterCommonCell }
                });
            }

            // Push the mass of raw materials
            List<string> rawHeaders = entries.SelectMany(x => x.MassOfRawMaterials.Keys).Distinct().OrderBy(x => x).ToList();
            IEnumerable<TableRow> rawRows = entries.Select(entry => new TableRow { Content = rawHeaders.Select(h => entry.MassOfRawMaterials.ContainsKey(h) ? entry.MassOfRawMaterials[h] as object : "").ToList() });
            pushItems1.Add(new PushItem { Objects = common, Config = new ExcelPushConfig { Worksheet = "Mass of raw materials" } });
            pushItems2.Add(new PushItem
            {
                Objects = new List<object> { new TableRow { Content = rawHeaders.ToList<object>() } }.Concat(rawRows).ToList(),
                Config = new ExcelPushConfig { Worksheet = "Mass of raw materials", StartingCell = afterCommonCell }
            });

            // Push the original extras
            List<Dictionary<string, IOriginalExtras>> extras = entries.Select(entry => entry.OriginalExtras).ToList();
            Type extraType = entries.SelectMany(x => x.OriginalExtras.Values).Where(x => x != null).First().GetType();
            IOriginalExtras defaultExtras = Activator.CreateInstance(extraType) as IOriginalExtras;
            List<string> extraProperties = extraType.PropertyNames().Except(new List<string> { "CustomData", "Tags", "BHoM_Guid", "Fragments", "Name" }).ToList();
            List<string> extraModules = extras.SelectMany(x => x.Keys).Distinct().OrderBy(x => x).ToList();
            TableRow extraHeaderRow = new TableRow { Content = extraModules.ToList<object>() };
            
            foreach (string prop in extraProperties)
            {
                List<TableRow> row = extras.Select(extra => new TableRow { Content = extraModules.Select(m => extra.ContainsKey(m) ? extra[m]?.PropertyValue(prop) : "" as object).ToList() }).ToList();
                pushItems1.Add(new PushItem { Objects = common, Config = new ExcelPushConfig { Worksheet = prop } });
                pushItems2.Add(new PushItem { 
                    Objects = new List<object> { extraHeaderRow }.Concat(row).ToList(), 
                    Config = new ExcelPushConfig { Worksheet = prop, StartingCell = afterCommonCell } 
                });
            }

            excelAdapter.Push(pushItems1, "", PushType.DeleteThenCreate);
            excelAdapter.Push(pushItems2, "", PushType.UpdateOrCreateOnly);

            return new List<object> { report };
        }


        /***************************************************/
        /**** Fallback Methods                          ****/
        /***************************************************/

        private ExcelAdapter CreateAdapter(ActionConfig config)
        {
            BH.Engine.Base.Compute.RecordError($"Action configs of type {config?.GetType()} are not supported.");
            return null;
        }

        /***************************************************/
    }
}




