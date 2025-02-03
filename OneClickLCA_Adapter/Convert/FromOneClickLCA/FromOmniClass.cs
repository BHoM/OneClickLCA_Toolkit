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

        public static RICSCategory FromOmniClass(string category)
        {
            if (string.IsNullOrEmpty(category))
                return RICSCategory.Undefined;

            int index = category.ToList<char>().FindIndex(x => x >= 65 && x <= 122);
            string code = index >= 0 ? category.Substring(0, index).Trim(new char[] { ' ', '.' }) : category;

            if (m_Mapping_OmniClass.ContainsKey(code))
                return m_Mapping_OmniClass[code];
            else
                return RICSCategory._9;
        }


        /***************************************************/
        /**** Private Static Fields                     ****/
        /***************************************************/

        private static Dictionary<string, RICSCategory> m_Mapping_OmniClass = new Dictionary<string, RICSCategory>
        {
            [""] = RICSCategory.Undefined,
            ["21-01 10 10"] = RICSCategory._1_1,
            ["21-01 10 20"] = RICSCategory._1_1,
            ["21-01 20 10"] = RICSCategory._1_2_3,
            ["21-01 40 10"] = RICSCategory._1_2_1,
            ["21-01 40 20"] = RICSCategory._1_2_1,
            ["21-01 40 30"] = RICSCategory._1_2,
            ["21-01 40 40"] = RICSCategory._1_2,
            ["21-01 40 90"] = RICSCategory._1_2,
            ["21-01 60 10"] = RICSCategory._0_1_2,
            ["21-01 60 20"] = RICSCategory._0_1_2,
            ["21-01 90 10"] = RICSCategory._0_1_2,
            ["21-01 90 20"] = RICSCategory._0_1_2,
            ["21-01 90 30"] = RICSCategory._0_1_2,
            ["21-01 90 40"] = RICSCategory._0_1_2,
            ["21-02 10 10"] = RICSCategory._2_1,
            ["21-02 10 10 10"] = RICSCategory._2_1,
            ["21-02 10 10 10 01"] = RICSCategory._2_1_2,
            ["21-02 10 10 10 02"] = RICSCategory._2_1_1,
            ["21-02 10 10 20"] = RICSCategory._2_2_1,
            ["21-02 10 10 30"] = RICSCategory._2_2_2,
            ["21-02 10 10 40"] = RICSCategory._2_2_2,
            ["21-02 10 10 50"] = RICSCategory._2_4,
            ["21-02 10 10 90"] = RICSCategory._2_2_2,
            ["21-02 10 20"] = RICSCategory._2_3,
            ["21-02 10 20 10"] = RICSCategory._2_3,
            ["21-02 10 20 20"] = RICSCategory._2_3,
            ["21-02 10 20 30"] = RICSCategory._2_3,
            ["21-02 10 20 90"] = RICSCategory._2_3,
            ["21-02 10 80"] = RICSCategory._2_4,
            ["21-02 20 10"] = RICSCategory._2_5_1,
            ["21-02 20 20"] = RICSCategory._2_6_1,
            ["21-02 20 50"] = RICSCategory._2_6_3,
            ["21-02 20 70"] = RICSCategory._2_6,
            ["21-02 20 80"] = RICSCategory._2_6,
            ["21-02 20 90"] = RICSCategory._2_5_1,
            ["21-02 30 10"] = RICSCategory._2_5_3,
            ["21-02 30 20"] = RICSCategory._2_5,
            ["21-02 30 40"] = RICSCategory._2_5,
            ["21-02 30 60"] = RICSCategory._2_6_2,
            ["21-02 30 80"] = RICSCategory._2_5,
            ["21-03 10 10"] = RICSCategory._2_7_1,
            ["21-03 10 20"] = RICSCategory._2_7_2,
            ["21-03 10 30"] = RICSCategory._2_8,
            ["21-03 10 40"] = RICSCategory._2_8,
            ["21-03 10 60"] = RICSCategory._3_2_1,
            ["21-03 10 70"] = RICSCategory._3_3,
            ["21-03 10 90"] = RICSCategory._3_1,
            ["21-03 20 10"] = RICSCategory._3_1,
            ["21-03 20 20"] = RICSCategory._3_1,
            ["21-03 20 30"] = RICSCategory._3_2_3,
            ["21-03 20 40"] = RICSCategory._3_2,
            ["21-03 20 50"] = RICSCategory._3_3,
            ["21-04 10"] = RICSCategory._5_5_3,
            ["21-04 20"] = RICSCategory._5_1,
            ["21-04 20 10"] = RICSCategory._5_1_1,
            ["21-04 20 20"] = RICSCategory._5_1_1,
            ["21-04 20 30"] = RICSCategory._5_1_2_1,
            ["21-04 20 50"] = RICSCategory._5_1,
            ["21-04 20 60"] = RICSCategory._5_1_3_1,
            ["21-04 30"] = RICSCategory._5_2,
            ["21-04 30 10"] = RICSCategory._5_2,
            ["21-04 30 20"] = RICSCategory._5_2_1,
            ["21-04 30 30"] = RICSCategory._5_2_2,
            ["21-04 30 50"] = RICSCategory._5_2_3,
            ["21-04 30 60"] = RICSCategory._5_2_4,
            ["21-04 30 70"] = RICSCategory._5_2,
            ["21-04 40"] = RICSCategory._5_5_1,
            ["21-04 40 10"] = RICSCategory._5_5_1,
            ["21-04 40 30"] = RICSCategory._5_5_1,
            ["21-04 50"] = RICSCategory._5_3,
            ["21-04 50 10"] = RICSCategory._5_3_2_1,
            ["21-04 50 20"] = RICSCategory._5_3_2_1,
            ["21-04 50 30"] = RICSCategory._5_3_2_1,
            ["21-04 50 40"] = RICSCategory._5_3_1,
            ["21-04 50 80"] = RICSCategory._5_3_2_1,
            ["21-04 60"] = RICSCategory._5_3_2_2,
            ["21-04 60 10"] = RICSCategory._5_3_2_2,
            ["21-04 60 20"] = RICSCategory._5_3_2_2,
            ["21-04 60 30"] = RICSCategory._5_3_2_2,
            ["21-04 60 60"] = RICSCategory._5_3_2_2,
            ["21-04 60 90"] = RICSCategory._5_3_2_2,
            ["21-04 70"] = RICSCategory._5_3_2_2,
            ["21-04 70 10"] = RICSCategory._5_3_2_2,
            ["21-04 70 30"] = RICSCategory._5_3_2_2,
            ["21-04 70 50"] = RICSCategory._5_3_2_2,
            ["21-04 70 70"] = RICSCategory._5_3_2_2,
            ["21-04 70 90"] = RICSCategory._5_3_2_2,
            ["21-04 80"] = RICSCategory._5_3_2_2,
            ["21-05 20 10"] = RICSCategory._9,
            ["21-05 20 50"] = RICSCategory._9,
            ["21-07 10 70"] = RICSCategory._8_2,
            ["21-07 20 10"] = RICSCategory._8_1_1,
            ["21-07 20 20"] = RICSCategory._8_1_1,
            ["21-07 20 30"] = RICSCategory._8_1_1,
            ["21-07 20 40"] = RICSCategory._8_1_1,
            ["21-07 20 50"] = RICSCategory._8_2,
            ["21-07 20 60"] = RICSCategory._8_2,
            ["21-07 20 80"] = RICSCategory._8_2
        };

        /***************************************************/
    }
}


