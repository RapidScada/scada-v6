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
 * Module   : ScadaData
 * Summary  : Represents the base class for TCP listeners which waits for client connections
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Log;
using Scada.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Server
{
    /// <summary>
    /// Represents the base class for TCP listeners which waits for client connections.
    /// <para>Представляет базовый класс TCP-прослушивателей, которые ожидают подключения клиентов.</para>
    /// </summary>
    public abstract class BaseListener
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
        /// The maximum number of unsuccessful login attempts per minute.
        /// </summary>
        protected const int MaxLoginPerMinute = 100;
        /// <summary>
        /// The duration of login blocking for security reasons, min.
        /// </summary>
        protected const int LoginBlockDuration = 1;
        /// <summary>
        /// The period of disconnection of inactive clients.
        /// </summary>
        private static readonly TimeSpan DisconnectPeriod = TimeSpan.FromSeconds(5);
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
        /// The queue for protection against password brute forcing.
        /// </summary>
        protected Queue<DateTime> protectionQueue;
        /// <summary>
        /// The login unblocking time (UTC).
        /// </summary>
        protected DateTime loginUnblockDT;
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
        public BaseListener(ListenerOptions listenerOptions, ILog log)
        {
            this.listenerOptions = listenerOptions ?? throw new ArgumentNullException("listenerOptions");
            this.log = log ?? throw new ArgumentNullException("log");

            tcpListener = null;
            clients = null;
            protectionQueue = null;
            loginUnblockDT = DateTime.MinValue;
            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Prepares the listener for operating.
        /// </summary>
        protected void PrepareListener()
        {
            tcpListener = new TcpListener(IPAddress.Any, listenerOptions.Port);
            tcpListener.Start();

            clients = new ConcurrentDictionary<long, ConnectedClient>();
            protectionQueue = new Queue<DateTime>();
            loginUnblockDT = DateTime.MinValue;
            terminated = false;
        }

        /// <summary>
        /// Finalizes the listener operating.
        /// </summary>
        protected void FinalizeListener()
        {
            tcpListener.Stop();
            DisconnectAll();
            tcpListener = null;

            clients = null;
            protectionQueue = null;
            loginUnblockDT = DateTime.MinValue;
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
                    while (tcpListener.Pending() && !terminated && 
                        CreateSession(out ConnectedClient client))
                    {
                        TcpClient tcpClient = tcpListener.AcceptTcpClient();
                        tcpClient.SendTimeout = listenerOptions.Timeout;
                        tcpClient.ReceiveTimeout = listenerOptions.Timeout;

                        Thread clientTread = new Thread(ClientExecute);
                        client.Init(tcpClient, clientTread);
                        log.WriteAction(string.Format(Locale.IsRussian ?
                            "Клиент {0} подключился" :
                            "Client {0} connected", client.Address));
                        clientTread.Start(client);
                    }

                    // disconnect inactive clients
                    DateTime utcNow = DateTime.UtcNow;
                    if (utcNow - disconnectDT >= DisconnectPeriod)
                    {
                        disconnectDT = utcNow;
                        RemoveInactiveSessions();
                    }
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, Locale.IsRussian ?
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
                log.WriteAction(string.Format(Locale.IsRussian ?
                    "Создана сессия с ид. {0}" :
                    "Session with ID {0} created", sessionID));
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
                if (utcNow - pair.Value.ActivityTime > ClientLifetime)
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

                log.WriteAction(string.Format(Locale.IsRussian ?
                    "Клиент {0} отключился" :
                    "Client {0} disconnected", client.Address));
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
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
                log.WriteException(ex, Locale.IsRussian ?
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
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при приёме данных от клиента {0}" :
                    "Error receiving data from the client {0}", client.Address);
            }
        }

        /// <summary>
        /// Receives a data packet from the client.
        /// </summary>
        protected bool ReceiveDataPacket(ConnectedClient client, out DataPacket dataPacket)
        {
            bool formatError = true;
            string errDescr = "";
            byte[] inBuf = client.InBuf;
            int bytesToRead = HeaderLength + 2;
            int bytesRead = client.NetStream.Read(inBuf, 0, bytesToRead);
            dataPacket = null;

            if (bytesRead == bytesToRead)
            {
                DataPacket request = new DataPacket
                {
                    TransactionID = BitConverter.ToUInt16(inBuf, 0),
                    DataLength = BitConverter.ToInt32(inBuf, 2),
                    SessionID = BitConverter.ToInt64(inBuf, 6),
                    FunctionID = BitConverter.ToUInt16(inBuf, 14),
                    Buffer = inBuf
                };

                if (request.DataLength + 6 > inBuf.Length)
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
                log.WriteError(string.Format(Locale.IsRussian ?
                    "Некорректный формат данных, полученных от клиента {0}: {1}" :
                    "Incorrect format of data received from the client {0}: {1}",
                    client.Address, errDescr));

                ClearNetStream(client.NetStream, inBuf);
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
                // process standard request
                bool handled = true; // request was handled

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

                // process custom request
                if (!handled)
                {
                    ProcessCustomRequest(client, request, out response, out handled);

                    if (!handled)
                    {
                        response = new ResponsePacket(request, client.OutBuf);
                        response.SetError(ErrorCode.IllegalFunction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при обработке запроса 0x{0} для клиента {1}" :
                    "Error processing request 0x{0} for the client {1}", 
                    request.FunctionID.ToString("X4"), client.Address);

                response = new ResponsePacket(request, client.OutBuf);
                response.SetError(ex is ProtocolException pe ? pe.ErrorCode : ErrorCode.InternalServerError);
            }

            // send response
            if (response != null)
                client.NetStream.Write(response.Buffer, 0, response.BufferLength);
        }

        /// <summary>
        /// Gets the information about the current session.
        /// </summary>
        protected void GetSessionInfo(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            response = new ResponsePacket(request, client.OutBuf) { SessionID = client.SessionID };
            CopyString(GetServerName(), response.Buffer, ArgumentIndex, out int endIndex);
            response.BufferLength = endIndex;
            response.Encode();
        }

        /// <summary>
        /// Performs a login function.
        /// </summary>
        protected void Login(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] inBuf = client.InBuf;
            int index = ArgumentIndex;
            string username = GetString(inBuf, index, out index);
            string encryptedPassword = GetString(inBuf, index, out index);
            string instance = GetString(inBuf, index, out index);
            string password = DecryptPassword(encryptedPassword, client.SessionID, listenerOptions.SecretKey);

            byte[] outBuf = client.OutBuf;
            response = new ResponsePacket(request, outBuf);
            index = ArgumentIndex;

            if (ProtectBruteForce(out string errMsg) &&
                ValidateUser(client, username, password, instance, out int roleID, out errMsg))
            {
                outBuf[index] = 1;
                BitConverter.GetBytes(roleID).CopyTo(outBuf, index + 1);
                CopyString("", outBuf, index + 5, out index);
            }
            else
            {
                outBuf[index++] = 0;
                outBuf[index++] = 0;
                outBuf[index++] = 0;
                outBuf[index++] = 0;
                outBuf[index++] = 0;
                CopyString(errMsg, outBuf, index, out index);

                RegisterUnsuccessfulLogin();
                Thread.Sleep(WrongPasswordDelay);
            }

            response.BufferLength = index;
            response.Encode();
        }
        
        /// <summary>
        /// Protects the application from brute force attacks.
        /// </summary>
        protected bool ProtectBruteForce(out string errMsg)
        {
            // remove outdated login attempts
            DateTime utcNow = DateTime.UtcNow;
            DateTime startDT = utcNow.AddMinutes(-1);
            int loginCount;

            lock (protectionQueue)
            {
                while (protectionQueue.Count > 0)
                {
                    DateTime loginDT = protectionQueue.Peek();

                    if (loginDT < startDT)
                        protectionQueue.Dequeue();
                    else
                        break;
                }

                loginCount = protectionQueue.Count;
            }

            // block or unblock login
            if (loginUnblockDT > DateTime.MinValue)
            {
                if (loginCount <= MaxLoginPerMinute && utcNow <= loginUnblockDT)
                {
                    loginUnblockDT = DateTime.MinValue;
                    log.WriteAction(Locale.IsRussian ?
                        "Вход в систему разблокирован" :
                        "Login unblocked");
                }
            }
            else if (loginCount > MaxLoginPerMinute)
            {
                loginUnblockDT = utcNow.AddMinutes(LoginBlockDuration);
                log.WriteError(string.Format(Locale.IsRussian ?
                    "Вход в систему заблокирован до {0} в целях безопасности" :
                    "Login blocked until {0} for security reasons",
                    loginUnblockDT.ToLocalizedTimeString()));
            }

            if (loginUnblockDT > DateTime.MinValue)
            {
                errMsg = Locale.IsRussian ?
                    "Вход в систему заблокирован в целях безопасности" :
                    "Login blocked until for security reasons";
                return false;
            }
            else
            {
                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Registers an unsuccessful login attempt.
        /// </summary>
        protected void RegisterUnsuccessfulLogin()
        {
            lock (protectionQueue)
            {
                protectionQueue.Enqueue(DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Gets the server and the session status.
        /// </summary>
        protected void GetStatus(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            byte[] outBuf = client.OutBuf;
            response = new ResponsePacket(request, outBuf);
            outBuf[ArgumentIndex] = (byte)(ServerIsReady() ? 1 : 0);
            outBuf[ArgumentIndex + 1] = (byte)(client.IsLoggedIn ? 1 : 0);
            response.ArgumentLength = 2;
            response.Encode();
        }

        /// <summary>
        /// Terminates the client session.
        /// </summary>
        protected void TerminateSession(ConnectedClient client, DataPacket request)
        {
            if (clients.TryRemove(client.SessionID, out ConnectedClient value))
            {
                ResponsePacket response = new ResponsePacket(request, client.OutBuf) { ArgumentLength = 0 };
                response.Encode();
                client.SendResponse(response);
                DisconnectClient(value);
            }
        }

        /// <summary>
        /// Gets the information about the file.
        /// </summary>
        protected void GetFileInfo(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            string fileName = GetFileName(client.InBuf, ArgumentIndex, out int index);
            FileInfo fileInfo = new FileInfo(fileName);
            byte[] outBuf = client.OutBuf;
            response = new ResponsePacket(request, outBuf);

            if (fileInfo.Exists)
            {
                outBuf[ArgumentIndex] = 1;
                BitConverter.GetBytes(fileInfo.LastWriteTimeUtc.Ticks).CopyTo(outBuf, ArgumentIndex + 1);
                BitConverter.GetBytes(fileInfo.Length).CopyTo(outBuf, ArgumentIndex + 9);
            }
            else
            {
                Array.Clear(outBuf, ArgumentIndex, 17);
            }

            response.ArgumentLength = 17;
            response.Encode();
        }

        /// <summary>
        /// Gets the file name from the buffer.
        /// </summary>
        protected string GetFileName(byte[] buffer, int startIndex, out int endIndex)
        {
            ushort directoryID = BitConverter.ToUInt16(buffer, startIndex);
            string path = GetString(buffer, startIndex + 2, out endIndex);
            return Path.Combine(GetDirectory(directoryID), ScadaUtils.NormalPathSeparators(path));
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        protected void DownloadFile(ConnectedClient client, DataPacket request)
        {
            byte[] inBuf = client.InBuf;
            string fileName = GetFileName(inBuf, ArgumentIndex, out int index);
            long offset = BitConverter.ToInt64(inBuf, index);
            int count = BitConverter.ToInt32(inBuf, index + 8);
            SeekOrigin origin = inBuf[index + 12] == 0 ? SeekOrigin.Begin : SeekOrigin.End;
            DateTime newerThan = GetTime(inBuf, index + 13, out index);
            FileInfo fileInfo = new FileInfo(fileName);

            if (!fileInfo.Exists)
            {
                ResponsePacket response = CreateDownloadResponse(request, client.OutBuf, 
                    1, 1, FileReadingResult.FileNotFound, 0);
                client.SendResponse(response);
            }
            else if (fileInfo.LastAccessTimeUtc <= newerThan)
            {
                ResponsePacket response = CreateDownloadResponse(request, client.OutBuf,
                    1, 1, FileReadingResult.FileOutdated, 0);
                client.SendResponse(response);
            }
            else
            {
                using (FileStream stream =
                    new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    // set file reading position
                    if (offset > 0)
                    {
                        offset = Math.Min(offset, stream.Length);
                        stream.Seek(origin == SeekOrigin.Begin ? offset : -offset, origin);
                    }

                    // prepare for reading
                    const int FileDataIndex = ArgumentIndex + 13;
                    const int BlockCapacity = BufferLenght - FileDataIndex;
                    long bytesToReadTotal = count > 0 ? 
                        Math.Min(count, stream.Length - stream.Position) : stream.Length;
                    long bytesReadTotal = 0;
                    int blockNumber = 1;
                    int blockCount = (int)Math.Ceiling((double)bytesToReadTotal / BlockCapacity);
                    byte[] outBuf = client.OutBuf;
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
                        int bytesToRead = (int)Math.Min(bytesToReadTotal - bytesReadTotal, BlockCapacity);
                        int bytesRead = stream.Read(outBuf, FileDataIndex, bytesToRead);
                        bytesReadTotal += bytesRead;
                        endOfFile = bytesRead < bytesToRead || bytesReadTotal == bytesToReadTotal;

                        // send response
                        ResponsePacket response = CreateDownloadResponse(request, outBuf, blockNumber, blockCount,
                            endOfFile ? FileReadingResult.EndOfFile : FileReadingResult.Successful, bytesRead);
                        client.SendResponse(response);
                        blockNumber++;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a response to the file download request.
        /// </summary>
        protected ResponsePacket CreateDownloadResponse(DataPacket request, byte[] outBuf, 
            int blockNumber, int blockCount, FileReadingResult fileReadingResult, int bytesRead)
        {
            ResponsePacket response = new ResponsePacket(request, outBuf);
            int index = ArgumentIndex;
            BitConverter.GetBytes(blockNumber).CopyTo(outBuf, index);
            BitConverter.GetBytes(blockCount).CopyTo(outBuf, index + 4);
            outBuf[index + 8] = (byte)fileReadingResult;
            BitConverter.GetBytes(bytesRead).CopyTo(outBuf, index + 9);
            response.ArgumentLength = 13 + bytesRead;
            response.Encode();
            return response;
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        protected void UploadFile(ConnectedClient client, DataPacket request, out ResponsePacket response)
        {
            DecodeUploadPacket(request, out int blockNumber, out int blockCount,
                out bool endOfFile, out string fileName, out int bytesToWrite, out int fileDataIndex);

            log.WriteAction(string.Format(Locale.IsRussian ?
                "Приём файла {0}" :
                "Receiving the file {0}", fileName));

            if (!AcceptFileUpload(fileName))
            {
                throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                    "Файл отклонён." :
                    "File rejected.");
            }

            if (blockNumber != 1)
            {
                throw new ProtocolException(ErrorCode.IllegalFunctionArguments, Locale.IsRussian ?
                    "Неверный номер блока." :
                    "Invalid block number.");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(fileName));

            try
            {
                using (FileStream stream =
                    new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    // write to destination file
                    stream.Write(client.InBuf, fileDataIndex, bytesToWrite);

                    while (!endOfFile && !client.Terminated)
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
                            DecodeUploadPacket(dataPacket, out int newBlockNumber, out blockCount,
                                out endOfFile, out fileName, out bytesToWrite, out fileDataIndex);

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
                            {
                                throw new ProtocolException(ErrorCode.IllegalFunctionArguments, Locale.IsRussian ?
                                    "Неверный номер блока." :
                                    "Invalid block number.");
                            }

                            // write to destination file
                            stream.Write(client.InBuf, fileDataIndex, bytesToWrite);
                        }
                        else
                        {
                            throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                                "Данные файла отсутствуют." :
                                "No file data.");
                        }
                    }
                }

                if (client.Terminated)
                {
                    throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                        "Операция отменена." :
                        "Operation cancelled.");
                }
            }
            catch
            {
                // delete file in case of error
                try { File.Delete(fileName); }
                catch { }
                throw;
            }

            response = new ResponsePacket(request, client.OutBuf) { ArgumentLength = 0 };
            response.Encode();
        }

        /// <summary>
        /// Decodes the file upload data packet.
        /// </summary>
        protected void DecodeUploadPacket(DataPacket dataPacket, out int blockNumber, out int blockCount,
            out bool endOfFile, out string fileName, out int bytesToWrite, out int fileDataIndex)
        {
            byte[] buffer = dataPacket.Buffer;
            int index = ArgumentIndex;
            blockNumber = BitConverter.ToInt32(buffer, index);
            blockCount = BitConverter.ToInt32(buffer, index + 4);
            endOfFile = buffer[index + 8] > 0;
            fileName = GetFileName(buffer, index + 9, out index);
            bytesToWrite = BitConverter.ToInt32(buffer, index);
            fileDataIndex = index + 4;
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
        /// Validates the username and password.
        /// </summary>
        protected virtual bool ValidateUser(ConnectedClient client, string username, string password, string instance,
            out int roleID, out string errMsg)
        {
            client.IsLoggedIn = true;
            client.Username = username;

            roleID = 0;
            errMsg = "";
            return true;
        }

        /// <summary>
        /// Gets the directory name by ID.
        /// </summary>
        protected virtual string GetDirectory(ushort directoryID)
        {
            throw new ProtocolException(ErrorCode.InvalidOperation, Locale.IsRussian ?
                "Операция не реализована." :
                "Operation is not implemented.");
        }

        /// <summary>
        /// Accepts or rejects the file upload.
        /// </summary>
        protected virtual bool AcceptFileUpload(string fileName)
        {
            return true;
        }

        /// <summary>
        /// Processes an incoming request by a derived class.
        /// </summary>
        protected virtual void ProcessCustomRequest(ConnectedClient client, DataPacket request, 
            out ResponsePacket response, out bool handled)
        {
            response = null;
            handled = false;
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
                        "Запуск прослушивателя" :
                        "Start listener");

                    PrepareListener();
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
                log.WriteException(ex, Locale.IsRussian ?
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
                    FinalizeListener();

                    log.WriteAction(Locale.IsRussian ?
                        "Прослушиватель остановлен" :
                        "Listener is stopped");
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при остановке прослушивателя" :
                    "Error stopping listener");
            }
        }
    }
}
