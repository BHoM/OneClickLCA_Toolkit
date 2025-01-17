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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Additional information found about an element in the original report for Level(s) life-cycle carbon (EN15804 +A1).")]
    public class  OriginalExtras_Levels : BHoMObject, IOriginalExtras
    {
        [Description("TODO")]
        public string Construction { get; set; } = "";

        [Description("TODO")]
        [DisplayText("Transformation process")]
        public string TransformationProcess { get; set; } = "";

        [Description("TODO")]
        public string UniClass { get; set; } = "";

        [Description("TODO")]
        [DisplayText("Csi master format")]
        public string CsiMasterFormat { get; set; } = "";

        [Description("TODO")]
        public string Class { get; set; } = "";

        [Description("TODO")]
        [DisplayText("Imported label")]
        public string ImportedLabel { get; set; } = "";

        [Description("TODO")]
        [DisplayText("Use of renewable primary energy resources as raw materials MJ")]
        public double RenewablePrimaryEnergyUseAsRawmaterials { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Total use of primary energy ex. raw materials MJ")]
        public double PrimaryEnergyUseExRawMaterials { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Total use of renewable primary energy MJ")]
        public double RenewablePrimaryEnergyUse { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Total use of non renewable primary energy MJ")]
        public double NonRenewablePrimaryEnergyUse { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Use of net fresh water m³")]
        public double NetFreshWaterUse { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Energy kWh")]
        public double Energy { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Water consumption m³")]
        public double WaterConsumption { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Distance traveled km")]
        public double DistanceTraveled { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Fuel consumption litres")]
        public double FuelConsumption { get; set; } = 0;
    }
}


