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
 * Summary  : Represents device data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents device data.
    /// <para>Представляет данные устройства.</para>
    /// </summary>
    /// <remarks>The class is thread safe.</remarks>
    public class DeviceData
    {
        private readonly int deviceNum;             // the device number
        private readonly Queue<DeviceSlice> slices; // the historical data queue
        private readonly Queue<DeviceEvent> events; // the event queue
        private readonly DeviceDataView dataView;   // converts data to a string representation
        private readonly object curDataLock;        // syncronizes access to current data

        private DeviceTags deviceTags; // the device tags
        private bool[] modifiedFlags;  // the tag modification flags
        private CnlData[] rawData;     // the raw tag data


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceData(int deviceNum)
        {
            this.deviceNum = deviceNum;
            slices = new Queue<DeviceSlice>();
            events = new Queue<DeviceEvent>();
            dataView = new DeviceDataView();
            curDataLock = new object();

            deviceTags = null;
            modifiedFlags = null;
            rawData = null;
        }


        /// <summary>
        /// Gets or sets the data for the device tag at the specified index.
        /// </summary>
        public CnlData this[int tagIndex]
        {
            get
            {
                return GetCnlData(tagIndex, 0);
            }
            set
            {
                SetCnlData(tagIndex, 0, value);
            }
        }

        /// <summary>
        /// Gets or sets the data for the device tag at the specified code.
        /// </summary>
        public CnlData this[string tagCode]
        {
            get
            {
                DeviceTag deviceTag = deviceTags[tagCode];
                return GetCnlData(deviceTag.Index, 0);
            }
            set
            {
                DeviceTag deviceTag = deviceTags[tagCode];
                SetCnlData(deviceTag.Index, 0, value);
            }
        }


        /// <summary>
        /// Gets the data for the device tag at the specified index.
        /// </summary>
        private CnlData GetCnlData(int tagIndex, int offset)
        {
            if (rawData == null)
            {
                return CnlData.Empty;
            }
            else
            {
                lock (curDataLock)
                {
                    DeviceTag deviceTag = deviceTags[tagIndex];
                    return rawData[deviceTag.DataIndex + offset];
                }
            }
        }

        /// <summary>
        /// Sets the data for the device tag at the specified index.
        /// </summary>
        private void SetCnlData(int tagIndex, int offset, CnlData value)
        {
            if (rawData != null)
            {
                lock (curDataLock)
                {
                    DeviceTag deviceTag = deviceTags[tagIndex];

                    if (rawData[deviceTag.DataIndex + offset] != value)
                        modifiedFlags[tagIndex] = true;

                    rawData[deviceTag.DataIndex + offset] = value;
                }
            }
        }

        /// <summary>
        /// Sets the display values of the tags.
        /// </summary>
        private void SetDisplayValues()
        {
            if (deviceTags != null)
            {
                foreach (DeviceTag deviceTag in deviceTags)
                {
                    int tagIndex = deviceTag.Index;
                    dataView.SetDisplayValue(tagIndex, 0, FormatTagData(deviceTag));

                    if (deviceTag.IsNumericArray)
                    {
                        for (int i = 0, len = deviceTag.DataLength; i < len; i++)
                        {
                            CnlData cnlData = GetCnlData(tagIndex, i);
                            dataView.SetDisplayValue(tagIndex, i + 1, FormatNumericData(deviceTag, cnlData));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Converts the current tag data to a string value.
        /// </summary>
        private string FormatTagData(DeviceTag deviceTag)
        {
            int tagIndex = deviceTag.Index;
            CnlData cnlData = this[tagIndex];

            if (cnlData.IsUndefined)
            {
                return CommonPhrases.UndefinedSign;
            }
            else
            {
                switch (deviceTag.DataType)
                {
                    case TagDataType.Double:
                        return deviceTag.DataLength > 1 ? "Double[]" : FormatNumericData(deviceTag, cnlData);

                    case TagDataType.Int64:
                        return deviceTag.DataLength > 1 ? "Int64[]" : FormatNumericData(deviceTag, cnlData);

                    case TagDataType.ASCII:
                        return GetAscii(tagIndex);

                    case TagDataType.Unicode:
                        return GetUnicode(tagIndex);

                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// Converts the current tag data with a numeric data type to a string value.
        /// </summary>
        private string FormatNumericData(DeviceTag deviceTag, CnlData cnlData)
        {
            if (cnlData.IsUndefined)
                return CommonPhrases.UndefinedSign;

            const string DefaultFormat = "N3";

            try
            {
                TagFormat tagFormat = deviceTag.Format;

                if (tagFormat == null)
                {
                    return deviceTag.DataType == TagDataType.Int64 ?
                        BitConverter.DoubleToInt64Bits(cnlData.Val).ToString() :
                        cnlData.Val.ToString(DefaultFormat);
                }
                else
                {
                    string FormatEnum(int val)
                    {
                        return tagFormat.EnumValues != null && 0 <= val && val < tagFormat.EnumValues.Length
                            ? tagFormat.EnumValues[val]
                            : val.ToString();
                    }

                    bool FormatIsHex(string format)
                    {
                        return format[0] == 'x' || format[0] == 'X';
                    }

                    if (deviceTag.DataType == TagDataType.Int64)
                    {
                        long longVal = BitConverter.DoubleToInt64Bits(cnlData.Val);

                        switch (tagFormat.FormatType)
                        {
                            case TagFormatType.Enum:
                                return FormatEnum((int)longVal);

                            case TagFormatType.Date:
                                DateTime dt = new DateTime(longVal, DateTimeKind.Utc).ToLocalTime();
                                return string.IsNullOrEmpty(tagFormat.Format) 
                                    ? dt.ToLocalizedString() 
                                    : dt.ToString(tagFormat.Format);

                            default:
                                if (string.IsNullOrEmpty(tagFormat.Format))
                                {
                                    return longVal.ToString();
                                }
                                else
                                {
                                    string s = longVal.ToString(tagFormat.Format);
                                    return FormatIsHex(tagFormat.Format) ? s + 'h' : s;
                                }
                        }
                    }
                    else
                    {
                        double doubleVal = cnlData.Val;

                        switch (tagFormat.FormatType)
                        {
                            case TagFormatType.Enum:
                                return FormatEnum((int)doubleVal);

                            case TagFormatType.Date:
                                DateTime dt = DateTime.FromOADate(doubleVal).ToLocalTime();
                                return string.IsNullOrEmpty(tagFormat.Format) 
                                    ? dt.ToLocalizedString() 
                                    : dt.ToString(tagFormat.Format);

                            default:
                                return string.IsNullOrEmpty(tagFormat.Format) 
                                    ? doubleVal.ToString(DefaultFormat) 
                                    : FormatIsHex(tagFormat.Format) 
                                        ? ((int)doubleVal).ToString(tagFormat.Format) + 'h' 
                                        : doubleVal.ToString(tagFormat.Format);
                        }
                    }
                }
            }
            catch
            {
                return cnlData.Val.ToString(DefaultFormat);
            }
        }


        /// <summary>
        /// Initializes the device data to maintain the specified device tags.
        /// </summary>
        public void Init(DeviceTags deviceTags)
        {
            this.deviceTags = deviceTags ?? throw new ArgumentNullException(nameof(deviceTags));
            int dataLength = 0;

            foreach (DeviceTag deviceTag in deviceTags)
            {
                deviceTag.DataIndex = dataLength;
                dataLength += deviceTag.DataLength;
            }

            modifiedFlags = new bool[deviceTags.Count];
            rawData = new CnlData[dataLength];
            dataView.PrepareCurData(deviceTags);
        }

        /// <summary>
        /// Gets the floating point value of the tag.
        /// </summary>
        public double Get(int tagIndex)
        {
            CnlData cnlData = this[tagIndex];
            return cnlData.IsDefined ? cnlData.Val : 0.0;
        }

        /// <summary>
        /// Gets the floating point value of the tag.
        /// </summary>
        public double Get(string tagCode)
        {
            CnlData cnlData = this[tagCode];
            return cnlData.IsDefined ? cnlData.Val : 0.0;
        }

        /// <summary>
        /// Gets the integer value of the tag.
        /// </summary>
        public long GetInt64(int tagIndex)
        {
            CnlData cnlData = this[tagIndex];
            return cnlData.IsDefined ? BitConverter.DoubleToInt64Bits(cnlData.Val) : 0;
        }

        /// <summary>
        /// Gets the integer value of the tag.
        /// </summary>
        public long GetInt64(string tagCode)
        {
            CnlData cnlData = this[tagCode];
            return cnlData.IsDefined ? BitConverter.DoubleToInt64Bits(cnlData.Val) : 0;
        }

        /// <summary>
        /// Gets the array of floating point values.
        /// </summary>
        public double[] GetDoubleArray(int tagIndex)
        {
            lock (curDataLock)
            {
                int arrayLength = deviceTags[tagIndex].DataLength;
                double[] array = new double[arrayLength];

                for (int i = 0; i < arrayLength; i++)
                {
                    CnlData cnlData = GetCnlData(tagIndex, i);
                    array[i] = cnlData.IsDefined ? cnlData.Val : 0.0;
                }

                return array;
            }
        }

        /// <summary>
        /// Gets the array of floating point values.
        /// </summary>
        public double[] GetDoubleArray(string tagCode)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            return GetDoubleArray(deviceTag.Index);
        }

        /// <summary>
        /// Gets the array of bytes.
        /// </summary>
        public byte[] GetByteArray(int tagIndex)
        {
            if (deviceTags[tagIndex].DataLength > 1)
            {
                double[] doubleArray = GetDoubleArray(tagIndex);
                int dataLength = doubleArray.Length * 8;
                byte[] byteArray = new byte[dataLength];
                Buffer.BlockCopy(doubleArray, 0, byteArray, 0, dataLength);
                return byteArray;
            }
            else
            {
                return BitConverter.GetBytes(Get(tagIndex));
            }
        }

        /// <summary>
        /// Gets the array of bytes.
        /// </summary>
        public byte[] GetByteArray(string tagCode)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            return GetByteArray(deviceTag.Index);
        }

        /// <summary>
        /// Gets the ASCII string value of the tag.
        /// </summary>
        public string GetAscii(int tagIndex)
        {
            byte[] array = GetByteArray(tagIndex);
            return Encoding.ASCII.GetString(array).TrimEnd((char)0);
        }

        /// <summary>
        /// Gets the ASCII string value of the tag.
        /// </summary>
        public string GetAscii(string tagCode)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            return GetAscii(deviceTag.Index);
        }

        /// <summary>
        /// Gets the Unicode string value of the tag.
        /// </summary>
        public string GetUnicode(int tagIndex)
        {
            byte[] array = GetByteArray(tagIndex);
            return Encoding.Unicode.GetString(array).TrimEnd((char)0);
        }

        /// <summary>
        /// Gets the Unicode string value of the tag.
        /// </summary>
        public string GetUnicode(string tagCode)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            return GetUnicode(deviceTag.Index);
        }

        /// <summary>
        /// Gets the date and time value of the tag.
        /// </summary>
        public DateTime GetDateTime(int tagIndex)
        {
            CnlData cnlData = this[tagIndex];

            if (cnlData.IsDefined)
            {
                DeviceTag deviceTag = deviceTags[tagIndex];
                
                if (deviceTag.DataType == TagDataType.Int64)
                    return new DateTime(BitConverter.DoubleToInt64Bits(cnlData.Val), DateTimeKind.Utc);
                else if (deviceTag.DataType == TagDataType.Double)
                    return DateTime.FromOADate(cnlData.Val);
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// Gets the date and time value of the tag.
        /// </summary>
        public DateTime GetDateTime(string tagCode)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            return GetDateTime(deviceTag.Index);
        }

        /// <summary>
        /// Sets the floating point value and status of the tag.
        /// </summary>
        public void Set(int tagIndex, double val, int stat)
        {
            this[tagIndex] = new CnlData(val, stat);
        }

        /// <summary>
        /// Sets the floating point value of the tag.
        /// </summary>
        public void Set(int tagIndex, double val)
        {
            this[tagIndex] = double.IsNaN(val) ?
                CnlData.Empty :
                new CnlData(val, CnlStatusID.Defined);
        }

        /// <summary>
        /// Sets the floating point value and status of the tag.
        /// </summary>
        public void Set(string tagCode, double val, int stat)
        {
            this[tagCode] = new CnlData(val, stat);
        }

        /// <summary>
        /// Sets the floating point value of the tag.
        /// </summary>
        public void Set(string tagCode, double val)
        {
            this[tagCode] = double.IsNaN(val) ?
                CnlData.Empty :
                new CnlData(val, CnlStatusID.Defined);
        }

        /// <summary>
        /// Sets the integer value and status of the tag.
        /// </summary>
        public void SetInt64(int tagIndex, long val, int stat)
        {
            this[tagIndex] = new CnlData(BitConverter.Int64BitsToDouble(val), stat);
        }

        /// <summary>
        /// Sets the integer value and status of the tag.
        /// </summary>
        public void SetInt64(string tagCode, long val, int stat)
        {
            this[tagCode] = new CnlData(BitConverter.Int64BitsToDouble(val), stat);
        }

        /// <summary>
        /// Sets the array of floating point values and status starting from the specified tag.
        /// </summary>
        public void SetDoubleArray(int tagIndex, double[] vals, int stat)
        {
            lock (curDataLock)
            {
                int dataLen = deviceTags[tagIndex].DataLength;
                int valLen = vals.Length;

                for (int i = 0; i < dataLen; i++)
                {
                    CnlData cnlData = new CnlData(i < valLen ? vals[i] : 0.0, stat);
                    SetCnlData(tagIndex, i, cnlData);
                }
            }
        }

        /// <summary>
        /// Sets the array of floating point values and status starting from the specified tag.
        /// </summary>
        public void SetDoubleArray(string tagCode, double[] vals, int stat)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            SetDoubleArray(deviceTag.Index, vals, stat);
        }

        /// <summary>
        /// Sets the array of bytes and status starting from the specified tag.
        /// </summary>
        public void SetByteArray(int tagIndex, byte[] vals, int stat)
        {
            int dataLength = vals.Length;
            int arrayLength = dataLength % 8 == 0 ? dataLength / 8 : dataLength / 8 + 1;
            double[] doubleArray = new double[arrayLength];
            Buffer.BlockCopy(vals, 0, doubleArray, 0, dataLength);
            SetDoubleArray(tagIndex, doubleArray, stat);
        }

        /// <summary>
        /// Sets the array of bytes and status starting from the specified tag.
        /// </summary>
        public void SetByteArray(string tagCode, byte[] vals, int stat)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            SetByteArray(deviceTag.Index, vals, stat);
        }

        /// <summary>
        /// Sets the ASCII string value and status of the tag.
        /// </summary>
        public void SetAscii(int tagIndex, string s, int stat)
        {
            SetByteArray(tagIndex, Encoding.ASCII.GetBytes(s ?? ""), stat);
        }

        /// <summary>
        /// Sets the ASCII string value and status of the tag.
        /// </summary>
        public void SetAscii(string tagCode, string s, int stat)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            SetAscii(deviceTag.Index, s, stat);
        }

        /// <summary>
        /// Sets the Unicode string value and status of the tag.
        /// </summary>
        public void SetUnicode(int tagIndex, string s, int stat)
        {
            SetByteArray(tagIndex, Encoding.Unicode.GetBytes(s ?? ""), stat);
        }

        /// <summary>
        /// Sets the Unicode string value and status of the tag.
        /// </summary>
        public void SetUnicode(string tagCode, string s, int stat)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            SetUnicode(deviceTag.Index, s, stat);
        }

        /// <summary>
        /// Sets the date and time value and status of the tag.
        /// </summary>
        public void SetDateTime(int tagIndex, DateTime dateTime, int stat)
        {
            DeviceTag deviceTag = deviceTags[tagIndex];

            if (deviceTag.DataType == TagDataType.Int64)
                SetInt64(tagIndex, dateTime.ToUniversalTime().Ticks, stat);
            else if (deviceTag.DataType == TagDataType.Double)
                Set(tagIndex, dateTime.ToUniversalTime().ToOADate(), stat);
        }

        /// <summary>
        /// Sets the date and time value and status of the tag.
        /// </summary>
        public void SetDateTime(string tagCode, DateTime dateTime, int stat)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            SetDateTime(deviceTag.Index, dateTime, stat);
        }

        /// <summary>
        /// Sets the device status tag.
        /// </summary>
        public void SetStatusTag(DeviceStatus status)
        {
            if (deviceTags.ContainsTag(CommUtils.StatusTagCode))
                Set(CommUtils.StatusTagCode, (double)status);
        }

        /// <summary>
        /// Adds the specified term to the tag value.
        /// </summary>
        public void Add(int tagIndex, double val)
        {
            Set(tagIndex, Get(tagIndex) + val);
        }

        /// <summary>
        /// Adds the specified term to the tag value.
        /// </summary>
        public void Add(string tagCode, double val)
        {
            Set(tagCode, Get(tagCode) + val);
        }

        /// <summary>
        /// Sets all tags to undefined.
        /// </summary>
        public void Invalidate()
        {
            Invalidate(0, deviceTags.Count);
        }

        /// <summary>
        /// Sets the specified tag to undefined.
        /// </summary>
        public void Invalidate(int tagIndex)
        {
            lock (curDataLock)
            {
                DeviceTag deviceTag = deviceTags[tagIndex];
                int idx = deviceTag.DataIndex;
                int len = deviceTag.DataLength;
                bool modified = false;

                for (int i = 0; i < len; i++)
                {
                    if (rawData[idx] != CnlData.Empty)
                    {
                        rawData[idx] = CnlData.Empty;
                        modified = true;
                    }

                    idx++;
                }

                modifiedFlags[tagIndex] = modified;
            }
        }

        /// <summary>
        /// Sets the specified tag to undefined.
        /// </summary>
        public void Invalidate(string tagCode)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            Invalidate(deviceTag.Index);
        }

        /// <summary>
        /// Sets the specified tag range to undefined.
        /// </summary>
        public void Invalidate(int tagIndex, int tagCount)
        {
            for (int i = 0; i < tagCount; i++)
            {
                Invalidate(tagIndex + i);
            }
        }

        /// <summary>
        /// Sets the specified tag range to undefined.
        /// </summary>
        public void Invalidate(string tagCode, int tagCount)
        {
            DeviceTag deviceTag = deviceTags[tagCode];
            Invalidate(deviceTag.Index, tagCount);
        }

        /// <summary>
        /// Gets a slice of the current data of all tags.
        /// </summary>
        public DeviceSlice GetCurrentData()
        {
            if (deviceTags.Count == 0)
                return DeviceSlice.Empty;

            lock (curDataLock)
            {
                DeviceSlice deviceSlice = new DeviceSlice(DateTime.UtcNow, deviceTags.Count, rawData.Length)
                {
                    DeviceNum = deviceNum
                };

                int tagIndex = 0;
                int dataIndex = 0;

                foreach (DeviceTag deviceTag in deviceTags)
                {
                    deviceSlice.DeviceTags[tagIndex] = deviceTag;

                    for (int i = 0, len = deviceTag.DataLength; i < len; i++)
                    {
                        deviceSlice.CnlData[dataIndex] = rawData[dataIndex];
                        dataIndex++;
                    }

                    tagIndex++;
                }

                Array.Clear(modifiedFlags, 0, modifiedFlags.Length);
                return deviceSlice;
            }
        }

        /// <summary>
        /// Gets a slice of the current data for modified tags.
        /// </summary>
        public DeviceSlice GetModifiedData()
        {
            if (deviceTags.Count == 0)
                return DeviceSlice.Empty;

            lock (curDataLock)
            {
                int tagCount = 0;   // number of modified tags
                int dataLength = 0; // data length of modified tags

                foreach (DeviceTag deviceTag in deviceTags)
                {
                    if (modifiedFlags[deviceTag.Index])
                    {
                        tagCount++;
                        dataLength += deviceTag.DataLength;
                    }
                }

                if (tagCount == 0)
                    return DeviceSlice.Empty;

                DeviceSlice deviceSlice = new DeviceSlice(DateTime.UtcNow, tagCount, dataLength)
                {
                    DeviceNum = deviceNum
                };

                int tagIndex = 0;
                int dataIndex = 0;

                foreach (DeviceTag deviceTag in deviceTags)
                {
                    if (modifiedFlags[deviceTag.Index])
                    {
                        modifiedFlags[deviceTag.Index] = false;
                        deviceSlice.DeviceTags[tagIndex] = deviceTag;

                        for (int i = 0, len = deviceTag.DataLength; i < len; i++)
                        {
                            deviceSlice.CnlData[dataIndex] = rawData[deviceTag.DataIndex + i];
                            dataIndex++;
                        }

                        tagIndex++;
                    }
                }

                return deviceSlice;
            }
        }

        /// <summary>
        /// Adds the slice of historical data to the queue.
        /// </summary>
        public void EnqueueSlice(DeviceSlice deviceSlice)
        {
            if (deviceSlice == null)
                throw new ArgumentNullException(nameof(deviceSlice));

            deviceSlice.DeviceNum = deviceNum;
            dataView.AddSlice(deviceSlice);

            lock (slices)
            {
                slices.Enqueue(deviceSlice);
            }
        }

        /// <summary>
        /// Removes and returns the slice of historical data at the beginning of the queue.
        /// </summary>
        public bool DequeueSlice(out DeviceSlice deviceSlice)
        {
            lock (slices)
            {
                if (slices.Count > 0)
                {
                    deviceSlice = slices.Dequeue();
                    return true;
                }
                else
                {
                    deviceSlice = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Adds the device event to the queue.
        /// </summary>
        public void EnqueueEvent(DeviceEvent deviceEvent)
        {
            if (deviceEvent == null)
                throw new ArgumentNullException(nameof(deviceEvent));

            deviceEvent.DeviceNum = deviceNum;
            dataView.AddEvent(deviceEvent);

            lock (events)
            {
                events.Enqueue(deviceEvent);
            }
        }

        /// <summary>
        /// Removes and returns the device event at the beginning of the queue.
        /// </summary>
        public bool DequeueEvent(out DeviceEvent deviceEvent)
        {
            lock (events)
            {
                if (events.Count > 0)
                {
                    deviceEvent = events.Dequeue();
                    return true;
                }
                else
                {
                    deviceEvent = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Registers the telecontrol command for display.
        /// </summary>
        public void RegisterCommand(TeleCommand cmd)
        {
            dataView.AddCommand(cmd);
        }

        /// <summary>
        /// Appends a string representation of the device data to the string builder.
        /// </summary>
        public void AppendInfo(StringBuilder sb)
        {
            SetDisplayValues();
            dataView.AppendCurrentDataInfo(sb);
            dataView.AppendHistoricalDataInfo(sb);
            dataView.AppendEventInfo(sb);
            dataView.AppendCommandInfo(sb);
        }
    }
}
