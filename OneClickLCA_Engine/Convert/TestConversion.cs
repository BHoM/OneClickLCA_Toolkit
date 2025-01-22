/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using BH.Engine.Base;
using BH.oM.Base;
using BH.oM.Base.Attributes;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.Engine.Adapters.OneClickLCA
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Interface Methods                         ****/
        /***************************************************/

        [Description("Create a list of Resources based on environmental metrics. Resources can be duplicated if there are multiple values present for each phase of life.")]
        [Input("entries", "custom objects used to create duplication list based on environmental metrics.")]
        [Output("Duplicated objects based on phase of life metrics.")]
        public static List<CustomObject> TestConversion(List<CustomObject> entries)
        {
            if (entries == null)
            {
                BH.Engine.Base.Compute.RecordError("You didn't input anything.");
                return null;
            }

            // List of valid property names to search
            List<string> validPropertyNames = new List<string>
            {
                "A1", "A2", "A3", "A1toA3", "A4", "A5", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B1toB7", "C1", "C2", "C3", "C4", "C1toC4", "D"
            };

            // Prefix for the property names
            string prefix = "BH.oM.LifeCycleAssessment.Results.ClimateChangeTotalNoBiogenicMaterialResult.";

            // Create a new list to store the duplicate entries
            List<CustomObject> newObjects = new List<CustomObject>();

            // Loop over each custom object
            foreach (var co in entries)
            {
                // Check if the Resource property exists
                object resource = BH.Engine.Base.Query.PropertyValue(co, "Resource");
                if (resource != null)
                {
                    // Check if "EnvironmentalMetrics" property exists and has at least one item
                    object environmentalMetrics = BH.Engine.Base.Query.PropertyValue(co, "EnvironmentalMetrics");
                    if (environmentalMetrics != null && ((List<object>)environmentalMetrics).Count > 0)
                    {
                        var metricsList = (List<object>)environmentalMetrics;
                        var firstMetric = metricsList[0]; // This assumes the Results class, if you want to improve this method you could also loop over the different types of metrics or hard code to a specific class. I left this as ClimateChangeTotalNoBiogenicMaterialResult until the user specified anything else. 

                        // Get all of the property names of the EnvironmentalMetrics Object
                        List<string> propertyNames = new List<string>(Base.Query.GetAllPropertyFullNames(firstMetric));

                        // Loop over the EnvironmentalMetrics Objects Property Names
                        foreach (string propertyName in propertyNames)
                        {
                            // Extract the suffix of the property name
                            string shortPropertyName = propertyName.Replace(prefix, "");

                            // Check if the short property name is in the list of valid property names
                            if (validPropertyNames.Contains(shortPropertyName))
                            {
                                // Extract the property value and test if the value is not equal to zero and not NaN
                                object propValue = BH.Engine.Base.Query.PropertyValue(firstMetric, shortPropertyName);

                                if (propValue != null && (double)propValue != 0 && !double.IsNaN((double)propValue))
                                {
                                    // Create a new CustomObject for each valid propValue
                                    CustomObject newObject = co.DeepClone();

                                    // Add the new object to the list
                                    newObjects.Add(newObject);
                                }
                            }
                        }
                    }
                }
                else
                {
                    BH.Engine.Base.Compute.RecordError("Resource property could not be found.");
                }
            }
            return newObjects;
        }
    }
}