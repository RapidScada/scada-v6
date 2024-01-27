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
 * Summary  : Represents an automated system event
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents an automated system event.
    /// <para>Представляет событие автоматизированной системы.</para>
    /// </summary>
    [Serializable]
    public class Event
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Event()
        {
            EventID = 0;
            Timestamp = DateTime.MinValue;
            Hidden = false;
            CnlNum = 0;
            ObjNum = 0;
            DeviceNum = 0;
            PrevCnlVal = 0.0;
            PrevCnlStat = 0;
            CnlVal = 0.0;
            CnlStat = 0;
            Severity = 0;
            AckRequired = false;
            Ack = false;
            AckTimestamp = DateTime.MinValue;
            AckUserID = 0;
            TextFormat = EventTextFormat.Full;
            Text = "";
            Data = null;
            Position = -1;
        }


        /// <summary>
        /// Gets or sets the server-assigned event ID.
        /// </summary>
        public long EventID { get; set; }

        /// <summary>
        /// Gets or sets the event timestamp (UTC).
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event should be hidden in UI.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets the channel number.
        /// </summary>
        public int CnlNum { get; set; }

        /// <summary>
        /// Gets or sets the object number.
        /// </summary>
        public int ObjNum { get; set; }

        /// <summary>
        /// Gets or sets the device number.
        /// </summary>
        public int DeviceNum { get; set; }

        /// <summary>
        /// Gets or sets the previos channel value.
        /// </summary>
        public double PrevCnlVal { get; set; }

        /// <summary>
        /// Gets or sets the previos channel status.
        /// </summary>
        public int PrevCnlStat { get; set; }

        /// <summary>
        /// Gets or sets the current channel value.
        /// </summary>
        public double CnlVal { get; set; }

        /// <summary>
        /// Gets or sets the current channel status.
        /// </summary>
        public int CnlStat { get; set; }

        /// <summary>
        /// Gets or sets the event severity.
        /// </summary>
        public int Severity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event should be acknowledged.
        /// </summary>
        public bool AckRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is acknowledged.
        /// </summary>
        public bool Ack { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the event was acknowledged.
        /// </summary>
        public DateTime AckTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the user ID who acknowledged the event.
        /// </summary>
        public int AckUserID { get; set; }

        /// <summary>
        /// Gets or sets the text format.
        /// </summary>
        public EventTextFormat TextFormat { get; set; }

        /// <summary>
        /// Gets or sets the custom text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the custom data.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets the event position in a file or stream.
        /// </summary>
        public long Position { get; set; }
    }
}
