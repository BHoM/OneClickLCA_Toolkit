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

using BH.oM.Base;
using BH.oM.Base.Attributes;
using BH.oM.Quantities.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Additional information found about an element in the original report for BREEAM/ GLA UK.")]
    public class  OriginalExtras_BREEAM : BHoMObject, IOriginalExtras
    {
        [Description("TODO")]
        public string Construction { get; set; } = "";

        [Description("TODO")]
        [DisplayText("Transformation process")]
        public string TransformationProcess { get; set; } = "";

        [Description("TODO")]
        public string UniClass { get; set; } = "";

        [Description("TODO")]
        [DisplayText("End of life process")]
        public string EOLProcess { get; set; } = "";

        [Description("TODO")]
        [DisplayText("Non hazardous waste disposed")]
        [Mass]
        public double NonHazardousWasteDisposed { get; set; } = 0;

        [Description("TODO")]
        [Mass]
        public double Energy { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Water consumption")]
        [Volume]
        public double WaterConsumption { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Distance traveled")]
        [Length]
        public double DistanceTraveled { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Fuel consumption")]
        [Volume]
        public double FuelConsumption { get; set; } = 0;
    }
}


