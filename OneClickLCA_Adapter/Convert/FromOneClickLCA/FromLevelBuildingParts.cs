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

using BH.oM.Adapters.OneClickLCA;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.OneClickLCA
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static RICSCategory FromLevelBuildingParts(string category)
        {
            if (string.IsNullOrEmpty(category))
                return RICSCategory.Undefined;

            int index = category.ToList<char>().FindIndex(x => x >= 65 && x <= 122);
            string code = index >= 0 ? category.Substring(0, index).Trim(new char[] { ' ', '.' }) : category;

            if (m_Mapping_Levels.ContainsKey(code))
                return m_Mapping_Levels[code];
            else
                return RICSCategory._9;
        }


        /***************************************************/
        /**** Private Static Fields                     ****/
        /***************************************************/

        private static Dictionary<string, RICSCategory> m_Mapping_Levels = new Dictionary<string, RICSCategory>
        {
            [""] = RICSCategory.Undefined,
            ["1"] = RICSCategory._2,
            ["1.1"] = RICSCategory._1_1,
            ["1.1.1"] = RICSCategory._1_1,
            ["1.1.2"] = RICSCategory._1_2_3,
            ["1.1.3"] = RICSCategory._1_2_3,
            ["1.2"] = RICSCategory._2_1,
            ["1.2.1"] = RICSCategory._2_1,
            ["1.2.2"] = RICSCategory._2_2,
            ["1.2.3"] = RICSCategory._2_1_1,
            ["1.2.4"] = RICSCategory._2_2_1,
            ["1.3"] = RICSCategory._2_7_2,
            ["1.3.1"] = RICSCategory._1_2_1,
            ["1.3.2"] = RICSCategory._2_7_2,
            ["1.3.3"] = RICSCategory._2_4,
            ["1.4"] = RICSCategory._2_5_1,
            ["1.4.1"] = RICSCategory._2_5_1,
            ["1.4.2"] = RICSCategory._2_6,
            ["1.4.3"] = RICSCategory._2_5_1,
            ["1.5"] = RICSCategory._2_3,
            ["1.5.1"] = RICSCategory._2_2_1,
            ["1.5.2"] = RICSCategory._2_5_1,
            ["1.6"] = RICSCategory._8_1_1,
            ["1.6.1"] = RICSCategory._8_1_1,
            ["2"] = RICSCategory._5,
            ["2.1"] = RICSCategory._4_4,
            ["2.1.1"] = RICSCategory._5_1_1,
            ["2.1.2"] = RICSCategory._4_4,
            ["2.1.3"] = RICSCategory._3_3,
            ["2.1.4"] = RICSCategory._3_1,
            ["2.1.5"] = RICSCategory._3_2,
            ["2.2"] = RICSCategory._5_3_1,
            ["2.2.1"] = RICSCategory._5_3_1,
            ["2.2.2"] = RICSCategory._5_3_2,
            ["2.3"] = RICSCategory._5,
            ["2.3.1"] = RICSCategory._5_2_1,
            ["2.3.2"] = RICSCategory._5_2_2,
            ["2.3.3"] = RICSCategory._5_3_2,
            ["2.4"] = RICSCategory._5_2_4,
            ["2.4.1"] = RICSCategory._5_2_3,
            ["2.4.2"] = RICSCategory._5_2_4,
            ["2.5"] = RICSCategory._5_1,
            ["2.5.1"] = RICSCategory._5_1_2,
            ["2.5.2"] = RICSCategory._5_2_1,
            ["2.5.3"] = RICSCategory._5_1,
            ["2.5.4"] = RICSCategory._5_1_3,
            ["2.6"] = RICSCategory._5_5_5,
            ["2.6.1"] = RICSCategory._5_5_3,
            ["2.6.2"] = RICSCategory._5_5_1,
            ["2.6.3"] = RICSCategory._5_3_2,
            ["2.6.4"] = RICSCategory._5_3_2,
            ["3"] = RICSCategory._8_3,
            ["3.1"] = RICSCategory._8_3_2,
            ["3.1.1"] = RICSCategory._8_1_1,
            ["3.1.2"] = RICSCategory._8_1,
            ["3.2"] = RICSCategory._8_2,
            ["3.2.1"] = RICSCategory._8_1_1,
            ["3.2.2"] = RICSCategory._8_1,
            ["3.2.3"] = RICSCategory._8_3_1
        };

        /***************************************************/
    }
}


