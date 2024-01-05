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
 * Summary  : Provides constants and helper methods for the application protocol
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Lang;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Scada.Protocol
{
    /// <summary>
    /// Provides constants and helper methods for the application protocol.
    /// <para>Предоставляет константы и вспомогательные методы для протокола приложения.</para>
    /// </summary>
    public static class ProtocolUtils
    {
        /// <summary>
        /// Contains the major and minor protocol version.
        /// </summary>
        public const ushort ProtocolVersion = 0x0300;
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
        /// Encrypts the password.
        /// </summary>
        public static string EncryptPassword(string password, long sessionID, byte[] secretKey)
        {
            return secretKey == null 
                ? password 
                : ScadaUtils.Encrypt(password, secretKey, CreateIV(sessionID));
        }

        /// <summary>
        /// Decrypts the password.
        /// </summary>
        public static string DecryptPassword(string encryptedPassword, long sessionID, byte[] secretKey)
        {
            return secretKey == null
                ? encryptedPassword 
                : ScadaUtils.Decrypt(encryptedPassword, secretKey, CreateIV(sessionID));
        }

        /// <summary>
        /// Gets an encrypted server stamp.
        /// </summary>
        public static string GetServerStamp(long sessionID, byte[] secretKey)
        {
            byte[] bytes = secretKey == null
                ? BitConverter.GetBytes(sessionID)
                : ScadaUtils.EncryptBytes(BitConverter.GetBytes(sessionID), secretKey, CreateIV(sessionID));
            return ScadaUtils.ComputeHash(bytes);
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

        /// <summary>
        /// Throws an exception when block number in a data packet is not sequential.
        /// </summary>
        public static void ThrowBlockNumberException()
        {
            throw new ProtocolException(ErrorCode.IllegalFunctionArguments, Locale.IsRussian ?
                "Неверный номер блока." :
                "Invalid block number.");
        }

        /// <summary>
        /// Throws an exception when data size in a data packet does not match.
        /// </summary>
        public static void ThrowDataSizeException()
        {
            throw new ProtocolException(ErrorCode.IllegalFunctionArguments, Locale.IsRussian ?
                "Неверный размер данных." :
                "Invalid data size.");
        }
    }
}
