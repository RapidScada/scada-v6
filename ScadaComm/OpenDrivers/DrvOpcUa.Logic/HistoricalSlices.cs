// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Scada.Comm.Devices;
using Scada.Data.Models;

namespace Scada.Comm.Drivers.DrvOpcUa.Logic
{
    /// <summary>
    /// Contains device slices obtained by reading historical data.
    /// <para>Содержит срезы устройства, полученные в результате чтения исторических данных.</para>
    /// </summary>
    /// <remarks>
    /// Only device tags of the Double type that are not arrays are supported in history.
    /// </remarks>
    internal class HistoricalSlices
    {
        /// <summary>
        /// Represents information associated with a device tag.
        /// </summary>
        private class DeviceTagMeta(DeviceTag deviceTag)
        {
            public DeviceTag DeviceTag { get; } = deviceTag;
            public bool IsIncluded { get; init; }
            public int DataIndex { get; init; }
        }

        private readonly List<DeviceTagMeta> deviceTagMetas;
        private readonly DeviceTag[] includedDeviceTags;
        private readonly Dictionary<DateTime, DeviceSlice> sliceDict;
        private readonly List<DeviceSlice> sliceList;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HistoricalSlices(List<DeviceTag> deviceTags)
        {
            deviceTagMetas = [];
            sliceDict = [];
            sliceList = [];
            FillDeviceTagMetas(deviceTags);
            includedDeviceTags = deviceTagMetas.Where(m => m.IsIncluded).Select(m => m.DeviceTag).ToArray();
            DeviceTags = deviceTags ?? throw new ArgumentNullException(nameof(deviceTags));
        }


        /// <summary>
        /// Gets the device tags. A list item can be null.
        /// </summary>
        public List<DeviceTag> DeviceTags { get; }


        /// <summary>
        /// Fills in information associated with the device tags.
        /// </summary>
        private void FillDeviceTagMetas(List<DeviceTag> deviceTags)
        {
            // only the non-array device tags of double type are supported in history
            int dataIndex = 0;

            foreach (DeviceTag deviceTag in deviceTags)
            {
                if (deviceTag != null && deviceTag.DataType == TagDataType.Double && deviceTag.DataLength == 1)
                {
                    deviceTagMetas.Add(new DeviceTagMeta(deviceTag)
                    {
                        IsIncluded = true,
                        DataIndex = dataIndex++
                    });
                }
                else
                {
                    deviceTagMetas.Add(new DeviceTagMeta(deviceTag)
                    {
                        IsIncluded = false,
                        DataIndex = -1
                    });
                }
            }
        }

        /// <summary>
        /// Gets the device tag information by the tag index.
        /// </summary>
        private DeviceTagMeta GetDeviceTagMeta(int index)
        {
            return 0 <= index && index < deviceTagMetas.Count ? deviceTagMetas[index] : null;
        }

        /// <summary>
        /// Creates a new device slice containing the chosen tags.
        /// </summary>
        private DeviceSlice CreateDeviceSlice(DateTime timestamp)
        {
            return new DeviceSlice(timestamp, includedDeviceTags, new CnlData[includedDeviceTags.Length]);
        }

        /// <summary>
        /// Gets the timestamp of the data value.
        /// </summary>
        private static DateTime GetTimestamp(DataValue dataValue)
        {
            return dataValue.SourceTimestamp > DateTime.MinValue
                ? dataValue.SourceTimestamp
                : dataValue.ServerTimestamp;
        }

        /// <summary>
        /// Gets the device tag data from the value returned by the OPC server.
        /// </summary>
        private static CnlData GetDeviceTagData(DataValue dataValue)
        {
            try
            {
                return new CnlData(
                    Convert.ToDouble(dataValue.Value),
                    LogicUtils.GetDeviceTagStatus(dataValue.StatusCode));
            }
            catch
            {
                return CnlData.Empty;
            }
        }


        /// <summary>
        /// Adds a historical data value to the appropriate slice.
        /// </summary>
        public void AddDataValue(int deviceTagIndex, DataValue dataValue)
        {
            ArgumentNullException.ThrowIfNull(dataValue, nameof(dataValue));
            DeviceTagMeta tagMeta = GetDeviceTagMeta(deviceTagIndex);
            DateTime timestamp = GetTimestamp(dataValue);

            if (tagMeta != null && tagMeta.IsIncluded && timestamp > DateTime.MinValue)
            {
                if (!sliceDict.TryGetValue(timestamp, out DeviceSlice deviceSlice))
                {
                    deviceSlice = CreateDeviceSlice(timestamp);
                    sliceDict.Add(timestamp, deviceSlice);
                    sliceList.Add(deviceSlice);
                }

                deviceSlice.CnlData[tagMeta.DataIndex] = GetDeviceTagData(dataValue);
            }
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
