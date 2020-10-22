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
 * Summary  : Represents the base class for TCP clients which interacts with a server
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Log;
using Scada.Protocol;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using static Scada.BinaryConverter;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Client
{
    /// <summary>
    /// Represents the base class for TCP clients which interacts with a server.
    /// <para>Представляет базовый класс TCP-клиентов, которые взаимодействуют с сервером.</para>
    /// </summary>
    /// <remarks>The class is not thread safe.</remarks>
    public abstract class BaseClient
    {
        /// <summary>
        /// The maximum number of packet bytes to write to the communication log.
        /// </summary>
        protected const int MaxLoggingSize = 100;
        /// <summary>
        /// The period when reconnection is allowed.
        /// </summary>
        protected readonly TimeSpan ReconnectPeriod = TimeSpan.FromSeconds(5);
        /// <summary>
        /// The period of checking connection.
        /// </summary>
        protected readonly TimeSpan PingPeriod = TimeSpan.FromSeconds(30);

        /// <summary>
        /// The connection options.
        /// </summary>
        protected readonly ConnectionOptions connectionOptions;
        /// <summary>
        /// The input data buffer.
        /// </summary>
        protected readonly byte[] inBuf;
        /// <summary>
        /// The output data buffer.
        /// </summary>
        protected readonly byte[] outBuf;

        /// <summary>
        /// The client connection.
        /// </summary>
        protected TcpClient tcpClient;
        /// <summary>
        /// The stream for network access.
        /// </summary>
        protected NetworkStream netStream;
        /// <summary>
        /// The transaction ID counter.
        /// </summary>
        protected ushort transactionID;
        /// <summary>
        /// The connection attempt time (UTC).
        /// </summary>
        protected DateTime connAttemptDT;
        /// <summary>
        /// The last successful response time (UTC).
        /// </summary>
        protected DateTime responseDT;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseClient(ConnectionOptions connectionOptions)
        {
            this.connectionOptions = connectionOptions ?? throw new ArgumentNullException(nameof(connectionOptions));
            inBuf = new byte[BufferLenght];
            outBuf = new byte[BufferLenght];

            tcpClient = null;
            netStream = null;
            transactionID = 0;
            connAttemptDT = DateTime.MinValue;
            responseDT = DateTime.MinValue;

            CommLog = null;
            ClientState = ClientState.Disconnected;
            SessionID = 0;
            ServerName = "";
        }


        /// <summary>
        /// Gets the sets the detailed communication log.
        /// </summary>
        public ILog CommLog { get; set; }

        /// <summary>
        /// Gets the client communication state.
        /// </summary>
        public ClientState ClientState { get; protected set; }

        /// <summary>
        /// Gets the session ID.
        /// </summary>
        public long SessionID { get; protected set; }

        /// <summary>
        /// Gets the server name and version.
        /// </summary>
        public string ServerName { get; protected set; }


        /// <summary>
        /// Connects and authenticates with the server.
        /// </summary>
        protected void Connect()
        {
            // create connection
            tcpClient = new TcpClient
            {
                SendTimeout = connectionOptions.Timeout,
                ReceiveTimeout = connectionOptions.Timeout
            };

            // connect
            CommLog?.WriteAction("Connect to " + connectionOptions.Host);

            if (IPAddress.TryParse(connectionOptions.Host, out IPAddress address))
                tcpClient.Connect(address, connectionOptions.Port);
            else
                tcpClient.Connect(connectionOptions.Host, connectionOptions.Port);

            netStream = tcpClient.GetStream();
            ClientState = ClientState.Connected;
        }

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        protected void Disconnect()
        {
            if (tcpClient != null)
            {
                CommLog?.WriteAction("Disconnect");

                if (netStream != null)
                {
                    ClearNetStream(netStream, inBuf); // to disconnect correctly
                    netStream.Close();
                    netStream = null;
                }

                tcpClient.Close();
                tcpClient = null;

                ClientState = ClientState.Disconnected;
                SessionID = 0;
                ServerName = "";
            }
        }

        /// <summary>
        /// Restores a connection with the server.
        /// </summary>
        protected void RestoreConnection()
        {
            DateTime utcNow = DateTime.UtcNow;
            bool connectNeeded = false;

            if (ClientState >= ClientState.LoggedIn)
            {
                if (utcNow - responseDT > PingPeriod)
                {
                    try
                    {
                        // check connection
                        DataPacket request = CreateRequest(FunctionID.GetStatus, 10);
                        SendRequest(request);
                        ReceiveResponse(request);
                    }
                    catch
                    {
                        connectNeeded = true;
                    }
                }
            }
            else if (utcNow - connAttemptDT > ReconnectPeriod)
            {
                connectNeeded = true;
            }

            if (connectNeeded)
            {
                connAttemptDT = utcNow;
                Disconnect();
                Connect();

                GetSessionInfo(out long sessionID, out ushort protocolVersion, out string serverName);
                SessionID = sessionID;
                ServerName = serverName;

                if (protocolVersion != ProtocolVersion)
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Несовместимая версия протокола." :
                        "Incompatible protocol version.");
                }

                Login(out bool loggedIn, out int userID, out int roleID, out string errorMessage);

                if (loggedIn)
                {
                    ClientState = ClientState.LoggedIn;
                    CommLog?.WriteAction("User is logged in");
                }
                else
                {
                    throw new ScadaException(errorMessage);
                }
            }
            else if (ClientState >= ClientState.LoggedIn)
            {
                ClearNetStream(netStream, inBuf);
            }
            else
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Клиент не вошёл в систему. Попробуйте позже." :
                    "Client is not logged in. Try again later.");
            }
        }

        /// <summary>
        /// Creates a new data packet for request.
        /// </summary>
        protected DataPacket CreateRequest(ushort functionID, int dataLength = 0, bool nextTransaction = true)
        {
            if (nextTransaction)
                transactionID++;

            return new DataPacket
            {
                TransactionID = transactionID,
                DataLength = dataLength,
                SessionID = SessionID,
                FunctionID = functionID,
                Buffer = outBuf
            };
        }

        /// <summary>
        /// Sends the request to the server.
        /// </summary>
        protected void SendRequest(DataPacket request)
        {
            request.Encode();
            netStream.Write(request.Buffer, 0, request.BufferLength);

            if (CommLog != null)
            {
                CommLog.WriteAction(FunctionID.GetName(request.FunctionID));
                CommLog.WriteAction(BuildWritingText(request.Buffer, 0, request.BufferLength));
            }
        }

        /// <summary>
        /// Receives a response from the server.
        /// </summary>
        protected DataPacket ReceiveResponse(DataPacket request)
        {
            DataPacket response = null;
            bool formatError = true;
            string errDescr = "";
            int bytesToRead = HeaderLength + 2;
            int bytesRead = netStream.Read(inBuf, 0, bytesToRead);
            CommLog?.WriteAction(BuildReadingText(inBuf, 0, bytesToRead, bytesRead));

            if (bytesRead == bytesToRead)
            {
                response = new DataPacket
                {
                    TransactionID = BitConverter.ToUInt16(inBuf, 0),
                    DataLength = BitConverter.ToInt32(inBuf, 2),
                    SessionID = BitConverter.ToInt64(inBuf, 6),
                    FunctionID = BitConverter.ToUInt16(inBuf, 14),
                    Buffer = inBuf
                };

                if (response.DataLength + 6 > inBuf.Length)
                {
                    errDescr = Locale.IsRussian ?
                        "длина данных слишком велика" :
                        "data length is too big";
                }
                else if (response.TransactionID != request.TransactionID)
                {
                    errDescr = Locale.IsRussian ?
                        "неверный идентификатор транзакции" :
                        "incorrect transaction ID";
                }
                else if (response.SessionID != SessionID && SessionID != 0)
                {
                    errDescr = Locale.IsRussian ?
                        "неверный идентификатор сессии" :
                        "incorrect session ID";
                }
                else if ((response.FunctionID & 0x7FFF) != request.FunctionID)
                {
                    errDescr = Locale.IsRussian ?
                        "неверный идентификатор функции" :
                        "incorrect function ID";
                }
                else
                {
                    // read the rest of the data
                    bytesToRead = response.DataLength - 10;
                    bytesRead = ReadData(netStream, tcpClient.ReceiveTimeout, inBuf, HeaderLength + 2, bytesToRead);
                    CommLog?.WriteAction(BuildReadingText(inBuf, HeaderLength + 2, bytesToRead, bytesRead));

                    if (bytesRead == bytesToRead)
                    {
                        formatError = false;
                        responseDT = DateTime.UtcNow;

                        // handle error response
                        if ((response.FunctionID & 0x8000) > 0)
                        {
                            ErrorCode errorCode = (ErrorCode)inBuf[ArgumentIndex];
                            CommLog?.WriteError(errorCode.ToString());
                            throw new ProtocolException(errorCode);
                        }
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
                throw new ScadaException(Locale.IsRussian ?
                    "Некорректный формат данных, полученных от сервера: {0}" :
                    "Incorrect format of data received from the server: {0}", errDescr);
            }

            return response;
        }

        /// <summary>
        /// Builds text for reading logging.
        /// </summary>
        protected string BuildReadingText(byte[] buffer, int index, int bytesToRead, int bytesRead)
        {
            return "Receive (" + bytesRead + "/" + bytesToRead + "): " +
                ScadaUtils.BytesToString(buffer, index, Math.Min(bytesRead, MaxLoggingSize)) +
                (bytesRead <= MaxLoggingSize ? "" : "...") +
                (bytesToRead > 0 ? "" : "no data");
        }

        /// <summary>
        /// Builds text for writing logging.
        /// </summary>
        protected string BuildWritingText(byte[] buffer, int index, int bytesToWrite)
        {
            return "Send (" + bytesToWrite + "): " + 
                ScadaUtils.BytesToString(buffer, index, Math.Min(bytesToWrite, MaxLoggingSize)) +
                (bytesToWrite <= MaxLoggingSize ? "" : "...");
        }

        /// <summary>
        /// Gets the information about the current session.
        /// </summary>
        protected void GetSessionInfo(out long sessionID, out ushort protocolVersion, out string serverName)
        {
            DataPacket request = CreateRequest(FunctionID.GetSessionInfo, 10);
            SendRequest(request);

            DataPacket response = ReceiveResponse(request);
            sessionID = response.SessionID;
            int index = ArgumentIndex;
            protocolVersion = GetUInt16(inBuf, ref index);
            serverName = GetString(inBuf, ref index);
        }

        /// <summary>
        /// Logins to the server.
        /// </summary>
        protected void Login(out bool loggedIn, out int userID, out int roleID, out string errorMessage)
        {
            DataPacket request = CreateRequest(FunctionID.Login);
            int index = ArgumentIndex;
            CopyString(connectionOptions.User, outBuf, ref index);
            CopyString(EncryptPassword(connectionOptions.Password, SessionID, connectionOptions.SecretKey), 
                outBuf, ref index);
            CopyString(connectionOptions.Instance, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            DataPacket response = ReceiveResponse(request);
            index = ArgumentIndex;
            loggedIn = GetBool(inBuf, ref index);
            userID = GetInt32(inBuf, ref index);
            roleID = GetInt32(inBuf, ref index);
            errorMessage = GetString(inBuf, index);
        }

        /// <summary>
        /// Raises the Progress event.
        /// </summary>
        protected void OnProgress(int blockNumber, int blockCount)
        {
            Progress?.Invoke(this, new ProgressEventArgs(blockNumber, blockCount));
        }

        /// <summary>
        /// Gets the server and the session status.
        /// </summary>
        public void GetStatus(out bool serverIsReady, out bool userIsLoggedIn)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetStatus, 10);
            SendRequest(request);

            DataPacket response = ReceiveResponse(request);
            serverIsReady = inBuf[ArgumentIndex] > 0;
            userIsLoggedIn = inBuf[ArgumentIndex + 1] > 0;
        }

        /// <summary>
        /// Terminates the client session.
        /// </summary>
        public void TerminateSession()
        {
            RestoreConnection();
            DataPacket request = CreateRequest(FunctionID.TerminateSession, 10);
            SendRequest(request);
            ReceiveResponse(request);
            Disconnect();
        }

        /// <summary>
        /// Gets the information about the file.
        /// </summary>
        public void GetFileInfo(RelativePath relativePath, 
            out bool fileExists, out DateTime fileAge, out long fileLength)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetFileInfo);
            int index = ArgumentIndex;
            CopyFileName(relativePath.GetDirectoryID(), relativePath.Path, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            DataPacket response = ReceiveResponse(request);
            index = ArgumentIndex;
            fileExists = GetBool(inBuf, ref index);
            fileAge = GetTime(inBuf, ref index);
            fileLength = GetInt64(inBuf, ref index);
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        public void DownloadFile(RelativePath relativePath, long offset, int count, bool readFromEnd,
            DateTime newerThan, Func<Stream> createStreamFunc, 
            out DateTime fileAge, out FileReadingResult readingResult, out Stream stream)
        {
            if (createStreamFunc == null)
                throw new ArgumentNullException(nameof(createStreamFunc));

            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.DownloadFile);
            int index = ArgumentIndex;
            CopyFileName(relativePath.GetDirectoryID(), relativePath.Path, outBuf, ref index);
            CopyInt64(offset, outBuf, ref index);
            CopyInt32(count, outBuf, ref index);
            CopyBool(readFromEnd, outBuf, ref index);
            CopyTime(newerThan, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            int prevBlockNumber = 0;
            fileAge = DateTime.MinValue;
            readingResult = FileReadingResult.Successful;
            stream = null;

            try
            {
                while (readingResult == FileReadingResult.Successful)
                {
                    DataPacket response = ReceiveResponse(request);
                    index = ArgumentIndex;
                    int blockNumber = GetInt32(inBuf, ref index);
                    int blockCount = GetInt32(inBuf, ref index);
                    fileAge = GetTime(inBuf, ref index);
                    readingResult = (FileReadingResult)GetByte(inBuf, ref index);

                    if (blockNumber != prevBlockNumber + 1)
                        ThrowBlockNumberException();

                    if (readingResult == FileReadingResult.Successful ||
                        readingResult == FileReadingResult.EndOfFile)
                    {
                        if (stream == null)
                            stream = createStreamFunc();

                        int bytesToWrite = GetInt32(inBuf, ref index);
                        stream.Write(inBuf, index, bytesToWrite);
                    }

                    prevBlockNumber = blockNumber;
                    OnProgress(blockNumber, blockCount);
                }
            }
            catch
            {
                stream?.Dispose();
                stream = null;
                throw;
            }
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        public void DownloadFile(RelativePath relativePath, long offset, int count, bool readFromEnd,
            DateTime newerThan, string destFileName, out DateTime fileAge, out FileReadingResult readingResult)
        {
            DownloadFile(relativePath, offset, count, readFromEnd, newerThan,
                () => { return new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.Read); },
                out fileAge, out readingResult, out Stream stream);
            stream?.Dispose();
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        public void UploadFile(string srcFileName, RelativePath destPath, out bool fileAccepted)
        {
            if (!File.Exists(srcFileName))
                throw new ScadaException(CommonPhrases.FileNotFound);

            RestoreConnection();

            using (FileStream stream =
                new FileStream(srcFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // request permission to upload file
                const int FileDataIndex = ArgumentIndex + 9;
                const int BlockCapacity = BufferLenght - FileDataIndex;
                long bytesToReadTotal = stream.Length;
                int blockCount = (int)Math.Ceiling((double)bytesToReadTotal / BlockCapacity);

                DataPacket request = CreateRequest(FunctionID.UploadFile);
                int index = ArgumentIndex;
                CopyInt32(0, outBuf, ref index);
                CopyInt32(blockCount, outBuf, ref index);
                CopyFileName(destPath.GetDirectoryID(), destPath.Path, outBuf, ref index);
                request.BufferLength = index;
                SendRequest(request);

                DataPacket response = ReceiveResponse(request);
                fileAccepted = inBuf[ArgumentIndex] > 0;

                // upload file
                if (fileAccepted)
                {
                    int blockNumber = 0;
                    long bytesReadTotal = 0;
                    bool endOfFile = false;
                    request = null;

                    while (!endOfFile)
                    {
                        // read from file
                        int bytesToRead = (int)Math.Min(bytesToReadTotal - bytesReadTotal, BlockCapacity);
                        int bytesRead = stream.Read(outBuf, FileDataIndex, bytesToRead);
                        bytesReadTotal += bytesRead;
                        endOfFile = bytesRead < bytesToRead || bytesReadTotal == bytesToReadTotal;

                        // send data
                        request = CreateRequest(FunctionID.UploadFile, 0, false);
                        index = ArgumentIndex;
                        CopyInt32(++blockNumber, outBuf, ref index);
                        CopyBool(endOfFile, outBuf, ref index);
                        CopyInt32(bytesRead, outBuf, ref index);
                        request.BufferLength = FileDataIndex + bytesRead;
                        SendRequest(request);
                        OnProgress(blockNumber, blockCount);
                    }

                    if (request != null)
                        ReceiveResponse(request);
                }
            }
        }


        /// <summary>
        /// Occurs during the progress of a long-term operation.
        /// </summary>
        public event EventHandler<ProgressEventArgs> Progress;
    }
}
