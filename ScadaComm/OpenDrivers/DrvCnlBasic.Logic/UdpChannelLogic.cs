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
 * Summary  : Implements UDP channel logic
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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements UDP channel logic.
    /// <para>Реализует логику канала UDP.</para>
    /// </summary>
    public class UdpChannelLogic : ChannelLogic
    {
        /// <summary>
        /// The delay while waiting for a lock, ms.
        /// </summary>
        protected const int LockDelay = SlaveThreadDelay / 2;

        protected readonly UdpChannelOptions options;       // the channel options
        protected readonly IncomingRequestArgs requestArgs; // the incoming request arguments

        protected UdpConnection udpConn;        // the UDP connection
        protected DeviceDictionary deviceDict;  // the devices grouped by string address
        protected volatile bool terminated;     // necessary to stop receiving data
        protected volatile bool waitingForData; // asynchronous receive in progress


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UdpChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            options = new UdpChannelOptions(channelConfig.CustomOptions);
            requestArgs = new IncomingRequestArgs();

            udpConn = null;
            deviceDict = null;
            terminated = false;
            waitingForData = false;
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
                return "UDP";
            }
        }


        /// <summary>
        /// Starts receiving data by UDP.
        /// </summary>
        protected void StartUdpReceive()
        {
            try
            {
                lock (udpConn)
                {
                    waitingForData = true;
                    udpConn.BeginReceive(UdpReceiveCallback);
                }
            }
            catch (Exception ex)
            {
                waitingForData = false;
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при запуске приёма данных" :
                    "Error starting to receive data");
            }
        }

        /// <summary>
        /// Process data received by UDP.
        /// </summary>
        protected void UdpReceiveCallback(IAsyncResult ar)
        {
            byte[] buf = null;

            if (!terminated)
            {
                try
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    buf = udpConn.UdpClient.EndReceive(ar, ref remoteEP);
                    udpConn.RemoteAddress = remoteEP.Address.ToString();
                    udpConn.RemotePort = remoteEP.Port;

                    Log.WriteLine();
                    Log.WriteAction(Locale.IsRussian ?
                        "Получены данные от {0}:{1}" :
                        "Data received from {0}:{1}",
                        udpConn.RemoteAddress, udpConn.RemotePort);

                    if (buf == null)
                    {
                        Log.WriteAction(Locale.IsRussian ?
                            "Данные пусты" :
                            "Data is empty");
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, Locale.IsRussian ?
                        "Ошибка при приёме данных" :
                        "Error receiving data");
                }
            }

            if (buf != null)
            {
                if (options.DeviceMapping == DeviceMapping.ByIPAddress)
                {
                    if (deviceDict.GetDeviceGroup(udpConn.RemoteAddress, out DeviceDictionary.DeviceGroup deviceGroup))
                    {
                        // process the incoming request for the devices with the specified IP address
                        ProcessIncomingRequest(deviceGroup, buf, 0, buf.Length, requestArgs);
                    }
                    else
                    {
                        Log.WriteError(CommPhrases.UnableFindDevice, udpConn.RemoteAddress);
                    }
                }
                else if (options.DeviceMapping == DeviceMapping.ByDriver)
                {
                    // process the incoming request for any device
                    ProcessIncomingRequest(LineContext.SelectDevices(), buf, 0, buf.Length, requestArgs);
                }
            }

            waitingForData = false;

            if (!terminated)
            {
                Thread.Sleep(SlaveThreadDelay);
                StartUdpReceive();
            }
        }


        /// <summary>
        /// Makes the communication channel ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            CheckBehaviorSupport();

            if (options.DeviceMapping == DeviceMapping.ByHelloPacket)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Режим сопоставления устройств не поддерживается." :
                    "Device mapping mode is not supported.");
            }

            if (Behavior == ChannelBehavior.Slave &&
                options.DeviceMapping == DeviceMapping.ByIPAddress)
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
            udpConn = new UdpConnection(Log, new UdpClient(options.LocalUdpPort), 
                options.LocalUdpPort, options.RemoteUdpPort);
            SetDeviceConnection(udpConn);

            Log.WriteAction(Locale.IsRussian ?
                "Локальный UDP-порт {0} открыт" :
                "Local UDP port {0} is open", options.LocalUdpPort);

            if (Behavior == ChannelBehavior.Slave)
            {
                Log.WriteAction(Locale.IsRussian ?
                    "Запуск приёма данных по UDP на порту {0}" :
                    "Start receiving data via UDP on port {0}", options.LocalUdpPort);
                terminated = false;
                StartUdpReceive();
            }
        }

        /// <summary>
        /// Stops the communication channel.
        /// </summary>
        public override void Stop()
        {
            terminated = true;
            SetDeviceConnection(null);
            udpConn?.Close();

            Log.WriteLine();
            Log.WriteAction(Locale.IsRussian ?
                "Завершение приёма данных по UDP" :
                "Stop receiving data via UDP");
        }

        /// <summary>
        /// Performs actions before polling the specified device.
        /// </summary>
        public override void BeforeSession(DeviceLogic deviceLogic)
        {
            if (Behavior == ChannelBehavior.Master)
            {
                // update host and port of the connection
                if (string.IsNullOrEmpty(deviceLogic.StrAddress))
                {
                    udpConn.RemoteAddress = options.RemoteIpAddress;
                    udpConn.RemotePort = options.RemoteUdpPort;
                }
                else
                {
                    ScadaUtils.RetrieveHostAndPort(deviceLogic.StrAddress, options.RemoteUdpPort,
                        out string host, out int port);
                    udpConn.RemoteAddress = host;
                    udpConn.RemotePort = port;
                }
            }
            else
            {
                while (waitingForData)
                {
                    Thread.Sleep(LockDelay);
                }

                Monitor.Enter(udpConn.SyncRoot);
            }
        }

        /// <summary>
        /// Performs actions after polling the specified device.
        /// </summary>
        public override void AfterSession(DeviceLogic deviceLogic)
        {
            if (Behavior == ChannelBehavior.Master)
            {
                if (deviceLogic.DeviceStatus == DeviceStatus.Error)
                    udpConn.ClearDatagramBuffer();
            }
            else
            {
                Monitor.Exit(udpConn.SyncRoot);
            }
        }
    }
}
