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

        public static RICSCategory FromRICSv1(string category)
        {
            int index = category.ToList<char>().FindIndex(x => x >= 65 && x <= 122);
            string code = category.Substring(0, index).Trim(new char[] { ' ', '.' });

            if (m_Mapping_RICSv1.ContainsKey(code))
                return m_Mapping_RICSv1[code];
            else
                return RICSCategory._9;
        }


        /***************************************************/
        /**** Private Static Fields                     ****/
        /***************************************************/

        private static Dictionary<string, RICSCategory> m_Mapping_RICSv1 = new Dictionary<string, RICSCategory>
        {
            ["1"] = RICSCategory._1,
            ["1.1.1"] = RICSCategory._1_1,
            ["1.1.2"] = RICSCategory._1_1,
            ["1.1.3"] = RICSCategory._1_2,
            ["1.1.4"] = RICSCategory._1_1,
            ["1.1.5"] = RICSCategory._1_2_3,
            ["2"] = RICSCategory._2,
            ["2.1"] = RICSCategory._2_1,
            ["2.1.1"] = RICSCategory._2_1,
            ["2.1.2"] = RICSCategory._2_1,
            ["2.1.3"] = RICSCategory._2_1,
            ["2.1.4"] = RICSCategory._2_1,
            ["2.1.5"] = RICSCategory._2_1,
            ["2.1.6"] = RICSCategory._2_1,
            ["2.2"] = RICSCategory._2_2,
            ["2.2.1"] = RICSCategory._2_2,
            ["2.2.2"] = RICSCategory._2_2_1,
            ["2.2.3"] = RICSCategory._2_5_3,
            ["2.3"] = RICSCategory._2_3,
            ["2.3.1"] = RICSCategory._2_3,
            ["2.3.2"] = RICSCategory._2_5_3,
            ["2.3.3"] = RICSCategory._2_3,
            ["2.3.4"] = RICSCategory._2_5_3,
            ["2.3.5"] = RICSCategory._2_6_2,
            ["2.3.6"] = RICSCategory._2_5_4,
            ["2.4"] = RICSCategory._2_4,
            ["2.4.1"] = RICSCategory._2_4,
            ["2.4.2"] = RICSCategory._3_2_3,
            ["2.4.3"] = RICSCategory._2_4,
            ["2.4.4"] = RICSCategory._2_4_3,
            ["2.5"] = RICSCategory._2_5,
            ["2.5.1"] = RICSCategory._2_5_1,
            ["2.5.2"] = RICSCategory._2_5,
            ["2.5.3"] = RICSCategory._2_5_1,
            ["2.5.4"] = RICSCategory._2_5_1,
            ["2.5.5"] = RICSCategory._2_4_3,
            ["2.5.6"] = RICSCategory._2_5_4,
            ["2.6"] = RICSCategory._2_6,
            ["2.6.1"] = RICSCategory._2_6_1,
            ["2.6.2"] = RICSCategory._2_6_3,
            ["2.7"] = RICSCategory._2_7,
            ["2.7.1"] = RICSCategory._2_7_1,
            ["2.7.2"] = RICSCategory._2_7_1,
            ["2.7.3"] = RICSCategory._2_7,
            ["2.7.4"] = RICSCategory._2_7_1,
            ["2.8"] = RICSCategory._2_8,
            ["3"] = RICSCategory._3,
            ["3.1"] = RICSCategory._3_1,
            ["3.2"] = RICSCategory._3_2,
            ["3.2.1"] = RICSCategory._3_2_3,
            ["3.2.2"] = RICSCategory._3_2_1,
            ["3.3"] = RICSCategory._3_3,
            ["3.3.1"] = RICSCategory._3_3,
            ["3.3.2"] = RICSCategory._3_3,
            ["3.3.3"] = RICSCategory._3_3,
            ["4"] = RICSCategory._4,
            ["4.1.1"] = RICSCategory._4_1,
            ["4.1.2"] = RICSCategory._4_2,
            ["4.1.3"] = RICSCategory._4_3,
            ["4.1.4"] = RICSCategory._4_4,
            ["4.1.5"] = RICSCategory._4_5,
            ["4.1.6"] = RICSCategory._4,
            ["4.1.7"] = RICSCategory._4_1,
            ["4.1.8"] = RICSCategory._4_1,
            ["5"] = RICSCategory._5,
            ["5.1"] = RICSCategory._5_1,
            ["5.1.1"] = RICSCategory._5_1_1,
            ["5.1.2"] = RICSCategory._5_1_1,
            ["5.2"] = RICSCategory._5_5,
            ["5.3"] = RICSCategory._5_1,
            ["5.3.1"] = RICSCategory._5_1_3,
            ["5.3.2"] = RICSCategory._5_1_3,
            ["5.3.3"] = RICSCategory._5_5_4,
            ["5.4"] = RICSCategory._5_1,
            ["5.4.1"] = RICSCategory._5_1_2,
            ["5.4.2"] = RICSCategory._5_1_2,
            ["5.4.3"] = RICSCategory._5_2_1,
            ["5.4.4"] = RICSCategory._5_1_1,
            ["5.4.5"] = RICSCategory._5_1_3,
            ["5.5"] = RICSCategory._5_2,
            ["5.6"] = RICSCategory._5_2,
            ["5.6.1"] = RICSCategory._5_2_1,
            ["5.6.2"] = RICSCategory._5_2_1,
            ["5.6.3"] = RICSCategory._5_2_2,
            ["5.6.4"] = RICSCategory._5_2_2,
            ["5.6.5"] = RICSCategory._5_2_1,
            ["5.6.6"] = RICSCategory._5_2_4,
            ["5.6.7"] = RICSCategory._5_2_4,
            ["5.6.8"] = RICSCategory._5_2_4,
            ["5.7"] = RICSCategory._5_2,
            ["5.7.1"] = RICSCategory._5_2_3,
            ["5.7.2"] = RICSCategory._5_2_3,
            ["5.7.3"] = RICSCategory._5_2_4,
            ["5.8"] = RICSCategory._5_3,
            ["5.8.1"] = RICSCategory._5_3_2,
            ["5.8.2"] = RICSCategory._5_3_2,
            ["5.8.3"] = RICSCategory._5_3_1,
            ["5.8.4"] = RICSCategory._5_3_1,
            ["5.8.5"] = RICSCategory._5_4_1,
            ["5.8.6"] = RICSCategory._5_3_2,
            ["5.9"] = RICSCategory._5_5,
            ["5.9.1"] = RICSCategory._5_5_2,
            ["5.9.2"] = RICSCategory._5_5_2,
            ["5.10"] = RICSCategory._5_5,
            ["5.10.1"] = RICSCategory._5_5_3,
            ["5.10.2"] = RICSCategory._5_5_3,
            ["5.10.3"] = RICSCategory._5_5_3,
            ["5.10.4"] = RICSCategory._5_5_3,
            ["5.10.5"] = RICSCategory._5_5_3,
            ["5.10.6"] = RICSCategory._5_5_3,
            ["5.10.7"] = RICSCategory._5_5_3,
            ["5.10.8"] = RICSCategory._5_5_3,
            ["5.10.9"] = RICSCategory._5_5_3,
            ["5.10.10"] = RICSCategory._5_5_3,
            ["5.11"] = RICSCategory._5_5,
            ["5.11.1"] = RICSCategory._5_5_1,
            ["5.11.2"] = RICSCategory._5_5_1,
            ["5.11.3"] = RICSCategory._5_5_1,
            ["5.12"] = RICSCategory._5_3,
            ["5.12.1"] = RICSCategory._5_3_2,
            ["5.12.2"] = RICSCategory._5_3_2,
            ["5.12.3"] = RICSCategory._5_3_2,
            ["5.13"] = RICSCategory._5_5,
            ["5.13.1"] = RICSCategory._5_5_5,
            ["5.13.2"] = RICSCategory._5_5_5,
            ["5.13.3"] = RICSCategory._5_5_5,
            ["5.13.4"] = RICSCategory._5_5_5,
            ["5.13.5"] = RICSCategory._5_5_5,
            ["5.14"] = RICSCategory._5_5,
            ["6"] = RICSCategory._6,
            ["6.1"] = RICSCategory._6,
            ["6.1.1"] = RICSCategory._6,
            ["6.1.2"] = RICSCategory._6,
            ["6.1.3"] = RICSCategory._6,
            ["7"] = RICSCategory._7,
            ["7.1"] = RICSCategory._7_1,
            ["7.2"] = RICSCategory._7_2,
            ["7.3"] = RICSCategory._7_3,
            ["7.4"] = RICSCategory._7_2,
            ["7.5"] = RICSCategory._7_2,
            ["7.6"] = RICSCategory._7_2,
            ["8"] = RICSCategory._8,
            ["8.1.1"] = RICSCategory._8_1,
            ["8.1.2"] = RICSCategory._8_1,
            ["8.2.1"] = RICSCategory._8_1,
            ["8.2.2"] = RICSCategory._8_1,
            ["8.3.1"] = RICSCategory._8_2,
            ["8.3.2"] = RICSCategory._8_2,
            ["8.3.3"] = RICSCategory._8_2,
            ["8.4.1"] = RICSCategory._8_1,
            ["8.4.2"] = RICSCategory._8_1,
            ["8.4.3"] = RICSCategory._8_1,
            ["8.4.4"] = RICSCategory._8_1,
            ["8.5.1"] = RICSCategory._8_1,
            ["8.5.2"] = RICSCategory._8_1,
            ["8.6.1"] = RICSCategory._8_3,
            ["8.6.2"] = RICSCategory._8_3,
            ["8.6.3"] = RICSCategory._8_3,
            ["8.6.4"] = RICSCategory._8_3,
            ["8.7.1"] = RICSCategory._8_3,
            ["8.7.2"] = RICSCategory._8_3,
            ["8.7.3"] = RICSCategory._8_3,
            ["8.7.4"] = RICSCategory._8_3,
            ["8.7.5"] = RICSCategory._8_3,
            ["8.7.6"] = RICSCategory._8_3,
            ["8.7.7"] = RICSCategory._8_3,
            ["8.7.8"] = RICSCategory._8_3,
            ["8.7.9"] = RICSCategory._8_3,
            ["8.7.10"] = RICSCategory._8_3,
            ["8.7.11"] = RICSCategory._8_3,
            ["8.8.1"] = RICSCategory._8_3,
            ["8.8.2"] = RICSCategory._8_3,
            ["8.8.3"] = RICSCategory._8_3,
            ["9"] = RICSCategory._0_1,
            ["0.1"] = RICSCategory._0_1_1_1,
            ["0.2"] = RICSCategory._0_1_1_2,
            ["0.3"] = RICSCategory._0_1_2_1,
            ["0.4"] = RICSCategory._0_1_2_3,
            ["0.5"] = RICSCategory._0_1_2_4

        };

        /***************************************************/
    }
}


