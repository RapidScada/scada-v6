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
 * Module   : ScadaServerEngine
 * Summary  : Represents metadata about an input channel
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Entities;
using System;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents metadata about an input channel.
    /// <para>Представляет метаданные входного канала.</para>
    /// </summary>
    internal class CnlTag
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlTag(int index, InCnl inCnl)
        {
            Index = index;
            InCnl = inCnl;
            CalcEngine = null;
            CalcCnlDataFunc = null;
        }


        /// <summary>
        /// Gets or sets the index among the active input channels.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the input channel entity.
        /// </summary>
        public InCnl InCnl { get; set; }

        /// <summary>
        /// Gets or sets the object that calculates channel data.
        /// </summary>
        public CalcEngine CalcEngine { get; set; }

        /// <summary>
        /// Gets or sets the function that calculates channel data.
        /// </summary>
        public Func<object> CalcCnlDataFunc { get; set; }
    }
}
