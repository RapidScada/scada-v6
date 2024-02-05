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
 * Summary  : Specifies the flags for writing commands
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2023
 */

using System;

namespace Scada.Protocol
{
    /// <summary>
    /// Specifies the flags for writing commands.
    /// <para>Задаёт флаги для записи команд.</para>
    /// </summary>
    [Flags]
    public enum WriteCommandFlags : byte
    {
        /// <summary>
        /// Send command in one direction without applying formulas and generating events.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the command can be returned to the client that sent it.
        /// </summary>
        ReturnToSender = 1,

        /// <summary>
        /// Apply channel formulas.
        /// </summary>
        ApplyFormulas = 2,

        /// <summary>
        /// Enable channel events for a particular write operation.
        /// </summary>
        EnableEvents = 4,

        /// <summary>
        /// Write command with default behavior.
        /// </summary>
        Default = ApplyFormulas | EnableEvents,

        /// <summary>
        /// Enable all flags.
        /// </summary>
        EnableAll = ReturnToSender | ApplyFormulas | EnableEvents
    }
}
