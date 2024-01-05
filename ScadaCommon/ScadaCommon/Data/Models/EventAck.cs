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
 * Summary  : Contains information about an event acknowledgement
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Contains information about an event acknowledgement.
    /// <para>Содержит информацию о подтверждении события.</para>
    /// </summary>
    [Serializable]
    public class EventAck
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventAck()
        {
            EventID = 0;
            Timestamp = DateTime.MinValue;
            UserID = 0;
        }


        /// <summary>
        /// Gets or sets the event ID.
        /// </summary>
        public long EventID { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the event acknowledgement.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who acknowledged the event.
        /// </summary>
        public int UserID { get; set; }
    }
}
