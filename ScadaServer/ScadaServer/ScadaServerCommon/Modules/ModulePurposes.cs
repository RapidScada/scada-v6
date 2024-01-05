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
 * Module   : ScadaServerCommon
 * Summary  : Specifies the module purposes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using System;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Specifies the module purposes.
    /// <para>Задаёт предназначения модулей.</para>
    /// </summary>
    [Flags]
    public enum ModulePurposes
    {
        /// <summary>
        /// Indicates that a module implements data processing logic.
        /// </summary>
        Logic = 1,

        /// <summary>
        /// Indicates that a module implements data archive.
        /// </summary>
        Archive = 2,

        /// <summary>
        /// Indicates that a module implements user authorization and authentication.
        /// </summary>
        Auth = 4
    }
}
