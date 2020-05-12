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

using Scada.Protocol;
using System;
using System.Net;
using System.Net.Sockets;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Client
{
    /// <summary>
    /// Represents the base class for TCP clients which interacts with a server.
    /// <para>Представляет базовый класс TCP-клиентов, которые взаимодействуют с сервером.</para>
    /// </summary>
    public abstract class BaseClient
    {
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
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseClient(ConnectionOptions connectionOptions)
        {
            this.connectionOptions = connectionOptions ?? throw new ArgumentNullException("connectionOptions");
            inBuf = new byte[BufferLenght];
            outBuf = new byte[BufferLenght];

            tcpClient = null;
            netStream = null;
            transactionID = 0;

            SessionID = 0;
            ServerName = "";
        }

        
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
            if (IPAddress.TryParse(connectionOptions.Host, out IPAddress address))
                tcpClient.Connect(address, connectionOptions.Port);
            else
                tcpClient.Connect(connectionOptions.Host, connectionOptions.Port);

            netStream = tcpClient.GetStream();
        }

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        protected void Disconnect()
        {
            if (tcpClient != null)
            {
                if (netStream != null)
                {
                    ClearNetStream(netStream, inBuf); // to disconnect correctly
                    netStream.Close();
                    netStream = null;
                }

                tcpClient.Close();
                tcpClient = null;

                SessionID = 0;
                ServerName = "";
            }
        }

        /// <summary>
        /// Restores a connection with the server.
        /// </summary>
        protected void RestoreConnection()
        {
            Disconnect();
            Connect();

            GetSessionInfo(out long sessionID, out string serverName);
            SessionID = sessionID;
            ServerName = serverName;

            Login(out bool loggedIn, out int userRole, out string errorMessage);

            if (!loggedIn)
                throw new ScadaException(errorMessage);
        }

        /// <summary>
        /// Creates a new data packet for request.
        /// </summary>
        protected DataPacket CreateRequest(ushort functionID, int dataLength)
        {
            return new DataPacket
            {
                TransactionID = ++transactionID,
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
        }

        /// <summary>
        /// Receives a response from the server.
        /// </summary>
        protected DataPacket ReceiveResponse(DataPacket request)
        {
            DataPacket response = null;
            bool formatError = true;
            string errDescr = "";
            int bytesRead = netStream.Read(inBuf, 0, HeaderLength);

            if (bytesRead == HeaderLength)
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
                else if (response.FunctionID != request.FunctionID)
                {
                    errDescr = Locale.IsRussian ?
                        "неверный идентификатор функции" :
                        "incorrect function ID";
                }
                else
                {
                    // read the rest of the data
                    int bytesToRead = response.DataLength - 8;
                    bytesRead = ReadData(netStream, tcpClient.ReceiveTimeout, inBuf, HeaderLength, bytesToRead);

                    if (bytesRead == bytesToRead)
                    {
                        formatError = false;
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
                throw new ScadaException(string.Format(Locale.IsRussian ?
                    "Некорректный формат данных, полученных от сервера: {0}" :
                    "Incorrect format of data received from the server: {0}", errDescr));
            }

            return response;
        }

        /// <summary>
        /// Gets the information about the current session.
        /// </summary>
        protected void GetSessionInfo(out long sessionID, out string serverName)
        {
            DataPacket request = CreateRequest(FunctionID.GetSessionInfo, 10);
            SendRequest(request);

            DataPacket response = ReceiveResponse(request);
            sessionID = response.SessionID;
            serverName = GetString(inBuf, ArgumentIndex, out int index);
        }

        /// <summary>
        /// Logins to the server.
        /// </summary>
        protected void Login(out bool loggedIn, out int userRole, out string errorMessage)
        {
            DataPacket request = CreateRequest(FunctionID.GetSessionInfo, 10);
            CopyString(connectionOptions.User, outBuf, ArgumentIndex, out int index);
            CopyString(EncryptPassword(connectionOptions.Password, SessionID, connectionOptions.SecretKey),
                outBuf, index, out index);
            CopyString(connectionOptions.Instance, outBuf, index, out index);
            SendRequest(request);

            DataPacket response = ReceiveResponse(request);
            loggedIn = inBuf[ArgumentIndex] > 0;
            userRole = BitConverter.ToInt32(inBuf, ArgumentIndex + 1);
            errorMessage = GetString(inBuf, ArgumentIndex + 5, out index);
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
    }
}
