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

using BH.Adapter;
using BH.oM.Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.OneClickLCA
{
    public partial class OneClickLCAAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        [Description("Adapter for OneClickLCA.")]
        [Output("The created OneClickLCA adapter.")]
        public OneClickLCAAdapter()
        {
            // The Adapter constructor can be used to configure the Adapter behaviour.
            // For example:
            m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.CreateOnly; // Adapter `Push` Action simply calls "Create" method.
            
            // See the wiki, the AdapterSettings object and other Adapters to see how it can be configured.

            // If your toolkit needs to define this.AdapterComparers and or this.DependencyTypes,
            // this constructor has to populate those properties.
            // See the wiki for more information.
        }

        // You can add any other constructors that take more inputs here. 

        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        // You can add any private variable that should be in common to any other adapter methods here.
        // If you need to add some private methods, please consider first what their nature is:
        // if a method does not need any external call (API call, connection call, etc.)
        // we place them in the Engine project, and then reference them from the Adapter.
        // See the wiki for more information.

        /***************************************************/
    }
}


