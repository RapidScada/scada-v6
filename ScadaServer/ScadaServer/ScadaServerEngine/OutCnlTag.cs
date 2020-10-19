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
 * Summary  : Represents metadata about an output channel
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
    /// Represents metadata about an output channel.
    /// <para>Представляет метаданные канала управления.</para>
    /// </summary>
    internal class OutCnlTag
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OutCnlTag(OutCnl outCnl)
        {
            OutCnl = outCnl ?? throw new ArgumentNullException(nameof(outCnl));
            CalcEngine = null;
            CalcCmdDataFunc = null;
        }


        /// <summary>
        /// Gets the output channel entity.
        /// </summary>
        public OutCnl OutCnl { get; }

        /// <summary>
        /// Gets or sets the object that calculates command data.
        /// </summary>
        public CalcEngine CalcEngine { get; set; }

        /// <summary>
        /// Gets or sets the function that calculates command data.
        /// </summary>
        public Func<object> CalcCmdDataFunc { get; set; }
    }
}
