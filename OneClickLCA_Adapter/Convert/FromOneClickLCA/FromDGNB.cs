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

        public static RICSCategory FromDGNB(string category)
        {
            int index = category.ToList<char>().FindIndex(x => x >= 65 && x <= 122);
            string code = category.Split(new char[] { ' ' }).First();

            if (m_Mapping_DGNB.ContainsKey(code))
                return m_Mapping_DGNB[code];
            else
                return RICSCategory._9;
        }


        /***************************************************/
        /**** Private Static Fields                     ****/
        /***************************************************/

        private static Dictionary<string, RICSCategory> m_Mapping_DGNB = new Dictionary<string, RICSCategory>
        {
            ["310"] = RICSCategory._0_1_2,
            ["320"] = RICSCategory._1_1,
            ["330B"] = RICSCategory._2_6,
            ["330"] = RICSCategory._2_5,
            ["331"] = RICSCategory._2_1,
            ["332"] = RICSCategory._2_5,
            ["333"] = RICSCategory._2_1_1,
            ["334"] = RICSCategory._2_6,
            ["335"] = RICSCategory._2_5_1,
            ["336"] = RICSCategory._2_5_1,
            ["337"] = RICSCategory._2_5_1,
            ["338"] = RICSCategory._2_5_1,
            ["339"] = RICSCategory._2_5,
            ["340"] = RICSCategory._2_7,
            ["340B"] = RICSCategory._2_8,
            ["350"] = RICSCategory._2_2,
            ["350B"] = RICSCategory._1_2,
            ["360"] = RICSCategory._2_3,
            ["361"] = RICSCategory._2_2_1,
            ["362"] = RICSCategory._2_6_2,
            ["363"] = RICSCategory._2_5_3,
            ["364"] = RICSCategory._2_5_3,
            ["369"] = RICSCategory._2_5,
            ["370"] = RICSCategory._4,
            ["390"] = RICSCategory._0_1,
            ["400"] = RICSCategory._5_1,
            ["410"] = RICSCategory._5_1,
            ["420"] = RICSCategory._5_2_1,
            ["430"] = RICSCategory._5_2_3,
            ["440"] = RICSCategory._5_3,
            ["450"] = RICSCategory._5_3_2,
            ["460"] = RICSCategory._5_5,
            ["470"] = RICSCategory._5_5,
            ["480"] = RICSCategory._5_3,
            ["490"] = RICSCategory._5_3
        };

        /***************************************************/
    }
}


