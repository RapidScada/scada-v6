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
 * Summary  : Specifies the event bits
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Data.Const
{
    /// <summary>
    /// Specifies the event bits.
    /// <para>Задаёт биты событий.</para>
    /// </summary>
    public static class EventBit
    {
        /// <summary>
        /// The bit indicating whether events are enabled for the channel.
        /// </summary>
        public const int Enabled = 0;

        /// <summary>
        /// The bit indicating whether a client application should play a beep on event.
        /// </summary>
        public const int Beep = 1;

        /// <summary>
        /// The bit indicating whether an event should be raised when channel data changes.
        /// </summary>
        public const int DataChange = 2;

        /// <summary>
        /// The bit indicating whether an event should be raised when channel value changes.
        /// </summary>
        public const int ValueChange = 3;

        /// <summary>
        /// The bit indicating whether an event should be raised when channel status changes.
        /// </summary>
        public const int StatusChange = 4;

        /// <summary>
        /// The bit indicating whether an event should be raised when the channel becomes undefined, or vice versa.
        /// </summary>
        public const int CnlUndefined = 5;

        /// <summary>
        /// The bit indicating whether an event should be raised when a command defined by the channel has been sent.
        /// </summary>
        public const int Command = 6;
    }
}
