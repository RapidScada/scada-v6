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
 * Summary  : Represents the base class for device logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2020
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Drivers;
using Scada.Data.Models;
using Scada.Log;
using System;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents the base class for device logic.
    /// <para>Представляет базовый класс логики КП.</para>
    /// </summary>
    public abstract class DeviceLogic
    {
        private volatile bool terminated; // necessary to stop the current operation
        private Connection connection;    // the device connection


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
        {
            CommContext = commContext ?? throw new ArgumentNullException(nameof(commContext));
            LineContext = lineContext ?? throw new ArgumentNullException(nameof(lineContext));
            DeviceConfig = deviceConfig ?? throw new ArgumentNullException(nameof(deviceConfig));
            AppDirs = commContext.AppDirs;
            Log = lineContext.LineConfig.LineOptions.DetailedLog ? lineContext.Log : new LogStub();
            IsBound = lineContext.LineConfig.IsBound && deviceConfig.IsBound;
            DeviceNum = deviceConfig.DeviceNum;
            Title = CommUtils.GetDeviceTitle(DeviceNum, deviceConfig.Name);
            NumAddress = deviceConfig.NumAddress;
            StrAddress = deviceConfig.StrAddress;
            PollingOptions = deviceConfig.PollingOptions;
            ReqRetries = lineContext.LineConfig.LineOptions.ReqRetries;

            CanSendCommands = false;
            ConnectionRequired = false;
            LastSessionTime = DateTime.MinValue;
            LastCommandTime = DateTime.MinValue;
            DeviceTags = new DeviceTags();
            DeviceData = new DeviceData();

            terminated = false;
            connection = null;
        }


        /// <summary>
        /// Gets the application context.
        /// </summary>
        public ICommContext CommContext { get; }

        /// <summary>
        /// Gets the communication line context.
        /// </summary>
        protected ILineContext LineContext { get; }

        /// <summary>
        /// Gets the device configuration.
        /// </summary>
        protected DeviceConfig DeviceConfig { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        protected CommDirs AppDirs { get; }

        /// <summary>
        /// Gets the communication line log.
        /// </summary>
        protected ILog Log { get; }

        /// <summary>
        /// Gets a value indicating whether to stop the current operation.
        /// </summary>
        protected bool IsTerminated
        {
            get
            {
                return terminated;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the device is bound to the server.
        /// </summary>
        public bool IsBound { get; }

        /// <summary>
        /// Gets the device number.
        /// </summary>
        public int DeviceNum { get; }

        /// <summary>
        /// Gets the device title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the numeric address.
        /// </summary>
        public int NumAddress { get; }

        /// <summary>
        /// Gets the string address.
        /// </summary>
        public string StrAddress { get; }

        /// <summary>
        /// Gets the polling options.
        /// </summary>
        public PollingOptions PollingOptions { get; }

        /// <summary>
        /// Gets or sets the number of retries of the request in case of an error.
        /// </summary>
        public int ReqRetries { get; }

        /// <summary>
        /// Gets a value indicating whether the device can send telecontrol commands.
        /// </summary>
        public bool CanSendCommands { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether a connection is required to communicate with the device.
        /// </summary>
        public bool ConnectionRequired { get; protected set; }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value ?? ConnectionStub.Instance;
                OnConnectionSet();
            }
        }

        /// <summary>
        /// Gets the time (UTC) of the last device session.
        /// </summary>
        public DateTime LastSessionTime { get; protected set; }

        /// <summary>
        /// Gets the time (UTC) of the last device command.
        /// </summary>
        public DateTime LastCommandTime { get; protected set; }

        /// <summary>
        /// Gets the device tags.
        /// </summary>
        public DeviceTags DeviceTags { get; }

        /// <summary>
        /// Gets the device data.
        /// </summary>
        public DeviceData DeviceData { get; }

        /// <summary>
        /// Gets the current device status as text.
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
        /// Performs actions when starting a communication line.
        /// </summary>
        public virtual void OnCommLineStart()
        {
        }

        /// <summary>
        /// Performs actions when terminating a communication line.
        /// </summary>
        public virtual void OnCommLineTerminate()
        {
        }

        /// <summary>
        /// Performs actions after setting the connection.
        /// </summary>
        public virtual void OnConnectionSet()
        {
        }

        /// <summary>
        /// Binds the device to the configuration database.
        /// </summary>
        public virtual void Bind(BaseDataSet baseDataSet)
        {
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public virtual void Session()
        {

        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public virtual void SendCommand(TeleCommand cmd)
        {

        }

        /// <summary>
        /// Sets the current data to undefined.
        /// </summary>
        public virtual void InvalidateData()
        {
        }

        /// <summary>
        /// Gets the device information.
        /// </summary>
        public virtual string GetInfo()
        {
            return Title;
        }

        /// <summary>
        /// Prompts to terminate the current operation.
        /// </summary>
        public void Terminate()
        {
            terminated = true;
        }
    }
}
