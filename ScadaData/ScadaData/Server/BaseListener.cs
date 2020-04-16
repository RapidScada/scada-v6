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
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Scada.Server
{
    /// <summary>
    /// Represents the base class for TCP listeners which waits for client connections.
    /// <para>Представляет базовый класс TCP-прослушивателей, которые ожидают подключения клиентов.</para>
    /// </summary>
    public abstract class BaseListener
    {
        /// <summary>
        /// The maximum number of client connections.
        /// </summary>
        protected const int MaxConnections = 100;
        /// <summary>
        /// The period after which an inactive client is disconnected.
        /// </summary>
        protected static readonly TimeSpan DisconnectPeriod = TimeSpan.FromSeconds(60);

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
        /// The connected clients.
        /// </summary>
        protected List<ConnectedClient> clients;
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
            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Work cycle running in a separate thread.
        /// </summary>
        protected void Execute()
        {
            while (!terminated)
            {
                try
                {
                    // connect new clients
                    while (tcpListener.Pending() && clients.Count < MaxConnections && !terminated)
                    {
                        TcpClient tcpClient = tcpListener.AcceptTcpClient();
                        tcpClient.SendTimeout = listenerOptions.Timeout;
                        tcpClient.ReceiveTimeout = listenerOptions.Timeout;

                        Thread clientTread = new Thread(ClientExecute);
                        ConnectedClient client = new ConnectedClient(tcpClient, clientTread);
                        log.WriteAction(string.Format(Locale.IsRussian ? 
                            "Клиент {0} подключился" :
                            "Client {0} connected", client.Address));
                        clients.Add(client);
                        clientTread.Start(client);
                    }

                    // disconnect inactive clients
                    int clientIndex = 0;
                    DateTime utcNow = DateTime.UtcNow;

                    while (clientIndex < clients.Count && !terminated)
                    {
                        ConnectedClient client = clients[clientIndex];

                        if (utcNow - client.ActivityTime > DisconnectPeriod)
                        {
                            DisconnectClient(client);
                            clients.RemoveAt(clientIndex);
                        }
                        else
                        {
                            clientIndex++;
                        }
                    }

                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, Locale.IsRussian ?
                        "Ошибка в цикле работы прослушивателя" :
                        "Error in the listener work cycle");
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
                try
                {
                    if (client.TcpClient.Available > 0)
                    {
                        // receive data
                        client.ActivityTime = DateTime.UtcNow;
                        ReceiveData(client);
                    }

                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, Locale.IsRussian ?
                        "Ошибка в цикле работы клиента {0}" :
                        "Error in the client {0} work cycle", client.Address);
                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
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
                client.Thread.Join();
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
                foreach (ConnectedClient client in clients)
                {
                    client.Terminated = true;
                }

                foreach (ConnectedClient client in clients)
                {
                    client.Thread.Join();
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
                const int MinPacketLength = 16;
                bool formatError = true;
                string errDescr = "";
                byte[] inBuf = client.InBuf;
                int bytesRead = client.NetStream.Read(inBuf, 0, MinPacketLength);

                if (bytesRead == MinPacketLength)
                {
                    int dataLength = BitConverter.ToInt32(inBuf, 2);
                    long sessionID = BitConverter.ToInt64(inBuf, 6);

                    if (dataLength + 6 > inBuf.Length)
                    {
                        errDescr = Locale.IsRussian ?
                            "длина данных слишком велика" :
                            "data length is too big";
                    }
                    else if (client.SessionID != 0 && client.SessionID != sessionID)
                    {
                        errDescr = Locale.IsRussian ?
                            "неверный идентификатор сессии" :
                            "incorrect session ID";
                    }
                    else
                    {
                        // read the rest of the data
                        int bytesToRead = dataLength - 8;
                        bytesRead = bytesToRead > 0 ? client.ReadData(16, bytesToRead) : 0;

                        if (bytesRead == bytesToRead)
                        {
                            formatError = false;
                            ProcessRequest(client);
                        }
                        else
                        {
                            errDescr = Locale.IsRussian ?
                                "не удалось прочитать все данные" :
                                "unable to read all data";
                        }
                    }
                }

                if (formatError)
                {
                    log.WriteError(string.Format(Locale.IsRussian ?
                        "Некорректный формат данных, полученных от клиента {0}: {1}" :
                        "Incorrect format of data received from the client {0}: {1}",
                        client.Address, errDescr));

                    // clear the stream by receiving available data
                    if (client.NetStream.DataAvailable)
                        client.NetStream.Read(inBuf, 0, inBuf.Length);
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
        /// Processes an incoming request already stored in the client input buffer.
        /// </summary>
        protected void ProcessRequest(ConnectedClient client)
        {
            byte[] inBuf = client.InBuf;
            ushort transactionID = BitConverter.ToUInt16(inBuf, 0);
            ushort functionID = BitConverter.ToUInt16(inBuf, 14);

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

                    tcpListener = new TcpListener(IPAddress.Any, listenerOptions.Port);
                    tcpListener.Start();

                    clients = new List<ConnectedClient>();
                    terminated = false;
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
                    tcpListener.Stop();
                    DisconnectAll();

                    tcpListener = null;
                    clients = null;
                    thread = null;

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
