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
 * Module   : ScadaCommCommon
 * Summary  : Represents a communication channel type name
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents a communication channel type name.
    /// <para>Представляет наименование типа канала связи.</para>
    /// </summary>
    public struct ChannelTypeName
    {
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public ChannelTypeName(string code, string name)
        {
            Code = code;
            Name = name;
        }


        /// <summary>
        /// Gets or sets the channel type code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the channel type display name.
        /// </summary>
        public string Name { get; set; }
    }
}
