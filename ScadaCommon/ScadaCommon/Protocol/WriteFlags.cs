/*
 * Copyright 2023 Rapid Software LLC
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
 * Summary  : Specifies the writing data flags
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using System;

namespace Scada.Protocol
{
    /// <summary>
    /// Specifies the writing data flags.
    /// <para>Задаёт флаги записи данных.</para>
    /// </summary>
    [Flags]
    public enum WriteFlags : byte
    {
        /// <summary>
        /// Just write actual sent channel data.
        /// </summary>
        None = 0,

        /// <summary>
        /// Apply channel formulas.
        /// </summary>
        ApplyFormulas = 1,

        /// <summary>
        /// Enable channel events for a particular write operation.
        /// </summary>
        EnableEvents = 2,

        /// <summary>
        /// Enable all flags.
        /// </summary>
        EnableAll = ApplyFormulas | EnableEvents
    }
}
