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
 * Summary  : Represents a slice of channel data having the same timestamp
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a slice of channel data having the same timestamp.
    /// <para>Представляет срез данных каналов, имеющих одинаковую временную метку.</para>
    /// </summary>
    public class Slice
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Slice(DateTime timestamp, int length)
        {
            Timestamp = timestamp;
            CnlNums = new int[length];
            CnlData = new CnlData[length];
            DeviceNum = 0;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Slice(DateTime timestamp, int[] cnlNums)
        {
            Timestamp = timestamp;
            CnlNums = cnlNums ?? throw new ArgumentNullException(nameof(cnlNums));
            CnlData = new CnlData[cnlNums.Length];
            DeviceNum = 0;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Slice(DateTime timestamp, int[] cnlNums, CnlData[] cnlData)
        {
            Timestamp = timestamp;
            CnlNums = cnlNums ?? throw new ArgumentNullException(nameof(cnlNums));
            CnlData = cnlData ?? throw new ArgumentNullException(nameof(cnlData));
            DeviceNum = 0;

            if (cnlNums.Length != cnlData.Length)
                throw new ArgumentException("Invalid data size.");
        }


        /// <summary>
        /// Gets or sets the timestamp (UTC).
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets the channel numbers.
        /// </summary>
        public int[] CnlNums { get; }

        /// <summary>
        /// Gets the channel data corresponding to the channel numbers.
        /// </summary>
        public CnlData[] CnlData { get; }

        /// <summary>
        /// Gets or sets the device number.
        /// </summary>
        /// <remarks>
        /// Identifies the device to which the channels belong.
        /// The value is zero if the device is undefined.
        /// </remarks>
        public int DeviceNum { get; set; }

        /// <summary>
        /// Gets the number of channels in the slice.
        /// </summary>
        public int Length
        {
            get
            {
                return CnlNums.Length;
            }
        }
    }
}
