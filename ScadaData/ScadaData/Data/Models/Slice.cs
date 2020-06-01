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
 * Summary  : Represents a slice of input channels having the same timestamp
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a slice of input channels having the same timestamp.
    /// <para>Представляет срез входных каналов, имеющих одинаковую временную метку.</para>
    /// </summary>
    public class Slice
    {
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets the input channel numbers.
        /// </summary>
        public int[] CnlNums { get; protected set; }

        /// <summary>
        /// Gets the channel data corresponding to the channel numbers.
        /// </summary>
        public CnlData[] CnlData { get; protected set; }
    }
}
