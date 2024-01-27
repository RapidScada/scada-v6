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
 * Summary  : Converts base data types to an array of bytes and vice versa
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Scada
{
    /// <summary>
    /// Converts base data types to an array of bytes and vice versa.
    /// <para>Преобразует базовые типы данных в массив байтов и наоборот.</para>
    /// </summary>
    /// <remarks>BinaryConverter uses little endian format.</remarks>
    public static class BinaryConverter
    {
        /// <summary>
        /// Copies the 8-bit unsigned integer to the buffer.
        /// </summary>
        public static void CopyByte(byte value, byte[] buffer, ref int index)
        {
            buffer[index++] = value;
        }

        /// <summary>
        /// Copies the boolean value to the buffer.
        /// </summary>
        public static void CopyBool(bool value, byte[] buffer, int index)
        {
            buffer[index] = (byte)(value ? 1 : 0);
        }

        /// <summary>
        /// Copies the boolean value to the buffer.
        /// </summary>
        public static void CopyBool(bool value, byte[] buffer, ref int index)
        {
            CopyBool(value, buffer, index++);
        }

        /// <summary>
        /// Copies the 16-bit unsigned integer to the buffer.
        /// </summary>
        public static void CopyUInt16(ushort value, byte[] buffer, int index)
        {
            buffer[index++] = (byte)value;
            buffer[index++] = (byte)(value >> 8);
        }

        /// <summary>
        /// Copies the 16-bit unsigned integer to the buffer.
        /// </summary>
        public static void CopyUInt16(ushort value, byte[] buffer, ref int index)
        {
            CopyUInt16(value, buffer, index);
            index += 2;
        }

        /// <summary>
        /// Copies the 32-bit signed integer to the buffer.
        /// </summary>
        public static void CopyInt32(int value, byte[] buffer, int index)
        {
            buffer[index++] = (byte)value;
            buffer[index++] = (byte)(value >> 8);
            buffer[index++] = (byte)(value >> 16);
            buffer[index++] = (byte)(value >> 24);
        }

        /// <summary>
        /// Copies the 32-bit signed integer to the buffer.
        /// </summary>
        public static void CopyInt32(int value, byte[] buffer, ref int index)
        {
            CopyInt32(value, buffer, index);
            index += 4;
        }

        /// <summary>
        /// Copies the 64-bit signed integer to the buffer.
        /// </summary>
        public static void CopyInt64(long value, byte[] buffer, int index)
        {
            buffer[index++] = (byte)value;
            buffer[index++] = (byte)(value >> 8);
            buffer[index++] = (byte)(value >> 16);
            buffer[index++] = (byte)(value >> 24);
            buffer[index++] = (byte)(value >> 32);
            buffer[index++] = (byte)(value >> 40);
            buffer[index++] = (byte)(value >> 48);
            buffer[index++] = (byte)(value >> 56);
        }

        /// <summary>
        /// Copies the 64-bit signed integer to the buffer.
        /// </summary>
        public static void CopyInt64(long value, byte[] buffer, ref int index)
        {
            CopyInt64(value, buffer, index);
            index += 8;
        }

        /// <summary>
        /// Copies the double-precision floating point number to the buffer.
        /// </summary>
        public static void CopyDouble(double value, byte[] buffer, int index)
        {
            CopyInt64(BitConverter.DoubleToInt64Bits(value), buffer, index);
        }

        /// <summary>
        /// Copies the double-precision floating point number to the buffer.
        /// </summary>
        public static void CopyDouble(double value, byte[] buffer, ref int index)
        {
            CopyInt64(BitConverter.DoubleToInt64Bits(value), buffer, index);
            index += 8;
        }

        /// <summary>
        /// Encodes and copies the string to the buffer.
        /// </summary>
        public static void CopyString(string s, byte[] buffer, ref int index)
        {
            int stringLength = s == null ? 0 : s.Length;

            if (stringLength == 0)
            {
                buffer[index++] = 0;
                buffer[index++] = 0;
            }
            else
            {
                int dataLength = Encoding.UTF8.GetBytes(s, 0, s.Length, buffer, index + 2);

                if (dataLength > ushort.MaxValue)
                    throw new ArgumentException("String length exceeded.");

                CopyUInt16((ushort)dataLength, buffer, ref index);
                index += dataLength;
            }
        }

        /// <summary>
        /// Copies the date and time to the buffer.
        /// </summary>
        public static void CopyTime(DateTime dateTime, byte[] buffer, int index)
        {
            CopyInt64(dateTime.Ticks, buffer, index);
        }

        /// <summary>
        /// Copies the date and time to the buffer.
        /// </summary>
        public static void CopyTime(DateTime dateTime, byte[] buffer, ref int index)
        {
            CopyInt64(dateTime.Ticks, buffer, index);
            index += 8;
        }

        /// <summary>
        /// Copies the channel data to the buffer.
        /// </summary>
        public static void CopyCnlData(CnlData cnlData, byte[] buffer, ref int index)
        {
            CopyDouble(cnlData.Val, buffer, ref index);
            CopyUInt16((ushort)cnlData.Stat, buffer, ref index);
        }

        /// <summary>
        /// Copies the time range to the buffer.
        /// </summary>
        public static void CopyTimeRange(TimeRange timeRange, byte[] buffer, ref int index)
        {
            CopyTime(timeRange.StartTime, buffer, ref index);
            CopyTime(timeRange.EndTime, buffer, ref index);
            CopyBool(timeRange.EndInclusive, buffer, ref index);
        }

        /// <summary>
        /// Copies the file name to the buffer.
        /// </summary>
        public static void CopyFileName(int directoryID, string path, byte[] buffer, ref int index)
        {
            CopyInt32(directoryID, buffer, ref index);
            CopyString(path, buffer, ref index);
        }

        /// <summary>
        /// Copies the array of bytes to the buffer.
        /// </summary>
        public static void CopyByteArray(byte[] srcArray, byte[] buffer, ref int index)
        {
            if (srcArray == null)
            {
                CopyInt32(0, buffer, ref index);
            }
            else
            {
                CopyInt32(srcArray.Length, buffer, ref index);
                Buffer.BlockCopy(srcArray, 0, buffer, index, srcArray.Length);
                index += srcArray.Length;
            }
        }

        /// <summary>
        /// Copies the array of integers to the buffer.
        /// </summary>
        public static void CopyIntArray(int[] srcArray, byte[] buffer, ref int index)
        {
            if (srcArray == null)
            {
                CopyInt32(0, buffer, ref index);
            }
            else
            {
                CopyInt32(srcArray.Length, buffer, ref index);
                int dataLength = srcArray.Length * 4;
                Buffer.BlockCopy(srcArray, 0, buffer, index, dataLength);
                index += dataLength;
            }
        }

        /// <summary>
        /// Copies the collection of integers to the buffer.
        /// </summary>
        public static void CopyIntCollection(ICollection<int> values, byte[] buffer, ref int index)
        {
            if (values == null)
            {
                CopyInt32(0, buffer, ref index);
            }
            else
            {
                CopyInt32(values.Count, buffer, ref index);

                foreach (int value in values)
                {
                    CopyInt32(value, buffer, ref index);
                }
            }
        }

        /// <summary>
        /// Copies the collection of double-precision floating point numbers to the buffer.
        /// </summary>
        public static void CopyDoubleCollection(ICollection<double> values, byte[] buffer, ref int index)
        {
            if (values == null)
            {
                CopyInt32(0, buffer, ref index);
            }
            else
            {
                CopyInt32(values.Count, buffer, ref index);

                foreach (double value in values)
                {
                    CopyDouble(value, buffer, ref index);
                }
            }
        }

        /// <summary>
        /// Copies the channel data array to the buffer.
        /// </summary>
        public static void CopyCnlDataArray(CnlData[] srcArray, byte[] buffer, ref int index)
        {
            if (srcArray == null)
            {
                CopyInt32(0, buffer, ref index);
            }
            else
            {
                CopyInt32(srcArray.Length, buffer, ref index);

                foreach (CnlData cnlData in srcArray)
                {
                    CopyCnlData(cnlData, buffer, ref index);
                }
            }
        }

        /// <summary>
        /// Copies the slice to the buffer.
        /// </summary>
        public static void CopySlice(Slice slice, byte[] buffer, ref int index)
        {
            if (slice == null)
            {
                CopyTime(DateTime.MinValue, buffer, ref index);
                CopyInt32(0, buffer, ref index);
                CopyInt32(0, buffer, ref index);
            }
            else
            {
                CopyTime(slice.Timestamp, buffer, ref index);
                CopyIntArray(slice.CnlNums, buffer, ref index);

                foreach (CnlData cnlData in slice.CnlData)
                {
                    CopyCnlData(cnlData, buffer, ref index);
                }

                CopyInt32(slice.DeviceNum, buffer, ref index);
            }
        }

        /// <summary>
        /// Copies the event to the buffer.
        /// </summary>
        public static void CopyEvent(Event ev, byte[] buffer, ref int index)
        {
            if (ev != null)
            {
                CopyInt64(ev.EventID, buffer, ref index);
                CopyTime(ev.Timestamp, buffer, ref index);
                CopyBool(ev.Hidden, buffer, ref index);
                CopyInt32(ev.CnlNum, buffer, ref index);
                CopyInt32(ev.ObjNum, buffer, ref index);
                CopyInt32(ev.DeviceNum, buffer, ref index);
                CopyDouble(ev.PrevCnlVal, buffer, ref index);
                CopyUInt16((ushort)ev.PrevCnlStat, buffer, ref index);
                CopyDouble(ev.CnlVal, buffer, ref index);
                CopyUInt16((ushort)ev.CnlStat, buffer, ref index);
                CopyUInt16((ushort)ev.Severity, buffer, ref index);
                CopyBool(ev.AckRequired, buffer, ref index);
                CopyBool(ev.Ack, buffer, ref index);
                CopyTime(ev.AckTimestamp, buffer, ref index);
                CopyInt32(ev.AckUserID, buffer, ref index);
                CopyByte((byte)ev.TextFormat, buffer, ref index);
                CopyString(ev.Text, buffer, ref index);
                CopyByteArray(ev.Data, buffer, ref index);
            }
        }

        /// <summary>
        /// Copies the data filter to the buffer.
        /// </summary>
        public static void CopyDataFilter(DataFilter filter, byte[] buffer, ref int index)
        {
            if (filter == null)
            {
                CopyInt32(0, buffer, ref index);    // limit
                CopyInt32(0, buffer, ref index);    // offset
                CopyBool(false, buffer, ref index); // origing from begin
                CopyBool(false, buffer, ref index); // require all
                CopyInt32(0, buffer, ref index);    // conditions count
            }
            else
            {
                CopyInt32(filter.Limit, buffer, ref index);
                CopyInt32(filter.Offset, buffer, ref index);
                CopyBool(filter.OriginBegin, buffer, ref index);
                CopyBool(filter.RequireAll, buffer, ref index);
                CopyInt32(filter.Conditions.Count, buffer, ref index);

                foreach (FilterCondition condition in filter.Conditions)
                {
                    CopyString(condition.ColumnName, buffer, ref index);
                    CopyByte((byte)condition.Operator, buffer, ref index);
                    CopyByte((byte)condition.DataType, buffer, ref index);

                    switch (condition.DataType)
                    {
                        case ColumnDataType.Integer:
                        case ColumnDataType.Boolean:
                            if (condition.Argument is ICollection<int> intCollection)
                            {
                                CopyIntCollection(intCollection, buffer, ref index);
                            }
                            else if (condition.Argument is int intVal)
                            {
                                CopyInt32(1, buffer, ref index);
                                CopyInt32(intVal, buffer, ref index);
                            }
                            break;

                        case ColumnDataType.Double:
                            if (condition.Argument is ICollection<double> doubleCollection)
                            {
                                CopyDoubleCollection(doubleCollection, buffer, ref index);
                            }
                            else if (condition.Argument is double doubleVal)
                            {
                                CopyInt32(1, buffer, ref index);
                                CopyDouble(doubleVal, buffer, ref index);
                            }
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Gets a 8-bit unsigned integer from the buffer.
        /// </summary>
        public static byte GetByte(byte[] buffer, ref int index)
        {
            return buffer[index++];
        }

        /// <summary>
        /// Gets a boolean value from the buffer.
        /// </summary>
        public static bool GetBool(byte[] buffer, ref int index)
        {
            return buffer[index++] > 0;
        }

        /// <summary>
        /// Gets a 16-bit unsigned integer from the buffer.
        /// </summary>
        public static ushort GetUInt16(byte[] buffer, ref int index)
        {
            ushort value = BitConverter.ToUInt16(buffer, index);
            index += 2;
            return value;
        }

        /// <summary>
        /// Gets a 32-bit signed integer from the buffer.
        /// </summary>
        public static int GetInt32(byte[] buffer, ref int index)
        {
            int value = BitConverter.ToInt32(buffer, index);
            index += 4;
            return value;
        }

        /// <summary>
        /// Gets a 64-bit signed integer from the buffer.
        /// </summary>
        public static long GetInt64(byte[] buffer, ref int index)
        {
            long value = BitConverter.ToInt64(buffer, index);
            index += 8;
            return value;
        }

        /// <summary>
        /// Gets a double-precision floating point number from the buffer.
        /// </summary>
        public static double GetDouble(byte[] buffer, ref int index)
        {
            double value = BitConverter.ToDouble(buffer, index);
            index += 8;
            return value;
        }

        /// <summary>
        /// Gets a string from the buffer.
        /// </summary>
        public static string GetString(byte[] buffer, int index)
        {
            int dataLength = BitConverter.ToUInt16(buffer, index);
            return dataLength > 0 ? Encoding.UTF8.GetString(buffer, index + 2, dataLength) : "";
        }

        /// <summary>
        /// Gets a string from the buffer.
        /// </summary>
        public static string GetString(byte[] buffer, ref int index)
        {
            int dataLength = GetUInt16(buffer, ref index);
            string s = dataLength > 0 ? Encoding.UTF8.GetString(buffer, index, dataLength) : "";
            index += dataLength;
            return s;
        }

        /// <summary>
        /// Gets date and time from the buffer.
        /// </summary>
        public static DateTime GetTime(byte[] buffer, int index)
        {
            return ScadaUtils.TicksToTime(BitConverter.ToInt64(buffer, index));
        }

        /// <summary>
        /// Gets date and time from the buffer.
        /// </summary>
        public static DateTime GetTime(byte[] buffer, ref int index)
        {
            DateTime value = ScadaUtils.TicksToTime(BitConverter.ToInt64(buffer, index));
            index += 8;
            return value;
        }

        /// <summary>
        /// Gets an channel data from the buffer.
        /// </summary>
        public static CnlData GetCnlData(byte[] buffer, ref int index)
        {
            return new CnlData(
                GetDouble(buffer, ref index),
                GetUInt16(buffer, ref index));
        }

        /// <summary>
        /// Gets an time range from the buffer.
        /// </summary>
        public static TimeRange GetTimeRange(byte[] buffer, ref int index)
        {
            return new TimeRange(
                GetTime(buffer, ref index),
                GetTime(buffer, ref index),
                GetBool(buffer, ref index));
        }

        /// <summary>
        /// Gets an array of bytes from the buffer.
        /// </summary>
        public static byte[] GetByteArray(byte[] buffer, ref int index)
        {
            int arrayLength = GetInt32(buffer, ref index);

            if (arrayLength > 0)
            {
                byte[] array = new byte[arrayLength];
                Buffer.BlockCopy(buffer, index, array, 0, arrayLength);
                index += arrayLength;
                return array;
            }
            else
            {
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// Gets an array of integers from the buffer.
        /// </summary>
        public static int[] GetIntArray(byte[] buffer, ref int index)
        {
            int arrayLength = GetInt32(buffer, ref index);

            if (arrayLength > 0)
            {
                int[] array = new int[arrayLength];
                int dataLength = arrayLength * 4;
                Buffer.BlockCopy(buffer, index, array, 0, dataLength);
                index += dataLength;
                return array;
            }
            else
            {
                return Array.Empty<int>();
            }
        }

        /// <summary>
        /// Gets an array of double-precision floating point numbers from the buffer.
        /// </summary>
        public static double[] GetDoubleArray(byte[] buffer, ref int index)
        {
            int arrayLength = GetInt32(buffer, ref index);

            if (arrayLength > 0)
            {
                double[] array = new double[arrayLength];
                int dataLength = arrayLength * 8;
                Buffer.BlockCopy(buffer, index, array, 0, dataLength);
                index += dataLength;
                return array;
            }
            else
            {
                return Array.Empty<double>();
            }
        }

        /// <summary>
        /// Gets a channel data array from the buffer.
        /// </summary>
        public static CnlData[] GetCnlDataArray(byte[] buffer, ref int index)
        {
            int arrayLength = GetInt32(buffer, ref index);

            if (arrayLength > 0)
            {
                CnlData[] array = new CnlData[arrayLength];

                for (int i = 0; i < arrayLength; i++)
                {
                    array[i] = new CnlData(
                        BitConverter.ToDouble(buffer, index),
                        BitConverter.ToUInt16(buffer, index + 8));
                    index += 10;
                }

                return array;
            }
            else
            {
                return Array.Empty<CnlData>();
            }
        }

        /// <summary>
        /// Gets a slice from the buffer.
        /// </summary>
        public static Slice GetSlice(byte[] buffer, ref int index)
        {
            Slice slice = new Slice(
                GetTime(buffer, ref index),
                GetIntArray(buffer, ref index));

            for (int i = 0, len = slice.Length; i < len; i++)
            {
                slice.CnlData[i] = GetCnlData(buffer, ref index);
            }

            slice.DeviceNum = GetInt32(buffer, ref index);
            return slice;
        }

        /// <summary>
        /// Gets an event from the buffer.
        /// </summary>
        public static Event GetEvent(byte[] buffer, ref int index)
        {
            return new Event
            {
                EventID = GetInt64(buffer, ref index),
                Timestamp = GetTime(buffer, ref index),
                Hidden = GetBool(buffer, ref index),
                CnlNum = GetInt32(buffer, ref index),
                ObjNum = GetInt32(buffer, ref index),
                DeviceNum = GetInt32(buffer, ref index),
                PrevCnlVal = GetDouble(buffer, ref index),
                PrevCnlStat = GetUInt16(buffer, ref index),
                CnlVal = GetDouble(buffer, ref index),
                CnlStat = GetUInt16(buffer, ref index),
                Severity = GetUInt16(buffer, ref index),
                AckRequired = GetBool(buffer, ref index),
                Ack = GetBool(buffer, ref index),
                AckTimestamp = GetTime(buffer, ref index),
                AckUserID = GetInt32(buffer, ref index),
                TextFormat = (EventTextFormat)GetByte(buffer, ref index),
                Text = GetString(buffer, ref index),
                Data = GetByteArray(buffer, ref index)
            };
        }

        /// <summary>
        /// Gets a data filter from the buffer.
        /// </summary>
        public static DataFilter GetDataFilter(Type itemType, byte[] buffer, ref int index)
        {
            DataFilter filter = new DataFilter(itemType)
            {
                Limit = GetInt32(buffer, ref index),
                Offset = GetInt32(buffer, ref index),
                OriginBegin = GetBool(buffer, ref index),
                RequireAll = GetBool(buffer, ref index)
            };

            int conditionCount = GetInt32(buffer, ref index);

            for (int i = 0; i < conditionCount; i++)
            {
                string columnName = GetString(buffer, ref index);
                FilterOperator filterOperator = (FilterOperator)GetByte(buffer, ref index);
                ColumnDataType dataType = (ColumnDataType)GetByte(buffer, ref index);
                IList args = null;

                switch (dataType)
                {
                    case ColumnDataType.Integer:
                    case ColumnDataType.Boolean:
                        args = GetIntArray(buffer, ref index);
                        break;
                    case ColumnDataType.Double:
                        args = GetDoubleArray(buffer, ref index);
                        break;
                }

                filter.AddCondition(columnName, filterOperator, args);
            }

            return filter;
        }
    }
}
