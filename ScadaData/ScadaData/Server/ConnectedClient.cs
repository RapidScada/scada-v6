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
        /// Disconnects the client.
        /// </summary>
        public void Disconnect()
        {
            NetStream.Close();
            TcpClient.Close();
        }
    }
}
