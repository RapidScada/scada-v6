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
 * Module   : ScadaServerCommon
 * Summary  : Contains channels organized in categories
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Data.Entities;
using System.Collections.Generic;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Contains active channels organized in categories.
    /// <para>Содержит активные каналы, организованные по категориям.</para>
    /// </summary>
    /// <remarks>Includes additional channel numbers for arrays and strings.</remarks>
    public class ClassifiedChannels
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ClassifiedChannels()
        {
            ArcCnls = new SortedList<int, Cnl>();
            OutCnls = new SortedList<int, Cnl>();
            MeasCnls = new SortedList<int, Cnl>();
            CalcCnls = new SortedList<int, Cnl>();
        }


        /// <summary>
        /// Gets the archivable channels.
        /// </summary>
        public SortedList<int, Cnl> ArcCnls { get; }

        /// <summary>
        /// Gets the output channels.
        /// </summary>
        public SortedList<int, Cnl> OutCnls { get; }

        /// <summary>
        /// Gets the channels measured by devices.
        /// </summary>
        public SortedList<int, Cnl> MeasCnls { get; }

        /// <summary>
        /// Gets the channels of the calculated type.
        /// </summary>
        public SortedList<int, Cnl> CalcCnls { get; }
    }
}
