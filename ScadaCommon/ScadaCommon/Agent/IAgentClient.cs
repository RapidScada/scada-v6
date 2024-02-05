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
 * Summary  : Defines functionality to interact with the Agent service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2023
 */

using Scada.Data.Models;
using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Scada.Agent
{
    /// <summary>
    /// Defines functionality to interact with the Agent service.
    /// <para>Определяет функциональность для взаимодействия со службой Агента.</para>
    /// </summary>
    public interface IAgentClient
    {
        /// <summary>
        /// Gets an object that can be used to synchronize access to the client.
        /// </summary>
        object SyncRoot { get; }


        /// <summary>
        /// Tests the connection with the Agent service.
        /// </summary>
        bool TestConnection(out string errMsg);

        /// <summary>
        /// Gets the current status of the specified service.
        /// </summary>
        ServiceStatus GetServiceStatus(ServiceApp serviceApp);

        /// <summary>
        /// Gets the current statuses of the specified services.
        /// </summary>
        ServiceStatus[] GetServiceStatus(ServiceApp[] serviceApps);

        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        bool ControlService(ServiceApp serviceApp, ServiceCommand cmd, int timeout);

        /// <summary>
        /// Downloads the configuration part to the file.
        /// </summary>
        void DownloadConfig(string destFileName, TopFolder topFolder);

        /// <summary>
        /// Uploads the configuration from the file.
        /// </summary>
        void UploadConfig(string srcFileName, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of short file names in the specified path.
        /// </summary>
        ICollection<string> GetFileList(RelativePath path, string searchPattern);

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
