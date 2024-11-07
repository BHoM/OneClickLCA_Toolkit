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
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;

namespace BH.Adapter.OneClickLCA
{
    public partial class OneClickLCAAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private OneClickReport PopulateReport(OneClickReport report, TableRow headerRow, List<TableRow> contentRows)
        {
            List<string> headers = headerRow.Content.Select(x => x?.ToString()).ToList();
            List<List<object>> content = contentRows.Select(x => x.Content).ToList();

            List<Dictionary<string, string>> entries = contentRows
                .Select(row => row.Content.Zip(headers, (cell, header) => new { cell, header })
                                          .ToDictionary(x => x.header, x => x.cell?.ToString()))
                .ToList();

            switch (report?.Indicator)
            {
                case Indicator.BREEAM:
                    return PopulateReport_BREEAM(report, entries);
                case Indicator.DGNB:
                    return PopulateReport_DGNB(report, entries);
                case Indicator.LEED_Intl:
                    return PopulateReport_LEEDIntl(report, entries);
                case Indicator.LEED_US:
                    return PopulateReport_LEEDUS(report, entries);
                case Indicator.Levels:
                    return PopulateReport_Levels(report, entries);
                case Indicator.WholeLifeCarbonAssessment:
                    return PopulateReport_WLCA(report, entries);
                default:
                    return report;
            }
        }

        /***************************************************/

        private double ToDouble(string input, double defaultValue = 0)
        {
            double val = defaultValue;
            double.TryParse(input, out val);
            return val;
        }

        /***************************************************/

        private string GetText(Dictionary<string, string> dictionary, string key)
        {
            if (dictionary == null || !dictionary.ContainsKey(key))
                return "";
            else
                return dictionary[key];
        }

        /***************************************************/

        private double GetDouble(Dictionary<string, string> dictionary, string key, double defaultValue = 0)
        {
            return ToDouble(GetText(dictionary, key), defaultValue);
        }

        /***************************************************/

        private double GetDouble(Dictionary<string, double> dictionary, string key, double defaultValue = 0)
        {
            if (dictionary == null || !dictionary.ContainsKey(key))
                return defaultValue;
            else
                return dictionary[key];
        }

        /***************************************************/

        private List<Dictionary<string, Dictionary<string, string>>> GetEntries(IEnumerable<Dictionary<string, string>> group)
        {
            return group.GroupBy(g => g["Section"])
                .SelectMany(g => g.Select((item, index) => new { item, index, section = g.Key }))
                .GroupBy(x => x.index, x => new { x.item, x.section })
                .Select(g => g.ToDictionary(x => x.section, x => x.item))
                .ToList();
        }

        /***************************************************/

        private Dictionary<string, double> GetTotals(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping, double factor = 1)
        {
            Dictionary<string, double> totals = sections
                .ToDictionary(x => x.Key, x => factor * GetDouble(x.Value, propName));

            if (mapping != null)
            {
                foreach (var kvp in mapping)
                    totals[kvp.Key] = kvp.Value.Select(x => GetDouble(totals, x)).Sum();
            }

            if (!totals.ContainsKey("B1-B7") && totals.Keys.Any(k => k.StartsWith("B")))
                totals["B1-B7"] = GetDouble(totals, "B1") + GetDouble(totals, "B2") + GetDouble(totals, "B3") + GetDouble(totals, "B4") + GetDouble(totals, "B5") + GetDouble(totals, "B6") + GetDouble(totals, "B7");

            if (!totals.ContainsKey("C1-C4") && totals.Keys.Any(k => k.StartsWith("C")))
                totals["C1-C4"] = GetDouble(totals, "C1") + GetDouble(totals, "C2") + GetDouble(totals, "C3") + GetDouble(totals, "C4");

            return totals;
        }

        /***************************************************/

