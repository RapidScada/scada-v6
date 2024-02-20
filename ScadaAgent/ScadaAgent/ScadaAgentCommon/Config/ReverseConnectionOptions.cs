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
 * Module   : ScadaAgentCommon
 * Summary  : Represents reverse connection options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Client;

namespace Scada.Agent.Config
{
    /// <summary>
    /// Represents reverse connection options.
    /// <para>Представляет параметры обратного соединения.</para>
    /// </summary>
    public class ReverseConnectionOptions : ConnectionOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether the connection is in use.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
