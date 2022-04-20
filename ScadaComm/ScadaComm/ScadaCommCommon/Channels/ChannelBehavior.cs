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
 * Module   : ScadaCommCommon
 * Summary  : Specifies the behaviors of communication channels
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Specifies the behaviors of communication channels.
    /// <para>Задает варианты поведения каналов связи.</para>
    /// </summary>
    public enum ChannelBehavior
    {
        /// <summary>
        /// Cyclically requests data.
        /// </summary>
        Master,

        /// <summary>
        /// Waits for data.
        /// </summary>
        Slave,

        /// <summary>
        /// Non-standard behavior.
        /// </summary>
        Mixed
    }
}
