/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Represents a slice of current or historical data created by a device driver
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Models;
using System;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents a slice of current or historical data created by a device driver.
    /// <para>Представляет срез текущих или архивных данных, созданный драйвером устройства.</para>
    /// </summary>
    public class DeviceSlice
    {
        /// <summary>
        /// Specifies an empty slice.
        /// </summary>
        public static readonly DeviceSlice Empty = new DeviceSlice(DateTime.MinValue, 0, 0);


        /// <summary>
        /// Represents a method that executes when a slice is sent successfully or unsuccessfully.
        /// </summary>
        public delegate void DataSentDelegate(DeviceSlice deviceSlice, string dataSourceCode);


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceSlice(DateTime timestamp, int tagCount, int dataLength)
        {
            Timestamp = timestamp;
            DeviceTags = new DeviceTag[tagCount];
            CnlData = new CnlData[dataLength];
            DeviceNum = 0;
            ArchiveMask = Data.Models.ArchiveMask.Default;
            Descr = "";
            DataSentCallback = null;
            FailedToSendCallback = null;
        }


        /// <summary>
        /// Gets or sets the timestamp (UTC).
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets the device tags whose data is included in the slice.
        /// </summary>
        public DeviceTag[] DeviceTags { get; }

        /// <summary>
        /// Gets the channel data corresponding to the device tags.
        /// </summary>
        public CnlData[] CnlData { get; }

        /// <summary>
        /// Gets or sets the device number.
        /// </summary>
        public int DeviceNum { get; set; }

        /// <summary>
        /// Gets or sets the mask that identifies the target archives.
        /// </summary>
        public int ArchiveMask { get; set; }

        /// <summary>
        /// Gets or sets the description to display.
        /// </summary>
        public string Descr { get; set; }

        /// <summary>
        /// Gets or sets the method that is executed when the slice is successfully sent.
        /// </summary>
        public DataSentDelegate DataSentCallback { get; set; }

        /// <summary>
        /// Gets or sets the method that is executed when the slice could not be sent.
        /// </summary>
        public DataSentDelegate FailedToSendCallback { get; set; }

        /// <summary>
        /// Gets a value indicating whether this slice is empty.
        /// </summary>
        public bool IsEmpty => DeviceTags.Length == 0;
    }
}
