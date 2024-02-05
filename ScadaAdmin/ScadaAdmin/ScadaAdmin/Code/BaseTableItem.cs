﻿/*
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
 * Module   : Administrator
 * Summary  : Represents the configuration database table and related information
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2021
 */

using Scada.Data.Tables;
using System;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Represents the configuration database table and related information.
    /// <para>Представляет таблицу базы конфигурации и связанную с ней информацию.</para>
    /// </summary>
    internal class BaseTableItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseTableItem(IBaseTable baseTable, TableFilter tableFilter)
        {
            BaseTable = baseTable ?? throw new ArgumentNullException(nameof(baseTable));
            DeviceNum = tableFilter != null && tableFilter.ColumnName == "DeviceNum" && 
                tableFilter.Argument is int val ? val : 0;
        }


        /// <summary>
        /// Gets the configuration database table.
        /// </summary>
        public IBaseTable BaseTable { get; }

        /// <summary>
        /// Gets the filter by device.
        /// </summary>
        public int DeviceNum { get; }
    }
}
