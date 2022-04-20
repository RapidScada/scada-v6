// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements the TCP server channel logic.
    /// <para>Реализует логику канала TCP-сервер.</para>
    /// </summary>
    public class TcpServerChannelLogic : ChannelLogic
    {
        /// <summary>
        /// The maximum length of the identification packet.
        /// </summary>
        protected const int HelloPacketLength = 100;

        protected readonly TcpServerChannelOptions options; // the channel options
        protected readonly IncomingRequestArgs requestArgs; // the incoming request arguments
        protected readonly byte[] inBuf;                    // the input data buffer
        protected readonly List<TcpConnection> connList;    // the client connections

        protected TcpListener tcpListener;     // listens for TCP connections
        protected TcpConnection currentConn;   // the connection used for the current session
        protected DeviceDictionary deviceDict; // the devices grouped by string address
        protected Thread thread;               // the thread for receiving data
        protected volatile bool terminated;    // necessary to stop the thread


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TcpServerChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            options = new TcpServerChannelOptions(channelConfig.CustomOptions);
            requestArgs = new IncomingRequestArgs();
            inBuf = new byte[InBufferLenght];
            connList = new List<TcpConnection>();

            tcpListener = null;
            currentConn = null;
            deviceDict = null;
            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Gets the channel behavior.
        /// </summary>
        protected override ChannelBehavior Behavior
        {
            get
            {
                return options.Behavior;
            }
        }

        /// <summary>
        /// Gets the current communication channel status as text.
        /// </summary>
        public override string StatusText
        {
            get
            {
                return Locale.IsRussian ? "TCP-сервер" : "TCP server";
            }
        }


        /// <summary>
        /// Communication cycle running in a separate thread.
        /// </summary>
        protected void Execute()
        {
            int threadDelay = options.Behavior == ChannelBehavior.Slave ? SlaveThreadDelay : ScadaUtils.ThreadDelay;
            TimeSpan clientLifetime = TimeSpan.FromSeconds(options.ClientLifetime);

            while (!terminated)
            {
                TcpConnection conn = null;

                try
                {
                    // open incoming connections
                    while (tcpListener.Pending() && !terminated)
                    {
                        conn = new TcpConnection(Log, tcpListener.AcceptTcpClient());
                        Log.WriteAction(Locale.IsRussian ?
                            "Соединение с клиентом {0}" :
                            "Connect to the client {0}", conn.RemoteAddress);

                        if (options.ConnectionMode == ConnectionMode.Shared)
                        {
                            BindDevices(conn, LineContext.SelectDevices());
                        }
                        else if (options.DeviceMapping == DeviceMapping.ByIPAddress)
                        {
                            if (deviceDict.GetDeviceGroup(conn.RemoteAddress,
                                out DeviceDictionary.DeviceGroup deviceGroup))
                            {
                                BindDevices(conn, deviceGroup);
                            }
                            else
                            {
                                Log.WriteError(CommPhrases.UnableFindDevice, conn.RemoteAddress);
                                conn.CloseMark = true;
                            }
                        }

                        lock (connList)
                        {
                            connList.Add(conn);
                        }
                    }

                    // receive data from connected clients
                    DateTime utcNow = DateTime.UtcNow;
                    int connIdx = 0;

                    while (connIdx < connList.Count && !terminated)
                    {
                        conn = connList[connIdx];

                        lock (conn)
                        {
                            // receive available data
                            if (conn.TcpClient.Available > 0 && !conn.CloseMark)
                                ReceiveData(conn);

                            // close inactive connection
                            if (utcNow - conn.ActivityTime > clientLifetime || conn.CloseMark)
                            {
                                Log.WriteAction(Locale.IsRussian ?
                                    "Отключение клиента {0}" :
                                    "Disconnect the client {0}", conn.RemoteAddress);
                                conn.Close();

                                lock (connList)
                                {
                                    connList.RemoveAt(connIdx);
                                }
                            }
                            else
                            {
                                connIdx++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (conn == null)
                    {
                        Log.WriteError(ex, Locale.IsRussian ?
                            "Ошибка при взаимодействии с клиентами" :
                            "Error communicating with clients");
                    }
                    else
                    {
                        Log.WriteError(ex, Locale.IsRussian ?
                            "Ошибка при взаимодействии с клиентом {0}" :
                            "Error communicating with the client {0}", conn.RemoteAddress);
                    }
                }
                finally
                {
                    Thread.Sleep(threadDelay);
                }
            }
        }

        /// <summary>
        /// Binds the devices to the connection.
        /// </summary>
        protected void BindDevices(TcpConnection conn, IEnumerable<DeviceLogic> devices)
        {
            // device has a reference to a one connection,
            // so the existing device connection is replaced by the new one,
            // the existing connection is marked for closing in master mode,
            // connection can be bound with multiple devices in any mode

            if (options.Behavior == ChannelBehavior.Master)
            {
                foreach (DeviceLogic deviceLogic in devices)
                {
                    if (deviceLogic.Connection is TcpConnection existingConn)
                    {
                        existingConn.CloseMark = true;
                        existingConn.ClearBoundDevices();
                    }
                }
            }

            foreach (DeviceLogic deviceLogic in devices)
            {
                Log.WriteAction(Locale.IsRussian ?
                    "Устройство {0} привязано к клиенту {1}" :
                    "The device {0} is bound to the client {1}",
                    deviceLogic.Title, conn.RemoteAddress);

                conn.BindDevice(deviceLogic);
            }
        }

        /// <summary>
        /// Binds the devices to the connection according to the incoming request.
        /// </summary>
        protected void BindByRequest(TcpConnection conn)
        {
            if (options.DeviceMapping == DeviceMapping.ByHelloPacket)
            {
                if (conn.JustConnected && ReceiveHelloPacket(conn, out string helloStr, out string helloHex))
                {
                    if (deviceDict.GetDeviceGroup(helloStr, out DeviceDictionary.DeviceGroup deviceGroup))
                    {
                        BindDevices(conn, deviceGroup);
                    }
                    else if (deviceDict.GetDeviceGroup(helloHex, out deviceGroup))
                    {
                        BindDevices(conn, deviceGroup);
                    }
                    else
                    {
                        Log.WriteError(CommPhrases.UnableFindDevice, helloStr + ", " + helloHex);
                        conn.CloseMark = true;
                    }
                }
            }
            else if (options.DeviceMapping == DeviceMapping.ByDriver)
            {
                if (ReceiveIncomingRequest(LineContext.SelectDevices(), conn, requestArgs))
                    BindDevices(conn, requestArgs.TargetDevices);
                else
                    conn.ClearNetStream(inBuf);
            }
        }

        /// <summary>
        /// Receives data from the client.
        /// </summary>
        protected void ReceiveData(TcpConnection conn)
        {
            if (Behavior == ChannelBehavior.Master)
            {
                if (options.ConnectionMode == ConnectionMode.Individual && 
                    conn.BoundDevices.Count == 0)
                {
                    BindByRequest(conn);
                }
            }
            else if (Behavior == ChannelBehavior.Slave)
            {
                if (options.ConnectionMode == ConnectionMode.Individual)
                {
                    if (conn.BoundDevices.Count > 0)
                    {
                        if (!ReceiveIncomingRequest(conn.BoundDevices, conn, requestArgs))
                            conn.ClearNetStream(inBuf);
                    }
                    else
                    {
                        BindByRequest(conn);
                    }
                }
                else // ConnectionMode.Shared
                {
                    if (!ReceiveIncomingRequest(LineContext.SelectDevices(), conn, requestArgs))
                        conn.ClearNetStream(inBuf);
                }
            }
        }

        /// <summary>
        /// Receives an identification packet.
        /// </summary>
        protected bool ReceiveHelloPacket(TcpConnection conn, out string helloStr, out string helloHex)
        {
            Log.WriteAction(Locale.IsRussian ?
                "Приём идентификационного пакета данных от клиента {0}" :
                "Receive an identification packet from the client {0}", conn.RemoteAddress);

            int readCnt = conn.ReadAvailable(inBuf, 0, HelloPacketLength, ProtocolFormat.String, out string logText);
            Log.WriteLine(logText);

            if (readCnt > 0)
            {
                helloStr = conn.Encoding.GetString(inBuf, 0, readCnt);
                helloHex = "0x" + ScadaUtils.BytesToHex(inBuf, 0, readCnt);
                return true;
            }
            else
            {
                helloStr = "";
                helloHex = "";
                return false;
            }
        }


        /// <summary>
        /// Makes the communication channel ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            CheckBehaviorSupport();
            tcpListener = new TcpListener(IPAddress.Any, options.TcpPort);

            if (options.DeviceMapping == DeviceMapping.ByIPAddress ||
                options.DeviceMapping == DeviceMapping.ByHelloPacket)
            {
                deviceDict = new DeviceDictionary();
                deviceDict.AddRange(LineContext.SelectDevices());
            }
        }

        /// <summary>
        /// Starts the communication channel.
        /// </summary>
        public override void Start()
        {
            tcpListener.Start();
            Log.WriteAction(Locale.IsRussian ?
                "Прослушиватель соединений на порту {0} запущен" :
                "Connection listener on port {0} is started", options.TcpPort);

            terminated = false;
            thread = new Thread(Execute);
            thread.Start();
        }

        /// <summary>
        /// Stops the communication channel.
        /// </summary>
        public override void Stop()
        {
            if (thread != null)
            {
                terminated = true;
                thread.Join();
                thread = null;
            }

            tcpListener.Stop();
            tcpListener = null;
            Log.WriteLine();
            Log.WriteAction(Locale.IsRussian ?
                "Прослушиватель соединений остановлен" :
                "Connection listener is stopped");

            connList.ForEach(conn => conn.Close());
            connList.Clear();
        }

        /// <summary>
        /// Performs actions before polling the specified device.
        /// </summary>
        public override void BeforeSession(DeviceLogic deviceLogic)
        {
            currentConn = deviceLogic.Connection as TcpConnection;

            if (currentConn != null && Behavior == ChannelBehavior.Slave)
                Monitor.Enter(currentConn.SyncRoot);

        }

        /// <summary>
        /// Performs actions after polling the specified device.
        /// </summary>
        public override void AfterSession(DeviceLogic deviceLogic)
        {
            if (currentConn != null)
            {
                if (Behavior == ChannelBehavior.Master)
                {
                    if (deviceLogic.DeviceStatus == DeviceStatus.Error && currentConn.Connected)
                        currentConn.ClearNetStream(inBuf);
                }
                else if (Behavior == ChannelBehavior.Slave)
                {
                    Monitor.Exit(currentConn.SyncRoot);
                }
            }
        }

        /// <summary>
        /// Appends information about the communication channel to the string builder.
        /// </summary>
        public override void AppendInfo(StringBuilder sb)
        {
            TcpConnection[] connArr;

            lock (connList)
            {
                connArr = connList.ToArray(); // make a snapshot
            }

            string header = Locale.IsRussian ?
                "Подключенные клиенты (" + connArr.Length + ")" :
                "Connected Clients (" + connArr.Length + ")";

            sb
                .AppendLine()
                .AppendLine(header)
                .Append('-', header.Length).AppendLine();

            if (connArr.Length > 0)
            {
                foreach (TcpConnection conn in connArr)
                {
                    sb.Append(conn.RemoteAddress).Append("; ");
                    IReadOnlyCollection<DeviceLogic> boundDevices = conn.BoundDevices;

                    if (boundDevices.Count > 0)
                    {
                        if (options.ConnectionMode == ConnectionMode.Individual)
                        {
                            bool first = true;

                            foreach (DeviceLogic deviceLogic in boundDevices)
                            {
                                if (first)
                                    first = false;
                                else
                                    sb.Append(", ");

                                sb.Append(deviceLogic.Title);
                            }
                        }
                        else
                        {
                            sb.Append(Locale.IsRussian ?
                                "Все устройства привязаны" :
                                "All devices are bound");
                        }
                    }
                    else
                    {
                        sb.Append(Locale.IsRussian ?
                            "Привязанные устройства отсутствуют" : 
                            "No bound devices");
                    }

                    sb.Append("; ").AppendLine(conn.ActivityTime.ToLocalizedTimeString());
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
