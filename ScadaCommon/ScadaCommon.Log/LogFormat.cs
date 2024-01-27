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
 * Module   : ScadaCommon.Log
 * Summary  : Specifies the log formats
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Log
{
    /// <summary>
    /// Specifies the log formats.
    /// <para>Задаёт форматы журнала.</para>
    /// </summary>
    public enum LogFormat
    {
        /// <summary>
        /// The simple format that includes timestamp and text.
        /// </summary>
        Simple,

        /// <summary>
        /// The full format that includes timestamp, computer, user, action type and text.
        /// </summary>
        Full
    }
}
