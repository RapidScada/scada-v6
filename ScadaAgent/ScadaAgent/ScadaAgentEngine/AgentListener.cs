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
 * Summary  : Represents a TCP listener which waits for client connections
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Agent.Config;
using Scada.Client;
using Scada.Lang;
using Scada.Log;
using Scada.Protocol;
using Scada.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using static Scada.BinaryConverter;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Represents a TCP listener which waits for client connections.
    /// <para>Представляет TCP-прослушиватель, который ожидает подключения клиентов.</para>
    /// </summary>
    internal class AgentListener : ListenerBase
    {
        /// <summary>
        /// Consolidates clients of a proxy instance.
        /// </summary>
        private class ClientBundle
        {
            public ConnectedClient AdminClient { get; set; }
            public ConnectedClient AgentClient { get; set; }
        }

        /// <summary>
        /// Represents a context of an upload operation.
        /// </summary>
        private class UploadContext
        {
            public string FileName { get; set; }
            public bool IsConfig { get; set; }
        }

        /// <summary>
        /// The period for sending heartbeat to remote Agents.
        /// </summary>
        private static readonly TimeSpan HeartbeatPeriod = TimeSpan.FromSeconds(30);

        private readonly CoreLogic coreLogic;                               // the Agent logic instance
        private readonly ReverseConnectionOptions reverseConnectionOptions; // the reverse connection options
        private readonly Dictionary<string, ClientBundle> clientBundles;    // the clients of proxy instances
        
        private ReverseClient reverseClient;           // the client that connects to the main Agent
        private Thread reverseClientThread;            // the reverse client thread
        private volatile bool reverseClientTerminated; // requires to stop the reverse client thread
        private DateTime heartbeatDT;                  // the last time a heartbeat was sent to remote Agents


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AgentListener(CoreLogic coreLogic, ListenerOptions listenerOptions, 
            ReverseConnectionOptions reverseConnectionOptions)
            : base(listenerOptions, coreLogic.Log)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException(nameof(coreLogic));
            this.reverseConnectionOptions = reverseConnectionOptions ?? 
                throw new ArgumentNullException(nameof(reverseConnectionOptions));
            clientBundles = new Dictionary<string, ClientBundle>();

            reverseClient = null;
            reverseClientThread = null;
            reverseClientTerminated = false;
            heartbeatDT = DateTime.MinValue;

            CustomFunctions = new HashSet<int> 
            { 
                FunctionID.DownloadFile, 
                FunctionID.UploadFile 
            };
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
        /// Gets the instance associated with the client, or throws an exception if it is undefined.
        /// </summary>
        private ScadaInstance GetClientInstance(ConnectedClient client)
        {
            return GetClientTag(client).Instance ??
                throw new InvalidOperationException("Client instance must not be null.");
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
        /// Registers the newly connected client for the proxy instance.
        /// </summary>
        private void RegisterClient(ConnectedClient client, ScadaInstance instance)
        {
            if (instance.ProxyMode && clientBundles.TryGetValue(instance.Name, out ClientBundle clientBundle))
            {
                lock (clientBundle)
                {
                    if (client.RoleID == AgentRoleID.Administrator)
                        clientBundle.AdminClient = client;
                    else if (client.RoleID == AgentRoleID.Agent)
                        clientBundle.AgentClient = client;
                }
            }
        }

        /// <summary>
        /// Unregisters the disconnected connected client for the proxy instance.
        /// </summary>
        private void UnregisterClient(ConnectedClient client, ScadaInstance instance)
        {
            if (instance != null && instance.ProxyMode && 
                clientBundles.TryGetValue(instance.Name, out ClientBundle clientBundle))
            {
                lock (clientBundle)
                {
                    if (client.RoleID == AgentRoleID.Administrator)
                    {
                        if (clientBundle.AdminClient == client)
                            clientBundle.AdminClient = null;
                    }
                    else if (client.RoleID == AgentRoleID.Agent)
                    {
                        if (clientBundle.AgentClient == client)
                            clientBundle.AgentClient = null;
                    }
                }
            }
        }

        /// <summary>
        /// Sends hearbeat requests to remote Agents to keep them connected.
        /// </summary>
        private void SendHearbeat()
        {
            foreach (ClientBundle clientBundle in clientBundles.Values)
            {
                if (clientBundle.AgentClient is ConnectedClient agentClient &&
                    agentClient.Tag is ClientTag clientTag && !clientTag.IsReverse &&
                    (heartbeatDT - agentClient.ActivityTime).TotalMilliseconds > agentClient.TcpClient.ReceiveTimeout)
                {
                    DataPacket request = new DataPacket
                    {
                        TransactionID = 0,
                        DataLength = 10,
                        SessionID = agentClient.SessionID,
                        FunctionID = FunctionID.AgentHeartbeat,
                        Buffer = agentClient.OutBuf
                    };

                    try
                    {
                        request.Encode();

                        lock (agentClient)
                        {
                            agentClient.NetStream.Write(request.Buffer, 0, request.BufferLength);
                            agentClient.RegisterActivity();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(Locale.IsRussian ?
                            "Ошибка при отправке пульса {0}: {1}" :
                            "Error sending heartbeat to {0}: {1}", agentClient.Address, ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Redirects the request from the Administrator client to an Agent client.
        /// </summary>
        private bool RedirectRequest(ConnectedClient adminClient, ScadaInstance instance, DataPacket dataPacket)
        {
            if (clientBundles.TryGetValue(instance.Name, out ClientBundle clientBundle) &&
                clientBundle.AdminClient == adminClient &&
                clientBundle.AgentClient is ConnectedClient agentClient)
            {
                try
                {
                    lock (agentClient)
                    {
                        dataPacket.SessionID = agentClient.SessionID;
                        dataPacket.Encode();
                        agentClient.NetStream.Write(dataPacket.Buffer, 0, dataPacket.BufferLength);
                        agentClient.RegisterActivity();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    log.WriteError(Locale.IsRussian ?
                        "Ошибка при переадресации запроса на {0}: {1}" :
                        "Error redirecting request to {0}: {1}", agentClient.Address, ex.Message);
                    return false;
                }
                finally
                {
                    dataPacket.SessionID = adminClient.SessionID;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Redirects the response from the Agent client to an Administrator client.
        /// </summary>
        private void RedirectResponse(ConnectedClient agentClient, ScadaInstance instance, DataPacket dataPacket)
        {
            if (clientBundles.TryGetValue(instance.Name, out ClientBundle clientBundle) &&
                clientBundle.AgentClient == agentClient &&
                clientBundle.AdminClient is ConnectedClient adminClient)
            {
                try
                {
                    lock (adminClient)
                    {
                        dataPacket.SessionID = adminClient.SessionID;
                        dataPacket.Encode();
                        adminClient.NetStream.Write(dataPacket.Buffer, 0, dataPacket.BufferLength);
                    }
                }
                catch (Exception ex)
                {
                    log.WriteError(Locale.IsRussian ?
                        "Ошибка при переадресации ответа на {0}: {1}" :
                        "Error redirecting response to {0}: {1}", agentClient.Address, ex.Message);
                }
            }
        }

        /// <summary>
        /// Gets the current status of the specified service.
        /// </summary>
        private void GetServiceStatus(ConnectedClient client, ScadaInstance instance, DataPacket request,
            out ResponsePacket response)
        {
            ServiceApp serviceApp = (ServiceApp)client.InBuf[ArgumentIndex];
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
            byte[] buffer = client.InBuf;
            int index = ArgumentIndex;
            ServiceApp serviceApp = (ServiceApp)GetByte(buffer, ref index);
            ServiceCommand serviceCommand = (ServiceCommand)GetByte(buffer, ref index);
            int timeout = GetInt32(buffer, ref index);
            bool result = instance.ControlService(serviceApp, serviceCommand, timeout);

            response = new ResponsePacket(request, client.OutBuf);
            index = ArgumentIndex;
            CopyBool(result, client.OutBuf, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Checks that the specified path contains a service.
        /// </summary>
        private static bool IsServiceFolder(RelativePath path)
        {
            return
                path.TopFolder == TopFolder.Server ||
                path.TopFolder == TopFolder.Comm ||
                path.TopFolder == TopFolder.Web;
        }

        /// <summary>
        /// Checks that the specified path contains configuration.
        /// </summary>
        private static bool IsConfigFolder(RelativePath path)
        {
            return
                path.TopFolder == TopFolder.Base ||
                path.TopFolder == TopFolder.View ||
                path.TopFolder == TopFolder.Server && path.AppFolder == AppFolder.Config ||
                path.TopFolder == TopFolder.Comm && path.AppFolder == AppFolder.Config ||
                path.TopFolder == TopFolder.Web && path.AppFolder == AppFolder.Config;
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
            // create empty client bundles
            foreach (string instanceName in coreLogic.GetProxyInstanceNames())
            {
                clientBundles.Add(instanceName, new ClientBundle
                {
                    AdminClient = null,
                    AgentClient = null
                });
            }

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
            if (client.Tag is ClientTag clientTag)
            {
                // disconnect inactive reverse client
                if (clientTag.IsReverse)
                    reverseClient.MarkAsDisconnected();

                // unregister client of proxy instance
                UnregisterClient(client, clientTag.Instance);
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
                RegisterClient(client, scadaInstance);
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
            response = null;
            handled = true;

            if (!GetClientInstance(client, out ScadaInstance instance))
            {
                response = new ResponsePacket(request, client.OutBuf);
                response.SetError(ErrorCode.InvalidOperation);
            }
            else if (instance.ProxyMode)
            {
                if (client.RoleID == AgentRoleID.Administrator)
                {
                    // redirect request to remote Agent
                    if (!RedirectRequest(client, instance, request))
                    {
                        response = new ResponsePacket(request, client.OutBuf);
                        response.SetError(ErrorCode.ProxyError);
                    }
                }
                else
                {
                    // redirect response to Administrator
                    RedirectResponse(client, instance, request);
                }
            }
            else
            {
                switch (request.FunctionID)
                {
                    case FunctionID.AgentHeartbeat:
                        // no response
                        break;

                    case FunctionID.GetServiceStatus:
                        GetServiceStatus(client, instance, request, out response);
                        break;

                    case FunctionID.ControlService:
                        ControlService(client, instance, request, out response);
                        break;

                    case FunctionID.DownloadFile:
                        DownloadFile(client, request);
                        break;

                    case FunctionID.UploadFile:
                        UploadFile(client, request, out response);
                        break;

                    default:
                        handled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets a list of short names of files or directories in the specified path.
        /// </summary>
        protected override ICollection<string> GetFileList(ConnectedClient client,
            bool searchForFiles, RelativePath path, string searchPattern)
        {
            if (IsServiceFolder(path) && path.AppFolder == AppFolder.Log && searchForFiles)
            {
                string directory = GetClientInstance(client).PathBuilder.GetAbsolutePath(path);

                if (Directory.Exists(directory))
                {
                    return Directory.EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly)
                        .Select(fullName => Path.GetFileName(fullName))
                        .ToArray();
                }
            }

            return Array.Empty<string>();
        }

        /// <summary>
        /// Gets the information associated with the specified file.
        /// </summary>
        protected override ShortFileInfo GetFileInfo(ConnectedClient client, RelativePath path)
        {
            if (IsServiceFolder(path) && path.AppFolder == AppFolder.Log)
            {
                string fileName = GetClientInstance(client).PathBuilder.GetAbsolutePath(path);
                return new ShortFileInfo(new FileInfo(fileName));
            }
            else if (IsConfigFolder(path) && path.Path.StartsWith(AgentConst.DownloadConfigPrefix))
            {
                return new ShortFileInfo
                {
                    Exists = true,
                    LastWriteTime = DateTime.MinValue,
                    Length = 0
                };
            }
            else
            {
                throw new ProtocolException(ErrorCode.IllegalFunctionArguments, CommonPhrases.PathNotSupported);
            }
        }

        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        protected override BinaryReader OpenRead(ConnectedClient client, RelativePath path)
        {
            ScadaInstance instance = GetClientInstance(client);
            string fileName;

            if (IsServiceFolder(path) && path.AppFolder == AppFolder.Log)
            {
                fileName = instance.PathBuilder.GetAbsolutePath(path);
            }
            else if (IsConfigFolder(path) && path.Path.StartsWith(AgentConst.DownloadConfigPrefix))
            {
                fileName = Path.Combine(coreLogic.AppDirs.TempDir, path.Path);
                if (!instance.PackConfig(fileName, path))
                    throw new ProtocolException(ErrorCode.InternalServerError);
            }
            else
            {
                throw new ProtocolException(ErrorCode.IllegalFunctionArguments, CommonPhrases.PathNotSupported);
            }

            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new BinaryReader(stream, Encoding.UTF8, false);
        }

        /// <summary>
        /// Accepts or rejects the file upload.
        /// </summary>
        protected override bool AcceptUpload(ConnectedClient client, RelativePath path, out object uploadContext)
        {
            UploadContext context = new UploadContext 
            { 
                FileName = GetClientInstance(client).PathBuilder.GetAbsolutePath(path),
                IsConfig = path.TopFolder == TopFolder.Agent && path.AppFolder == AppFolder.Temp &&
                    path.Path.StartsWith(AgentConst.UploadConfigPrefix)
            };

            uploadContext = context;
            return context.IsConfig || path.TopFolder == TopFolder.Comm && path.AppFolder == AppFolder.Cmd;
        }

        /// <summary>
        /// Opens an existing file or creates a new file for writing.
        /// </summary>
        protected override BinaryWriter OpenWrite(ConnectedClient client, RelativePath path, object uploadContext)
        {
            string fileName = ((UploadContext)uploadContext).FileName;
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            return new BinaryWriter(stream, Encoding.UTF8, false);
        }

        /// <summary>
        /// Breaks writing and deletes the corrupted file.
        /// </summary>
        protected override void BreakWriting(ConnectedClient client, RelativePath path, object uploadContext)
        {
            string fileName = ((UploadContext)uploadContext).FileName;
            File.Delete(fileName);
        }

        /// <summary>
        /// Processes the successfully uploaded file.
        /// </summary>
        protected override void ProcessFile(ConnectedClient client, RelativePath path, object uploadContext)
        {
            UploadContext context = (UploadContext)uploadContext;

            if (context.IsConfig)
            {
                if (!GetClientInstance(client).UnpackConfig(context.FileName))
                    throw new ProtocolException(ErrorCode.InternalServerError);
            }
        }
    }
}
