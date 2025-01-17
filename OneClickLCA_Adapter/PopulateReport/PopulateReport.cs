﻿/*
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
using BH.oM.LifeCycleAssessment.MaterialFragments;
using BH.oM.LifeCycleAssessment.Results;
using System;
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
            List<List<string>> content = contentRows.Select(x => x.Content.Select(c => c?.ToString()).ToList()).ToList();

            return PopulateReport(report, headers, content);
        }

        /***************************************************/

        private OneClickReport PopulateReport(OneClickReport report, List<string> headers, List<List<string>> content)
        {
            List<Dictionary<string, string>> entries = content
                .Select(row => row.Zip(headers, (cell, header) => new { cell, header })
                                          .ToDictionary(x => x.header, x => x.cell))
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
                case Indicator.Levels_Carbon_A1:
                case Indicator.Levels_Carbon_A1A2:
                case Indicator.Levels_Assessment_A1:
                case Indicator.Levels_Assessment_A2:
                case Indicator.Levels_Assessment_A2_NewVersionAvailable:
                    return PopulateReport_Levels(report, entries);
                case Indicator.WholeLifeCarbonAssessment:
                    return PopulateReport_WLCA(report, entries);
                default:
                    {
                        string message = "This type of report is not supported. The only valid indicators are currently:\n"
                            + Enum.GetValues(typeof(Indicator)).Cast<Indicator>()
                                .Where(x => x != Indicator.Undefined)
                                .Select(x => " - " +  x.ToText())
                                .Aggregate((a, b) => a + "\n" + b);
                        BH.Engine.Base.Compute.RecordError(message);
                    }
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

        private MaterialResult GetMaterialResult(Type resultType, Dictionary<string, Dictionary<string, string>> sections, string propName, string materialName = "", string epdName = "", Dictionary<string, List<string>> mapping = null, double factor = 1)
        {
            // Get the totals
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

            // Create the material result
            List<double> resultValues = new List<double>
            {
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
            };

            return BH.Engine.LifeCycleAssessment.Create.MaterialResult(resultType, materialName, epdName, resultValues);
        }

        /***************************************************/
    }
}





