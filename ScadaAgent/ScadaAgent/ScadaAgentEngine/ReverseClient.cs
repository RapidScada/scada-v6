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
 * Module   : ScadaAgentEngine
 * Summary  : Represents a client for reverse Agent connection
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Client;
using Scada.Lang;
using System;
using System.Net.Sockets;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Represents a client for reverse Agent connection.
    /// <para>Представляет клиента для обратного подключения Агента.</para>
    /// </summary>
    internal class ReverseClient : ClientBase
    {
        /// <summary>
        /// The period when reconnection is allowed.
        /// </summary>
        private new readonly TimeSpan ReconnectPeriod = TimeSpan.FromSeconds(10);


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ReverseClient(ConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Gets the client connection.
        /// </summary>
        public TcpClient TcpClient => tcpClient;

        /// <summary>
        /// Gets a value indicating whether a connection attempt is allowed.
        /// </summary>
        public bool ConnectionAllowed
        {
            get
            {
                return DateTime.UtcNow - connAttemptDT > ReconnectPeriod;
            }
        }


        /// <summary>
        /// Restores a connection with the server.
        /// </summary>
        public bool RestoreConnection(out string errMsg)
        {
            try
            {
                RestoreConnection();
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при подключении обратного клиента" :
                    "Error connecting reverse client");
                return false;
            }
        }

        /// <summary>
        /// Sets the client state to disconnected.
        /// </summary>
        public void MarkAsDisconnected()
        {
            tcpClient = null;
            netStream = null;
            ClientState = ClientState.Disconnected;
            SessionID = 0;
            ServerName = "";
        }
    }
}
