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
using BH.Engine.Base;
using BH.oM.Adapter;
using BH.oM.Adapters.Excel;
using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using BH.oM.LifeCycleAssessment.Results;
using System;
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

        private OneClickReport PopulateReport_DGNB(OneClickReport report, List<Dictionary<string, string>> entries)
        {
            AdditionalInputs additionalInputs = report.FindFragment<AdditionalInputs>();
            if (additionalInputs == null)
            {
                BH.Engine.Base.Compute.RecordError("The request needs to be provided with the building area and life expectancy in order to calculate teh environmental metrics.");
                return report;
            }

            IEnumerable<Dictionary<string, Dictionary<string, string>>> groups = entries
                .Where(x => !string.IsNullOrWhiteSpace(GetText(x, "Resource")) && Regex.IsMatch(GetText(x, "KG DIN 276"), "^[1-9]"))
                .GroupBy(x => GetText(x, "Resource") + " - " + GetText(x, "KG DIN 276") + " - " + GetText(x, "Comment") + " - " + GetText(x, "User input"))
                .SelectMany(x => GetEntries(x));

            report.Entries = new List<ReportEntry>();

            foreach (var group in groups)
            {
                Dictionary<string, string> first = group.Values.First();
                string materialName = GetText(first, "Resource");
                string datasource = GetText(first, "Datasource");

                Dictionary<string, List<string>> mapping = new Dictionary<string, List<string>>
                {
                    ["B4"] = new List<string> { "B4", "B4-Abfall" }
                };

                double factor = additionalInputs.FloorArea * additionalInputs.BuildingLifeExpectancy;

                List<MetricAccessor> metricAccessors = new List<MetricAccessor>
                {
                    new MetricAccessor { Type = typeof(ClimateChangeTotalNoBiogenicMaterialResult), Name = "Global warming kg CO₂e/m²/a", Factor = factor },
                    new MetricAccessor { Type = typeof(AcidificationMaterialResult), Name = "Acidification kg SO₂e/m²/a", Factor = factor },
                    new MetricAccessor { Type = typeof(EutrophicationCMLMaterialResult), Name = "Eutrophication kg PO₄e/m²/a", Factor = factor },
                    new MetricAccessor { Type = typeof(OzoneDepletionMaterialResult), Name = "Ozone Depletion kg CFC11e/m²/a", Factor = factor },
                    new MetricAccessor { Type = typeof(PhotochemicalOzoneCreationCMLMaterialResult), Name = "Formation of ozone of lower atmosphere kg Ethenee/m²/a", Factor = factor },
                    new MetricAccessor { Type = typeof(AbioticDepletionFossilResourcesMaterialResult), Name = "Abiotic depletion potential (ADP-fossil fuels) for fossil resources MJ/m²/a", Factor = factor * 1000000 },
                    new MetricAccessor { Type = typeof(AbioticDepletionMineralsAndMetalsMaterialResult), Name = "Abiotic depletion potential (ADP-elements) for non fossil resources kg Sbe/m²/a", Factor = factor }
                };

                report.Entries.Add(new ReportEntry
                {
                    Resource = materialName,
                    Quantity = GetDouble(first, "User input", double.NaN),
                    QuantityUnit = GetText(first, "Unit"),
                    MassOfRawMaterials = group.ToDictionary(x => x.Key, x => GetDouble(x.Value, "Mass of raw materials kg", double.NaN)),
                    RICSCategory = Convert.FromDGNB(GetText(first, "KG DIN 276")),
                    OriginalCategory = GetText(first, "KG DIN 276"),
                    EnvironmentalMetrics = metricAccessors.Select(x => GetMaterialResult(x.Type, group, x.Name, materialName, datasource, mapping, x.Factor )).ToList(),
                    Question = GetText(first, "Question"),
                    Comment = GetText(first, "Comment"),
                    ServiceLife = GetText(first, "Service life"),
                    ResourceType = GetText(first, "Resource type"),
                    Datasource = datasource,
                    YearsOfReplacement = GetDouble(first, "Years of replacement", double.NaN),
                    OriginalExtras = group.ToDictionary(x => x.Key, x => new OriginalExtras_DGNB
                    {
                        NonRenewablePrimaryEnergyUse = factor * GetDouble(x.Value, "Total use of non renewable primary energy MJ/m²/a", double.NaN) * 1000000,
                        RenewablePrimaryEnergyUse = factor * GetDouble(x.Value, "Total use of renewable primary energy MJ/m²/a", double.NaN) * 1000000,
                        PrimaryEnergyUse = factor * GetDouble(x.Value, "Total use of primary energy MJ/m²/a", double.NaN) * 1000000,
                        NetFreshWaterUse = factor * GetDouble(x.Value, "Use of net fresh water m³/m²/a", double.NaN)
                    } as IOriginalExtras)
                });
            }

            return report;
        }

        /***************************************************/
    }
}





