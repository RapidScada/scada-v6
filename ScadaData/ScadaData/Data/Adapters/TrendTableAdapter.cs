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
 * Summary  : Represents a mechanism to read and write trend tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data.Adapters
{
    /// <summary>
    /// Represents a mechanism to read and write trend tables.
    /// <para>Представляет механизм для чтения и записи таблиц трендов.</para>
    /// </summary>
    public class TrendTableAdapter
    {
        /// <summary>
        /// The major version number.
        /// </summary>
        protected const ushort MajorVersion = 1;
        /// <summary>
        /// The minor version number.
        /// </summary>
        protected const ushort MinorVersion = 0;
        /// <summary>
        /// The header size in a file.
        /// </summary>
        protected const int HeaderSize = 50;


    }
}
