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
using Scada.Log;
using Scada.Protocol;
using System;
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
        private readonly CoreLogic coreLogic; // the server logic instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerListener(CoreLogic coreLogic, ListenerOptions listenerOptions, ILog log)
            : base(listenerOptions, log)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException("coreLogic");
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
            }
            else
            {
                int[] cnlNums = GetIntArray(buffer, ref index);
                bool useCache = GetBool(buffer, ref index);
                cnlData = coreLogic.GetCurrentData(cnlNums, useCache, out cnlListID);
            }

            byte[] outBuf = client.OutBuf;
            response = new ResponsePacket(request, outBuf);
            index = ArgumentIndex;
            CopyInt64(cnlData == null ? 0 : cnlListID, outBuf, ref index);
            CopyCnlDataArray(cnlData, outBuf, ref index);
            response.BufferLength = index;
            response.Encode();
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
                cnlData[i] = new CnlData(
                    GetDouble(buffer, ref idx2),
                    GetInt32(buffer, ref idx2));
            }

            index += cnlCnt * 16;
            bool applyFormulas = buffer[index] > 0;
            coreLogic.WriteCurrentData(deviceNum, cnlNums, cnlData, applyFormulas);

            response = new ResponsePacket(request, client.OutBuf);
            response.Encode();
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
            out int roleID, out string errMsg)
        {
            if (coreLogic.ValidateUser(username, password, out roleID, out errMsg))
            {
                if (client.IsLoggedIn)
                {
                    log.WriteAction(string.Format(Locale.IsRussian ?
                        "Проверка имени и пароля пользователя {0} успешна" :
                        "Checking username and password of the user {0} is successful", username));
                    return true;
                }
                else if (roleID == RoleID.Application)
                {
                    log.WriteAction(string.Format(Locale.IsRussian ?
                        "Пользователь {0} успешно аутентифицирован" :
                        "The user {0} is successfully authenticated", username));

                    client.IsLoggedIn = true;
                    client.Username = username;
                    return true;
                }
                else
                {
                    errMsg = Locale.IsRussian ?
                        "Недостаточно прав" :
                        "Insufficient rights";
                    log.WriteAction(string.Format(Locale.IsRussian ?
                        "Пользователь {0} имеет недостаточно прав. Требуется роль Приложение" :
                        "The user {0} has insufficient rights. The Application role required", username));
                    return false;
                }
            }
            else
            {
                if (client.IsLoggedIn)
                {
                    log.WriteAction(string.Format(Locale.IsRussian ?
                        "Проверка имени и пароля пользователя {0} с отрицательным результатом: {1}" :
                        "Checking username and password of the user {0} is not successful: {1}", username, errMsg));
                    return false;
                }
                else
                {
                    log.WriteAction(string.Format(Locale.IsRussian ?
                        "Неудачная попытка аутентификации пользователя {0}: {1}" :
                        "Unsuccessful attempt to authenticate the user {0}: {1}", username, errMsg));
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the directory name by ID.
        /// </summary>
        protected override string GetDirectory(int directoryID)
        {
            return @"C:\SCADA-v6\";
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

                case FunctionID.WriteCurrentData:
                    WriteCurrentData(client, request, out response);
                    break;

                default:
                    handled = false;
                    break;
            }
        }
    }
}
