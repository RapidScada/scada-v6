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
 * Module   : DrvCnlBasic
 * Summary  : Implements TCP client channel logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvCnlBasic.Logic.Options;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements TCP client channel logic.
    /// <para>Реализует логику канала TCP-клиент.</para>
    /// </summary>
    public class TcpClientChannel : ChannelLogic
    {
        /// <summary>
        /// The length of the input buffer in slave mode, 10 kB.
        /// </summary>
        protected const int SlaveInBufLen = 10240;

        protected readonly TcpClientChannelOptions options; // the channel options
        protected readonly DeviceDictionary deviceDict;     // the devices grouped by string address

        protected List<TcpConnection> indivConnList; // the individual connections
        protected TcpConnection sharedConn;          // the shared connection
        protected Thread thread;                     // the thread for receiving data in slave mode
        protected volatile bool terminated;          // necessary to stop the thread


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TcpClientChannel(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            options = new TcpClientChannelOptions(channelConfig.CustomOptions);
            deviceDict = new DeviceDictionary();

            indivConnList = null;
            sharedConn = null;
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
                if (Locale.IsRussian)
                {
                    return sharedConn == null ?
                        "TCP-клиент" :
                        (sharedConn.Connected ? "TCP-клиент, подключен" : "TCP-клиент, не подключен");
                }
                else
                {
                    return sharedConn == null ?
                        "TCP client" :
                        (sharedConn.Connected ? "TCP client, connected" : "TCP client, disconnected");
                }
            }
        }


        /// <summary>
        /// Listens to the individual connections for incoming data.
        /// </summary>
        protected void ListenIndividualConn()
        {
            try
            {
                IncomingRequestArgs requestArgs = new IncomingRequestArgs();
                byte[] buffer = new byte[SlaveInBufLen];

                while (!terminated)
                {
                    foreach (TcpConnection conn in indivConnList)
                    {
                        if (sharedConn.TcpClient.Available > 0 &&
                            !ReceiveIncomingRequest(conn.BoundDevices, conn, requestArgs))
                        {
                            sharedConn.ClearNetStream(buffer);
                        }
                    }

                    Thread.Sleep(SlaveThreadDelay);
                }
            }
            catch (Exception ex)
            {
                // this exception should never happen
                Log.WriteException(ex, Locale.IsRussian ?
                    "Неустранимая ошибка при приёме данных через индивидуальное соединение." :
                    "Fatal error while receiving data over an individual connection.");
            }
        }

        /// <summary>
        /// Listens to the shared connection for incoming data.
        /// </summary>
        protected void ListenSharedConn()
        {
            try
            {
                IncomingRequestArgs requestArgs = new IncomingRequestArgs();
                byte[] buffer = new byte[SlaveInBufLen];

                while (!terminated)
                {
                    if (sharedConn.TcpClient.Available > 0 &&
                        !ReceiveIncomingRequest(LineContext.SelectDevices(), sharedConn, requestArgs))
                    {
                        sharedConn.ClearNetStream(buffer);
                    }

                    Thread.Sleep(SlaveThreadDelay);
                }
            }
            catch (Exception ex)
            {
                // this exception should never happen
                Log.WriteException(ex, Locale.IsRussian ?
                    "Неустранимая ошибка при приёме данных через общее соединение." :
                    "Fatal error while receiving data over a shared connection.");
            }
        }


        /// <summary>
        /// Makes the communication channel ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            CheckBehaviorSupport();
            deviceDict.AddRange(LineContext.SelectDevices());

            if (options.ConnectionMode == ConnectionMode.Individual)
            {
                indivConnList = new List<TcpConnection>();

                foreach (DeviceDictionary.DeviceList deviceList in deviceDict.DeviceMap.Values)
                {
                    TcpConnection conn = new TcpConnection(Log, new TcpClient())
                    {
                        ReconnectAfter = options.ReconnectAfter
                    };

                    indivConnList.Add(conn);

                    foreach (DeviceLogic deviceLogic in deviceList)
                    {
                        conn.BindDevice(deviceLogic);
                        deviceLogic.Connection = conn;
                    }
                }
            }
            else // ConnectionMode.Shared
            {
                sharedConn = new TcpConnection(Log, new TcpClient())
                {
                    ReconnectAfter = options.ReconnectAfter
                };
            }
        }

        /// <summary>
        /// Starts the communication channel.
        /// </summary>
        public override void Start()
        {
            if (options.Behavior == ChannelBehavior.Slave)
            {
                terminated = false;
                thread = options.ConnectionMode == ConnectionMode.Individual ?
                    new Thread(ListenIndividualConn) :
                    new Thread(ListenSharedConn);
                thread.Start();
            }
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

            if (options.ConnectionMode == ConnectionMode.Individual)
            {
                if (indivConnList != null)
                {
                    indivConnList.ForEach(conn => conn.Close());
                    indivConnList.Clear();
                }
            }
            else
            {
                SetDeviceConnection(null);
                sharedConn?.Close();
            }
        }

        /// <summary>
        /// Performs actions before polling the specified device.
        /// </summary>
        public override void BeforeSession(DeviceLogic deviceLogic)
        {
            // connect if disconnected
            if (deviceLogic.Connection is TcpConnection conn && !conn.Connected)
            {
                // get host and port
                string host;
                int port;

                if (conn == sharedConn)
                {
                    host = options.Host;
                    port = options.TcpPort;
                }
                else if (conn.RemotePort > 0)
                {
                    host = conn.RemoteAddress;
                    port = conn.RemotePort;
                }
                else
                {
                    ScadaUtils.RetrieveHostAndPort(deviceLogic.StrAddress, options.TcpPort, out host, out port);
                }

                // connect
                Log.WriteLine();
                Log.WriteAction(Locale.IsRussian ?
                    "Соединение с {0}:{1}" :
                    "Connect to {0}:{1}", host, port);

                if (conn.NetStream != null) // connection was already open but broken
                    conn.Renew();

                conn.Open(host, port);
            }
        }

        /// <summary>
        /// Performs actions after polling the specified device.
        /// </summary>
        public override void AfterSession(DeviceLogic deviceLogic)
        {
            // disconnect according to the options, or in case of error
            if ((!options.StayConnected || deviceLogic.DeviceStatus == DeviceStatus.Error) &&
                deviceLogic.Connection is TcpConnection conn && conn.Connected)
            {
                Log.WriteLine();
                Log.WriteAction(Locale.IsRussian ?
                    "Отключение от {0}" :
                    "Disconnect from {0}", conn.RemoteAddress);
                conn.Disconnect();
            }
        }
    }
}
