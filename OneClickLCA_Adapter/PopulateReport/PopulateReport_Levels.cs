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
using static System.Collections.Specialized.BitVector32;

namespace BH.Adapter.OneClickLCA
{
    public partial class OneClickLCAAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private OneClickReport PopulateReport_Levels(OneClickReport report, List<Dictionary<string, string>> entries)
        {
            IEnumerable<Dictionary<string, Dictionary<string, string>>> groups = entries
                .Where(x => !string.IsNullOrWhiteSpace(GetText(x, "Resource")) && Regex.IsMatch(GetText(x, "Building Parts"), "^[1-9]"))
                .GroupBy(x => GetText(x, "Resource") + " - " + GetText(x, "Building Parts") + " - " + GetText(x, "Comment") + " - " + GetText(x, "User input"))
                .SelectMany(x => GetEntries(x));

            report.Entries = new List<ReportEntry>();

            foreach (var group in groups)
            {
                Dictionary<string, string> first = group.Values.First();

                Dictionary<string, List<string>> mapping = new Dictionary<string, List<string>>
                {
                    ["B4"] = new List<string> { "B4-B5" }
                };

                report.Entries.Add(new ReportEntry
                {
                    Resource = GetText(first, "Resource"),
                    Quantity = GetDouble(first, "User input", double.NaN),
                    QuantityUnit = GetText(first, "Unit"),
                    MassOfRawMaterials = group.ToDictionary(x => x.Key, x => GetDouble(x.Value, "Mass of raw materials kg", double.NaN)),
                    RICSCategory = Convert.FromLevelBuildingParts(GetText(first, "Building Parts")),
                    OriginalCategory = GetText(first, "Building Parts"),
                    EnvironmentalMetrics = GetEnvironmentalMetrics(group, mapping, report.Indicator),
                    Question = GetText(first, "Question"),
                    Comment = GetText(first, "Comment"),
                    ServiceLife = GetText(first, "Service life"),
                    ResourceType = GetText(first, "Resource type"),
                    Datasource = GetText(first, "Datasource"),
                    YearsOfReplacement = GetDouble(first, "Years of replacement", double.NaN),
                    Name = GetText(first, "Name"),
                    Thickness = GetDouble(first, "Thickness mm", double.NaN) / 1000,
                    OriginalExtras = group.ToDictionary(x => x.Key, x => GetOriginalExtras(x.Value, report.Indicator) as IOriginalExtras)
                });
            }

            return report;
        }

        /***************************************************/

        List<EnvironmentalMetric> GetEnvironmentalMetrics(Dictionary<string, Dictionary<string, string>> sections, Dictionary<string, List<string>> mapping, Indicator indicator)
        {
            return GetCarbonMetrics(sections, mapping, indicator)
                .Concat(GetOtherMetrics(sections, mapping, indicator))
                .ToList();
        }

        /***************************************************/

        List<EnvironmentalMetric> GetCarbonMetrics(Dictionary<string, Dictionary<string, string>> sections, Dictionary<string, List<string>> mapping, Indicator indicator)
        {
            switch (indicator)
            {
                case Indicator.Levels_Assessment_A2:
                case Indicator.Levels_Assessment_A2_NewVersionAvailable:
                    return new List<EnvironmentalMetric>
                     {
                        GetGWP(sections, "Global Warming Potential total kg CO₂e", mapping),
                        GetBiogenicCarbon(sections, "Global Warming Potential biogenic kg CO₂e", mapping),
                        GetClimateChangeFossil(sections, "Global Warming Potential fossil kg CO₂e", mapping),
                        GetClimateChangeLandUse(sections, "Global Warming Potential, LULUC kg CO₂e", mapping)
                     };
                case Indicator.Levels_Assessment_A1:
                case Indicator.Levels_Carbon_A1:
                    return new List<EnvironmentalMetric>
                     {
                        GetGWP(sections, "Global warming kg CO₂e", mapping),
                        GetBiogenicCarbon(sections, "Biogenic carbon storage kg CO₂e bio", mapping)
                     };
                case Indicator.Levels_Carbon_A1A2:
                    return new List<EnvironmentalMetric>
                     {
                        GetGWP(sections, "Global Warming Potential total kg CO₂e", mapping),
                        GetBiogenicCarbon(sections, "Global Warming Potential biogenic kg CO₂e", mapping),
                        GetClimateChangeLandUse(sections, "Global Warming Potential, LULUC kg CO₂e", mapping)
                     };
                default:
                    return new List<EnvironmentalMetric>();
            }
        }

