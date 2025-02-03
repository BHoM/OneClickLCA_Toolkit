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
using BH.Adapter.OneClickLCA.Objects;
using BH.Engine.Adapter;
using BH.oM.Adapter;
using BH.oM.Adapters.Excel;
using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using BH.oM.LifeCycleAssessment.Results;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BH.Adapter.OneClickLCA
{
    public partial class OneClickLCAAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private OneClickReport PopulateReport_WLCA(OneClickReport report, List<Dictionary<string, string>> entries)
        {
            IEnumerable<Dictionary<string, Dictionary<string, string>>> groups = entries
                .Where(x => !string.IsNullOrWhiteSpace(GetText(x, "Resource")))
                .GroupBy(x => GetText(x, "Resource") + " - " + GetText(x, "RICS category") + " - " + GetText(x, "Comment") + " - " + GetText(x, "User input"))
                .SelectMany(x => GetEntries(x));

            report.Entries = new List<ReportEntry>();

            foreach (var group in groups) {
                Dictionary<string, string> first = group.Values.First();
                string materialName = GetText(first, "Resource");
                string datasource = GetText(first, "Datasource");

                Dictionary<string, List<string>> mapping = new Dictionary<string, List<string>>
                {
                    ["B6"] = new List<string> { "B6a", "B6b" }
                };

                List<MetricAccessor> metricAccessors = new List<MetricAccessor>
                {
                    new MetricAccessor { Type = typeof(ClimateChangeTotalNoBiogenicMaterialResult), Name = "TOTAL kg CO₂e" },
                    new MetricAccessor { Type = typeof(ClimateChangeBiogenicMaterialResult), Name = "Biogenic carbon storage kg CO₂e bio" }
                };

                report.Entries.Add(new ReportEntry
                {
                    Resource = materialName,
                    Quantity = GetDouble(first, "User input", double.NaN),
                    QuantityUnit = GetText(first, "Unit"),
                    MassOfRawMaterials = group.ToDictionary(x => x.Key, x => GetDouble(x.Value, "Mass of raw materials kg", double.NaN)),
                    RICSCategory = Convert.FromRICSv1(GetText(first, "RICS category")),
                    OriginalCategory = GetText(first, "RICS category"),
                    EnvironmentalMetrics = metricAccessors.Select(x => GetMaterialResult(x.Type, group, x.Name, materialName, datasource, mapping)).ToList(),
                    Question = GetText(first, "Question"),
                    Comment = GetText(first, "Comment"),
                    ServiceLife = GetText(first, "Service life"),
                    ResourceType = GetText(first, "Resource type"),
                    Datasource = datasource,
                    YearsOfReplacement = GetDouble(first, "Years of replacement", double.NaN),
                    OriginalExtras = group.ToDictionary(x => x.Key, x => new OriginalExtras_WLCA
                    {
                        Construction = GetText(x.Value, "Construction"),
                        TransformationProcess = GetText(x.Value, "Transformation process"),
                        UniClass = GetText(x.Value, "uniClass"),
                        EstimatedReusableMaterials = GetDouble(x.Value, "Estimated reusable materials kg", double.NaN),
                        EstimatedRecyclableMaterials = GetDouble(x.Value, "Estimated recyclable materials kg", double.NaN),
                        EOLProcess = GetText(x.Value, "EOL Process")
                    } as IOriginalExtras)
                });
            }
            
            return report;
        }

        /***************************************************/
    }
}





