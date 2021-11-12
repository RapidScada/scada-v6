﻿/*
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
        /// <summary>
        /// The period for sending heartbeat to remote Agents.
        /// </summary>
        private static readonly TimeSpan HeartbeatPeriod = TimeSpan.FromSeconds(30);

        private readonly CoreLogic coreLogic;                               // the Agent logic instance
        private readonly ReverseConnectionOptions reverseConnectionOptions; // the reverse connection options
        
        private ReverseClient reverseClient;           // the client that connects to the main Agent
        private Thread reverseClientThread;            // the reverse client thread
        private volatile bool reverseClientTerminated; // requires to stop the reverse client thread
        private DateTime heartbeatDT;                  // the last time a heartbeat was sent to remote Agents


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
            heartbeatDT = DateTime.MinValue;
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
                            if (CreateSession(reverseClient.SessionID, out ConnectedClient client))
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
        /// Creates a new session with the predefined ID.
        /// </summary>
        private bool CreateSession(long sessionID, out ConnectedClient client)
        {
            bool sessionOK;

            if (clients.Count < MaxSessionCount)
            {
                client = new ConnectedClient();
                sessionOK = clients.TryAdd(sessionID, client);
            }
            else
            {
                client = null;
                sessionOK = false;
            }

            if (sessionOK)
            {
                log.WriteAction(Locale.IsRussian ?
                    "Создана сессия с ид. {0}" :
                    "Session with ID {0} created", sessionID);
                client.SessionID = sessionID;
                return true;
            }
            else
            {
                log.WriteError(Locale.IsRussian ?
                    "Не удалось создать сессию" :
                    "Unable to create session");
                client = null;
                return false;
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
        /// Gets the instance associated with the client.
        /// </summary>
        private bool GetClientInstance(ConnectedClient client, out ScadaInstance scadaInstance)
        {
            scadaInstance = GetClientTag(client).Instance;
            return scadaInstance != null;
        }

        /// <summary>
        /// Sends hearbeat requests to remote Agents to keep them connected.
        /// </summary>
        private void SendHearbeat()
        {
            foreach (ConnectedClient client in clients.Values)
            {
                if (client.RoleID == AgentRoleID.Agent &&
                    client.Tag is ClientTag clientTag && !clientTag.IsReverse)
                {
                    DataPacket request = new DataPacket
                    {
                        TransactionID = 0,
                        DataLength = 10,
                        SessionID = client.SessionID,
                        FunctionID = FunctionID.AgentHeartbeat,
                        Buffer = client.OutBuf
                    };

                    try
                    {
                        request.Encode();
                        client.NetStream.Write(request.Buffer, 0, request.BufferLength);
                        client.RegisterActivity();
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(Locale.IsRussian ?
                            "Ошибка при отправке пульса {0}: {1}" :
                            "Error sending heartbeat to {0}: {1}", client.Address, ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current status of the specified service.
        /// </summary>
        private void GetServiceStatus(ConnectedClient client, ScadaInstance instance, DataPacket request,
            out ResponsePacket response)
        {
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
        private void ControlService(ConnectedClient client, ScadaInstance instance, DataPacket request, 
            out ResponsePacket response)
        {
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
        /// Performs actions on a new iteration of the work cycle.
        /// </summary>
        protected override void OnIteration()
        {
            // send heartbeat to Agents
            DateTime utcNow = DateTime.UtcNow;

            if (utcNow - heartbeatDT >= HeartbeatPeriod)
            {
                heartbeatDT = utcNow;
                SendHearbeat();
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
                errMsg = Locale.IsRussian ?
                    "Пользователь уже выполнил вход" :
                    "User is already logged in";
            }
            else if (!coreLogic.GetInstance(instance, out ScadaInstance scadaInstance))
            {
                errMsg = Locale.IsRussian ?
                    "Неизвестный экземпляр" :
                    "Unknown instance";
            }
            else if (scadaInstance.ValidateUser(username, password, out userID, out roleID, out errMsg))
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

            userID = 0;
            roleID = 0;
            log.WriteError(Locale.IsRussian ?
                "Ошибка аутентификации пользователя {0}: {1}" :
                "Authentication failed for user {0}: {1}", username, errMsg);
            return false;
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
            if (!GetClientInstance(client, out ScadaInstance instance))
            {
                response = new ResponsePacket(request, client.OutBuf);
                response.SetError(ErrorCode.InvalidOperation);
                handled = true;
            }
            else if (instance.ProxyMode)
            {
                if (client.RoleID == AgentRoleID.Administrator)
                {
                    // TODO: redirect request to remote Agent
                    response = new ResponsePacket(request, client.OutBuf);
                    response.SetError(ErrorCode.IllegalFunction);
                    handled = true;
                }
                else
                {
                    // TODO: redirect response to Administrator
                    response = null;
                    handled = true;
                }
            }
            else
            {
                response = null;
                handled = true;

                switch (request.FunctionID)
                {
                    case FunctionID.GetServiceStatus:
                        GetServiceStatus(client, instance, request, out response);
                        break;

                    case FunctionID.ControlService:
                        ControlService(client, instance, request, out response);
                        break;

                    case FunctionID.AgentHeartbeat:
                        // no response
                        break;

                    default:
                        handled = false;
                        break;
                }
            }
        }
    }
}
