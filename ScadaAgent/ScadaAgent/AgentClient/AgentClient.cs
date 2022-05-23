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
 * Module   : ScadaAgentClient
 * Summary  : Represents a TCP client which interacts with the Agent service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using static Scada.BinaryConverter;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Agent.Client
{
    /// <summary>
    /// Represents a TCP client which interacts with the Agent service.
    /// <para>Представляет TCP-клиента, который взаимодействует со службой Агента.</para>
    /// </summary>
    public class AgentClient : ClientBase, IAgentClient
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AgentClient(ConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Reads all lines from the stream.
        /// </summary>
        private static List<string> ReadAllLines(Stream stream, bool skipFirstLine)
        {
            stream.Position = 0;
            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    if (skipFirstLine)
                        skipFirstLine = false;
                    else
                        lines.Add(reader.ReadLine());
                }
            }

            return lines;
        }


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
        public bool ControlService(ServiceApp serviceApp, ServiceCommand cmd, int timeout)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.ControlService);
            int index = ArgumentIndex;
            CopyByte((byte)serviceApp, outBuf, ref index);
            CopyByte((byte)cmd, outBuf, ref index);
            CopyInt32(timeout, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            ReceiveResponse(request);
            return inBuf[ArgumentIndex] > 0;
        }

        /// <summary>
        /// Downloads the configuration part to the file.
        /// </summary>
        public void DownloadConfig(string destFileName, TopFolder topFolder)
        {
            if (destFileName == null)
                throw new ArgumentNullException(nameof(destFileName));

            RelativePath relativePath;
            string shortFileName = Path.GetFileName(destFileName);

            if (!shortFileName.StartsWith(AgentConst.DownloadConfigPrefix))
                throw new ArgumentException("Invalid file name.", nameof(destFileName));

            switch (topFolder)
            {
                case TopFolder.Base:
                case TopFolder.View:
                    relativePath = new RelativePath(topFolder, AppFolder.Root, shortFileName);
                    break;

                case TopFolder.Server:
                case TopFolder.Comm:
                case TopFolder.Web:
                    relativePath = new RelativePath(topFolder, AppFolder.Config, shortFileName);
                    break;

                default:
                    throw new ScadaException(Locale.IsRussian ?
                        "Невозможно определить файлы для скачивания." :
                        "Unable to define files to download.");
            }

            DownloadFile(relativePath, destFileName, true);
        }

        /// <summary>
        /// Uploads the configuration from the file.
        /// </summary>
        public void UploadConfig(string srcFileName, CancellationToken cancellationToken)
        {
            UploadFile(srcFileName, new RelativePath(TopFolder.Agent, AppFolder.Temp, Path.GetFileName(srcFileName)),
                out bool fileAccepted, cancellationToken);

            if (!fileAccepted)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Конфигурация отклонена Агентом." :
                    "Configuration rejected by Agent.");
            }
        }

        /// <summary>
        /// Reads the text file.
        /// </summary>
        public bool ReadTextFile(RelativePath path, ref DateTime newerThan, out ICollection<string> lines)
        {
            return ReadTextFile(path, 0, ref newerThan, out lines);
        }

        /// <summary>
        /// Reads the rest of the text file.
        /// </summary>
        public bool ReadTextFile(RelativePath path, long offsetFromEnd, ref DateTime newerThan, out ICollection<string> lines)
        {
            bool readFromEnd = offsetFromEnd > 0;
            DownloadFile(path, offsetFromEnd, 0, readFromEnd, newerThan, false, () => new MemoryStream(),
                out DateTime lastWriteTime, out FileReadingResult readingResult, out Stream stream);

            try
            {
                if (readingResult == FileReadingResult.Completed)
                {
                    lines = ReadAllLines(stream, readFromEnd);
                    newerThan = lastWriteTime;
                    return true;
                }
                else if (readingResult == FileReadingResult.FileOutdated)
                {
                    lines = null;
                    return false;
                }
                else
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Ошибка при чтении файла {0}: {1}" :
                        "Error reading file {0}: {1}", path, readingResult.ToString(Locale.IsRussian));
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public void SendCommand(ServiceApp serviceApp, TeleCommand cmd)
        {
            // initialize command ID and timestamp
            DateTime utcNow = DateTime.UtcNow;

            if (cmd.CommandID == 0)
                cmd.CommandID = ScadaUtils.GenerateUniqueID(utcNow);

            if (cmd.CreationTime == DateTime.MinValue)
                cmd.CreationTime = utcNow;

            // upload command
            using (MemoryStream stream = new MemoryStream())
            {
                cmd.Save(stream);
                stream.Position = 0;

                string fileName = string.Format("cmd_{0}.dat", cmd.CommandID);
                UploadFile(stream, new RelativePath(serviceApp, AppFolder.Cmd, fileName), out bool fileAccepted);

                if (!fileAccepted)
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Команда отклонена Агентом." :
                        "Command rejected by Agent.");
                }
            }
        }
    }
}
