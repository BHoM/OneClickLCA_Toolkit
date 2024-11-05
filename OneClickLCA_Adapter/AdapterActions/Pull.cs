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

using BH.Adapter;
using BH.Adapter.Excel;
using BH.Engine.Adapter;
using BH.Engine.Base;
using BH.oM.Adapter;
using BH.oM.Adapters.Excel;
using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base;
using BH.oM.Data.Requests;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.OneClickLCA
{
    public partial class OneClickLCAAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Method Overrides                          ****/
        /***************************************************/

        public override IEnumerable<object> Pull(IRequest request = null, PullType pullOption = PullType.AdapterDefault, ActionConfig actionConfig = null)
        {
            return _Pull(request as dynamic);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IEnumerable<object> _Pull(ReportRequest request)
        {
            ExcelAdapter excelAdapter = new ExcelAdapter(new FileSettings { FileName = request.FileName, Directory = request.Directory });

            // Pull the meta data
            CustomObject metadata = excelAdapter.Pull(BH.Engine.Excel.Create.ObjectRequest("", "A1:D2")).FirstOrDefault() as CustomObject;  
            if (metadata == null)
            {
                BH.Engine.Base.Compute.RecordError("Failed to pull the metadata from the report. Make sure that the file is in the correct location.");
                return new List<object>();
            }
            OneClickReport report = FillReportMetadata(metadata.CustomData);

            // Attach any potential additional inputs to the report
            if (request.AdditionalInputs != null)
                report.Fragments.Add(request.AdditionalInputs);

            // Get the valid range of cells by looking at the column headers
            TableRow headers = excelAdapter.Pull(BH.Engine.Excel.Create.CellValuesRequest("", "A3:AZ3")).FirstOrDefault() as TableRow;
            if (headers == null || headers.Content?.Count == 0)
            {
                BH.Engine.Base.Compute.RecordError("Failed to pull the column headers from the report. Make sure you provide the correct file.");
                return new List<object>();
            }
            int nbRows = headers.Content.FindIndex(x => string.IsNullOrWhiteSpace(x?.ToString()));
            string cellRange = $"A4:{BH.Engine.Excel.Query.ColumnName(nbRows-1)}";
            headers.Content = headers.Content.Take(nbRows).ToList();

            // Get the report content
            List<TableRow> content = excelAdapter.Pull(BH.Engine.Excel.Create.CellValuesRequest("", cellRange)).OfType<TableRow>().ToList();
            if (content?.Count == 0)
            {
                BH.Engine.Base.Compute.RecordError("Failed to pull the content from the report. Make sure you provide the correct file.");
                return new List<object>();
            }

            return new List<object> { PopulateReport(report, headers, content) };
        }

        /***************************************************/

        private OneClickReport FillReportMetadata(Dictionary<string, object> metadata)
        {
            OneClickReport report = new OneClickReport();

            if (metadata.ContainsKey("Entity users"))
                report.EntityUsers = metadata["Entity users"].ToString().Split(',').ToList();

            if (metadata.ContainsKey("Project name"))
                report.ProjectName = metadata["Project name"].ToString();

            if (metadata.ContainsKey("Design name"))
                report.DesignName = metadata["Design name"].ToString();

            if (metadata.ContainsKey("Indicator name"))
                report.Indicator = BH.Engine.Base.Compute.ParseEnum<Indicator>(metadata["Indicator name"].ToString());

            return report;
        }


        /***************************************************/
        /**** Fallback Methods                          ****/
        /***************************************************/

        private IEnumerable<object> _Pull(IRequest request)
        {
            if (request == null)
                BH.Engine.Base.Compute.RecordError($"Please provide a valid request for the Pull to work correctly.");
            else
                BH.Engine.Base.Compute.RecordError($"Only requests of type {request.GetType()} are not supported.");

            return new List<IBHoMObject>();
        }

        /***************************************************/
    }
}





