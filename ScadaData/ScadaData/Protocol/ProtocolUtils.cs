/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : Provides constants and helper methods for the application protocol
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using System;
using System.Net.Sockets;
using System.Text;

namespace Scada.Protocol
{
    /// <summary>
    /// Provides constants and helper methods for the application protocol.
    /// <para>Предоставляет константы и вспомогательные методы для протокола приложения.</para>
    /// </summary>
    public static class ProtocolUtils
    {
        /// <summary>
        /// The input and output buffer length, 1 MB.
        /// </summary>
        public const int BufferLenght = 1048576;
        /// <summary>
        /// The packet header length.
        /// </summary>
        public const int HeaderLength = 14;
        /// <summary>
        /// The index of the function arguments in the data packet.
        /// </summary>
        public const int ArgumentIndex = 16;


        /// <summary>
        /// Creates an initialization vector based on the session ID.
        /// </summary>
        private static byte[] CreateIV(long sessionID)
        {
            byte[] iv = new byte[ScadaUtils.IVSize];
            byte[] sessBuf = BitConverter.GetBytes(sessionID);
            int sessBufLen = sessBuf.Length;

            for (int i = 0; i < ScadaUtils.IVSize; i++)
            {
                iv[i] = sessBuf[i % sessBufLen];
            }

            return iv;
        }

        /// <summary>
        /// Copies the 8-bit unsigned integer to the buffer.
        /// </summary>
        public static void CopyByte(byte value, byte[] buffer, int startIndex, out int endIndex)
        {
            buffer[startIndex] = value;
            endIndex = startIndex + 1;
        }

        /// <summary>
        /// Copies the boolean value to the buffer.
        /// </summary>
        public static void CopyBool(bool value, byte[] buffer, int startIndex, out int endIndex)
        {
            buffer[startIndex] = (byte)(value ? 1 : 0);
            endIndex = startIndex + 1;
        }

        /// <summary>
        /// Copies the 16-bit unsigned integer to the buffer.
        /// </summary>
        public static void CopyUInt16(ushort value, byte[] buffer, int startIndex, out int endIndex)
        {
            buffer[startIndex++] = (byte)(value >> 8);
            buffer[startIndex++] = (byte)value;
            endIndex = startIndex;
        }

        /// <summary>
        /// Copies the 32-bit signed integer to the buffer.
        /// </summary>
        public static void CopyInt32(int value, byte[] buffer, int startIndex, out int endIndex)
        {
            buffer[startIndex++] = (byte)(value >> 24);
            buffer[startIndex++] = (byte)(value >> 16);
            buffer[startIndex++] = (byte)(value >> 8);
            buffer[startIndex++] = (byte)value;
            endIndex = startIndex;
        }

        /// <summary>
        /// Copies the 64-bit signed integer to the buffer.
        /// </summary>
        public static void CopyInt64(long value, byte[] buffer, int startIndex, out int endIndex)
        {
            buffer[startIndex++] = (byte)(value >> 56);
            buffer[startIndex++] = (byte)(value >> 48);
            buffer[startIndex++] = (byte)(value >> 40);
            buffer[startIndex++] = (byte)(value >> 32);
            buffer[startIndex++] = (byte)(value >> 24);
            buffer[startIndex++] = (byte)(value >> 16);
            buffer[startIndex++] = (byte)(value >> 8);
            buffer[startIndex++] = (byte)value;
            endIndex = startIndex;
        }

        /// <summary>
        /// Copies the double-precision floating point value to the buffer.
        /// </summary>
        public static void CopyDouble(double value, byte[] buffer, int startIndex, out int endIndex)
        {
            CopyInt64(BitConverter.DoubleToInt64Bits(value), buffer, startIndex, out endIndex);
        }

        /// <summary>
        /// Encodes and copies the string to the buffer.
        /// </summary>
        public static void CopyString(string s, byte[] buffer, int startIndex, out int endIndex)
        {
            int stringLength = s == null ? 0 : s.Length;

            if (stringLength == 0)
            {
                buffer[startIndex] = 0;
                buffer[startIndex + 1] = 0;
                endIndex = startIndex + 2;
            }
            else
            {
                byte[] stringData = Encoding.UTF8.GetBytes(s);
                int dataLength = stringData.Length;

                if (dataLength > ushort.MaxValue)
                    throw new ScadaException("String length exceeded.");

                CopyUInt16((ushort)dataLength, buffer, startIndex, out startIndex);
                stringData.CopyTo(buffer, startIndex);
                endIndex = startIndex + dataLength;
            }
        }

