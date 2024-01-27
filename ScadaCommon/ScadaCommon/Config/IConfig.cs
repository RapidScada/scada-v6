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
 * Module   : ScadaCommon
 * Summary  : Defines functionality of a configuration object
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Config
{
    /// <summary>
    /// Defines functionality of a configuration object.
    /// <para>Определяет функциональность объекта конфигурации.</para>
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        bool Load(string fileName, out string errMsg);

        /// <summary>
        /// Saves the configuration to the specified file.
        /// </summary>
        bool Save(string fileName, out string errMsg);
    }
}
