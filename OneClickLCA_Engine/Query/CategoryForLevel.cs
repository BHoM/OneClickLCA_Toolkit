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
using BH.oM.Base.Attributes;
using BH.oM.OneClickLCA.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Gets the corresponding RICS category for the provided level of detail. If requesting a higher or equal level than the input category, that input will be returned.")]
        [Input("category", "Original RICS category.")]
        [Output("category", "Corresponding RICS category for the provided level of detail.")]
        public static RICSCategory CategoryForLevel(this RICSCategory category, int level)
        {
            if (level < 1 || level > 3) 
            {
                BH.Engine.Base.Compute.RecordError("The category level should be between 1 and 3");
                return category;
            }

            int currentLevel = category.CategoryLevel();
            string code = category.CategoryCode();

            while (currentLevel > level)
            {
                string[] split = code.Split('.');
                code = split.Take(split.Length - 1).Aggregate((a, b) => a + "." + b);
                category = Create.RICSCategory(code);
                currentLevel = category.CategoryLevel();
            }

            return Create.RICSCategory(code);
        }

        /***************************************************/

    }
}


