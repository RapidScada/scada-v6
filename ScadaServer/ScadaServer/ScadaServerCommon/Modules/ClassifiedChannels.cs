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
        /// Gets or sets the archivable channels.
        /// </summary>
        public Dictionary<int, Cnl> ArcCnls { get; set; }

        /// <summary>
        /// Gets or sets the output channels.
        /// </summary>
        public Dictionary<int, Cnl> OutCnls { get; set; }

        /// <summary>
        /// Gets or sets the channels measured by devices.
        /// </summary>
        public Dictionary<int, Cnl> MeasCnls { get; set; }

        /// <summary>
        /// Gets or sets the channels of the calculated type.
        /// </summary>
        public Dictionary<int, Cnl> CalcCnls { get; set; }
    }
}
