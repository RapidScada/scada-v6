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
 * Summary  : Represents a context of a historical archive update operation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents a context of a historical archive update operation.
    /// <para>Представляет контекст операции обновления исторического архива.</para>
    /// </summary>
    public class UpdateContext
    {
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public UpdateContext(DateTime timestamp, int deviceNum)
        {
            Timestamp = timestamp;
            DeviceNum = deviceNum;
            Stopwatch = Stopwatch.StartNew();
            UpdatedData = new Dictionary<int, CnlData>();
            UpdatedCount = 0;
            LostCount = 0;
        }


        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the device number.
        /// </summary>
        public int DeviceNum { get; }

        /// <summary>
        /// Gets the stopwatch to measure update time.
        /// </summary>
        public Stopwatch Stopwatch { get; }

        /// <summary>
        /// Gets the channel data that was updated during the current operation.
        /// </summary>
        public Dictionary<int, CnlData> UpdatedData { get; }

        /// <summary>
        /// Gets or sets the number of updated data points.
        /// </summary>
        public int UpdatedCount { get; set; }

        /// <summary>
        /// Gets or sets the number of lost data points.
        /// </summary>
        public int LostCount { get; set; }
    }
}
