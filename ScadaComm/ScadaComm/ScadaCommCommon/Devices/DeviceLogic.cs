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
using System.Threading;

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
            Log = lineContext.LineConfig.LineOptions.DetailedLog ? lineContext.Log : LogStub.Instance;
            LastRequestOK = false;
            IsBound = lineContext.LineConfig.IsBound && deviceConfig.IsBound;
            DeviceNum = deviceConfig.DeviceNum;
            Title = CommUtils.GetDeviceTitle(DeviceNum, deviceConfig.Name);
            NumAddress = deviceConfig.NumAddress;
            StrAddress = deviceConfig.StrAddress;
            PollingOptions = deviceConfig.PollingOptions;
            ReqRetries = lineContext.LineConfig.LineOptions.ReqRetries;

            CanSendCommands = false;
            ConnectionRequired = false;
            DeviceStatus = DeviceStatus.Undefined;
            LastSessionTime = DateTime.MinValue;
            LastCommandTime = DateTime.MinValue;
            DeviceTags = new DeviceTags();
            DeviceData = new DeviceData();
            DeviceStats = new DeviceStats();

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
        /// Gets or sets a value indicating whether the last request to the device was successful.
        /// </summary>
        protected bool LastRequestOK { get; set; }

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
        /// Gets the current device status.
        /// </summary>
        public DeviceStatus DeviceStatus { get; protected set; }

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
        /// Gets the device statistics.
        /// </summary>
        public DeviceStats DeviceStats { get; }

        /// <summary>
        /// Gets the current device status as text.
        /// </summary>
        public virtual string StatusText
        {
            get
            {
                return DeviceStatus.ToString(Locale.IsRussian);
            }
        }


        /// <summary>
        /// Checks that a request should be attempted.
        /// </summary>
        protected bool RequestNeeded(ref int tryNum)
        {
            return !LastRequestOK && tryNum < ReqRetries && !IsTerminated;
        }

        /// <summary>
        /// Completes a request.
        /// </summary>
        protected void FinishRequest()
        {
            Thread.Sleep(PollingOptions.Delay);

            DeviceStats.RequestCount++;

            if (!LastRequestOK)
                DeviceStats.RequestErrors++;
        }

        /// <summary>
        /// Completes a session.
        /// </summary>
        protected void FinishSession()
        {
            DeviceStats.SessionCount++;

            if (LastRequestOK)
            {
                DeviceStatus = DeviceStatus.Normal;
            }
            else
            {
                DeviceStats.SessionErrors++;
                DeviceStatus = DeviceStatus.Error;
            }

            DeviceData.SetStatusTag(DeviceStatus);
        }

        /// <summary>
        /// Completes a telecontrol command.
        /// </summary>
        protected void FinishCommand()
        {
            DeviceStats.CommandCount++;

            if (LastRequestOK)
            {
                DeviceStatus = DeviceStatus.Normal;
            }
            else
            {
                DeviceStats.CommandErrors++;
                DeviceStatus = DeviceStatus.Error;
            }

            DeviceData.SetStatusTag(DeviceStatus);
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
        /// Checks that the device supports the specified channel behavior.
        /// </summary>
        public virtual bool CheckBehaviorSupport(ChannelBehavior behavior)
        {
            return behavior == ChannelBehavior.Master;
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public virtual void InitDeviceTags()
        {
        }

        /// <summary>
        /// Initializes the device data.
        /// </summary>
        public virtual void InitDeviceData()
        {
            DeviceTags.AddStatusTag();
            DeviceData.Init(DeviceTags);
        }
        
        /// <summary>
        /// Sets the current data to undefined.
        /// </summary>
        public virtual void InvalidateData()
        {
            DeviceData.Invalidate();
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
            LastSessionTime = DateTime.UtcNow;
            LastRequestOK = true;
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public virtual void SendCommand(TeleCommand cmd)
        {
            LastCommandTime = DateTime.UtcNow;
            LastRequestOK = true;
        }

        /// <summary>
        /// Receives an unread incoming request in slave mode.
        /// </summary>
        public virtual void ReceiveIncomingRequest(Connection conn, IncomingRequestArgs requestArgs)
        {

        }

        /// <summary>
        /// Processes the incoming request that has already been read in slave mode.
        /// </summary>
        public virtual void ProcessIncomingRequest(byte[] buffer, int offset, int count, 
            IncomingRequestArgs requestArgs)
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
