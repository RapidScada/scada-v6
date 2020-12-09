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
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements UDP channel logic.
    /// <para>Реализует логику канала UDP.</para>
    /// </summary>
    public class UdpChannelLogic : ChannelLogic
    {
        protected readonly UdpChannelOptions options; // the channel options
        protected UdpConnection udpConn;    // the UDP connection
        protected volatile bool terminated; // necessary to stop receiving data


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UdpChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            options = new UdpChannelOptions(channelConfig.CustomOptions);
            udpConn = null;
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
                return udpConn == null ? base.StatusText : "UDP";
            }
        }


        /// <summary>
        /// Starts receiving data by UDP.
        /// </summary>
        protected void StartUdpReceive()
        {
            try
            {
                if (!udpConn.Connected)
                    udpConn.Renew();

                udpConn.UdpClient.BeginReceive(new AsyncCallback(UdpReceiveCallback), null);
            }
            catch (Exception ex)
            {
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
            // приём данных, если соединение установлено
            byte[] buf = null;

            if (udpConn.Connected)
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

            if (buf != null && !terminated)
            {
                if (options.Mapping == DeviceMapping.ByIPAddress)
                {
                    if (LineContext.GetDevice(udpConn.RemoteAddress, out DeviceLogic deviceLogic))
                    {
                        // process the incoming request for the specified device
                        ProcessIncomingRequest(deviceLogic, buf, 0, buf.Length, new IncomingRequestArgs());
                    }
                    else
                    {
                        Log.WriteError(Locale.IsRussian ?
                            "Не удалось найти КП по IP-адресу {0}" :
                            "Unable to find device by IP address {0}", udpConn.RemoteAddress);
                    }
                }
                else if (options.Mapping == DeviceMapping.ByDriver)
                {
                    // process the incoming request for any device
                    ProcessIncomingRequest(buf, 0, buf.Length, new IncomingRequestArgs());
                }
            }

            if (!terminated)
                StartUdpReceive();
        }


        /// <summary>
        /// Makes the communication channel ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            CheckBehaviorSupport();

            if (options.Mapping == DeviceMapping.ByFirstPacket)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Режим сопоставления устройств не поддерживается." :
                    "Device mapping mode is not supported.");
            }
        }

        /// <summary>
        /// Starts the communication channel.
        /// </summary>
        public override void Start()
        {
            udpConn = new UdpConnection(Log, new UdpClient(options.LocalUdpPort), 
                options.LocalUdpPort, options.RemoteUdpPort);

            Log.WriteAction(Locale.IsRussian ?
                "Локальный UDP-порт {0} открыт" :
                "Local UDP port {0} is open", options.LocalUdpPort);

            SetDeviceConnection(udpConn);

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
            if (udpConn != null && Behavior == ChannelBehavior.Master)
            {
                udpConn.RemoteAddress = string.IsNullOrEmpty(deviceLogic.StrAddress) ?
                    options.RemoteIpAddress :
                    deviceLogic.StrAddress;
            }
        }

        /// <summary>
        /// Performs actions after polling the specified device.
        /// </summary>
        public override void AfterSession(DeviceLogic deviceLogic)
        {
            // clear the datagram buffer in case of session error
            if (deviceLogic.DeviceStatus == DeviceStatus.Error && Behavior == ChannelBehavior.Master &&
                deviceLogic.Connection is UdpConnection conn && udpConn.Connected)
            {
                conn.ClearDatagramBuffer();
            }
        }
    }
}
