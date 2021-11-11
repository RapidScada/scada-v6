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
 * Module   : ScadaAgentClient
 * Summary  : Represents a TCP client which interacts with the Agent service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Protocol;
using System;
using System.Collections.Generic;
using static Scada.BinaryConverter;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Agent.Client
{
    /// <summary>
    /// Represents a TCP client which interacts with the Agent service.
    /// <para>Представляет TCP-клиента, который взаимодействует со службой Агента.</para>
    /// </summary>
    public class AgentClient : BaseClient, IAgentClient
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AgentClient(ConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
            IsLocal = 
                string.Equals(connectionOptions.Host, "localhost", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(connectionOptions.Host, "127.0.0.1");
        }


        /// <summary>
        /// Gets a value indicating whether the connection is local.
        /// </summary>
        public bool IsLocal { get; }


        /// <summary>
        /// Tests the connection with the Agent service.
        /// </summary>
        public bool TestConnection(out string errMsg)
        {
            try
            {
                RestoreConnection();

                if (ClientState == ClientState.LoggedIn)
                {
                    errMsg = "";
                    return true;
                }
                else
                {
                    errMsg = Locale.IsRussian ?
                        "Агент недоступен" :
                        "Agent unavailable";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Gets the current status of the specified service.
        /// </summary>
        public bool GetServiceStatus(ServiceApp serviceApp, out ServiceStatus serviceStatus)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetServiceStatus);
            outBuf[ArgumentIndex] = (byte)serviceApp;
            request.ArgumentLength = 1;
            SendRequest(request);

            ReceiveResponse(request);
            bool parsed = inBuf[ArgumentIndex] > 0;
            serviceStatus = (ServiceStatus)inBuf[ArgumentIndex + 1];
            return parsed;
        }

        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        public bool ControlService(ServiceApp serviceApp, ServiceCommand cmd)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.ControlService);
            outBuf[ArgumentIndex] = (byte)serviceApp;
            outBuf[ArgumentIndex + 1] = (byte)cmd;
            request.ArgumentLength = 2;
            SendRequest(request);

            ReceiveResponse(request);
            return inBuf[ArgumentIndex] > 0;
        }

        /// <summary>
        /// Downloads the configuration to the file.
        /// </summary>
        public void DownloadConfig(string destFileName, ConfigTransferOptions transferOptions)
        {

        }

        /// <summary>
        /// Uploads the configuration from the file.
        /// </summary>
        public void UploadConfig(string srcFileName, ConfigTransferOptions transferOptions)
        {

        }

        /// <summary>
        /// Reads the text file.
        /// </summary>
        public bool ReadTextFile(RelativePath path, ref DateTime newerThan, out ICollection<string> lines)
        {
            lines = null;
            return false;
        }

        /// <summary>
        /// Reads the rest of the text file.
        /// </summary>
        public bool ReadTextFile(RelativePath path, long offsetFromEnd, ref DateTime newerThan, out ICollection<string> lines)
        {
            lines = null;
            return false;
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public void SendCommand(ServiceApp serviceApp, TeleCommand cmd)
        {

        }
    }
}
