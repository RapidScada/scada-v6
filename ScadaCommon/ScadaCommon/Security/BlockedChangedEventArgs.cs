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
 * Summary  : Provides data for the BlockedChanged event
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;

namespace Scada.Security
{
    /// <summary>
    /// Provides data for the BlockedChanged event.
    /// <para>Предоставляет данные для события BlockedChanged.</para>
    /// </summary>
    public class BlockedChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the timestamp when the event occured.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to restrict user access.
        /// </summary>
        public bool Blocked { get; set; }

        /// <summary>
        /// Gets or sets a message describing the action taken.
        /// </summary>
        public string Message { get; set; }
    }
}
