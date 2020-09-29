/*
 * Copyright 2020 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaData
 * Summary  : Represents a filter for selecting arbitrary data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents a filter for selecting arbitrary data.
    /// <para>Представляет фильтр для выбора произвольных данных.</para>
    /// </summary>
    public class DataFilter
    {
        /// <summary>
        /// The collection of properties for the item type.
        /// </summary>
        protected PropertyDescriptorCollection itemProperties;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataFilter(Type itemType)
        {
            ItemType = itemType ?? throw new ArgumentNullException("itemType");
            itemProperties = TypeDescriptor.GetProperties(itemType);

            Limit = 0;
            Offset = 0;
            OriginBegin = true;
            Conditions = new List<FilterCondition>();
        }


        /// <summary>
        /// Gets the type of the filtered items.
        /// </summary>
        public Type ItemType { get; }

        /// <summary>
        /// Gets or sets the limit on the number of selected items.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the number of selected items to skip.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets a value whether to get a limited number of items from the beginning or from the end.
        /// </summary>
        public bool OriginBegin { get; set; }

        /// <summary>
        /// Gets the filter conditions.
        /// </summary>
        public List<FilterCondition> Conditions { get; }


        /// <summary>
        /// Adds a new filter condition.
        /// </summary>
        public FilterCondition AddCondition(string columnName, FilterOperator filterOperator, params object[] args)
        {
            FilterCondition condition = 
                new FilterCondition(columnName, itemProperties[columnName], filterOperator, args);
            Conditions.Add(condition);
            return condition;
        }

        /// <summary>
        /// Checks if the specified item satisfies all the conditions.
        /// </summary>
        public bool IsSatisfied(object item)
        {
            if (item != null && item.GetType() == ItemType)
            {
                foreach (FilterCondition condition in Conditions)
                {
                    if (!condition.IsSatisfied(condition.ColumnProperty.GetValue(item)))
                        return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a shallow copy of the current object.
        /// </summary>
        public DataFilter ShallowCopy()
        {
            return (DataFilter)MemberwiseClone();
        }
    }
}
