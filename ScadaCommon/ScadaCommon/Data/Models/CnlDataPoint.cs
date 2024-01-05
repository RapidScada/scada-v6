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
 * Summary  : Represents a channel data point
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a channel data point.
    /// <para>Представляет точку данных канала.</para>
    /// </summary>
    public struct CnlDataPoint
    {
        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public CnlDataPoint(int cnlNum, DateTime timestamp)
        {
            CnlNum = cnlNum;
            Timestamp = timestamp;
            Val = 0.0;
            Stat = 0;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public CnlDataPoint(int cnlNum, DateTime timestamp, double val, int stat)
        {
            CnlNum = cnlNum;
            Timestamp = timestamp;
            Val = val;
            Stat = stat;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public CnlDataPoint(int cnlNum, DateTime timestamp, CnlData cnlData)
        {
            CnlNum = cnlNum;
            Timestamp = timestamp;
            Val = cnlData.Val;
            Stat = cnlData.Stat;
        }


        /// <summary>
        /// Gets or sets the channel number.
        /// </summary>
        public int CnlNum { get; set; }

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
    }
}
