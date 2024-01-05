/*
 * Copyright 2024 Rapid Software LLC
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
 * Module   : ScadaCommon
 * Summary  : Represents a filter for selecting arbitrary data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;

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
            ItemType = itemType ?? throw new ArgumentNullException(nameof(itemType));
            itemProperties = TypeDescriptor.GetProperties(itemType);

            Limit = 0;
            Offset = 0;
            OriginBegin = true;
            RequireAll = true;
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
        /// Gets or sets a value indicating whether to get a limited number of items from the beginning or from the end.
        /// </summary>
        public bool OriginBegin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all conditions must be satisfied.
        /// </summary>
        /// <remarks>True means the AND operator, false means the OR operator.</remarks>
        public bool RequireAll { get; set; }

        /// <summary>
        /// Gets the filter conditions.
        /// </summary>
        public List<FilterCondition> Conditions { get; }


        /// <summary>
        /// Adds a new filter condition.
        /// </summary>
        public FilterCondition AddCondition(string columnName, FilterOperator filterOperator, IEnumerable args)
        {
            FilterCondition condition =
                new FilterCondition(columnName, itemProperties[columnName], filterOperator, args);
            Conditions.Add(condition);
            return condition;
        }

        /// <summary>
        /// Adds a new filter condition.
        /// </summary>
        public FilterCondition AddCondition(string columnName, FilterOperator filterOperator, params object[] args)
        {
            return AddCondition(columnName, filterOperator, args as IEnumerable);
        }

        /// <summary>
        /// Checks if the specified item satisfies the conditions.
        /// </summary>
        public bool IsSatisfied(object item)
        {
            if (item == null || item.GetType() != ItemType)
            {
                return false;
            }
            else if (Conditions.Count == 0)
            {
                return true;
            }
            else if (RequireAll)
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
                foreach (FilterCondition condition in Conditions)
                {
                    if (condition.IsSatisfied(condition.ColumnProperty.GetValue(item)))
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the SQL filter expression.
        /// </summary>
        public string GetSqlFilter(Dictionary<string, string> columnNameMap, string prefix, string suffix,
            Func<DbParameter> createParamFunc, out List<DbParameter> dbParams)
        {
            if (createParamFunc == null)
                throw new ArgumentNullException(nameof(createParamFunc));
            if (columnNameMap == null)
                throw new ArgumentNullException(nameof(columnNameMap));

            List<string> filters = new List<string>();
            dbParams = new List<DbParameter>();

            foreach (FilterCondition condition in Conditions)
            {
                if (columnNameMap.TryGetValue(condition.ColumnName, out string dbColumnName))
                    filters.Add(condition.GetSqlFilter(dbColumnName, createParamFunc, dbParams));
            }

            string oper = RequireAll ? " AND " : " OR ";
            return filters.Count > 0 ? prefix + string.Join(oper, filters) + suffix : "";
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
