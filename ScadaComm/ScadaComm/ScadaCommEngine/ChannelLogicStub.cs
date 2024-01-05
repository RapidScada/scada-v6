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
 * Module   : ScadaCommEngine
 * Summary  : Represents a stub for empty channel logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Drivers;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Represents a stub for empty channel logic.
    /// <para>Представляет заглушку для пустой логики канала.</para>
    /// </summary>
    internal class ChannelLogicStub : ChannelLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelLogicStub(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
        }
    }
}
