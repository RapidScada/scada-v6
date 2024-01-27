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
 * Module   : ScadaCommCommon
 * Summary  : Represents device statistics
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents device statistics.
    /// <para>Представляет статистику устройства.</para>
    /// </summary>
    public class DeviceStats
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceStats()
        {
            SessionCount = 0;
            SessionErrors = 0;
            CommandCount = 0;
            CommandErrors = 0;
            RequestCount = 0;
            RequestErrors = 0;
        }


        /// <summary>
        /// Gets or sets the total number of sessions.
        /// </summary>
        public int SessionCount { get; set; }

        /// <summary>
        /// Gets or sets the number of failed sessions.
        /// </summary>
        public int SessionErrors { get; set; }

        /// <summary>
        /// Gets or sets the total number of commands.
        /// </summary>
        public int CommandCount { get; set; }

        /// <summary>
        /// Gets or sets the number of failed commands.
        /// </summary>
        public int CommandErrors { get; set; }

        /// <summary>
        /// Gets or sets the total number of requests.
        /// </summary>
        public int RequestCount { get; set; }

        /// <summary>
        /// Gets or sets the number of failed requests.
        /// </summary>
        public int RequestErrors { get; set; }
    }
}
