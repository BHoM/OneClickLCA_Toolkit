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
    [Description("RICS WLCA Standard 2023  - concept design stage.")]
    public enum Indicator
    {
        Undefined,
        [DisplayText("Whole life carbon assessment, GLA / RICS / Green Mark")]
        WholeLifeCarbonAssessment,
        [DisplayText("LCA for BREEAM UK")]
        BREEAM,
        [DisplayText("LCA for DGNB (DE)")]
        DGNB,
        [DisplayText("LCA for LEED, Int´l (CML)")]
        LEED_Intl,
        [DisplayText("Level(s) life-cycle assessment (EN15804 +A1)")]
        Levels_Assessment_A1,
        [DisplayText("Level(s) life-cycle assessment (EN15804 +A2)")]
        Levels_Assessment_A2,
        [DisplayText("Level(s) life-cycle assessment (EN15804 +A2) (new version available)")]
        Levels_Assessment_A2_NewVersionAvailable,
        [DisplayText("Level(s) life-cycle carbon (EN15804 +A1)")]
        Levels_Carbon_A1,
        [DisplayText("Level(s) life-cycle carbon (EN15804 +A1/+A2)")]
        Levels_Carbon_A1A2,
        [DisplayText("LCA for LEED, US (TRACI)")]
        LEED_US
    }
}