        private ClimateChangeTotalNoBiogenicMetric GetGWP(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            Dictionary<string, double> totals = GetTotals(sections, propName, mapping, factor);

            return new ClimateChangeTotalNoBiogenicMetric(
                double.NaN,
                double.NaN,
                double.NaN,
                GetDouble(totals, "A1-A3", double.NaN),
                GetDouble(totals, "A4", double.NaN),
                GetDouble(totals, "A5", double.NaN),
                GetDouble(totals, "B1", double.NaN),
                GetDouble(totals, "B2", double.NaN),
                GetDouble(totals, "B3", double.NaN),
                GetDouble(totals, "B4", double.NaN),
                GetDouble(totals, "B5", double.NaN),
                GetDouble(totals, "B6", double.NaN),
                GetDouble(totals, "B7", double.NaN),
                GetDouble(totals, "B1-B7", double.NaN),
                GetDouble(totals, "C1", double.NaN),
                GetDouble(totals, "C2", double.NaN),
                GetDouble(totals, "C3", double.NaN),
                GetDouble(totals, "C4", double.NaN),
                GetDouble(totals, "C1-C4", double.NaN),
                GetDouble(totals, "D", double.NaN)
            );
        }

        /***************************************************/

        private ClimateChangeBiogenicMetric GetBiogenicCarbon(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            Dictionary<string, double> totals = GetTotals(sections, propName, mapping, factor);

            return new ClimateChangeBiogenicMetric(
                double.NaN,
                double.NaN,
                double.NaN,
                GetDouble(totals, "A1-A3", double.NaN),
                GetDouble(totals, "A4", double.NaN),
                GetDouble(totals, "A5", double.NaN),
                GetDouble(totals, "B1", double.NaN),
                GetDouble(totals, "B2", double.NaN),
                GetDouble(totals, "B3", double.NaN),
                GetDouble(totals, "B4", double.NaN),
                GetDouble(totals, "B5", double.NaN),
                GetDouble(totals, "B6", double.NaN),
                GetDouble(totals, "B7", double.NaN),
                GetDouble(totals, "B1-B7", double.NaN),
                GetDouble(totals, "C1", double.NaN),
                GetDouble(totals, "C2", double.NaN),
                GetDouble(totals, "C3", double.NaN),
                GetDouble(totals, "C4", double.NaN),
                GetDouble(totals, "C1-C4", double.NaN),
                GetDouble(totals, "D", double.NaN)
            );
        }

        /***************************************************/

        private AcidificationMetric GetAcidification(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            Dictionary<string, double> totals = GetTotals(sections, propName, mapping, factor);

            return new AcidificationMetric(
                double.NaN,
                double.NaN,
                double.NaN,
                GetDouble(totals, "A1-A3", double.NaN),
                GetDouble(totals, "A4", double.NaN),
                GetDouble(totals, "A5", double.NaN),
                GetDouble(totals, "B1", double.NaN),
                GetDouble(totals, "B2", double.NaN),
                GetDouble(totals, "B3", double.NaN),
                GetDouble(totals, "B4", double.NaN),
                GetDouble(totals, "B5", double.NaN),
                GetDouble(totals, "B6", double.NaN),
                GetDouble(totals, "B7", double.NaN),
                GetDouble(totals, "B1-B7", double.NaN),
                GetDouble(totals, "C1", double.NaN),
                GetDouble(totals, "C2", double.NaN),
                GetDouble(totals, "C3", double.NaN),
                GetDouble(totals, "C4", double.NaN),
                GetDouble(totals, "C1-C4", double.NaN),
                GetDouble(totals, "D", double.NaN)
            );
        }

        /***************************************************/

        private EutrophicationCMLMetric GetEutrophicationCML(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            Dictionary<string, double> totals = GetTotals(sections, propName, mapping, factor);

            return new EutrophicationCMLMetric(
                double.NaN,
                double.NaN,
                double.NaN,
                GetDouble(totals, "A1-A3", double.NaN),
                GetDouble(totals, "A4", double.NaN),
                GetDouble(totals, "A5", double.NaN),
                GetDouble(totals, "B1", double.NaN),
                GetDouble(totals, "B2", double.NaN),
                GetDouble(totals, "B3", double.NaN),
                GetDouble(totals, "B4", double.NaN),
                GetDouble(totals, "B5", double.NaN),
                GetDouble(totals, "B6", double.NaN),
                GetDouble(totals, "B7", double.NaN),
                GetDouble(totals, "B1-B7", double.NaN),
                GetDouble(totals, "C1", double.NaN),
                GetDouble(totals, "C2", double.NaN),
                GetDouble(totals, "C3", double.NaN),
                GetDouble(totals, "C4", double.NaN),
                GetDouble(totals, "C1-C4", double.NaN),
                GetDouble(totals, "D", double.NaN)
            );
        }

