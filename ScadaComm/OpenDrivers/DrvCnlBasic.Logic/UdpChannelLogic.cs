// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Lang;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements the UDP channel logic.
    /// <para>Реализует логику канала UDP.</para>
    /// </summary>
    public class UdpChannelLogic : ChannelLogic
    {
        protected readonly UdpChannelOptions options;       // the channel options
        protected readonly IncomingRequestArgs requestArgs; // the incoming request arguments
        protected readonly object deviceLock;               // syncronizes access to devices

        protected UdpConnection masterConn;     // the UDP connection for polling
        protected UdpConnection slaveConn;      // the UDP connection for listening
        protected DeviceDictionary deviceDict;  // the devices grouped by string address
        protected volatile bool terminated;     // necessary to stop receiving data


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UdpChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            options = new UdpChannelOptions(channelConfig.CustomOptions);
            requestArgs = new IncomingRequestArgs();
            deviceLock = new object();

            masterConn = null;
            slaveConn = null;
            deviceDict = null;
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
                return "UDP";
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
                    buf = slaveConn.UdpClient.EndReceive(ar, ref remoteEP);
                    slaveConn.RemoteAddress = remoteEP.Address.ToString();
                    slaveConn.RemotePort = remoteEP.Port;

                    Log.WriteLine();
                    Log.WriteAction(Locale.IsRussian ?
                        "Получены данные от {0}:{1}" :
                        "Data received from {0}:{1}",
                        slaveConn.RemoteAddress, slaveConn.RemotePort);

                    if (buf == null)
                    {
                        Log.WriteAction(Locale.IsRussian ?
                            "Данные пусты" :
                            "Data is empty");
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(ex, Locale.IsRussian ?
                        "Ошибка при приёме данных" :
                        "Error receiving data");
                }
            }

            if (buf != null)
            {
                if (options.DeviceMapping == DeviceMapping.ByIPAddress)
                {
                    if (deviceDict.GetDeviceGroup(slaveConn.RemoteAddress, 
                        out DeviceDictionary.DeviceGroup deviceGroup))
                    {
                        lock (deviceLock)
                        {
                            // process the incoming request for the devices with the specified IP address
                            masterConn.RemoteAddress = slaveConn.RemoteAddress;
                            ProcessIncomingRequest(deviceGroup, buf, 0, buf.Length, requestArgs);
                        }
                    }
                    else
                    {
                        Log.WriteError(CommPhrases.UnableFindDevice, slaveConn.RemoteAddress);
                    }
                }
                else if (options.DeviceMapping == DeviceMapping.ByDriver)
                {
                    lock (deviceLock)
                    {
                        // process the incoming request for any device
                        masterConn.RemoteAddress = slaveConn.RemoteAddress;
                        ProcessIncomingRequest(LineContext.SelectDevices(), buf, 0, buf.Length, requestArgs);
                    }
                }
            }

            if (!terminated)
                slaveConn.BeginReceive(UdpReceiveCallback);
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
            // close connections in case of restart
            masterConn?.Close();
            slaveConn?.Close();

            if (Behavior == ChannelBehavior.Master)
            {
                masterConn = new UdpConnection(Log, new UdpClient(options.LocalUdpPort), options.RemoteUdpPort);
                slaveConn = null;
            }
            else
            {
                masterConn = new UdpConnection(Log, new UdpClient(), options.RemoteUdpPort); // auto assign local port
                slaveConn = new UdpConnection(Log, new UdpClient(options.LocalUdpPort), options.RemoteUdpPort);

                Log.WriteAction(Locale.IsRussian ?
                    "Локальный UDP-порт {0} открыт" :
                    "Local UDP port {0} is open", options.LocalUdpPort);

                Log.WriteAction(Locale.IsRussian ?
                    "Запуск приёма данных по UDP на порту {0}" :
                    "Start receiving data via UDP on port {0}", options.LocalUdpPort);
                terminated = false;
                slaveConn.BeginReceive(UdpReceiveCallback);
            }

            SetDeviceConnection(masterConn);
        }

        /// <summary>
        /// Stops the communication channel.
        /// </summary>
        public override void Stop()
        {
            terminated = true;
            SetDeviceConnection(null);
            masterConn?.Close();
            slaveConn?.Close();

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
                    masterConn.RemoteAddress = options.RemoteIpAddress;
                    masterConn.RemotePort = options.RemoteUdpPort;
                }
                else
                {
                    ScadaUtils.RetrieveHostAndPort(deviceLogic.StrAddress, options.RemoteUdpPort,
                        out string host, out int port);
                    masterConn.RemoteAddress = host;
                    masterConn.RemotePort = port;
                }
            }
            else
            {
                Monitor.Enter(deviceLock);
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
                    masterConn.ClearDatagramBuffer();
            }
            else
            {
                Monitor.Exit(deviceLock);
            }
        }
    }
}
