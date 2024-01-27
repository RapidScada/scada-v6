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
 * Summary  : Represents a trend point, which consists of a timestamp and channel data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a trend point, which consists of a timestamp and channel data.
    /// <para>Представляет точку тренда, которая состоит из метки времени и данных канала.</para>
    /// </summary>
    public struct TrendPoint : IComparable<TrendPoint>
    {
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public TrendPoint(DateTime timestamp)
        {
            Timestamp = timestamp;
            Val = 0.0;
            Stat = 0;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public TrendPoint(DateTime timestamp, double val, int stat)
        {
            Timestamp = timestamp;
            Val = val;
            Stat = stat;
        }


        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the channel value.
        /// </summary>
        public double Val { get; set; }

        /// <summary>
        /// Gets or sets the channel status.
        /// </summary>
        public int Stat { get; set; }


        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        public int CompareTo(TrendPoint other)
        {
            return Timestamp.CompareTo(other.Timestamp);
        }
    }
}
