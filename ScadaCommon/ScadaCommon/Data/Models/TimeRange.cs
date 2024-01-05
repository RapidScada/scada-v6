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
 * Summary  : Represents a time range
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a time range.
    /// <para>Представляет временной диапазон.</para>
    /// </summary>
    public struct TimeRange
    {
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public TimeRange(DateTime startTime, DateTime endTime, bool endInclusive)
        {
            StartTime = startTime;
            EndTime = endTime;
            EndInclusive = endInclusive;
        }


        /// <summary>
        /// Gets the key that identifies the time range.
        /// </summary>
        public string Key
        {
            get
            {
                return StartTime.ToString("O") + "_" + EndTime.ToString("O") + "_" + EndInclusive;
            }
        }

        /// <summary>
        /// Gets or sets the start timestamp.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end timestamp.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whther to include the end timestamp.
        /// </summary>
        public bool EndInclusive { get; set; }
    }
}