        /***************************************************/

        private EutrophicationTRACIMetric GetEutrophicationTRACI(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            Dictionary<string, double> totals = GetTotals(sections, propName, mapping, factor);

            return new EutrophicationTRACIMetric(
                double.NaN,
                double.NaN,
                double.NaN,
                GetDouble(totals, "A1-A3", double.NaN),
                GetDouble(totals, "A4", double.NaN),
                GetDouble(totals, "A5", double.NaN),
                GetDouble(totals, "B1", double.NaN),
                GetDouble(totals, "B2", double.NaN),
                GetDouble(totals, "B3", double.NaN),
                GetDouble(totals, "B4", double.NaN),
                GetDouble(totals, "B5", double.NaN),
                GetDouble(totals, "B6", double.NaN),
                GetDouble(totals, "B7", double.NaN),
                GetDouble(totals, "B1-B7", double.NaN),
                GetDouble(totals, "C1", double.NaN),
                GetDouble(totals, "C2", double.NaN),
                GetDouble(totals, "C3", double.NaN),
                GetDouble(totals, "C4", double.NaN),
                GetDouble(totals, "C1-C4", double.NaN),
                GetDouble(totals, "D", double.NaN)
            );
        }

        /***************************************************/

        private OzoneDepletionMetric GetOzoneDepletion(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            Dictionary<string, double> totals = GetTotals(sections, propName, mapping, factor);

            return new OzoneDepletionMetric(
                double.NaN,
                double.NaN,
                double.NaN,
                GetDouble(totals, "A1-A3", double.NaN),
                GetDouble(totals, "A4", double.NaN),
                GetDouble(totals, "A5", double.NaN),
                GetDouble(totals, "B1", double.NaN),
                GetDouble(totals, "B2", double.NaN),
                GetDouble(totals, "B3", double.NaN),
                GetDouble(totals, "B4", double.NaN),
                GetDouble(totals, "B5", double.NaN),
                GetDouble(totals, "B6", double.NaN),
                GetDouble(totals, "B7", double.NaN),
                GetDouble(totals, "B1-B7", double.NaN),
                GetDouble(totals, "C1", double.NaN),
                GetDouble(totals, "C2", double.NaN),
                GetDouble(totals, "C3", double.NaN),
                GetDouble(totals, "C4", double.NaN),
                GetDouble(totals, "C1-C4", double.NaN),
                GetDouble(totals, "D", double.NaN)
            );
        }

        /***************************************************/

        private PhotochemicalOzoneCreationCMLMetric GetPhotochemicalOzoneCreationCML(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            Dictionary<string, double> totals = GetTotals(sections, propName, mapping, factor);

            return new PhotochemicalOzoneCreationCMLMetric(
                double.NaN,
                double.NaN,
                double.NaN,
                GetDouble(totals, "A1-A3", double.NaN),
                GetDouble(totals, "A4", double.NaN),
                GetDouble(totals, "A5", double.NaN),
                GetDouble(totals, "B1", double.NaN),
                GetDouble(totals, "B2", double.NaN),
                GetDouble(totals, "B3", double.NaN),
                GetDouble(totals, "B4", double.NaN),
                GetDouble(totals, "B5", double.NaN),
                GetDouble(totals, "B6", double.NaN),
                GetDouble(totals, "B7", double.NaN),
                GetDouble(totals, "B1-B7", double.NaN),
                GetDouble(totals, "C1", double.NaN),
                GetDouble(totals, "C2", double.NaN),
                GetDouble(totals, "C3", double.NaN),
                GetDouble(totals, "C4", double.NaN),
                GetDouble(totals, "C1-C4", double.NaN),
                GetDouble(totals, "D", double.NaN)
            );
        }

