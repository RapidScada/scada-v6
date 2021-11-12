/*
 * Copyright 2021 Rapid Software LLC
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
 * Summary  : Represents a TCP listener which waits for client connections
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Agent.Config;
using Scada.Client;
using Scada.Lang;
using Scada.Log;
using Scada.Protocol;
using Scada.Server;
using System;
using System.Threading;
using static Scada.BinaryConverter;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Represents a TCP listener which waits for client connections.
    /// <para>Представляет TCP-прослушиватель, который ожидает подключения клиентов.</para>
    /// </summary>
    internal class AgentListener : BaseListener
    {
        private readonly CoreLogic coreLogic;                               // the Agent logic instance
        private readonly ReverseConnectionOptions reverseConnectionOptions; // the reverse connection options
        
        private ReverseClient reverseClient;           // the client that connects to the main Agent
        private Thread reverseClientThread;            // the reverse client thread
        private volatile bool reverseClientTerminated; // requires to stop the reverse client thread


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AgentListener(CoreLogic coreLogic, ListenerOptions listenerOptions,
            ReverseConnectionOptions reverseConnectionOptions, ILog log) 
            : base(listenerOptions, log)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException(nameof(coreLogic));
            this.reverseConnectionOptions = reverseConnectionOptions ?? 
                throw new ArgumentNullException(nameof(reverseConnectionOptions));

            reverseClient = null;
            reverseClientThread = null;
            reverseClientTerminated = false;
        }


        /// <summary>
        /// Executes the reverse client loop.
        /// </summary>
        private void ReverseClientExecute()
        {
            reverseClient = new ReverseClient(reverseConnectionOptions);

            while (!reverseClientTerminated)
            {
                try
                {
                    // reconnect reverse client
                    if (reverseClient.ClientState != ClientState.LoggedIn && reverseClient.ConnectionAllowed)
                    {
                        if (reverseClient.RestoreConnection(out string errMsg))
                        {
                            if (CreateSession(out ConnectedClient client))
                            {
                                Thread clientThread = new Thread(ClientExecute);
                                client.Init(reverseClient.TcpClient, clientThread);
                                client.IsLoggedIn = true;
                                client.Username = reverseConnectionOptions.Username;
                                client.UserID = reverseClient.UserID;
                                client.RoleID = reverseClient.RoleID;

                                coreLogic.GetInstance(reverseConnectionOptions.Instance, out ScadaInstance instance);
                                client.Tag = new ClientTag(true) { Instance = instance };

                                log.WriteAction(Locale.IsRussian ?
                                    "Обратный клиент подключился к {0}" :
                                    "Reverse client connected to {0}", client.Address);
                                clientThread.Start(client);
                            }
                        }
                        else if (!string.IsNullOrEmpty(errMsg))
                        {
                            log.WriteError(errMsg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, Locale.IsRussian ?
                        "Ошибка в цикле обратного клиента" :
                        "Error in the reverse client loop");
                }
                finally
                {
                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }
        }

        /// <summary>
        /// Gets the client tag, or throws an exception if it is undefined.
        /// </summary>
        private ClientTag GetClientTag(ConnectedClient client)
        {
            return client.Tag as ClientTag ?? 
                throw new InvalidOperationException("Client tag must not be null.");
        }

        /// <summary>
        /// Gets the instance associated with the client, or throws an exception if it is undefined.
        /// </summary>
        private ScadaInstance GetClientInstance(ConnectedClient client)
        {
            return GetClientTag(client).Instance ?? 
                throw new InvalidOperationException("Client instance must not be null.");
        }

        /// <summary>
        /// Gets the current status of the specified service.
        /// </summary>
        private void GetServiceStatus(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            ScadaInstance instance = GetClientInstance(client);
            ServiceApp serviceApp = (ServiceApp)request.Buffer[ArgumentIndex];
            bool result = instance.GetServiceStatus(serviceApp, out ServiceStatus serviceStatus);

            byte[] buffer = client.OutBuf;
            response = new ResponsePacket(request, buffer);
            int index = ArgumentIndex;
            CopyBool(result, buffer, ref index);
            CopyByte((byte)serviceStatus, buffer, ref index);
            response.BufferLength = index;
        }
        
        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        private void ControlService(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            ScadaInstance instance = GetClientInstance(client);
            ServiceApp serviceApp = (ServiceApp)request.Buffer[ArgumentIndex];
            ServiceCommand serviceCommand = (ServiceCommand)request.Buffer[ArgumentIndex + 1];
            bool result = instance.ControlService(serviceApp, serviceCommand);

            response = new ResponsePacket(request, client.OutBuf);
            int index = ArgumentIndex;
            CopyBool(result, client.OutBuf, ref index);
            response.BufferLength = index;
        }


        /// <summary>
        /// Gets the server name and version.
        /// </summary>
        protected override string GetServerName()
        {
            return Locale.IsRussian ?
                "Агент " + EngineUtils.AppVersion :
                "Agent " + EngineUtils.AppVersion;
        }

        /// <summary>
        /// Gets a value indicating whether the server is ready.
        /// </summary>
        protected override bool ServerIsReady()
        {
            return coreLogic.IsReady;
        }

        /// <summary>
        /// Performs actions when starting the listener.
        /// </summary>
        protected override void OnListenerStart()
        {
            // start reverse client thread
            if (reverseConnectionOptions.Enabled)
            {
                reverseClientThread = new Thread(ReverseClientExecute);
                reverseClientThread.Start();
            }
        }

        /// <summary>
        /// Performs actions when stopping the listener.
        /// </summary>
        protected override void OnListenerStop()
        {
            // stop reverse client thread
            if (reverseClientThread != null)
            {
                reverseClientTerminated = true;
                reverseClientThread.Join();
            }
        }

        /// <summary>
        /// Performs actions when initializing the connected client.
        /// </summary>
        protected override void OnClientInit(ConnectedClient client)
        {
            client.Tag = new ClientTag(false);
        }

        /// <summary>
        /// Performs actions when disconnecting the client.
        /// </summary>
        protected override void OnClientDisconnect(ConnectedClient client)
        {
            // disconnect inactive reverse client
            if (client.Tag is ClientTag clientTag && clientTag.IsReverse)
            {
                reverseClient.MarkAsDisconnected();
            }
        }

        /// <summary>
        /// Validates the username and password.
        /// </summary>
        protected override bool ValidateUser(ConnectedClient client, string username, string password, string instance,
            out int userID, out int roleID, out string errMsg)
        {
            if (client.IsLoggedIn)
            {
                userID = 0;
                roleID = 0;
                errMsg = Locale.IsRussian ?
                    "Пользователь уже выполнил вход" :
                    "User is already logged in";
                return false;
            }
            else if (coreLogic.GetInstance(instance, out ScadaInstance scadaInstance))
            {
                if (scadaInstance.ValidateUser(username, password, out userID, out roleID, out errMsg))
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Пользователь {0} успешно аутентифицирован" :
                        "User {0} is successfully authenticated", username);

                    client.IsLoggedIn = true;
                    client.Username = username;
                    client.UserID = userID;
                    client.RoleID = roleID;
                    GetClientTag(client).Instance = scadaInstance;
                    return true;
                }
                else
                {
                    log.WriteError(Locale.IsRussian ?
                        "Ошибка аутентификации пользователя {0}: {1}" :
                        "Authentication failed for user {0}: {1}", username, errMsg);
                    return false;
                }
            }
            else
            {
                userID = 0;
                roleID = 0;
                errMsg = Locale.IsRussian ?
                    "Неизвестный экземпляр" :
                    "Unknown instance";
                return false;
            }
        }

        /// <summary>
        /// Gets the role name of the connected client.
        /// </summary>
        protected override string GetRoleName(ConnectedClient client)
        {
            return client == null ? "" : EngineUtils.GetRoleName(client.RoleID, Locale.IsRussian);
        }

        /// <summary>
        /// Processes the incoming request.
        /// </summary>
        protected override void ProcessCustomRequest(ConnectedClient client, DataPacket request,
            out ResponsePacket response, out bool handled)
        {
            response = null;
            handled = true;

            switch (request.FunctionID)
            {
                case FunctionID.GetServiceStatus:
                    GetServiceStatus(client, request, out response);
                    break;

                case FunctionID.ControlService:
                    ControlService(client, request, out response);
                    break;

                default:
                    handled = false;
                    break;
            }
        }
    }
}
