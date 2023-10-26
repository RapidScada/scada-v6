/*
 * Copyright 2023 Rapid Software LLC
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
 * Modified : 2023
 */

using Scada.Client;
using Scada.Data.Adapters;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Data.TwoFactorAuth;
using Scada.Lang;
using Scada.Protocol;
using Scada.Storages;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
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
        /// Writes the channel data.
        /// </summary>
        private void WriteChannelData(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            WriteDataFlags flags = (WriteDataFlags)GetByte(buffer, ref index);
            int archiveMask = GetInt32(buffer, ref index);
            int sliceCnt = GetInt32(buffer, ref index);
            bool isCurrent = flags.HasFlag(WriteDataFlags.IsCurrent);
            int maxCurDataAge = coreLogic.AppConfig.GeneralOptions.MaxCurDataAge;
            DateTime utcNow = isCurrent && maxCurDataAge > 0 ? DateTime.UtcNow : DateTime.MinValue;

            for (int i = 0; i < sliceCnt; i++)
            {
                Slice slice = BinaryConverter.GetSlice(buffer, ref index);

                if (isCurrent)
                {
                    if (maxCurDataAge > 0 && slice.Timestamp > DateTime.MinValue &&
                        (utcNow - slice.Timestamp).TotalSeconds > maxCurDataAge)
                    {
                        // write current data as historical
                        coreLogic.WriteHistoricalData(ArchiveMask.Default, slice, flags);
                    }
                    else
                    {
                        // write current data
                        coreLogic.WriteCurrentData(slice, flags);
                    }
                }
                else
                {
                    // write historical data
                    coreLogic.WriteHistoricalData(archiveMask, slice, flags);
                }
            }

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

            EventAck eventAck = new EventAck
            {
                EventID = GetInt64(buffer, ref index),
                Timestamp = GetTime(buffer, ref index),
                UserID = GetInt32(buffer, ref index)
            };

            if (eventAck.UserID <= 0)
                eventAck.UserID = client.UserID;

            coreLogic.AckEvent(eventAck);
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
                ClientName = client.Username,
                UserID = GetInt32(buffer, ref index),
                CnlNum = GetInt32(buffer, ref index),
                CmdVal = GetDouble(buffer, ref index),
                CmdData = GetByteArray(buffer, ref index)
            };

            if (command.UserID <= 0)
                command.UserID = client.UserID;

            WriteCommandFlags flags = (WriteCommandFlags)buffer[index];
            CommandResult commandResult = coreLogic.SendCommand(command, flags);

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
        protected override UserValidationResult ValidateUser(ConnectedClient client,
            string username, string password, string instance)
        {
            UserValidationResult result = coreLogic.ValidateUser(username, password);

            if (result.IsValid)
            {
                if (client.IsLoggedIn)
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Проверка имени и пароля пользователя {0} успешна" :
                        "Checking username and password for user {0} is successful", username);
                }
                else if (result.RoleID == RoleID.Application)
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Пользователь {0} успешно аутентифицирован" :
                        "User {0} is successfully authenticated", username);

                    client.IsLoggedIn = true;
                    client.Username = username;
                    client.UserID = result.UserID;
                    client.RoleID = result.RoleID;
                }
                else
                {
                    result.IsValid = false;
                    result.ErrorMessage = Locale.IsRussian ?
                        "Недостаточно прав" :
                        "Insufficient rights";
                    log.WriteError(Locale.IsRussian ?
                        "Пользователь {0} имеет недостаточно прав. Требуется роль Приложение" :
                        "User {0} has insufficient rights. The Application role required", username);
                }
            }
            else
            {
                if (client.IsLoggedIn)
                {
                    log.WriteError(Locale.IsRussian ?
                        "Результат проверки имени и пароля пользователя {0} отрицательный: {1}" :
                        "Checking username and password for user {0} is not successful: {1}", 
                        username, result.ErrorMessage);
                }
                else
                {
                    log.WriteError(Locale.IsRussian ?
                        "Ошибка аутентификации пользователя {0}: {1}" :
                        "Authentication failed for user {0}: {1}", 
                        username, result.ErrorMessage);
                }
            }

            return result;
        }


        /// <summary>
        /// 网页用户登录认证
        /// </summary>
        protected override WebUserValidationResult ValidateWebUser(ConnectedClient client,
            string username, string password, string loginType, string browserIdentity, string clientIpAddr, string instance)
        {
            WebUserValidationResult result = coreLogic.ValidateWebUser(username, password, loginType, browserIdentity);
            result.ClientIpAddress = clientIpAddr;
            result.UserName = username;
            coreLogic.EnqueueLoginLog(client, result);

            if (result.IsValid)
            {
                if (client.IsLoggedIn)
                {
                    log.WriteAction("Checking web username and password for user {0} is successful", username);
                }
                else if (result.RoleID == RoleID.Application)
                {
                    log.WriteAction("Web user {0} is successfully authenticated", username);

                    client.IsLoggedIn = true;
                    client.Username = username;
                    client.UserID = result.UserID;
                    client.RoleID = result.RoleID;
                }
                else
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Insufficient rights";
                    log.WriteError("Web user {0} has insufficient rights. The Application role required", username);
                }
            }
            else
            {
                if (client.IsLoggedIn)
                {
                    log.WriteError("Checking web username and password for user {0} is not successful: {1}", username, result.ErrorMessage);
                }
                else
                {
                    log.WriteError("Authentication failed for web user {0}: {1}", username, result.ErrorMessage);
                }
            }

            return result;
        }


        /// <summary>
        /// Finds a user by ID.
        /// </summary>
        protected override User FindUser(int userID)
        {
            return coreLogic.FindUser(userID);
        }

        /// <summary>
        /// 获取多重密钥
        /// </summary>
        protected override TwoFactorAuthInfoResult GetTwoFAKey(int userID)
        {
            return coreLogic.GetTwoFAKey(userID);
        }

        /// <summary>
        /// 校验多重认证随机码
        /// </summary>
        protected override TwoFactorAuthValidateResult VerifyTwoFAKey(int userID, int code,bool trustDevice, string browserId)
        {
            return coreLogic.VerifyTwoFAKey(userID, code, trustDevice, browserId);
        }


        /// <summary>
        /// 获取用户列表
        /// </summary>
        private void GetUserList(ConnectedClient client, DataPacket request)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int offset = GetInt32(buffer, ref index);
            int limit = GetInt32(buffer, ref index);
            string username = GetString(buffer, ref index);

            var userTable = coreLogic.ConfigDatabase.UserTable;
            var allUsers = userTable.EnumerateItems().Cast<User>();
            allUsers = allUsers.Where(x => x.UserID > 10);//系统用户默认不显示
            if (!string.IsNullOrEmpty(username))
            {
                allUsers = allUsers.Where(u => u.Name.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            var pageUsers = allUsers.Skip(offset).Take(limit).ToList();

            int allUserCount = allUsers.Count();
            int pageUserCount = pageUsers.Count();
            buffer = client.OutBuf;

            index = ArgumentIndex;
            ResponsePacket response = new ResponsePacket(request, buffer);
            CopyInt32(allUserCount, buffer, ref index); //总量
            CopyInt32(pageUserCount, buffer, ref index); //当前页数量

            for (int i = 0; i < pageUserCount; i++)
            {
                CopyUser(pageUsers[i], buffer, ref index);
            }
            response.BufferLength = index;
            client.SendResponse(response);
        }

        /// <summary>
        /// 获取用户登录日志列表
        /// </summary>
        private void GetUserLoginLogList(ConnectedClient client, DataPacket request)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int offset = GetInt32(buffer, ref index);
            int limit = GetInt32(buffer, ref index);
            string username = GetString(buffer, ref index);

            var userLogTable = coreLogic.ConfigDatabase.UserLoginLogTable;
            var allUserLogs = userLogTable.EnumerateItems().Cast<UserLoginLog>().OrderByDescending(x=>x.LoginTime).ToList();

            var userTable = coreLogic.ConfigDatabase.UserTable;
            var allUsers = userLogTable.EnumerateItems().Cast<User>();
            if (!string.IsNullOrEmpty(username))
            {
                username = username.ToLower();
                //allUsers = allUsers.Where(u => u.Name.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0);
                //var userIdArr = allUsers.Select(x => x.UserID).ToArray();
                allUserLogs = allUserLogs.Where(u => !string.IsNullOrEmpty(u.UserName) && u.UserName.ToLower().Contains(username)).ToList();
            }
            var pageUserLogs = allUserLogs.Skip(offset).Take(limit).ToList();

            int allUserCount = allUserLogs.Count();
            int pageUserCount = pageUserLogs.Count();
            buffer = client.OutBuf;

            index = ArgumentIndex;
            ResponsePacket response = new ResponsePacket(request, buffer);
            CopyInt32(allUserCount, buffer, ref index); //总量
            CopyInt32(pageUserCount, buffer, ref index); //当前页数量

            for (int i = 0; i < pageUserCount; i++)
            {
                CopyUserLoginLog(pageUserLogs[i], buffer, ref index);
            }
            response.BufferLength = index;
            client.SendResponse(response);
        }

        /// <summary>
        /// 下载登录日志，csv格式
        /// </summary>
        private void DownloadUserLoginLog(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            string username = GetString(buffer, ref index);

            var userLogTable = coreLogic.ConfigDatabase.UserLoginLogTable;
            var allUserLogs = userLogTable.EnumerateItems().Cast<UserLoginLog>().OrderByDescending(x => x.LoginTime).ToList();

            var userTable = coreLogic.ConfigDatabase.UserTable;
            var allUsers = userLogTable.EnumerateItems().Cast<User>();
            if (!string.IsNullOrEmpty(username))
            {
                username = username.ToLower();
                //allUsers = allUsers.Where(u => u.Name.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0);
                //var userIdArr = allUsers.Select(x => x.UserID).ToArray();
                allUserLogs = allUserLogs.Where(u => !string.IsNullOrEmpty(u.UserName) && u.UserName.ToLower().Contains(username)).ToList();
            }

            var csvRes = true;
            var resMsg = string.Empty;
            try
            {
                //生成csv文件
                var filePath = Path.Combine(coreLogic.AppDirs.InstanceDir, "TempFile");
                //删除10天前的文件
                if(Directory.Exists(filePath) && Directory.GetCreationTime(filePath) < DateTime.Now.AddDays(-10)) { Directory.Delete(filePath); }
                if(!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                var fileFullName = Path.Combine(filePath,$"UserLoginLog_{DateTime.Now:yyyyMMddHHmmss}.csv");
                using (Stream stream = new FileStream(fileFullName,FileMode.Create,FileAccess.Write))
                {
                    using (var sw = new StreamWriter(stream,Encoding.UTF8))
                    {
                        sw.WriteLine($"ID,User Name,Login IP,Login Time,Login Status,Login Desc");
                        for (int i = 0; i < allUserLogs.Count; i++)
                        {
                            var userLog = allUserLogs[i];
                            sw.WriteLine($"{userLog.Id},{userLog.UserName},{userLog.LoginIP},{userLog.LoginTime:G},{(userLog.LoginStatus== 0 ? "Fail": "Success")},{userLog.LoginDesc}");
                        }
                        sw.Close();
                        stream.Close();
                    }
                }
                //返回文件名
                resMsg = Path.GetFileName(fileFullName);
            }
            catch (Exception ex)
            {
                csvRes = false;
                resMsg = ex.Message;
            }

            buffer = client.OutBuf;
            index = ArgumentIndex;
            response = new ResponsePacket(request, buffer);
            CopyBool(csvRes, buffer, ref index);
            CopyString(resMsg, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// 新增、修改用户
        /// </summary>
        private void AddOrUpdateUser(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            User user = GetUser(buffer, ref index);
            var res = coreLogic.AddOrUpdateUser(user, out string errMsg);

            buffer = client.OutBuf;
            index = ArgumentIndex;
            response = new ResponsePacket(request, buffer);
            CopyBool(res, buffer, ref index);
            CopyInt32(user.UserID, buffer, ref index);
            CopyString(errMsg, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// 启用、禁用用户
        /// </summary>
        private void EnabledUser(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int userID = GetInt32(buffer, ref index);
            bool enable = GetBool(buffer, ref index);

            var res = coreLogic.EnabledUser(userID, enable, out string errMsg);
            buffer = client.OutBuf;
            index = ArgumentIndex;
            response = new ResponsePacket(request, buffer);
            CopyBool(res, buffer, ref index);
            CopyString(errMsg, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// 删除用户（软删除）
        /// </summary>
        private void DeleteUser(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int userID = GetInt32(buffer, ref index);

            var res = coreLogic.DeleteUser(userID, out string errMsg);
            buffer = client.OutBuf;
            index = ArgumentIndex;
            response = new ResponsePacket(request, buffer);
            CopyBool(res, buffer, ref index);
            CopyString(errMsg, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// 重置用户多重认证
        /// </summary>
        private void ResetUserTwoFA(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int userID = GetInt32(buffer, ref index);

            var res = coreLogic.ResetUserTwoFA(userID, out string errMsg);
            buffer = client.OutBuf;
            index = ArgumentIndex;
            response = new ResponsePacket(request, buffer);
            CopyBool(res, buffer, ref index);
            CopyString(errMsg, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        private void ResetUserPwd(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int userID = GetInt32(buffer, ref index);
            string newPwd = GetString(buffer, ref index);

            var res = coreLogic.ResetUserPwd(userID, newPwd, out string errMsg);
            buffer = client.OutBuf;
            index = ArgumentIndex;
            response = new ResponsePacket(request, buffer);
            CopyBool(res, buffer, ref index);
            CopyString(errMsg, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// 修改密码用户
        /// </summary>
        private void ModifyPwdWeb(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int userID = GetInt32(buffer, ref index);
            string oldPwd = GetString(buffer, ref index);
            string newPwd = GetString(buffer, ref index);

            var res = coreLogic.ModifyPwdWeb(userID, oldPwd, newPwd, out string errMsg);
            buffer = client.OutBuf;
            index = ArgumentIndex;
            response = new ResponsePacket(request, buffer);
            CopyBool(res, buffer, ref index);
            CopyString(errMsg, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// 校验密码
        /// </summary>
        private void WebCheckPwd(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = request.Buffer;
            int index = ArgumentIndex;
            int userID = GetInt32(buffer, ref index);

            var res = coreLogic.WebCheckPwd(userID, out string errMsg);
            buffer = client.OutBuf;
            index = ArgumentIndex;
            response = new ResponsePacket(request, buffer);
            CopyBool(res, buffer, ref index);
            CopyString(errMsg, buffer, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Updates the client mode.
        /// </summary>
        protected override void UpdateClientMode(ConnectedClient client, int clientMode)
        {
            GetClientTag(client).CommandsEnabled = new ScadaClientMode(clientMode).EnableIncomingCommands;
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

                case FunctionID.WriteChannelData:
                    WriteChannelData(client, request, out response);
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


                case FunctionID.GetUserList:
                    GetUserList(client, request);
                    break;
                case FunctionID.UserAddOrUpdate:
                    AddOrUpdateUser(client, request, out response);
                    break;
                case FunctionID.UserEnabled:
                    EnabledUser(client, request, out response);
                    break;
                case FunctionID.DeleteUser:
                    DeleteUser(client, request, out response);
                    break;
                case FunctionID.ResetUserTwoFA:
                    ResetUserTwoFA(client, request, out response);
                    break;
                case FunctionID.ResetUserPwd:
                    ResetUserPwd(client, request, out response);
                    break;
                case FunctionID.WebModifyPwd:
                    ModifyPwdWeb(client, request, out response);
                    break;
                case FunctionID.WebCheckPwd:
                    WebCheckPwd(client, request, out response);
                    break;

                case FunctionID.GetUserLoginLogList:
                    GetUserLoginLogList(client, request);
                    break;
                case FunctionID.DownloadUserLoginLog:
                    DownloadUserLoginLog(client, request, out response);
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
        public void EnqueueCommand(TeleCommand command, bool returnToSender)
        {
            foreach (KeyValuePair<long, ConnectedClient> pair in clients)
            {
                ConnectedClient client = pair.Value;

                if (returnToSender ||
                    string.IsNullOrEmpty(command.ClientName) ||
                    !string.Equals(command.ClientName, client.Username, StringComparison.OrdinalIgnoreCase))
                {
                    GetClientTag(client).AddCommand(command);
                }
            }
        }
    }
}
