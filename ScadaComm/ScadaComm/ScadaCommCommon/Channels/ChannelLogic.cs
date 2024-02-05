/*
 * Copyright 2024 Rapid Software LLC
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
 * Module   : ScadaCommCommon
 * Summary  : Represents the base class for communication channel logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents the base class for communication channel logic.
    /// <para>Представляет базовый класс логики канала связи.</para>
    /// </summary>
    /// <remarks>The base class can be used to create a stub.</remarks>
    public abstract class ChannelLogic
    {
        /// <summary>
        /// The length of the input buffer, 10 kB.
        /// </summary>
        protected const int InBufferLenght = 10240;
        /// <summary>
        /// The delay in thread iteration for slave, ms.
        /// </summary>
        protected const int SlaveThreadDelay = 20;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
        {
            LineContext = lineContext ?? throw new ArgumentNullException(nameof(lineContext));
            ChannelConfig = channelConfig ?? throw new ArgumentNullException(nameof(channelConfig));
            Log = lineContext.Log;
            Title = channelConfig.TypeCode;
        }


        /// <summary>
        /// Gets the communication line context.
        /// </summary>
        protected ILineContext LineContext { get; }

        /// <summary>
        /// Gets the communication channel configuration.
        /// </summary>
        protected ChannelConfig ChannelConfig { get; }

        /// <summary>
        /// Gets the communication line log.
        /// </summary>
        protected ILog Log { get; }

        /// <summary>
        /// Gets the channel behavior.
        /// </summary>
        protected virtual ChannelBehavior Behavior
        {
            get
            {
                return ChannelBehavior.Master;
            }
        }

        /// <summary>
        /// Gets the communication channel title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the current communication channel status as text.
        /// </summary>
        public virtual string StatusText
        {
            get
            {
                return Locale.IsRussian ?
                    "Не определён" :
                    "Undefined";
            }
        }


        /// <summary>
        /// Checks that all devices on the communication line support the channel behavior.
        /// </summary>
        protected void CheckBehaviorSupport()
        {
            HashSet<string> deviceTypeNames = new HashSet<string>();

            foreach (DeviceLogic deviceLogic in LineContext.SelectDevices())
            {
                deviceTypeNames.Add(deviceLogic.GetType().FullName);

                if (!deviceLogic.CheckBehaviorSupport(Behavior))
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Поведение {0} канала связи не поддерживается устройством {1}." :
                        "{0} behavior of the communication channel is not supported by the device {1}",
                        Behavior, deviceLogic.Title);
                }
            }

            if (Behavior == ChannelBehavior.Slave && deviceTypeNames.Count > 1)
            {
                Log.WriteWarning(Locale.IsRussian ?
                    "Не рекомендуется использовать устройства разных типов на одной линии связи" :
                    "It is not recommended to use devices of different types on the same communication line");
            }
        }

        /// <summary>
        /// Sets the connection for all devices on the communication line.
        /// </summary>
        protected void SetDeviceConnection(Connection conn)
        {
            foreach (DeviceLogic deviceLogic in LineContext.SelectDevices())
            {
                deviceLogic.Connection = conn;
            }
        }

        /// <summary>
        /// Receives an unread incoming request for the specified device.
        /// </summary>
        protected bool ReceiveIncomingRequest(DeviceLogic deviceLogic, Connection conn, 
            IncomingRequestArgs requestArgs)
        {
            try
            {
                deviceLogic.ReceiveIncomingRequest(conn, requestArgs);
                return !requestArgs.HasError;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при приёме входящего запроса для устройства {0}" :
                    "Error receiving incoming request for the device {0}", deviceLogic.Title);
                return false;
            }
        }

        /// <summary>
        /// Receives an unread incoming request for specified devices.
        /// </summary>
        protected bool ReceiveIncomingRequest(IEnumerable<DeviceLogic> devices, Connection conn, 
            IncomingRequestArgs requestArgs)
        {
            requestArgs.SetToDefault();

            foreach (DeviceLogic deviceLogic in devices)
            {
                ReceiveIncomingRequest(deviceLogic, conn, requestArgs);

                if (!requestArgs.NextDevice)
                    break;
            }

            return !requestArgs.HasError;
        }

        /// <summary>
        /// Processes the incoming request just read for the specified device.
        /// </summary>
        protected bool ProcessIncomingRequest(DeviceLogic deviceLogic, byte[] buffer, int offset, int count,
            IncomingRequestArgs requestArgs)
        {
            try
            {
                deviceLogic.ProcessIncomingRequest(buffer, offset, count, requestArgs);
                return !requestArgs.HasError;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при обработке входящего запроса для устройства {0}" :
                    "Error processing incoming request for the device {0}", deviceLogic.Title);
                return false;
            }
        }

        /// <summary>
        /// Processes the incoming request just read for specified devices.
        /// </summary>
        protected bool ProcessIncomingRequest(IEnumerable<DeviceLogic> devices, byte[] buffer, int offset, int count,
            IncomingRequestArgs requestArgs)
        {
            requestArgs.SetToDefault();

            foreach (DeviceLogic deviceLogic in devices)
            {
                ProcessIncomingRequest(deviceLogic, buffer, offset, count, requestArgs);

                if (!requestArgs.NextDevice)
                    break;
            }

            return !requestArgs.HasError;
        }

        /// <summary>
        /// Makes the communication channel ready for operating.
        /// </summary>
        /// <remarks>If an exception occurs, the communication line will not be started.</remarks>
        public virtual void MakeReady()
        {
        }

        /// <summary>
        /// Starts the communication channel.
        /// </summary>
        /// <remarks>If an exception occurs, the communication channel is restarted.</remarks>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Stops the communication channel.
        /// </summary>
        public virtual void Stop()
        {
        }

        /// <summary>
        /// Performs actions before polling the specified device.
        /// </summary>
        public virtual void BeforeSession(DeviceLogic deviceLogic)
        {
        }

        /// <summary>
        /// Performs actions after polling the specified device.
        /// </summary>
        public virtual void AfterSession(DeviceLogic deviceLogic)
        {
        }

        /// <summary>
        /// Appends information about the communication channel to the string builder.
        /// </summary>
        public virtual void AppendInfo(StringBuilder sb)
        {
        }
    }
}
