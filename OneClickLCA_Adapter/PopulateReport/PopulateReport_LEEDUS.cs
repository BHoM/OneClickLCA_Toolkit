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
using BH.oM.Adapter;
using BH.oM.Adapters.Excel;
using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.LifeCycleAssessment.MaterialFragments;
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

        private OneClickReport PopulateReport_LEEDUS(OneClickReport report, List<Dictionary<string, string>> entries)
        {
            IEnumerable<IEnumerable<Dictionary<string, string>>> groups = entries
                .Where(x => !string.IsNullOrWhiteSpace(GetText(x, "Resource")) && Regex.IsMatch(GetText(x, "Omniclass"), "^[1-9]"))
                .GroupBy(x => GetText(x, "Resource") + " - " + GetText(x, "Omniclass") + " - " + GetText(x, "Comment") + " - " + GetText(x, "User input"));

            report.Entries = new List<ReportEntry>();

            foreach (var group in groups)
            {
                Dictionary<string, string> first = group.First();
                IEnumerable<IGrouping<string, Dictionary<string, string>>> sections = group.GroupBy(x => GetText(x, "Section"));

                Dictionary<string, List<string>> mapping = new Dictionary<string, List<string>>
                {
                    ["B4"] = new List<string> { "B4-B5" }
                };

                report.Entries.Add(new ReportEntry
                {
                    Resource = GetText(first, "Resource"),
                    Quantity = GetDouble(first, "User input"),
                    QuantityUnit = GetText(first, "Unit"),
                    MassOfRawMaterials = GetText(first, "Mass of raw materials kg"),
                    RICSCategory = Convert.FromOmniClass(GetText(first, "Omniclass")),
                    OriginalCategory = GetText(first, "Omniclass"),
                    EnvironmentalMetrics = new List<EnvironmentalMetric>
                    {
                        GetGWP(sections, "Global warming kg CO₂e", mapping),
                        GetBiogenicCarbon(sections, "Biogenic carbon storage kg CO₂e bio", mapping),
                        GetAcidification(sections, "Acidification kg SO₂e", mapping),
                        GetEutrophicationTRACI(sections, "Eutrophication kg Ne", mapping),
                        GetOzoneDepletion(sections, "Ozone Depletion kg CFC11e", mapping),
                        GetPhotochemicalOzoneCreationTRACI(sections, "Formation of tropospheric ozone kg O3e", mapping)
                    },
                    Question = GetText(first, "Question"),
                    Comment = GetText(first, "Comment"),
                    ServiceLife = GetText(first, "Service life"),
                    ResourceType = GetText(first, "Resource type"),
                    Datasource = GetText(first, "Datasource"),
                    YearsOfReplacement = GetDouble(first, "Years of replacement"),
                    Name = GetText(first, "Name"),
                    Thickness = GetDouble(first, "Thickness in") * 0.0254,
                    OriginalExtras = new OriginalExtras_LEEDUS
                    {
                        Construction = GetText(first, "Construction"),
                        TransformationProcess = GetText(first, "Transformation process"),
                        UniClass = GetText(first, "uniClass"),
                        NonRenewableEnergyDepletion = GetDouble(first, "Depletion of nonrenewable energy MJ") * 1000000,
                        CsiMasterFormat = GetText(first, "csiMasterformat"),
                        Class = GetText(first, "class"),
                        ImportedLabel = GetText(first, "Imported label")
                    }
                });
            }

            return report;
        }

        /***************************************************/
    }
}





