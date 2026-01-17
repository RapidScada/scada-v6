// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Scada.Comm.Devices;

namespace Scada.Comm.Drivers.DrvOpcUa.Logic
{
    /// <summary>
    /// Contains device slices obtained by reading historical data.
    /// <para>Содержит срезы устройства, полученные в результате чтения исторических данных.</para>
    /// </summary>
    internal class HistoricalSlices
    {
        private readonly Dictionary<DateTime, DeviceSlice> sliceDict;
        private readonly List<DeviceSlice> sliceList;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HistoricalSlices(List<DeviceTag> deviceTags)
        {
            sliceDict = [];
            sliceList = [];

            DeviceTags = deviceTags ?? throw new ArgumentNullException(nameof(deviceTags));
        }


        /// <summary>
        /// Gets the device tags. A list item can be null.
        /// </summary>
        public List<DeviceTag> DeviceTags { get; }


        /// <summary>
        /// Adds a historical data value to the appropriate slice.
        /// </summary>
        public void AddDataValue(int deviceTagIndex, DataValue dataValue)
        {

        }

        /// <summary>
        /// Adds the historical slices to the device data queue.
        /// </summary>
        public void EnqueueSlices(DeviceData deviceData)
        {
            ArgumentNullException.ThrowIfNull(deviceData, nameof(deviceData));
            sliceList.ForEach(deviceData.EnqueueSlice);
        }
    }
}