        /// <summary>
        /// Gets the required size in a buffer to store the string.
        /// </summary>
        public static int MeasureString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 2;
            }
            else
            {
                int dataLength = Encoding.UTF8.GetBytes(s).Length;

                if (dataLength > ushort.MaxValue)
                    throw new ScadaException("String length exceeded.");

                return dataLength + 2;
            }
        }

        /// <summary>
        /// Gets a string from the buffer.
        /// </summary>
        public static string GetString(byte[] buffer, int startIndex, out int endIndex)
        {
            int dataLength = BitConverter.ToUInt16(buffer, startIndex);
            startIndex += 2;
            endIndex = startIndex + dataLength;
            return dataLength > 0 ? Encoding.UTF8.GetString(buffer, startIndex, dataLength) : "";
        }

        /// <summary>
        /// Encodes and copies the date and time to the buffer.
        /// </summary>
        public static void CopyTime(DateTime dateTime, byte[] buffer, int startIndex, out int endIndex)
        {
            CopyInt64(dateTime.Ticks, buffer, startIndex, out endIndex);
        }

        /// <summary>
        /// Gets date and time from the buffer.
        /// </summary>
        public static DateTime GetTime(byte[] buffer, int startIndex, out int endIndex)
        {
            endIndex = startIndex + 8;
            return new DateTime(BitConverter.ToInt64(buffer, startIndex), DateTimeKind.Utc);
        }

        /// <summary>
        /// Encodes and copies the file name to the buffer.
        /// </summary>
        public static void CopyFileName(ushort directoryID, string path, 
            byte[] buffer, int startIndex, out int endIndex)
        {
            CopyUInt16(directoryID, buffer, startIndex, out startIndex);
            CopyString(path, buffer, startIndex, out endIndex);
        }

        /// <summary>
        /// Encodes and copies the array of integers to the buffer.
        /// </summary>
        public static void CopyIntArray(int[] srcArray, byte[] buffer, int startIndex, out int endIndex)
        {
            int arrayLength = srcArray == null ? 0 : srcArray.Length;
            CopyInt32(arrayLength, buffer, startIndex, out startIndex);

            if (srcArray != null)
                Buffer.BlockCopy(srcArray, 0, buffer, startIndex, arrayLength);

            endIndex = startIndex + arrayLength * 4 ;
        }

        /// <summary>
        /// Gets an array of integers from the buffer.
        /// </summary>
        public static int[] GetIntArray(byte[] buffer, int startIndex, out int endIndex)
        {
            int arrayLength = BitConverter.ToInt32(buffer, startIndex);
            int dataLength = arrayLength * 4;
            startIndex += 4;
            endIndex = startIndex + dataLength;

            if (arrayLength > 0)
            {
                int[] array = new int[arrayLength];
                Buffer.BlockCopy(buffer, startIndex, array, 0, dataLength);
                return array;
            }
            else
            {
                return new int[0];
            }
        }

        /// <summary>
        /// Encodes and copies the channel data array to the buffer.
        /// </summary>
        public static void CopyCnlDataArray(CnlData[] srcArray, byte[] buffer, int startIndex, out int endIndex)
        {
            int arrayLength = srcArray == null ? 0 : srcArray.Length;
            CopyInt32(arrayLength, buffer, startIndex, out startIndex);

            if (srcArray != null)
            {
                foreach (CnlData cnlData in srcArray)
                {
                    CopyDouble(cnlData.Val, buffer, startIndex, out startIndex);
                    CopyInt32(cnlData.Stat, buffer, startIndex, out startIndex);
                }
            }

            endIndex = startIndex;
        }

        /// <summary>
        /// Gets a channel data array from the buffer.
        /// </summary>
        public static CnlData[] GetCnlDataArray(byte[] buffer, int startIndex, out int endIndex)
        {
            int arrayLength = BitConverter.ToInt32(buffer, startIndex);
            endIndex = startIndex + 4;

            if (arrayLength > 0)
            {
                CnlData[] array = new CnlData[arrayLength];

                for (int i = 0; i < arrayLength; i++)
                {
                    array[i] = new CnlData(
                        BitConverter.ToDouble(buffer, endIndex),
                        BitConverter.ToInt32(buffer, endIndex + 8));
                    endIndex += 12;
                }

                return array;
            }
            else
            {
                return new CnlData[0];
            }
        }

        /// <summary>
        /// Encrypts the password.
        /// </summary>
        public static string EncryptPassword(string password, long sessionID, byte[] secretKey)
        {
            return secretKey == null ?
                password :
                ScadaUtils.Encrypt(password, secretKey, CreateIV(sessionID));
        }

        /// <summary>
        /// Decrypts the password.
        /// </summary>
        public static string DecryptPassword(string encryptedPassword, long sessionID, byte[] secretKey)
        {
            return secretKey == null ?
                encryptedPassword :
                ScadaUtils.Decrypt(encryptedPassword, secretKey, CreateIV(sessionID));
        }

        /// <summary>
        /// Reads a large amount of data into the buffer.
        /// </summary>
        public static int ReadData(NetworkStream netStream, int timeout, byte[] buffer, int offset, int size)
        {
            if (size > 0)
            {
                int bytesRead = 0;
                DateTime startDT = DateTime.UtcNow;

                do
                {
                    bytesRead += netStream.Read(buffer, bytesRead + offset, size - bytesRead);
                } while (bytesRead < size && (DateTime.UtcNow - startDT).TotalMilliseconds <= timeout);

                return bytesRead;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Clears the stream by receiving available data.
        /// </summary>
        public static void ClearNetStream(NetworkStream netStream, byte[] buffer)
        {
            if (netStream.DataAvailable)
                netStream.Read(buffer, 0, buffer.Length);
        }
    }
}
