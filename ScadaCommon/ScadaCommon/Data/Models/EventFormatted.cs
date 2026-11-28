/*
 * Copyright 2026 Rapid Software LLC
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
 * Summary  : Represents a formatted event
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2026
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a formatted event.
    /// <para>Представляет форматированное событие.</para>
    /// </summary>
    public class EventFormatted
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventFormatted()
        {
            Id = "0";
            Time = "";
            Obj = "";
            Dev = "";
            Cnl = "";
            Descr = "";
            Stat = "";
            Sev = "";
            Ack = "";
            Colors = null;
            Beep = false;
        }


        /// <summary>
        /// Gets or sets the event ID as a string.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the time in the user's time zone.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the object name.
        /// </summary>
        public string Obj { get; set; }

        /// <summary>
        /// Gets or sets the device name.
        /// </summary>
        public string Dev { get; set; }

        /// <summary>
        /// Gets or sets the channel name.
        /// </summary>
        public string Cnl { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Descr { get; set; }

        /// <summary>
        /// Gets or sets the channel status name.
        /// </summary>
        public string Stat { get; set; }

        /// <summary>
        /// Gets or sets the severity information.
        /// </summary>
        public string Sev { get; set; }

        /// <summary>
        /// Gets or sets the acknowledgement information.
        /// </summary>
        public string Ack { get; set; }

        /// <summary>
        /// Gets or sets the main, second and background colors.
        /// </summary>
        public string[] Colors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a client application should play a beep.
        /// </summary>
        public bool Beep { get; set; }


        /// <summary>
        /// Gets the main color, or null if it does not exist.
        /// </summary>
        public string GetMainColor()
        {
            return GetMainColor(out string color) ? color : null;
        }

        /// <summary>
        /// Gets the main color.
        /// </summary>
        public bool GetMainColor(out string color)
        {
            if (Colors != null && Colors.Length > 0)
            {
                color = Colors[0];
                return true;
            }
            else
            {
                color = null;
                return false;
            }
        }
    }
}
