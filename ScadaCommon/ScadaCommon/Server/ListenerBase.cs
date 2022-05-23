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
 * Module   : ScadaCommon
 * Summary  : Represents the base class for TCP listeners which waits for client connections
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Lang;
using Scada.Log;
using Scada.Protocol;
using Scada.Security;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static Scada.BinaryConverter;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Server
{
    /// <summary>
    /// Represents the base class for TCP listeners which waits for client connections.
    /// <para>Представляет базовый класс TCP-прослушивателей, которые ожидают подключения клиентов.</para>
    /// </summary>
    public abstract class ListenerBase
    {
        /// <summary>
        /// The maximum number of client sessions.
        /// </summary>
        protected const int MaxSessionCount = 100;
        /// <summary>
        /// The maximum number of attempts to get a unique session ID.
        /// </summary>
        protected const int MaxGetSessionIDAttempts = 100;
        /// <summary>
        /// The delay of communication if an invalid password is detected, ms.
        /// </summary>
        protected const int WrongPasswordDelay = 500;
        /// <summary>
        /// The maximum number of failed login attempts per minute.
        /// </summary>
        protected const int MaxFailsPerMinute = 100;
        /// <summary>
        /// The duration of login blocking, min.
        /// </summary>
        protected const int BlockingDuration = 1;
        /// <summary>
        /// The period of disconnection of inactive clients.
        /// </summary>
        protected static readonly TimeSpan DisconnectPeriod = TimeSpan.FromSeconds(5);
        /// <summary>
        /// The time after which an inactive client is disconnected.
        /// </summary>
        protected static readonly TimeSpan ClientLifetime = TimeSpan.FromSeconds(60);

        /// <summary>
        /// The listener options.
        /// </summary>
        protected readonly ListenerOptions listenerOptions;
        /// <summary>
        /// The listener log.
        /// </summary>
        protected readonly ILog log;

        /// <summary>
        /// Listens for TCP connections.
        /// </summary>
        protected TcpListener tcpListener;
        /// <summary>
        /// The connected clients accessed by session ID.
        /// </summary>
        protected ConcurrentDictionary<long, ConnectedClient> clients;
        /// <summary>
        /// Protects against password brute forcing.
        /// </summary>
        protected BruteForceProtector protector;
        /// <summary>
        /// The working thread of the listener.
        /// </summary>
        protected Thread thread;
        /// <summary>
        /// Necessary to stop the thread.
        /// </summary>
        protected volatile bool terminated;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ListenerBase(ListenerOptions listenerOptions, ILog log)
        {
            this.listenerOptions = listenerOptions ?? throw new ArgumentNullException(nameof(listenerOptions));
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            tcpListener = null;
            clients = null;
            protector = null;
            thread = null;
            terminated = false;

            CustomFunctions = null;
        }


        /// <summary>
        /// Gets or sets the standard function IDs that require custom processing by a derived listener.
        /// </summary>
        protected HashSet<int> CustomFunctions { get; set; }


        /// <summary>
        /// Prepares the listener for operating.
        /// </summary>
        protected void PrepareListener()
        {
            tcpListener = new TcpListener(IPAddress.Any, listenerOptions.Port);
            tcpListener.Start();

            clients = new ConcurrentDictionary<long, ConnectedClient>();
            protector = new BruteForceProtector(MaxFailsPerMinute, BlockingDuration);
            protector.BlockedChanged += (object sender, BlockedChangedEventArgs e) =>
            {
                log.WriteMessage(e.Message, e.Blocked ? LogMessageType.Warning : LogMessageType.Action);
            };
            terminated = false;
        }

        /// <summary>
        /// Finalizes the listener operating.
        /// </summary>
        protected void FinalizeListener()
        {
            DisconnectAll();
            tcpListener.Stop();
            tcpListener = null;

            clients = null;
            protector = null;
            thread = null;
        }

        /// <summary>
        /// Work cycle running in a separate thread.
        /// </summary>
        protected void Execute()
        {
            DateTime disconnectDT = DateTime.MinValue;

            while (!terminated)
            {
                try
                {
                    // connect new clients
                    while (tcpListener.Pending() && !terminated && ServerIsReady() &&
                        CreateSession(out ConnectedClient client))
                    {
                        TcpClient tcpClient = tcpListener.AcceptTcpClient();
                        tcpClient.SendTimeout = listenerOptions.Timeout;
                        tcpClient.ReceiveTimeout = listenerOptions.Timeout;

                        Thread clientThread = new Thread(ClientExecute);
                        client.Init(tcpClient, clientThread);
                        OnClientInit(client);
                        log.WriteAction(Locale.IsRussian ?
                            "Клиент {0} подключился" :
                            "Client {0} connected", client.Address);
                        clientThread.Start(client);
                    }

                    // disconnect inactive clients
                    DateTime utcNow = DateTime.UtcNow;
                    if (utcNow - disconnectDT >= DisconnectPeriod)
                    {
                        disconnectDT = utcNow;
                        RemoveInactiveSessions();
                    }

                    // custom processing
                    OnIteration();
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, Locale.IsRussian ?
                        "Ошибка в цикле работы прослушивателя" :
                        "Error in the listener work cycle");
                }
                finally
                {
                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }
        }

        /// <summary>
        /// Work cycle of a client.
        /// </summary>
        protected void ClientExecute(object clientArg)
        {
            ConnectedClient client = (ConnectedClient)clientArg;

            while (!client.Terminated)
            {
                ReceiveData(client);
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }

        /// <summary>
        /// Creates a new session.
        /// </summary>
        protected bool CreateSession(out ConnectedClient client)
        {
            long sessionID = 0;
            bool sessionOK = false;
            client = null;

            if (clients.Count < MaxSessionCount)
            {
                client = new ConnectedClient();
                sessionID = ScadaUtils.GetRandomLong();
                int attemptNum = 0;
                bool duplicated;

                while (duplicated = sessionID == 0 ||
                    ++attemptNum <= MaxGetSessionIDAttempts && !clients.TryAdd(sessionID, client))
                {
                    sessionID = ScadaUtils.GetRandomLong();
                }

                sessionOK = !duplicated;
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
        /// Removes the inactive sessions.
        /// </summary>
        protected void RemoveInactiveSessions()
        {
            DateTime utcNow = DateTime.UtcNow;
            List<long> keysToRemove = new List<long>();

            foreach (KeyValuePair<long, ConnectedClient> pair in clients)
            {
                if (utcNow - pair.Value.ActivityTime > ClientLifetime || pair.Value.Terminated)
                    keysToRemove.Add(pair.Key);
            }

            foreach (long key in keysToRemove)
            {
                if (clients.TryRemove(key, out ConnectedClient value))
                    DisconnectClient(value);
            }
        }

        /// <summary>
        /// Disconnects the client.
        /// </summary>
        protected void DisconnectClient(ConnectedClient client)
        {
            try
            {
                client.Terminated = true;
                client.JoinThread();
                client.Disconnect();
                OnClientDisconnect(client);

                log.WriteAction(Locale.IsRussian ?
                    "Клиент {0} отключился" :
                    "Client {0} disconnected", client.Address);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при отключении клиента {0}" :
                    "Error disconnecting client {0}", client.Address);
            }
        }

        /// <summary>
        /// Disconnects all clients.
        /// </summary>
        protected void DisconnectAll()
        {
            try
            {
                ICollection<ConnectedClient> clientList = clients.Values; // make a snapshot

                foreach (ConnectedClient client in clientList)
                {
                    client.Terminated = true;
                }

                foreach (ConnectedClient client in clientList)
                {
                    client.JoinThread();
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при отключении всех клиентов" :
                    "Error disconnecting all clients");
            }
        }

        /// <summary>
        /// Receives data from the client.
        /// </summary>
        protected void ReceiveData(ConnectedClient client)
        {
            try
            {
                if (client.NetStream.DataAvailable &&
                    ReceiveDataPacket(client, out DataPacket request))
                {
                    client.RegisterActivity();
                    ProcessRequest(client, request);
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при приёме данных от клиента {0}" :
                    "Error receiving data from client {0}", client.Address);
            }
        }

        /// <summary>
        /// Receives a data packet from the client.
        /// </summary>
        protected bool ReceiveDataPacket(ConnectedClient client, out DataPacket dataPacket)
        {
            bool formatError = true;
            string errDescr = "";
            byte[] buffer = client.InBuf;
            int bytesToRead = HeaderLength + 2;
            int bytesRead = client.NetStream.Read(buffer, 0, bytesToRead);
            dataPacket = null;

            if (bytesRead == bytesToRead)
            {
                DataPacket request = new DataPacket
                {
                    TransactionID = BitConverter.ToUInt16(buffer, 0),
                    DataLength = BitConverter.ToInt32(buffer, 2),
                    SessionID = BitConverter.ToInt64(buffer, 6),
                    FunctionID = BitConverter.ToUInt16(buffer, 14),
                    Buffer = buffer
                };

                if (request.DataLength + 6 > buffer.Length)
                {
                    errDescr = Locale.IsRussian ?
                        "длина данных слишком велика" :
                        "data length is too big";
                }
                else if (!(request.SessionID == 0 && request.FunctionID == FunctionID.GetSessionInfo ||
                    request.SessionID != 0 && request.SessionID == client.SessionID))
                {
                    errDescr = Locale.IsRussian ?
                        "неверный идентификатор сессии" :
                        "incorrect session ID";
                }
                else
                {
                    // read the rest of the data
                    bytesToRead = request.DataLength - 10;
                    bytesRead = client.ReadData(HeaderLength + 2, bytesToRead);

                    if (bytesRead == bytesToRead)
                    {
                        formatError = false;
                        dataPacket = request;
                    }
                    else
                    {
                        errDescr = Locale.IsRussian ?
                            "не удалось прочитать все данные" :
                            "unable to read all data";
                    }
                }
            }
            else
            {
                errDescr = Locale.IsRussian ?
                    "не удалось прочитать заголовок пакета данных" :
                    "unable to read data packet header";
            }

            if (formatError)
            {
                log.WriteError(Locale.IsRussian ?
                    "Некорректный формат данных, полученных от клиента {0}: {1}" :
                    "Incorrect format of data received from client {0}: {1}",
                    client.Address, errDescr);

                ClearNetStream(client.NetStream, buffer);
            }

            return dataPacket != null;
        }

        /// <summary>
        /// Processes an incoming request already stored in the client input buffer.
        /// </summary>
        protected void ProcessRequest(ConnectedClient client, DataPacket request)
        {
            ResponsePacket response = null; // response to send

            try
            {
                // check whether the client is logged in
                if (!client.IsLoggedIn && FunctionID.RequiresLoggedIn(request.FunctionID))
                {
                    throw new ProtocolException(ErrorCode.AccessDenied, Locale.IsRussian ?
                        "Требуется вход в систему." :
                        "Login required.");
                }

                // process standard request
                bool handled = true; // request was handled

                if (CustomFunctions == null || !CustomFunctions.Contains(request.FunctionID))
                {
                    switch (request.FunctionID)
                    {
                        case FunctionID.GetSessionInfo:
                            GetSessionInfo(client, request, out response);
                            break;

                        case FunctionID.Login:
                            Login(client, request, out response);
                            break;

                        case FunctionID.GetStatus:
                            GetStatus(client, request, out response);
                            break;

                        case FunctionID.TerminateSession:
                            TerminateSession(client, request);
                            break;

                        case FunctionID.GetFileList:
                            GetFileList(client, request, out response);
                            break;

                        case FunctionID.GetFileInfo:
                            GetFileInfo(client, request, out response);
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
                else
                {
                    handled = false;
                }

                // process custom request
                if (!handled)
                {
                    ProcessCustomRequest(client, request, out response, out handled);

                    if (!handled)
                    {
                        throw new ProtocolException(ErrorCode.IllegalFunction, Locale.IsRussian ?
                            "Недопустимая функция." :
                            "Illegal function.");
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при обработке запроса 0x{0} для клиента {1}" :
                    "Error processing request 0x{0} for client {1}",
                    request.FunctionID.ToString("X4"), client.Address);

                response = new ResponsePacket(request, client.OutBuf);
                response.SetError(ex is ProtocolException pe ? pe.ErrorCode : ErrorCode.InternalServerError);
            }

            // send response
            if (response != null)
                client.SendResponse(response);
        }

        /// <summary>
        /// Gets the information about the current session.
        /// </summary>
        protected void GetSessionInfo(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] outBuf = client.OutBuf;
            response = new ResponsePacket(request, outBuf) { SessionID = client.SessionID };
            int index = ArgumentIndex;
            CopyUInt16(ProtocolVersion, outBuf, ref index);
            CopyString(GetServerName(), outBuf, ref index);
            CopyString(GetServerStamp(client.SessionID, listenerOptions.SecretKey), outBuf, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Performs a login function.
        /// </summary>
        protected void Login(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = client.InBuf;
            int index = ArgumentIndex;
            string username = GetString(buffer, ref index);
            string encryptedPassword = GetString(buffer, ref index);
            string instance = GetString(buffer, ref index);

            buffer = client.OutBuf;
            response = new ResponsePacket(request, buffer);
            index = ArgumentIndex;

            if (!protector.IsBlocked(out string errMsg) &&
                DecryptPassword(encryptedPassword, client, out string password, out errMsg) &&
                ValidateUser(client, username, password, instance, out int userID, out int roleID, out errMsg))
            {
                CopyBool(true, buffer, ref index);
                CopyInt32(userID, buffer, ref index);
                CopyInt32(roleID, buffer, ref index);
                CopyString("", buffer, ref index);
            }
            else
            {
                CopyBool(false, buffer, ref index);
                CopyInt32(0, buffer, ref index);
                CopyInt32(0, buffer, ref index);
                CopyString(errMsg, buffer, ref index);

                protector.RegisterFailedLogin();
                Thread.Sleep(WrongPasswordDelay);
            }

            response.BufferLength = index;
        }

        /// <summary>
        /// Decrypts the password.
        /// </summary>
        protected bool DecryptPassword(string encryptedPassword, ConnectedClient client, 
            out string password, out string errMsg)
        {
            try
            {
                password = ProtocolUtils.DecryptPassword(encryptedPassword, client.SessionID, listenerOptions.SecretKey);
                errMsg = "";
                return true;
            }
            catch
            {
                log.WriteError(Locale.IsRussian ?
                    "Не удалось расшифровать пароль, предоставленный клиентом {0}" :
                    "Unable to decrypt password provided by client {0}", client.Address);

                password = "";
                errMsg = Locale.IsRussian ?
                    "Не удалось расшифровать пароль." :
                    "Unable to decrypt password.";
                return false;
            }
        }

        /// <summary>
        /// Gets the server and the session status.
        /// </summary>
        protected void GetStatus(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] outBuf = client.OutBuf;
            response = new ResponsePacket(request, outBuf);
            int index = ArgumentIndex;
            CopyBool(ServerIsReady(), outBuf, ref index);
            CopyBool(client.IsLoggedIn, outBuf, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Terminates the client session.
        /// </summary>
        protected void TerminateSession(ConnectedClient client, DataPacket request)
        {
            ResponsePacket response = new ResponsePacket(request, client.OutBuf);
            client.SendResponse(response);
            client.Terminated = true;
        }

        /// <summary>
        /// Gets a list of short file names in the specified path.
        /// </summary>
        protected void GetFileList(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] buffer = client.InBuf;
            int index = ArgumentIndex;
            ICollection<string> fileList = GetFileList(client,
                GetBool(buffer, ref index),
                GetFilePath(buffer, ref index),
                GetString(buffer, ref index));

            buffer = client.OutBuf;
            response = new ResponsePacket(request, buffer);
            index = ArgumentIndex;
            int itemIndex = 0;
            ushort itemCount = (ushort)fileList.Count;
            CopyUInt16(itemCount, buffer, ref index);

            foreach (string fileName in fileList)
            {
                CopyString(fileName, buffer, ref index);

                if (++itemIndex >= itemCount)
                    break;
            }

            response.BufferLength = index;
        }

        /// <summary>
        /// Gets the information associated with the file.
        /// </summary>
        protected void GetFileInfo(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            int index = ArgumentIndex;
            RelativePath filePath = GetFilePath(client.InBuf, ref index);
            ShortFileInfo fileInfo = GetFileInfo(client, filePath);

            byte[] outBuf = client.OutBuf;
            response = new ResponsePacket(request, outBuf);
            index = ArgumentIndex;
            CopyBool(fileInfo.Exists, outBuf, ref index);
            CopyTime(fileInfo.LastWriteTime, outBuf, ref index);
            CopyInt64(fileInfo.Length, outBuf, ref index);
            response.BufferLength = index;
        }

        /// <summary>
        /// Gets a relative file path from the buffer.
        /// </summary>
        protected RelativePath GetFilePath(byte[] buffer, ref int index)
        {
            return new RelativePath(
                GetInt32(buffer, ref index),
                GetString(buffer, ref index));
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        protected void DownloadFile(ConnectedClient client, DataPacket request)
        {
            byte[] buffer = client.InBuf;
            int index = ArgumentIndex;
            RelativePath filePath = GetFilePath(buffer, ref index);
            long offset = GetInt64(buffer, ref index);
            int count = GetInt32(buffer, ref index);
            SeekOrigin origin = buffer[index++] == 0 ? SeekOrigin.Begin : SeekOrigin.End;
            DateTime newerThan = GetTime(buffer, ref index);
            ShortFileInfo fileInfo = GetFileInfo(client, filePath);

            if (!fileInfo.Exists)
            {
                ResponsePacket response = CreateDownloadResponse(request, client.OutBuf,
                    1, 1, DateTime.MinValue, FileReadingResult.FileNotFound, 0);
                client.SendResponse(response);
            }
            else if (newerThan > DateTime.MinValue && newerThan >= fileInfo.LastWriteTime)
            {
                ResponsePacket response = CreateDownloadResponse(request, client.OutBuf,
                    1, 1, fileInfo.LastWriteTime, FileReadingResult.FileOutdated, 0);
                client.SendResponse(response);
            }
            else
            {
                using (BinaryReader reader = OpenRead(client, filePath))
                {
                    Stream stream = reader.BaseStream;

                    // set file reading position
                    if (offset > 0 || origin == SeekOrigin.End)
                    {
                        if (!stream.CanSeek)
                        {
                            throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                                "Поток не поддерживает поиск." :
                                "Stream does not support seeking.");
                        }

                        offset = Math.Min(offset, stream.Length);
                        stream.Seek(origin == SeekOrigin.Begin ? offset : -offset, origin);
                    }

                    // prepare for reading
                    const int FileDataIndex = ArgumentIndex + 21;
                    const int BlockCapacity = BufferLenght - FileDataIndex;
                    long bytesToReadTotal = stream.CanSeek 
                        ? count > 0 ? Math.Min(count, stream.Length - stream.Position) : stream.Length
                        : count;
                    long bytesReadTotal = 0;
                    int blockNumber = 1;
                    int blockCount = (int)Math.Ceiling((double)bytesToReadTotal / BlockCapacity);
                    bool endOfFile = false;

                    while (!endOfFile)
                    {
                        if (client.Terminated)
                        {
                            throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                                "Операция отменена." :
                                "Operation cancelled.");
                        }

                        // read from file
                        int bytesToRead = bytesToReadTotal > 0
                            ? (int)Math.Min(bytesToReadTotal - bytesReadTotal, BlockCapacity) 
                            : BlockCapacity;
                        int bytesRead = stream.Read(client.OutBuf, FileDataIndex, bytesToRead);
                        bytesReadTotal += bytesRead;
                        endOfFile = bytesRead < bytesToRead || 
                            bytesToReadTotal > 0 && bytesReadTotal == bytesToReadTotal;

                        // send response
                        ResponsePacket response = CreateDownloadResponse(
                            request, client.OutBuf, blockNumber, blockCount, fileInfo.LastWriteTime,
                            endOfFile ? FileReadingResult.Completed : FileReadingResult.BlockRead, bytesRead);
                        client.SendResponse(response);
                        blockNumber++;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a response to the file download request.
        /// </summary>
        protected ResponsePacket CreateDownloadResponse(DataPacket request, byte[] buffer,
            int blockNumber, int blockCount, DateTime lastWriteTime, FileReadingResult fileReadingResult, int bytesRead)
        {
            ResponsePacket response = new ResponsePacket(request, buffer);
            int index = ArgumentIndex;
            CopyInt32(blockNumber, buffer, ref index);
            CopyInt32(blockCount, buffer, ref index);
            CopyTime(lastWriteTime, buffer, ref index);
            CopyByte((byte)fileReadingResult, buffer, ref index);
            CopyInt32(bytesRead, buffer, ref index);
            response.BufferLength = index + bytesRead;
            return response;
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        protected void UploadFile(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            DecodeUploadPacket(request, out int blockNumber, out _, 
                out RelativePath filePath, out FileUploadState uploadState, out _, out _);

            if (blockNumber != 0)
                ThrowBlockNumberException();

            // check whether file is accepted
            bool fileAccepted = AcceptUpload(client, filePath, out object uploadContext);
            response = new ResponsePacket(request, client.OutBuf) { ArgumentLength = 1 };
            CopyBool(fileAccepted, client.OutBuf, ArgumentIndex);
            client.SendResponse(response);

            // receive file
            if (fileAccepted)
            {
                log.WriteAction(Locale.IsRussian ?
                    "Приём файла {0}" :
                    "Receive file {0}", filePath);

                response = new ResponsePacket(request, client.OutBuf);

                try
                {
                    using (BinaryWriter writer = OpenWrite(client, filePath, uploadContext))
                    {
                        while (uploadState == FileUploadState.DataAvailable && !client.Terminated)
                        {
                            // wait for data
                            DateTime endWaitingDT = DateTime.UtcNow.AddMilliseconds(client.TcpClient.ReceiveTimeout);
                            while (!client.NetStream.DataAvailable && DateTime.UtcNow <= endWaitingDT)
                            {
                                Thread.Sleep(ScadaUtils.ThreadDelay);
                            }

                            // receive next data packet
                            if (ReceiveDataPacket(client, out DataPacket dataPacket))
                            {
                                DecodeUploadPacket(dataPacket, out int newBlockNumber, out int blockCount, 
                                    out RelativePath path, out uploadState, out int bytesToWrite, out int fileDataIndex);

                                if (dataPacket.TransactionID != request.TransactionID)
                                {
                                    throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                                        "Неверный идентификатор транзакции." :
                                        "Invalid transaction ID.");
                                }

                                if (dataPacket.FunctionID != FunctionID.UploadFile)
                                {
                                    throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                                        "Ожидалась операция передачи файла." :
                                        "File upload operation expected.");
                                }

                                if (++blockNumber != newBlockNumber)
                                    ThrowBlockNumberException();

                                // write to destination file
                                writer.Write(client.InBuf, fileDataIndex, bytesToWrite);
                            }
                            else
                            {
                                throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                                    "Данные файла отсутствуют." :
                                    "No file data.");
                            }
                        }
                    }

                    if (uploadState == FileUploadState.UploadCanceled || client.Terminated)
                    {
                        throw new ProtocolException(ErrorCode.OperationCanceled, Locale.IsRussian ?
                            "Операция отменена." :
                            "Operation canceled.");
                    }
                }
                catch
                {
                    BreakWriting(client, filePath, uploadContext);
                    throw;
                }

                ProcessFile(client, filePath, uploadContext);
            }
            else
            {
                log.WriteAction(Locale.IsRussian ?
                    "Загрузка файла отклонена. Имя файла: {0}" :
                    "File upload rejected. File name: {0}", filePath);
                response = null;
            }
        }

        /// <summary>
        /// Decodes the file upload data packet.
        /// </summary>
        protected void DecodeUploadPacket(DataPacket dataPacket,
            out int blockNumber, out int blockCount, out RelativePath filePath,
            out FileUploadState uploadState, out int bytesToWrite, out int fileDataIndex)
        {
            byte[] buffer = dataPacket.Buffer;
            int index = ArgumentIndex;
            blockNumber = GetInt32(buffer, ref index);

            if (blockNumber == 0)
            {
                blockCount = GetInt32(buffer, ref index);
                filePath = GetFilePath(buffer, ref index);
                uploadState = FileUploadState.DataAvailable;
                bytesToWrite = 0;
                fileDataIndex = -1;
            }
            else
            {
                blockCount = 0;
                filePath = RelativePath.Empty;
                uploadState = (FileUploadState)GetByte(buffer, ref index);
                bytesToWrite = GetInt32(buffer, ref index);
                fileDataIndex = index;
            }
        }

        /// <summary>
        /// Gets the server name and version.
        /// </summary>
        protected abstract string GetServerName();

        /// <summary>
        /// Gets a value indicating whether the server is ready.
        /// </summary>
        protected virtual bool ServerIsReady()
        {
            return true;
        }

        /// <summary>
        /// Performs actions when starting the listener.
        /// </summary>
        protected virtual void OnListenerStart()
        {
        }

        /// <summary>
        /// Performs actions when stopping the listener.
        /// </summary>
        protected virtual void OnListenerStop()
        {
        }

        /// <summary>
        /// Performs actions on a new iteration of the work cycle.
        /// </summary>
        protected virtual void OnIteration()
        {
        }

        /// <summary>
        /// Performs actions when initializing the connected client.
        /// </summary>
        protected virtual void OnClientInit(ConnectedClient client)
        {
        }

        /// <summary>
        /// Performs actions when disconnecting the client.
        /// </summary>
        protected virtual void OnClientDisconnect(ConnectedClient client)
        {
        }

        /// <summary>
        /// Validates the username and password.
        /// </summary>
        protected virtual bool ValidateUser(ConnectedClient client, string username, string password, string instance,
            out int userID, out int roleID, out string errMsg)
        {
            client.IsLoggedIn = true;
            client.Username = username;

            userID = 0;
            roleID = 0;
            errMsg = "";
            return true;
        }

        /// <summary>
        /// Gets the role name of the connected client.
        /// </summary>
        protected virtual string GetRoleName(ConnectedClient client)
        {
            return client == null ? "" : client.RoleID.ToString();
        }

        /// <summary>
        /// Processes the incoming request by a derived class.
        /// </summary>
        protected virtual void ProcessCustomRequest(ConnectedClient client, DataPacket request,
            out ResponsePacket response, out bool handled)
        {
            response = null;
            handled = false;
        }

        /// <summary>
        /// Gets a list of short names of files or directories in the specified path.
        /// </summary>
        protected virtual ICollection<string> GetFileList(ConnectedClient client, 
            bool searchForFiles, RelativePath path, string searchPattern)
        {
            return Array.Empty<string>();
        }

        /// <summary>
        /// Gets the information associated with the specified file.
        /// </summary>
        protected virtual ShortFileInfo GetFileInfo(ConnectedClient client, RelativePath path)
        {
            return ShortFileInfo.FileNotExists;
        }

        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        protected virtual BinaryReader OpenRead(ConnectedClient client, RelativePath path)
        {
            throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                "Операция не реализована." :
                "Operation is not implemented.");
        }

        /// <summary>
        /// Accepts or rejects the file upload.
        /// </summary>
        protected virtual bool AcceptUpload(ConnectedClient client, RelativePath path, out object uploadContext)
        {
            uploadContext = null;
            return false;
        }

        /// <summary>
        /// Opens an existing file or creates a new file for writing.
        /// </summary>
        protected virtual BinaryWriter OpenWrite(ConnectedClient client, RelativePath path, object uploadContext)
        {
            throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                "Операция не реализована." :
                "Operation is not implemented.");
        }

        /// <summary>
        /// Breaks writing and deletes the corrupted file.
        /// </summary>
        protected virtual void BreakWriting(ConnectedClient client, RelativePath path, object uploadContext)
        {
        }

        /// <summary>
        /// Processes the successfully uploaded file.
        /// </summary>
        protected virtual void ProcessFile(ConnectedClient client, RelativePath path, object uploadContext)
        {
        }


        /// <summary>
        /// Starts listening.
        /// </summary>
        public bool Start()
        {
            try
            {
                if (thread == null)
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Запуск прослушивателя на порту {0}" :
                        "Start listener on port {0}", listenerOptions.Port);

                    PrepareListener();
                    OnListenerStart();
                    thread = new Thread(Execute);
                    thread.Start();
                }
                else
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Прослушиватель уже запущен" :
                        "Listener is already started");
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при запуске прослушивателя" :
                    "Error starting listener");
                return false;
            }
        }

        /// <summary>
        /// Stops listening.
        /// </summary>
        public void Stop()
        {
            try
            {
                if (thread != null)
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Остановка прослушивателя" :
                        "Stop listener");

                    terminated = true;
                    thread.Join();
                    OnListenerStop();
                    FinalizeListener();

                    log.WriteAction(Locale.IsRussian ?
                        "Прослушиватель остановлен" :
                        "Listener is stopped");
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при остановке прослушивателя" :
                    "Error stopping listener");
            }
        }

        /// <summary>
        /// Appends information about the connected clients to the string builder.
        /// </summary>
        public void AppendClientInfo(StringBuilder sb)
        {
            ICollection<ConnectedClient> clientList = clients?.Values; // make a snapshot
            int clientCount = clientList == null ? 0 : clientList.Count;

            string header = Locale.IsRussian ?
                "Подключенные клиенты (" + clientCount + ")" :
                "Connected Clients (" + clientCount + ")";

            sb.AppendLine(header);
            sb.Append('-', header.Length).AppendLine();

            if (clientCount > 0)
            {
                foreach (ConnectedClient client in clientList)
                {
                    sb.Append("[").Append(client.SessionID).Append("] ").Append(client.Address);

                    if (client.IsLoggedIn)
                    {
                        sb.Append("; ").Append(client.Username);
                        string roleName = GetRoleName(client);

                        if (!string.IsNullOrEmpty(roleName))
                            sb.Append(" (").Append(roleName).Append(")");

                        if (client.Tag?.ToString() is string tagStr && tagStr != "")
                            sb.Append("; ").Append(tagStr);
                    }

                    sb.Append("; ").AppendLine(client.ActivityTime.ToLocalTime().ToLocalizedTimeString());
                }
            }
            else
            {
                sb.AppendLine(Locale.IsRussian ? 
                    "Клиентов нет" : 
                    "No clients");
            }
        }
    }
}