        /***************************************************/

        List<EnvironmentalMetric> GetOtherMetrics(Dictionary<string, Dictionary<string, string>> sections, Dictionary<string, List<string>> mapping, Indicator indicator)
        {
            switch (indicator)
            {
                case Indicator.Levels_Assessment_A2:
                case Indicator.Levels_Assessment_A2_NewVersionAvailable:
                    return new List<EnvironmentalMetric>
                     {
                        GetOzoneDepletion(sections, "Depletion potential of the stratospheric ozone layer kg CFC11e", mapping),
                        GetAcidification(sections, "Acidification potential, Accumulated Exceedance mol H+ eq.", mapping),
                        GetEutrophicationFreshWater(sections, "Eutrophication fresh water kg P eq.", mapping),
                        GetEutrophicationMarine(sections, "Eutrophication aquatic marine kg N eq.", mapping),
                        GetEutrophicationTerrestrial(sections, "Eutrophication terrestrial mol N eq.", mapping),
                        GetPhotochemicalOzoneCreation(sections, "Formation potential of tropospheric ozone kg NMVOC eq.", mapping),
                        GetAbioticDepletionPotentialNonFossil(sections, "Abiotic depletion potential (ADP-elements) for non fossil resources (+A2) kg Sbe", mapping),
                        GetAbioticDepletionPotentialFossil(sections, "Abiotic depletion potential (ADP-fossil fuels) for fossil resources (+A2) MJ", mapping, 1000000)
                     };
                case Indicator.Levels_Assessment_A1:
                    return new List<EnvironmentalMetric>
                     {
                        GetOzoneDepletion(sections, "Ozone Depletion kg CFC11e", mapping),
                        GetAcidification(sections, "Acidification kg SO₂e", mapping),
                        GetEutrophicationCML(sections, "Eutrophication kg PO₄e", mapping),
                        GetPhotochemicalOzoneCreationCML(sections, "Formation of ozone of lower atmosphere kg Ethenee", mapping),
                        GetAbioticDepletionPotentialNonFossil(sections, "Abiotic depletion potential (ADP-elements) for non fossil resources kg Sbe", mapping),
                        GetAbioticDepletionPotentialFossil(sections, "Abiotic depletion potential (ADP-fossil fuels) for fossil resources MJ", mapping, 1000000)
                     };
                case Indicator.Levels_Carbon_A1:
                case Indicator.Levels_Carbon_A1A2:
                default:
                    return new List<EnvironmentalMetric>();
            }
        }

        private OriginalExtras_Levels GetOriginalExtras(Dictionary<string, string> section, Indicator indicator)
        {
            OriginalExtras_Levels extras = new OriginalExtras_Levels
            {
                Construction = GetText(section, "Construction"),
                TransformationProcess = GetText(section, "Transformation process"),
                UniClass = GetText(section, "uniClass"),
                CsiMasterFormat = GetText(section, "csiMasterformat"),
                Class = GetText(section, "class"),
                ImportedLabel = GetText(section, "Imported label")
            };

            switch (indicator)
            {
                case Indicator.Levels_Assessment_A2:
                case Indicator.Levels_Assessment_A2_NewVersionAvailable:
                    extras.WaterConsumption = GetDouble(section, "Water use m³ deprived");
                    break;
                case Indicator.Levels_Assessment_A1:
                    extras.RenewablePrimaryEnergyUseAsRawmaterials = GetDouble(section, "Use of renewable primary energy resources as raw materials MJ") * 1000000;
                    extras.PrimaryEnergyUseExRawMaterials = GetDouble(section, "Total use of primary energy ex. raw materials MJ") * 1000000;
                    extras.RenewablePrimaryEnergyUse = GetDouble(section, "Total use of renewable primary energy MJ") * 1000000;
                    extras.NonRenewablePrimaryEnergyUse = GetDouble(section, "Total use of non renewable primary energy MJ") * 1000000;
                    extras.NetFreshWaterUse = GetDouble(section, "Use of net fresh water m³");
                    extras.Energy = GetDouble(section, "Energy kWh") * 3600000;
                    extras.WaterConsumption = GetDouble(section, "Water consumption m³");
                    extras.DistanceTraveled = GetDouble(section, "Distance traveled km") * 1000;
                    extras.FuelConsumption = GetDouble(section, "Fuel consumption litres") * 0.001;
                    break;
                case Indicator.Levels_Carbon_A1:
                case Indicator.Levels_Carbon_A1A2:
                default:
                    break;
            }

            return extras;
        }

        /***************************************************/
    }
}





