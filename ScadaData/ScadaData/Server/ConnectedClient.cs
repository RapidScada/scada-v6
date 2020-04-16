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
 * Summary  : Represents a client connected to a server
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Scada.Server
{
    /// <summary>
    /// Represents a client connected to a server.
    /// <para>Представляет клиента, подключенного к серверу.</para>
    /// </summary>
    public sealed class ConnectedClient
    {
        /// <summary>
        /// The input buffer length, 1 MB.
        /// </summary>
        public const int InBufLenght = 1048576;
        /// <summary>
        /// The output buffer length, 1 MB.
        /// </summary>
        public const int OutBufLenght = 1048576;

        /// <summary>
        /// Necessary to stop the thread.
        /// </summary>
        public volatile bool Terminated;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConnectedClient(TcpClient tcpClient, Thread thread)
        {
            Terminated = false;
            TcpClient = tcpClient ?? throw new ArgumentNullException("tcpClient");
            NetStream = tcpClient.GetStream();
            Address = ((IPEndPoint)TcpClient.Client.RemoteEndPoint).Address.ToString();
            Thread = thread ?? throw new ArgumentNullException("thread");
            ActivityTime = DateTime.UtcNow;
            SessionID = 0;
            InBuf = new byte[InBufLenght];
            OutBuf = new byte[OutBufLenght];
        }


        /// <summary>
        /// Gets the client connection.
        /// </summary>
        public TcpClient TcpClient { get; private set; }

        /// <summary>
        /// Gets the stream for network access.
        /// </summary>
        public NetworkStream NetStream { get; private set; }

        /// <summary>
        /// Gets the IP address of the client.
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Gets the tread associated with the client.
        /// </summary>
        public Thread Thread { get; private set; }

        /// <summary>
        /// Gets or sets the time of last activity (UTC).
        /// </summary>
        public DateTime ActivityTime { get; set; }

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        public long SessionID { get; set; }

        /// <summary>
        /// Gets the input data buffer.
        /// </summary>
        public byte[] InBuf { get; private set; }

        /// <summary>
        /// Gets the output data buffer.
        /// </summary>
        public byte[] OutBuf { get; private set; }


        /// <summary>
        /// Disconnects the client.
        /// </summary>
        public void Disconnect()
        {
            NetStream.Close();
            TcpClient.Close();
        }

        /// <summary>
        /// Reads a large amount of data into the input buffer.
        /// </summary>
        public int ReadData(int offset, int size)
        {
            int bytesRead = 0;
            int timeout = TcpClient.ReceiveTimeout;
            DateTime startDT = DateTime.UtcNow;

            do
            {
                bytesRead += NetStream.Read(InBuf, bytesRead + offset, size - bytesRead);
            } while (bytesRead < size && (DateTime.UtcNow - startDT).TotalMilliseconds <= timeout);

            return bytesRead;
        }

        /// <summary>
        /// Gets the existing or creates a new input buffer.
        /// </summary>
        public byte[] GetInBuf(int length)
        {
            return length <= InBufLenght ? InBuf : new byte[length];
        }

        /// <summary>
        /// Gets the existing or creates a new output buffer.
        /// </summary>
        public byte[] GetOutBuf(int length)
        {
            return length <= OutBufLenght ? OutBuf : new byte[length];
        }
    }
}
