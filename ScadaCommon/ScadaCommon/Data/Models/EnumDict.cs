/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Represents a dictionary of enumerations retrieved from the Format table
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Data.Entities;
using System;
using System.Collections.Generic;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a dictionary of enumerations retrieved from the Format table.
    /// <para>Представляет словарь перечислений, полученных из таблицы Форматы.</para>
    /// </summary>
    public class EnumDict : Dictionary<int, EnumFormat>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumDict()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumDict(ConfigDataset configDataset)
            : base()
        {
            Init(configDataset);
        }


        /// <summary>
        /// Initializes the enumerations.
        /// </summary>
        public void Init(ConfigDataset configDataset)
        {
            if (configDataset == null)
                throw new ArgumentNullException(nameof(configDataset));

            foreach (Format format in configDataset.FormatTable.EnumerateItems())
            {
                if (format.IsEnum)
                    Add(format.FormatID, EnumFormat.Parse(format));
            }
        }
    }
}
