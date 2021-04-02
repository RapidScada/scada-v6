/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : ScadaCommon
 * Summary  : Represents the configuration transfer options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using Scada.Protocol;
using System.Collections.Generic;

namespace Scada.Agent
{
    /// <summary>
    /// Represents the configuration transfer options.
    /// <para>Представляет параметры передачи конфигурации.</para>
    /// </summary>
    public class ConfigTransferOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigTransferOptions()
        {
            ConfigParts = ConfigParts.All;
            IgnoredPaths = new List<RelativePath>();
        }


        /// <summary>
        /// Gets or sets the configuration parts.
        /// </summary>
        public ConfigParts ConfigParts { get; set; }

        /// <summary>
        /// Gets the ignored paths.
        /// </summary>
        public ICollection<RelativePath> IgnoredPaths { get; }
    }
}
