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
 * Summary  : Represents a filter condition
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents a filter condition.
    /// <para>Представляет условие фильтра.</para>
    /// </summary>
    public class FilterCondition
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FilterCondition(string columnName, PropertyDescriptor columnProperty, 
            FilterOperator filterOperator, params object[] args)
        {
            ColumnName = columnName ?? throw new ArgumentNullException("columnName");
            ColumnProperty = columnProperty ?? throw new ArgumentNullException("columnProperty");

            Type columnType = columnProperty.PropertyType.IsNullable() ?
                Nullable.GetUnderlyingType(columnProperty.PropertyType) : columnProperty.PropertyType;

            // not all ColumnDataType members are supported
            if (columnType == typeof(int))
                DataType = ColumnDataType.Integer;
            else if (columnType == typeof(double))
                DataType = ColumnDataType.Double;
            else if (columnType == typeof(bool))
                DataType = ColumnDataType.Boolean;
            else
                throw new ArgumentException(string.Format("Data type {0} is not supported.", columnType.FullName));

            Operator = filterOperator;
            SetArgument(args);
        }


        /// <summary>
        /// Gets the column name to filter.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Gets the column property.
        /// </summary>
        public PropertyDescriptor ColumnProperty { get; }

        /// <summary>
        /// Gets the column data type.
        /// </summary>
        public ColumnDataType DataType { get; }

        /// <summary>
        /// Gets or sets the filter operator.
        /// </summary>
        public FilterOperator Operator { get; }

        /// <summary>
        /// Gets the filter argument.
        /// </summary>
        public object Argument { get; protected set; }


        /// <summary>
        /// Sets the filter argument.
        /// </summary>
        public void SetArgument(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            bool argumentIsSet = true;

            switch (Operator)
            {
                case FilterOperator.Equals:
                    if (args.Length < 1)
                        throw new ArgumentException("Argument is required.", "args");

                    object arg = args[0];

                    switch (DataType)
                    {
                        case ColumnDataType.Integer:
                        case ColumnDataType.Boolean:
                            Argument = Convert.ToInt32(arg);
                            break;
                        case ColumnDataType.Double:
                            Argument = Convert.ToDouble(arg);
                            break;
                        default:
                            argumentIsSet = false;
                            break;
                    }
                    break;

                case FilterOperator.In:
                    switch (DataType)
                    {
                        case ColumnDataType.Integer:
                        case ColumnDataType.Boolean:
                            HashSet<int> intSet = new HashSet<int>();
                            Argument = intSet;
                            foreach (object intArg in args)
                            {
                                intSet.Add(Convert.ToInt32(intArg));
                            }
                            break;
                        case ColumnDataType.Double:
                            HashSet<double> doubleSet = new HashSet<double>();
                            Argument = doubleSet;
                            foreach (object doubleArg in args)
                            {
                                doubleSet.Add(Convert.ToDouble(doubleArg));
                            }
                            break;
                        default:
                            argumentIsSet = false;
                            break;
                    }
                    break;

                case FilterOperator.Between:
                    if (args.Length < 2)
                        throw new ArgumentException("Two arguments are required.", "args");

                    object beginArg = args[0];
                    object endArg = args[1];

                    switch (DataType)
                    {
                        case ColumnDataType.Integer:
                        case ColumnDataType.Boolean:
                            Argument = new int[] { Convert.ToInt32(beginArg), Convert.ToInt32(endArg) };
                            break;
                        case ColumnDataType.Double:
                            Argument = new double[] { Convert.ToDouble(beginArg), Convert.ToDouble(endArg) };
                            break;
                        default:
                            argumentIsSet = false;
                            break;
                    }
                    break;

                default:
                    argumentIsSet = false;
                    break;
            }

            if (!argumentIsSet)
                throw new InvalidOperationException("Unable to set argument.");
        }

        /// <summary>
        /// Checks if the specified property value satisfies the condition.
        /// </summary>
        public bool IsSatisfied(object value)
        {
            if (value == null)
                return false;

            switch (Operator)
            {
                case FilterOperator.Equals:
                    switch (DataType)
                    {
                        case ColumnDataType.Integer:
                        case ColumnDataType.Boolean:
                            return (int)Argument == Convert.ToInt32(value);
                        case ColumnDataType.Double:
                            return (double)Argument == Convert.ToDouble(value);
                    }
                    break;

                case FilterOperator.In:
                    switch (DataType)
                    {
                        case ColumnDataType.Integer:
                        case ColumnDataType.Boolean:
                            return ((HashSet<int>)Argument).Contains(Convert.ToInt32(value));
                        case ColumnDataType.Double:
                            return ((HashSet<double>)Argument).Contains(Convert.ToDouble(value));
                    }
                    break;

                case FilterOperator.Between:
                    IList args = (IList)Argument;
                    object beginArg = args[0];
                    object endArg = args[1];

                    switch (DataType)
                    {
                        case ColumnDataType.Integer:
                        case ColumnDataType.Boolean:
                            int intVal = Convert.ToInt32(value);
                            return (int)beginArg <= intVal && intVal <= (int)endArg;
                        case ColumnDataType.Double:
                            double doubleVal = Convert.ToDouble(value);
                            return (double)beginArg <= doubleVal && doubleVal <= (double)endArg;
                    }
                    break;
            }

            return false;
        }
    }
}
