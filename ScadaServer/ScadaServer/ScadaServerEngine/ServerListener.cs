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
 * Module   : ScadaServerEngine
 * Summary  : Represents a TCP listener which waits for client connections
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Protocol;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using static Scada.BinaryConverter;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents a TCP listener which waits for client connections.
    /// <para>Представляет TCP-прослушиватель, который ожидает подключения клиентов.</para>
    /// </summary>
    internal class ServerListener : BaseListener
    {
        /// <summary>
        /// The maximum number of events in one block.
        /// </summary>
        private const int EventBlockCapacity = 1000;

        private readonly CoreLogic coreLogic;         // the server logic instance
        private readonly ArchiveHolder archiveHolder; // holds archives
        private readonly ServerCache serverCache;     // the server level cache


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerListener(CoreLogic coreLogic, ArchiveHolder archiveHolder, ServerCache serverCache)
            : base(coreLogic?.Config.ListenerOptions, coreLogic?.Log)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException("coreLogic");
            this.archiveHolder = archiveHolder ?? throw new ArgumentNullException("archiveHolder");
            this.serverCache = serverCache ?? throw new ArgumentNullException("serverCache");
        }


        /// <summary>
        /// Gets the client tag or creates it if necessary.
        /// </summary>
        protected ClientTag GetClientTag(ConnectedClient client)
        {
            return client.Tag as ClientTag ?? throw new InvalidOperationException("Client tag must not be null.");
        }

        /// <summary>
        /// Gets the current data.
        /// </summary>
        protected void GetCurrentData(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            long cnlListID = GetInt64(buffer, ref index);
            CnlData[] cnlData;

            if (cnlListID > 0)
            {
                cnlData = coreLogic.GetCurrentData(cnlListID);

                if (cnlData == null)
                    cnlListID = 0;
            }
            else
            {
                bool useCache = GetBool(buffer, ref index);
                int[] cnlNums = GetIntArray(buffer, ref index);
                cnlData = coreLogic.GetCurrentData(cnlNums, useCache, out cnlListID);
            }

            byte[] outBuf = client.OutBuf;
            response = new ResponsePacket(request, outBuf);
            index = ArgumentIndex;
            CopyInt64(cnlListID, outBuf, ref index);
            CopyCnlDataArray(cnlData, outBuf, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Gets the trends of the specified input channels.
        /// </summary>
        protected void GetTrends(ConnectedClient client, DataPacket request)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int[] cnlNums = GetIntArray(buffer, ref index);
            DateTime startTime = GetTime(buffer, ref index);
            DateTime endTime = GetTime(buffer, ref index);
            bool endInclusive = GetBool(buffer, ref index);
            int archiveBit = GetByte(buffer, ref index);

            TrendBundle trendBundle = archiveHolder.GetTrends(cnlNums, startTime, endTime, endInclusive, archiveBit);
            List<DateTime> timestamps = trendBundle.Timestamps;
            int totalPointCount = timestamps.Count;
            int blockCapacity = (BufferLenght - ArgumentIndex - 16) / (8 + cnlNums.Length * 10);
            int blockCount = totalPointCount > 0 ? (int)Math.Ceiling((double)totalPointCount / blockCapacity) : 1;
            int pointIndex = 0;
            buffer = client.OutBuf;

            for (int blockNumber = 1; blockNumber <= blockCount; blockNumber++)
            {
                ResponsePacket response = new ResponsePacket(request, buffer);
                index = ArgumentIndex;
                CopyInt32(blockNumber, buffer, ref index);
                CopyInt32(blockCount, buffer, ref index);
                CopyInt32(totalPointCount, buffer, ref index);

                int pointCount = Math.Min(totalPointCount - pointIndex, blockCapacity); // points in this block
                CopyInt32(pointCount, buffer, ref index);

                for (int i = 0, ptIdx = pointIndex; i < pointCount; i++, ptIdx++)
                {
                    CopyTime(timestamps[ptIdx], buffer, ref index);
                }

                foreach (TrendBundle.CnlDataList trend in trendBundle.Trends)
                {
                    for (int i = 0, ptIdx = pointIndex; i < pointCount; i++, ptIdx++)
                    {
                        CopyCnlData(trend[ptIdx], buffer, ref index);
                    }
                }

                pointIndex += pointCount;
                response.BufferLength = index;
                client.SendResponse(response);
            }
        }

        /// <summary>
        /// Gets the slice of the specified input channels at the timestamp.
        /// </summary>
        protected void GetSlice(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int[] cnlNums = GetIntArray(buffer, ref index);
            DateTime timestamp = GetTime(buffer, ref index);
            int archiveBit = GetByte(buffer, ref index);

            Slice slice = archiveHolder.GetSlice(cnlNums, timestamp, archiveBit);
            response = new ResponsePacket(request, client.OutBuf);
            index = ArgumentIndex;
            CopyCnlDataArray(slice.CnlData, client.OutBuf, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Gets the time (UTC) when the archive was last written to.
        /// </summary>
        protected void GetLastWriteTime(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            int archiveBit = request.Buffer[ArgumentIndex];
            DateTime lastWriteTime = archiveHolder.GetLastWriteTime(archiveBit);

            response = new ResponsePacket(request, client.OutBuf) { ArgumentLength = 8 };
            CopyTime(lastWriteTime, client.OutBuf, ArgumentIndex);
        }

        /// <summary>
        /// Writes the current data.
        /// </summary>
        protected void WriteCurrentData(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int deviceNum = GetInt32(buffer, ref index);
            int cnlCnt = GetInt32(buffer, ref index);

            int[] cnlNums = new int[cnlCnt];
            CnlData[] cnlData = new CnlData[cnlCnt];

            for (int i = 0, idx1 = index, idx2 = idx1 + cnlCnt * 4; i < cnlCnt; i++)
            {
                cnlNums[i] = GetInt32(buffer, ref idx1);
                cnlData[i] = GetCnlData(buffer, ref idx2);
            }

            index += cnlCnt * 14;
            bool applyFormulas = buffer[index] > 0;
            coreLogic.WriteCurrentData(deviceNum, cnlNums, cnlData, applyFormulas);

            response = new ResponsePacket(request, client.OutBuf);
        }

        /// <summary>
        /// Writes the historical data.
        /// </summary>
        protected void WriteHistoricalData(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int deviceNum = GetInt32(buffer, ref index);
            DateTime timestamp = GetTime(buffer, ref index);
            int cnlCnt = GetInt32(buffer, ref index);
            Slice slice = new Slice(timestamp, cnlCnt);

            for (int i = 0, idx1 = index, idx2 = idx1 + cnlCnt * 4; i < cnlCnt; i++)
            {
                slice.CnlNums[i] = GetInt32(buffer, ref idx1);
                slice.CnlData[i] = GetCnlData(buffer, ref idx2);
            }

            index += cnlCnt * 14;
            int archiveMask = GetInt32(buffer, ref index);
            bool applyFormulas = buffer[index] > 0;
            coreLogic.WriteHistoricalData(deviceNum, slice, archiveMask, applyFormulas);

            response = new ResponsePacket(request, client.OutBuf);
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        protected void GetEventByID(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            long eventID = GetInt64(buffer, ref index);
            int archiveBit = GetByte(buffer, ref index);

            Event ev = archiveHolder.GetEventByID(eventID, archiveBit);
            buffer = client.OutBuf;
            response = new ResponsePacket(request, buffer);
            index = ArgumentIndex;

            if (ev == null)
            {
                CopyBool(false, buffer, ref index);
            }
            else
            {
                CopyBool(true, buffer, ref index);
                CopyEvent(ev, buffer, ref index);
            }

            response.BufferLength = index;
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        protected void GetEvents(ConnectedClient client, DataPacket request)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            DateTime startTime = GetTime(buffer, ref index);
            DateTime endTime = GetTime(buffer, ref index);
            bool endInclusive = GetBool(buffer, ref index);
            long dataFilterID = GetInt64(buffer, ref index);
            DataFilter dataFilter = null;

            if (dataFilterID > 0)
            {
                dataFilter = serverCache.DataFilterCache.Get(dataFilterID);

                if (dataFilter == null)
                    dataFilterID = 0;
            }
            else
            {
                bool useCache = GetBool(buffer, ref index);
                dataFilter = GetDataFilter(typeof(Event), buffer, ref index);

                if (useCache)
                {
                    dataFilterID = serverCache.GetNextID();
                    serverCache.DataFilterCache.Add(dataFilterID, dataFilter);
                }
            }

            int archiveBit = GetByte(buffer, ref index);

            List<Event> events = dataFilter == null ?
                new List<Event>() :
                archiveHolder.GetEvents(startTime, endTime, endInclusive, dataFilter, archiveBit);
            int totalEventCount = events.Count;
            int blockCount = totalEventCount > 0 ? (int)Math.Ceiling((double)totalEventCount / EventBlockCapacity) : 1;
            int eventIndex = 0;
            buffer = client.OutBuf;

            for (int blockNumber = 1; blockNumber <= blockCount; blockNumber++)
            {
                ResponsePacket response = new ResponsePacket(request, buffer);
                index = ArgumentIndex;
                CopyInt32(blockNumber, buffer, ref index);
                CopyInt32(blockCount, buffer, ref index);
                CopyInt64(dataFilterID, buffer, ref index);
                CopyInt32(totalEventCount, buffer, ref index);

                int eventCount = Math.Min(totalEventCount - eventIndex, EventBlockCapacity); // events in this block
                CopyInt32(eventCount, buffer, ref index);

                for (int i = 0; i < eventCount; i++)
                {
                    CopyEvent(events[eventIndex], buffer, ref index);
                    eventIndex++;
                }

                response.BufferLength = index;
                client.SendResponse(response);
            }
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        protected void WriteEvent(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            int index = ArgumentIndex;
            Event ev = GetEvent(request.Buffer, ref index);
            int archiveMask = GetInt32(request.Buffer, ref index);
            coreLogic.WriteEvent(ev, archiveMask);

            response = new ResponsePacket(request, client.OutBuf) { ArgumentLength = 8 } ;
            CopyInt64(ev.EventID, client.OutBuf, ArgumentIndex);
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        protected void AckEvent(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            long eventID = GetInt64(buffer, ref index);
            DateTime timestamp = GetTime(buffer, ref index);
            int userID = GetInt32(buffer, ref index);
            coreLogic.AckEvent(eventID, timestamp, userID);
            response = new ResponsePacket(request, client.OutBuf);
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        protected void SendCommand(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;

            TeleCommand command = new TeleCommand
            {
                UserID = GetInt32(buffer, ref index),
                OutCnlNum = GetInt32(buffer, ref index),
                CmdVal = GetDouble(buffer, ref index),
                CmdData = GetByteArray(buffer, ref index)
            };

            if (command.UserID <= 0)
                command.UserID = client.UserID;

            coreLogic.SendCommand(command, out CommandResult commandResult);

            buffer = client.OutBuf;
            response = new ResponsePacket(request, buffer);
            index = ArgumentIndex;
            CopyInt64(command.CommandID, buffer, ref index);
            CopyBool(commandResult.IsSuccessful, buffer, ref index);
            CopyString(commandResult.ErrorMessage, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Gets a telecontrol command from the server queue.
        /// </summary>
        protected void GetCommand(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            long commandToRemove = BitConverter.ToInt64(buffer, ArgumentIndex);
            TeleCommand command = GetClientTag(client).GetCommand(commandToRemove);

            buffer = client.OutBuf;
            response = new ResponsePacket(request, buffer);
            int index = ArgumentIndex;

            if (command == null)
            {
                CopyInt64(0, buffer, ref index);
            }
            else
            {
                CopyInt64(command.CommandID, buffer, ref index);
                CopyTime(command.CreationTime, buffer, ref index);
                CopyInt32(command.UserID, buffer, ref index);
                CopyInt32(command.OutCnlNum, buffer, ref index);
                CopyInt32(command.CmdTypeID, buffer, ref index);
                CopyInt32(command.ObjNum, buffer, ref index);
                CopyInt32(command.DeviceNum, buffer, ref index);
                CopyInt32(command.CmdNum, buffer, ref index);
                CopyString(command.CmdCode, buffer, ref index);
                CopyDouble(command.CmdVal, buffer, ref index);
                CopyByteArray(command.CmdData, buffer, ref index);
            }

            response.BufferLength = index;
        }

        /// <summary>
        /// Disables getting commands for the client.
        /// </summary>
        protected void DisableGettingCommands(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            log.WriteAction(Locale.IsRussian ?
                "Отключение получения команд для пользователя {1}" :
                "Disable getting commands for the user {1}", client.Username);
            GetClientTag(client).DisableGettingCommands();
            response = new ResponsePacket(request, client.OutBuf);
        }

        /// <summary>
        /// Gets the server name and version.
        /// </summary>
        protected override string GetServerName()
        {
            return Locale.IsRussian ?
                "Сервер " + ServerUtils.AppVersion :
                "Server " + ServerUtils.AppVersion;
        }

        /// <summary>
        /// Gets a value indicating whether the server is ready.
        /// </summary>
        protected override bool ServerIsReady()
        {
            return coreLogic.IsReady;
        }

        /// <summary>
        /// Validates the username and password.
        /// </summary>
        protected override bool ValidateUser(ConnectedClient client, string username, string password, string instance,
            out int userID, out int roleID, out string errMsg)
        {
            if (coreLogic.ValidateUser(username, password, out userID, out roleID, out errMsg))
            {
                if (client.IsLoggedIn)
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Проверка имени и пароля пользователя {0} успешна" :
                        "Checking username and password for user {0} is successful", username);
                    return true;
                }
                else if (roleID == RoleID.Application)
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Пользователь {0} успешно аутентифицирован" :
                        "User {0} is successfully authenticated", username);

                    client.IsLoggedIn = true;
                    client.Username = username;
                    client.UserID = userID;
                    client.RoleID = roleID;
                    return true;
                }
                else
                {
                    errMsg = Locale.IsRussian ?
                        "Недостаточно прав" :
                        "Insufficient rights";
                    log.WriteError(Locale.IsRussian ?
                        "Пользователь {0} имеет недостаточно прав. Требуется роль Приложение" :
                        "User {0} has insufficient rights. The Application role required", username);
                    return false;
                }
            }
            else
            {
                if (client.IsLoggedIn)
                {
                    log.WriteError(Locale.IsRussian ?
                        "Результат проверки имени и пароля пользователя {0} отрицательный: {1}" :
                        "Checking username and password for user {0} is not successful: {1}", username, errMsg);
                    return false;
                }
                else
                {
                    log.WriteError(Locale.IsRussian ?
                        "Ошибка аутентификации пользователя {0}: {1}" :
                        "Authentication failed for user {0}: {1}", username, errMsg);
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the directory name by ID.
        /// </summary>
        protected override string GetDirectory(int directoryID)
        {
            PathOptions pathOptions = coreLogic.Config.PathOptions;

            switch (directoryID)
            {
                case (int)TopFolder.Archive:
                    return pathOptions.ArcDir;
                case (int)TopFolder.ArchiveCopy:
                    return pathOptions.ArcCopyDir;
                case (int)TopFolder.Base:
                    return pathOptions.BaseDir;
                case (int)TopFolder.View:
                    return pathOptions.ViewDir;
                default:
                    throw new ProtocolException(ErrorCode.IllegalFunctionArguments, Locale.IsRussian ?
                        "Директория не поддерживается." :
                        "Directory not supported.");
            }
        }

        /// <summary>
        /// Accepts or rejects the file upload.
        /// </summary>
        protected override bool AcceptFileUpload(string fileName)
        {
            return false;
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
                case FunctionID.GetCurrentData:
                    GetCurrentData(client, request, out response);
                    break;

                case FunctionID.GetTrends:
                    GetTrends(client, request);
                    break;

                case FunctionID.GetSlice:
                    GetSlice(client, request, out response);
                    break;

                case FunctionID.GetLastWriteTime:
                    GetLastWriteTime(client, request, out response);
                    break;

                case FunctionID.WriteCurrentData:
                    WriteCurrentData(client, request, out response);
                    break;

                case FunctionID.WriteHistoricalData:
                    WriteHistoricalData(client, request, out response);
                    break;

                case FunctionID.GetEventByID:
                    GetEventByID(client, request, out response);
                    break;

                case FunctionID.GetEvents:
                    GetEvents(client, request);
                    break;

                case FunctionID.WriteEvent:
                    WriteEvent(client, request, out response);
                    break;

                case FunctionID.AckEvent:
                    AckEvent(client, request, out response);
                    break;

                case FunctionID.SendCommand:
                    SendCommand(client, request, out response);
                    break;

                case FunctionID.GetCommand:
                    GetCommand(client, request, out response);
                    break;

                case FunctionID.DisableGettingCommands:
                    DisableGettingCommands(client, request, out response);
                    break;

                default:
                    handled = false;
                    break;
            }
        }

        /// <summary>
        /// Performs actions when initializing the connected client.
        /// </summary>
        protected override void OnClientInit(ConnectedClient client)
        {
            client.Tag = new ClientTag();
        }

        /// <summary>
        /// Enqueues the command to be transferred to the connected cliens.
        /// </summary>
        public void EnqueueCommand(TeleCommand command)
        {
            foreach (KeyValuePair<long, ConnectedClient> pair in clients)
            {
                GetClientTag(pair.Value).AddCommand(command);
            }
        }
    }
}