        /***************************************************/

        private PhotochemicalOzoneCreationTRACIMetric GetPhotochemicalOzoneCreationTRACI(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            Dictionary<string, double> totals = GetTotals(sections, propName, mapping, factor);

            return new PhotochemicalOzoneCreationTRACIMetric(
                double.NaN,
                double.NaN,
                double.NaN,
                GetDouble(totals, "A1-A3", double.NaN),
                GetDouble(totals, "A4", double.NaN),
                GetDouble(totals, "A5", double.NaN),
                GetDouble(totals, "B1", double.NaN),
                GetDouble(totals, "B2", double.NaN),
                GetDouble(totals, "B3", double.NaN),
                GetDouble(totals, "B4", double.NaN),
                GetDouble(totals, "B5", double.NaN),
                GetDouble(totals, "B6", double.NaN),
                GetDouble(totals, "B7", double.NaN),
                GetDouble(totals, "B1-B7", double.NaN),
                GetDouble(totals, "C1", double.NaN),
                GetDouble(totals, "C2", double.NaN),
                GetDouble(totals, "C3", double.NaN),
                GetDouble(totals, "C4", double.NaN),
                GetDouble(totals, "C1-C4", double.NaN),
                GetDouble(totals, "D", double.NaN)
            );
        }

        /***************************************************/

        private AbioticDepletionFossilResourcesMetric GetAbioticDepletionPotentialFossil(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            Dictionary<string, double> totals = GetTotals(sections, propName, mapping, factor);

            return new AbioticDepletionFossilResourcesMetric(
                double.NaN,
                double.NaN,
                double.NaN,
                GetDouble(totals, "A1-A3", double.NaN),
                GetDouble(totals, "A4", double.NaN),
                GetDouble(totals, "A5", double.NaN),
                GetDouble(totals, "B1", double.NaN),
                GetDouble(totals, "B2", double.NaN),
                GetDouble(totals, "B3", double.NaN),
                GetDouble(totals, "B4", double.NaN),
                GetDouble(totals, "B5", double.NaN),
                GetDouble(totals, "B6", double.NaN),
                GetDouble(totals, "B7", double.NaN),
                GetDouble(totals, "B1-B7", double.NaN),
                GetDouble(totals, "C1", double.NaN),
                GetDouble(totals, "C2", double.NaN),
                GetDouble(totals, "C3", double.NaN),
                GetDouble(totals, "C4", double.NaN),
                GetDouble(totals, "C1-C4", double.NaN),
                GetDouble(totals, "D", double.NaN)
            );
        }

        /***************************************************/

        private AbioticDepletionMineralsAndMetalsMetric GetAbioticDepletionPotentialNonFossil(Dictionary<string, Dictionary<string, string>> sections, string propName, Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            Dictionary<string, double> totals = GetTotals(sections, propName, mapping, factor);

            return new AbioticDepletionMineralsAndMetalsMetric(
                double.NaN,
                double.NaN,
                double.NaN,
                GetDouble(totals, "A1-A3", double.NaN),
                GetDouble(totals, "A4", double.NaN),
                GetDouble(totals, "A5", double.NaN),
                GetDouble(totals, "B1", double.NaN),
                GetDouble(totals, "B2", double.NaN),
                GetDouble(totals, "B3", double.NaN),
                GetDouble(totals, "B4", double.NaN),
                GetDouble(totals, "B5", double.NaN),
                GetDouble(totals, "B6", double.NaN),
                GetDouble(totals, "B7", double.NaN),
                GetDouble(totals, "B1-B7", double.NaN),
                GetDouble(totals, "C1", double.NaN),
                GetDouble(totals, "C2", double.NaN),
                GetDouble(totals, "C3", double.NaN),
                GetDouble(totals, "C4", double.NaN),
                GetDouble(totals, "C1-C4", double.NaN),
                GetDouble(totals, "D", double.NaN)
            );
        }

        /***************************************************/
    }
}





