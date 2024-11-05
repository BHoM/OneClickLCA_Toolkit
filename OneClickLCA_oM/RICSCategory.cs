///*
// * This file is part of the Buildings and Habitats object Model (BHoM)
// * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
// *
// * Each contributor holds copyright over their respective contributions.
// * The project versioning (Git) records all such contribution source information.
// *                                           
// *                                                                              
// * The BHoM is free software: you can redistribute it and/or modify         
// * it under the terms of the GNU Lesser General Public License as published by  
// * the Free Software Foundation, either version 3.0 of the License, or          
// * (at your option) any later version.                                          
// *                                                                              
// * The BHoM is distributed in the hope that it will be useful,              
// * but WITHOUT ANY WARRANTY; without even the implied warranty of               
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
// * GNU Lesser General Public License for more details.                          
// *                                                                            
// * You should have received a copy of the GNU Lesser General Public License     
// * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
// */

//using BH.oM.Base;
//using BH.oM.Base.Attributes;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;

//namespace BH.oM.Adapters.OneClickLCA
//{
//    [Description("Represents a building element category as defined in RICS v2 (RICS WLCA Standard 2023).")]
//    public class RICSCategory : BHoMObject
//    {
//        [Description("Category of the building element for the concept design stage")]
//        [DisplayText("Element category for concept design stage")]
//        public ConceptDesignCategory ConceptDesignCategory { get; set; } = ConceptDesignCategory.Undefined;

//        [Description("Sub-category number of the building element for the planning/pre-tender estimating stage. Leave at zero if not available.")]
//        [DisplayText("Element sub-category for planning stage")]
//        public int PlanningStageSubCategory { get; set; } = 0;

//        [Description("Sub-category number of the building element for the post completion/most detailed stage. Leave at zero if not available.")]
//        [DisplayText("Element sub-category for completion stage")]
//        public int PostCompletionSubCategory { get; set; } = 0;

//        [Description("Scope notes clarifying the building elements included in this category.")]
//        public string ScopeNotes { get; set; } = "";

//        [Description("Shorter version of the category name for more compact display.")]
//        public string ShortName { get; set; } = "";

//        [Description("Full name.")]
//        public override string Name { get; set; } = "";
//    }
//}


