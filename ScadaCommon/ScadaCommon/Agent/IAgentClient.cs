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
 * Module   : ScadaCommon
 * Summary  : Defines functionality to interact with the Agent service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Protocol;
using System;
using System.Collections.Generic;

namespace Scada.Agent
{
    /// <summary>
    /// Defines functionality to interact with the Agent service.
    /// <para>Определяет функциональность для взаимодействия со службой Агента.</para>
    /// </summary>
    public interface IAgentClient
    {
        /// <summary>
        /// Gets a value indicating whether the connection is local.
        /// </summary>
        bool IsLocal { get; }


        /// <summary>
        /// Tests the connection with the agent.
        /// </summary>
        bool TestConnection(out string errMsg);

        /// <summary>
        /// Gets the current status of the specified service.
        /// </summary>
        bool GetServiceStatus(ServiceApp serviceApp, out ServiceStatus serviceStatus);

        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        bool ControlService(ServiceApp serviceApp, ServiceCommand cmd);

        /// <summary>
        /// Downloads the configuration to the file.
        /// </summary>
        void DownloadConfig(string destFileName, ConfigTransferOptions transferOptions);

        /// <summary>
        /// Uploads the configuration from the file.
        /// </summary>
        void UploadConfig(string srcFileName, ConfigTransferOptions transferOptions);

        /// <summary>
        /// Reads the text file.
        /// </summary>
        bool ReadTextFile(RelativePath path, ref DateTime newerThan, out ICollection<string> lines);

        /// <summary>
        /// Reads the rest of the text file.
        /// </summary>
        bool ReadTextFile(RelativePath path, long offsetFromEnd, ref DateTime newerThan, out ICollection<string> lines);

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        void SendCommand(ServiceApp serviceApp, TeleCommand cmd);
    }
}
