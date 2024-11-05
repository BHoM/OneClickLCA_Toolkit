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
    public enum ConceptDesignCategory
    {
        Undefined,
        [DisplayText("0.1 Treatment, demolition and facilitating works")]
        _0_1_TreatmentDemolitionAndFacilitatingWorks,
        [DisplayText("1 Sub-structure")]
        _1_SubStructure,
        [DisplayText("2 Superstructure")]
        _2_SuperStructure,
        [DisplayText("3 Finishes")]
        _3_Finishes,
        [DisplayText("4 FF&E")]
        _4_FFAndE,
        [DisplayText("5.1 Public Health")]
        _5_1_PublicHealth,
        [DisplayText("5.2 Heating, Ventilation and Cooling (HVAC)")]
        _5_2_HVAC,
        [DisplayText("5.3 Electrical installations")]
        _5_3_ElectricalInstallations,
        [DisplayText("5.4 On site renewable energy generation")]
        _5_4_OnSiteRenewableEnergyGeneration,
        [DisplayText("5.5 Systems including Life safety, Fuel installations, Lift and conveyor installations, Services equipment, Disposal installations, Specialist installations, Builders work in connection with services")]
        _5_5_OtherSystems,
        [DisplayText("6 Pre-fabricated buildings and units")]
        _6_PreFabricatedBuildingsAndUnits,
        [DisplayText("7 Works to existing buildings")]
        _7_WorksToExistingBuildings,
        [DisplayText("8 External work")]
        _8_ExternalWork,
        [DisplayText("9 Others")]
        _9_Others
    }
}


