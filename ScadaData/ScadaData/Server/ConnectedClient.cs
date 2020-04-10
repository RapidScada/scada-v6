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
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Scada.Server
{
    /// <summary>
    /// Represents a client connected to a server.
    /// <para>Представляет клиента, подключенного к серверу.</para>
    /// </summary>
    public class ConnectedClient
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConnectedClient(TcpClient tcpClient)
        {
            TcpClient = tcpClient ?? throw new ArgumentNullException("tcpClient");
        }

        /// <summary>
        /// Gets the client connection.
        /// </summary>
        public TcpClient TcpClient { get; protected set; }
    }
}
