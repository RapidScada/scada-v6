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
using Scada.Log;
using System;
using System.Collections.Generic;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents the base class for communication channel logic.
    /// <para>Представляет базовый класс логики канала связи.</para>
    /// </summary>
    /// <remarks>The base class can be used to create a stub.</remarks>
    public class ChannelLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
        {
            LineContext = lineContext ?? throw new ArgumentNullException(nameof(lineContext));
            ChannelConfig = channelConfig ?? throw new ArgumentNullException(nameof(channelConfig));
            Log = lineContext.Log;
            Title = channelConfig.TypeName;
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

            foreach (DeviceLogic deviceLogic in LineContext.EnumerateDevices())
            {
                deviceTypeNames.Add(deviceLogic.GetType().FullName);

                if (!deviceLogic.CheckBehaviorSupport(Behavior))
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Поведение {0} канала связи не поддерживается КП {1}." :
                        "{0} behavior of the communication channel is not supprted by the device {1}",
                        Behavior, deviceLogic.Title);
                }
            }

            if (Behavior == ChannelBehavior.Slave && deviceTypeNames.Count > 1)
            {
                Log.WriteWarning(Locale.IsRussian ?
                    "Не рекомендуется использовать КП разных типов на одной линии связи" :
                    "It is not recommended to use devices of different types on the same communication line");
            }
        }

        /// <summary>
        /// Sets the connection for all devices on the communication line.
        /// </summary>
        protected void SetDeviceConnection(Connection conn)
        {
            foreach (DeviceLogic deviceLogic in LineContext.EnumerateDevices())
            {
                deviceLogic.Connection = conn;
            }
        }

        /// <summary>
        /// Receives an unread incoming request.
        /// </summary>
        protected bool ReceiveIncomingRequest(Connection conn, IncomingRequestArgs requestArgs)
        {
            DeviceLogic currentDevice = null;

            try
            {
                requestArgs.SetToDefault();

                foreach (DeviceLogic deviceLogic in LineContext.EnumerateDevices())
                {
                    currentDevice = deviceLogic;
                    deviceLogic.ReceiveIncomingRequest(conn, requestArgs);

                    if (!requestArgs.NextDevice)
                        break;
                }

                return !requestArgs.HasError;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при приёме входящего запроса КП {0}" :
                    "Error receiving incoming request by the device {0}", 
                    currentDevice?.Title ?? "null");
                return false;
            }
        }

        /// <summary>
        /// Processes the incoming request that has already been read.
        /// </summary>
        protected bool ProcessIncomingRequest(byte[] buffer, int offset, int count, IncomingRequestArgs requestArgs)
        {
            DeviceLogic currentDevice = null;

            try
            {
                requestArgs.SetToDefault();

                foreach (DeviceLogic deviceLogic in LineContext.EnumerateDevices())
                {
                    currentDevice = deviceLogic;
                    deviceLogic.ProcessIncomingRequest(buffer, offset, count, requestArgs);

                    if (!requestArgs.NextDevice)
                        break;
                }

                return !requestArgs.HasError;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при обработке входящего запроса  КП {0}" :
                    "Error processing incoming request by the device {0}",
                    currentDevice?.Title ?? "null");
                return false;
            }
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
    }
}
