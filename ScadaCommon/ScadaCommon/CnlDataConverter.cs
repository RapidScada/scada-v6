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
 * Summary  : Converts channel data to base data types
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Const;
using Scada.Data.Models;
using System;
using System.Text;

namespace Scada
{
    /// <summary>
    /// Converts channel data to base data types.
    /// <para>Преобразует данные каналов в базовые типы данных.</para>
    /// </summary>
    public static class CnlDataConverter
    {
        /// <summary>
        /// Gets a double-precision floating point number from the buffer.
        /// </summary>
        public static double GetDouble(CnlData[] buffer, int index)
        {
            CnlData cnlData = buffer[index];
            return cnlData.IsDefined ? cnlData.Val : 0.0;
        }

        /// <summary>
        /// Gets a 64-bit signed integer from the buffer.
        /// </summary>
        public static long GetInt64(CnlData[] buffer, int index)
        {
            CnlData cnlData = buffer[index];
            return cnlData.IsDefined ? BitConverter.DoubleToInt64Bits(cnlData.Val) : 0;
        }

        /// <summary>
        /// Gets an array of double-precision floating point numbers from the buffer.
        /// </summary>
        public static double[] GetDoubleArray(CnlData[] buffer, int index, int count)
        {
            double[] array = new double[count];

            for (int i = 0, j = index; i < count; i++, j++)
            {
                CnlData cnlData = buffer[j];
                array[i] = cnlData.IsDefined ? cnlData.Val : 0.0;
            }

            return array;
        }

        /// <summary>
        /// Gets an array of 64-bit signed integers from the buffer.
        /// </summary>
        public static long[] GetInt64Array(CnlData[] buffer, int index, int count)
        {
            long[] array = new long[count];

            for (int i = 0, j = index; i < count; i++, j++)
            {
                CnlData cnlData = buffer[j];
                array[i] = cnlData.IsDefined ? BitConverter.DoubleToInt64Bits(cnlData.Val) : 0;
            }

            return array;
        }

        /// <summary>
        /// Gets an array of bytes from the buffer.
        /// </summary>
        public static byte[] GetByteArray(CnlData[] buffer, int index, int count)
        {
            if (count > 1)
            {
                double[] doubleArray = GetDoubleArray(buffer, index, count);
                int dataLength = doubleArray.Length * 8;
                byte[] byteArray = new byte[dataLength];
                Buffer.BlockCopy(doubleArray, 0, byteArray, 0, dataLength);
                return byteArray;
            }
            else
            {
                return BitConverter.GetBytes(GetDouble(buffer, index));
            }
        }

        /// <summary>
        /// Gets an ASCII string from the buffer.
        /// </summary>
        public static string GetAscii(CnlData[] buffer, int index, int count)
        {
            byte[] array = GetByteArray(buffer, index, count);
            return Encoding.ASCII.GetString(array).TrimEnd((char)0);
        }

        /// <summary>
        /// Gets a Unicode string from the buffer.
        /// </summary>
        public static string GetUnicode(CnlData[] buffer, int index, int count)
        {
            byte[] array = GetByteArray(buffer, index, count);
            return Encoding.Unicode.GetString(array).TrimEnd((char)0);
        }

        /// <summary>
        /// Gets the aggregated channel status.
        /// </summary>
        /// <remarks>Returns the Undefined status if at least one item has the Undefined status. 
        /// Otherwise, returns the status of the first item.</remarks>
        public static int GetStatus(CnlData[] buffer, int index, int count)
        {
            if (count > 1)
            {
                int status = CnlStatusID.Undefined;

                for (int i = 0; i < count; i++)
                {
                    CnlData cnlData = buffer[index + i];

                    if (cnlData.Stat == CnlStatusID.Undefined)
                        return CnlStatusID.Undefined;
                    else if (status == CnlStatusID.Undefined)
                        status = cnlData.Stat;
                }

                return status;
            }
            else
            {
                return buffer[index].Stat;
            }
        }


        /// <summary>
        /// Gets a 64-bit signed integer from the channel value.
        /// </summary>
        public static long DoubleToInt64(double cnlVal)
        {
            return BitConverter.DoubleToInt64Bits(cnlVal);
        }

        /// <summary>
        /// Gets an ASCII string from the channel value.
        /// </summary>
        public static string DoubleToAscii(double cnlVal)
        {
            return Encoding.ASCII.GetString(BitConverter.GetBytes(cnlVal)).TrimEnd((char)0);
        }

        /// <summary>
        /// Gets a Unicode string from the channel value.
        /// </summary>
        public static string DoubleToUnicode(double cnlVal)
        {
            return Encoding.Unicode.GetString(BitConverter.GetBytes(cnlVal)).TrimEnd((char)0);
        }
    }
}
