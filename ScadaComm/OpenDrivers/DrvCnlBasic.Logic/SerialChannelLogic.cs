/*
 * Copyright 2021 Rapid Software LLC
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
 * Summary  : Implements serial port channel logic
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
using System.IO.Ports;
using System.Threading;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements serial port channel logic.
    /// <para>Реализует логику канала последовательного порта.</para>
    /// </summary>
    internal class SerialChannelLogic : ChannelLogic
    {
        /// <summary>
        /// The maximum time allowed to elapse before the arrival of the next byte, ms.
        /// </summary>
        protected const int ReadIntervalTimeout = 100;

        protected readonly SerialChannelOptions options;    // the channel options
        protected readonly IncomingRequestArgs requestArgs; // the incoming request arguments

        protected SerialConnection serialConn; // the serial connection
        protected Thread thread;               // the thread for receiving data in slave mode
        protected volatile bool terminated;    // necessary to stop the thread


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SerialChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            options = new SerialChannelOptions(channelConfig.CustomOptions);
            requestArgs = new IncomingRequestArgs();

            serialConn = null;
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
                    return serialConn == null ?
                        "Последовательный порт" :
                        serialConn.SerialPort.PortName + (serialConn.Connected ? ", открыт" : ", закрыт");
                } 
                else
                {
                    return serialConn == null ?
                        "Serial port" :
                        serialConn.SerialPort.PortName + (serialConn.Connected ? ", open" : ", closed");
                }
            }
        }


        /// <summary>
        /// Opens the serial port.
        /// </summary>
        protected void OpenSerialPort()
        {
            Log.WriteLine();
            Log.WriteAction(Locale.IsRussian ?
                "Открытие последовательного порта {0}" :
                "Open serial port {0}", serialConn.SerialPort.PortName);
            serialConn.Open();
        }

        /// <summary>
        /// Closes the serial port.
        /// </summary>
        protected void CloseSerialPort()
        {
            Log.WriteLine();
            Log.WriteAction(Locale.IsRussian ?
                "Закрытие последовательного порта {0}" :
                "Close serial port {0}", serialConn.SerialPort.PortName);
            serialConn.Close();
        }
        
        /// <summary>
        /// Listens to the serial port for incoming data.
        /// </summary>
        /// <remarks>This method works in a separate thread. It is used on Linux.</remarks>
        protected void ListenSerialPort()
        {
            while (!terminated)
            {
                try
                {
                    lock (serialConn)
                    {
                        if (serialConn.Connected && serialConn.SerialPort.BytesToRead > 0 &&
                            !ReceiveIncomingRequest(LineContext.SelectDevices(), serialConn, requestArgs))
                        {
                            serialConn.DiscardInBuffer();
                        }
                    }

                    Thread.Sleep(SlaveThreadDelay);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, Locale.IsRussian ?
                        "Ошибка при прослушивании последовательного порта" :
                        "Error listening to the serial port");
                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }
        }


        /// <summary>
        /// Makes the communication channel ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            CheckBehaviorSupport();

            serialConn = new SerialConnection(Log, new SerialPort(
                options.PortName, options.BaudRate, options.Parity, options.DataBits, options.StopBits)
            { 
                DtrEnable = options.DtrEnable, 
                RtsEnable = options.RtsEnable 
            });

            SetDeviceConnection(serialConn);
        }

        /// <summary>
        /// Starts the communication channel.
        /// </summary>
        public override void Start()
        {
            OpenSerialPort();

            if (Behavior == ChannelBehavior.Slave)
            {
                terminated = false;
                thread = new Thread(ListenSerialPort);
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

            SetDeviceConnection(null);
            CloseSerialPort();
        }

        /// <summary>
        /// Performs actions before polling the specified device.
        /// </summary>
        public override void BeforeSession(DeviceLogic deviceLogic)
        {
            if (Behavior == ChannelBehavior.Slave)
                Monitor.Enter(serialConn.SyncRoot);

            if (!serialConn.Connected)
                OpenSerialPort();
        }

        /// <summary>
        /// Performs actions after polling the specified device.
        /// </summary>
        public override void AfterSession(DeviceLogic deviceLogic)
        {
            if (serialConn.Connected && serialConn.WriteError)
                CloseSerialPort();

            if (Behavior == ChannelBehavior.Slave)
                Monitor.Exit(serialConn.SyncRoot);
        }
    }
}
