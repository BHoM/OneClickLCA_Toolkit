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
using BH.oM.LifeCycleAssessment.MaterialFragments;
using BH.oM.LifeCycleAssessment.Results;
using BH.oM.Quantities.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BH.oM.Adapters.OneClickLCA
{
    [Description("Represents a building element alonside its environmental metrics as extracted from the Excel report.")]
    public class ReportEntry : BHoMObject
    {
        [Description("TODO")]
        public string Resource { get; set; } = "";

        [Description("TODO")]
        public double Quantity { get; set; } = 0;

        [Description("TODO")]
        [DisplayText("Quantity unit")]
        public string QuantityUnit { get; set; } = "";

        [Description("TODO")]
        [DisplayText("Mass of raw materials")]
        [Mass]
        public Dictionary<string, double> MassOfRawMaterials { get; set; } = new Dictionary<string, double>();

        [Description("TODO")]
        [DisplayText("RICS category")]
        public RICSCategory RICSCategory { get; set; }

        [Description("TODO")]
        [DisplayText("Original category")]
        public string OriginalCategory { get; set; }

        [Description("TODO")]
        [DisplayText("Environmental metrics")]
        public List<MaterialResult> EnvironmentalMetrics { get; set; } = new List<MaterialResult>();

        [Description("TODO")]
        public string Question { get; set; } = "";

        [Description("TODO")]
        public string Comment { get; set; } = "";

        [Description("TODO")]
        [DisplayText("Service life")]
        public string ServiceLife { get; set; } = "";

        [Description("TODO")]
        [DisplayText("Resource type")]
        public string ResourceType { get; set; } = "";

        [Description("TODO")]
        public string Datasource { get; set; } = "";

        [Description("TODO")]
        [DisplayText("Years of replacement")]
        public double YearsOfReplacement { get; set; } = 0;

        [Description("TODO")]
        [Length]
        public double Thickness { get; set; } = 0;

        [Description("Additional information found in the original report organised by section (i.e. LCA module)")]
        [DisplayText("Original extras")]
        public Dictionary<string, IOriginalExtras> OriginalExtras { get; set; }
    }
}


