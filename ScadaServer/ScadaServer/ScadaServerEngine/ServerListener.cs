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
 * Module   : ScadaServerEngine
 * Summary  : Represents a TCP listener which waits for client connections
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Adapters;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Protocol;
using Scada.Storages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Scada.BinaryConverter;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents a TCP listener which waits for client connections.
    /// <para>Представляет TCP-прослушиватель, который ожидает подключения клиентов.</para>
    /// </summary>
    internal class ServerListener : ListenerBase
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
            : base(coreLogic?.AppConfig.ListenerOptions, coreLogic?.Log)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException(nameof(coreLogic));
            this.archiveHolder = archiveHolder ?? throw new ArgumentNullException(nameof(archiveHolder));
            this.serverCache = serverCache ?? throw new ArgumentNullException(nameof(serverCache));
        }


        /// <summary>
        /// Gets the client tag, or throws an exception if it is undefined.
        /// </summary>
        private ClientTag GetClientTag(ConnectedClient client)
        {
            return client.Tag as ClientTag ?? throw new InvalidOperationException("Client tag must not be null.");
        }

        /// <summary>
        /// Gets the current data.
        /// </summary>
        private void GetCurrentData(ConnectedClient client, DataPacket request, out ResponsePacket response)
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

            buffer = client.OutBuf;
            response = new ResponsePacket(request, buffer);
            index = ArgumentIndex;
            CopyInt64(cnlListID, buffer, ref index);
            CopyCnlDataArray(cnlData, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Gets the trends of the specified channels.
        /// </summary>
        private void GetTrends(ConnectedClient client, DataPacket request)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int archiveBit = GetByte(buffer, ref index);
            TimeRange timeRange = GetTimeRange(buffer, ref index);
            int[] cnlNums = GetIntArray(buffer, ref index);

            TrendBundle trendBundle = archiveHolder.GetTrends(archiveBit, timeRange, cnlNums);
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
        /// Gets the available timestamps.
        /// </summary>
        private void GetTimestamps(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int archiveBit = GetByte(buffer, ref index);
            TimeRange timeRange = GetTimeRange(buffer, ref index);

            List<DateTime> timestamps = archiveHolder.GetTimestamps(archiveBit, timeRange);
            buffer = client.OutBuf;
            response = new ResponsePacket(request, buffer);
            index = ArgumentIndex;
            CopyInt32(timestamps.Count, buffer, ref index);

            foreach (DateTime timestamp in timestamps)
            {
                CopyTime(timestamp, buffer, ref index);
            }

            response.BufferLength = index;
        }

        /// <summary>
        /// Gets the slice of the specified channels at the timestamp.
        /// </summary>
        private void GetSlice(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int archiveBit = GetByte(buffer, ref index);
            DateTime timestamp = GetTime(buffer, ref index);
            int[] cnlNums = GetIntArray(buffer, ref index);

            Slice slice = archiveHolder.GetSlice(archiveBit, timestamp, cnlNums);
            response = new ResponsePacket(request, client.OutBuf);
            index = ArgumentIndex;
            CopyCnlDataArray(slice.CnlData, client.OutBuf, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Gets the time (UTC) when the archive was last written to.
        /// </summary>
        private void GetLastWriteTime(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            int archiveBit = request.Buffer[ArgumentIndex];
            DateTime lastWriteTime = archiveHolder.GetLastWriteTime(archiveBit);

            response = new ResponsePacket(request, client.OutBuf) { ArgumentLength = 8 };
            CopyTime(lastWriteTime, client.OutBuf, ArgumentIndex);
        }

        /// <summary>
        /// Writes the current data.
        /// </summary>
        private void WriteCurrentData(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int cnlCnt = GetInt32(buffer, ref index);
            int[] cnlNums = new int[cnlCnt];
            CnlData[] cnlData = new CnlData[cnlCnt];

            for (int i = 0, idx1 = index, idx2 = idx1 + cnlCnt * 4; i < cnlCnt; i++)
            {
                cnlNums[i] = GetInt32(buffer, ref idx1);
                cnlData[i] = GetCnlData(buffer, ref idx2);
            }

            index += cnlCnt * 14;
            int deviceNum = GetInt32(buffer, ref index);
            WriteFlags writeFlags = (WriteFlags)buffer[index];
            coreLogic.WriteCurrentData(cnlNums, cnlData, deviceNum, writeFlags);

            response = new ResponsePacket(request, client.OutBuf);
        }

        /// <summary>
        /// Writes the historical data.
        /// </summary>
        private void WriteHistoricalData(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int archiveMask = GetInt32(buffer, ref index);
            DateTime timestamp = GetTime(buffer, ref index);
            int cnlCnt = GetInt32(buffer, ref index);
            Slice slice = new Slice(timestamp, cnlCnt);

            for (int i = 0, idx1 = index, idx2 = idx1 + cnlCnt * 4; i < cnlCnt; i++)
            {
                slice.CnlNums[i] = GetInt32(buffer, ref idx1);
                slice.CnlData[i] = GetCnlData(buffer, ref idx2);
            }

            index += cnlCnt * 14;
            int deviceNum = GetInt32(buffer, ref index);
            WriteFlags writeFlags = (WriteFlags)buffer[index];
            coreLogic.WriteHistoricalData(archiveMask, slice, deviceNum, writeFlags);

            response = new ResponsePacket(request, client.OutBuf);
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        private void GetEventByID(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int archiveBit = GetByte(buffer, ref index);
            long eventID = GetInt64(buffer, ref index);

            Event ev = archiveHolder.GetEventByID(archiveBit, eventID);
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
        private void GetEvents(ConnectedClient client, DataPacket request)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int archiveBit = GetByte(buffer, ref index);
            TimeRange timeRange = GetTimeRange(buffer, ref index);
            long dataFilterID = GetInt64(buffer, ref index);
            DataFilter dataFilter;

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
                    dataFilterID = ScadaUtils.GenerateUniqueID();

                    if (!serverCache.DataFilterCache.Add(dataFilterID, dataFilter))
                        dataFilterID = 0;
                }
            }

            List<Event> events = dataFilter == null ?
                new List<Event>() :
                archiveHolder.GetEvents(archiveBit, timeRange, dataFilter);
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
        private void WriteEvent(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            int index = ArgumentIndex;
            int archiveMask = GetInt32(request.Buffer, ref index);
            Event ev = GetEvent(request.Buffer, ref index);
            coreLogic.WriteEvent(archiveMask, ev);

            response = new ResponsePacket(request, client.OutBuf) { ArgumentLength = 8 } ;
            CopyInt64(ev.EventID, client.OutBuf, ArgumentIndex);
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        private void AckEvent(ConnectedClient client, DataPacket request, out ResponsePacket response)
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
        private void SendCommand(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;

            TeleCommand command = new TeleCommand
            {
                UserID = GetInt32(buffer, ref index),
                CnlNum = GetInt32(buffer, ref index),
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
            CopyBool(commandResult.TransmitToClients, buffer, ref index);
            CopyString(commandResult.ErrorMessage, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Gets a telecontrol command from the server queue.
        /// </summary>
        private void GetCommand(ConnectedClient client, DataPacket request, out ResponsePacket response)
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
                CopyInt32(command.CnlNum, buffer, ref index);
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
        private void DisableGettingCommands(ConnectedClient client, DataPacket request, out ResponsePacket response)
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
                "Сервер " + EngineUtils.AppVersion :
                "Server " + EngineUtils.AppVersion;
        }

        /// <summary>
        /// Gets a value indicating whether the server is ready.
        /// </summary>
        protected override bool ServerIsReady()
        {
            return coreLogic.IsReady;
        }

        /// <summary>
        /// Performs actions when initializing the connected client.
        /// </summary>
        protected override void OnClientInit(ConnectedClient client)
        {
            client.Tag = new ClientTag();
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
        /// Gets the role name of the connected client.
        /// </summary>
        protected override string GetRoleName(ConnectedClient client)
        {
            Role role = client == null ? null : coreLogic.ConfigDatabase.RoleTable.GetItem(client.RoleID);
            return role == null ? "" : role.Name;
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

                case FunctionID.GetTimestamps:
                    GetTimestamps(client, request, out response);
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
        /// Gets the information associated with the specified file.
        /// </summary>
        protected override ShortFileInfo GetFileInfo(ConnectedClient client, RelativePath path)
        {
            switch (path.TopFolder)
            {
                case TopFolder.Base:
                    return coreLogic.ConfigDatabase.TableMap.ContainsKey(path.Path)
                        ? new ShortFileInfo
                        {
                            Exists = true,
                            LastWriteTime = coreLogic.ConfigDatabase.BaseTimestamp,
                            Length = 0
                        }
                        : ShortFileInfo.FileNotExists;

                case TopFolder.View:
                    return coreLogic.Storage.GetFileInfo(DataCategory.View, path.Path);

                default:
                    throw new ProtocolException(ErrorCode.IllegalFunctionArguments, CommonPhrases.PathNotSupported);
            }
        }

        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        protected override BinaryReader OpenRead(ConnectedClient client, RelativePath path)
        {
            switch (path.TopFolder)
            {
                case TopFolder.Base:
                    if (coreLogic.ConfigDatabase.TableMap.TryGetValue(path.Path, out IBaseTable baseTable))
                    {
                        BaseTableAdapter adapter = new BaseTableAdapter { Stream = new MemoryStream() };
                        adapter.Update(baseTable);
                        adapter.Stream.Position = 0;
                        return new BinaryReader(adapter.Stream, Encoding.UTF8, false);
                    }
                    else
                    {
                        throw new ScadaException(CommonPhrases.NamedFileNotFound, path);
                    }

                case TopFolder.View:
                    return coreLogic.Storage.OpenBinary(DataCategory.View, path.Path);

                default:
                    throw new ProtocolException(ErrorCode.IllegalFunctionArguments, CommonPhrases.PathNotSupported);
            }
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
