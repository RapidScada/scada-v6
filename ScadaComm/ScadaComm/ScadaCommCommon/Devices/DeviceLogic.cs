/*
 * Copyright 2022 Rapid Software LLC
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
 * Modified : 2022
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Drivers;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Log;
using Scada.Storages;
using System;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents the base class for device logic.
    /// <para>Представляет базовый класс логики устройства.</para>
    /// </summary>
    public abstract class DeviceLogic
    {
        private volatile bool terminated; // necessary to stop the current operation
        private Connection connection;    // the device connection
        private int lastInfoLength;       // the last info text length


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
        {
            CommContext = commContext ?? throw new ArgumentNullException(nameof(commContext));
            LineContext = lineContext ?? throw new ArgumentNullException(nameof(lineContext));
            DeviceConfig = deviceConfig ?? throw new ArgumentNullException(nameof(deviceConfig));
            AppDirs = commContext.AppDirs;
            Storage = commContext.Storage;
            Log = lineContext.LineConfig.LineOptions.DetailedLog ? lineContext.Log : LogStub.Instance;
            AssemblyName asmName = GetType().Assembly.GetName();
            DriverName = ScadaUtils.RemoveFileNameSuffixes(asmName.Name) + " " + asmName.Version;
            LastRequestOK = false;
            ReqRetries = lineContext.LineConfig.LineOptions.ReqRetries;
            IsBound = lineContext.LineConfig.IsBound && deviceConfig.IsBound;
            DeviceNum = deviceConfig.DeviceNum;
            Title = CommUtils.GetDeviceTitle(deviceConfig);
            NumAddress = deviceConfig.NumAddress;
            StrAddress = deviceConfig.StrAddress;
            PollingOptions = deviceConfig.PollingOptions;
            CanSendCommands = false;
            ConnectionRequired = true;
            DeviceStatus = DeviceStatus.Undefined;
            LastSessionTime = DateTime.MinValue;
            LastCommandTime = DateTime.MinValue;
            DeviceTags = new DeviceTags();
            DeviceData = new DeviceData(deviceConfig.DeviceNum);
            DeviceStats = new DeviceStats();

            terminated = false;
            connection = ConnectionStub.Instance;
            lastInfoLength = 0;
        }


        /// <summary>
        /// Gets the application context.
        /// </summary>
        protected ICommContext CommContext { get; }

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
        /// Gets the application storage.
        /// </summary>
        protected IStorage Storage { get; }

        /// <summary>
        /// Gets the communication line log.
        /// </summary>
        protected ILog Log { get; }

        /// <summary>
        /// Gets the display name of the driver.
        /// </summary>
        protected string DriverName { get; }

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
        /// Gets or sets the number of retries of the request in case of an error.
        /// </summary>
        protected int ReqRetries { get; }

        /// <summary>
        /// Gets a value indicating whether the device is bound to the configuration database.
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
            SleepPollingDelay();
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

            if (IsTerminated)
            {
                DeviceStatus = DeviceStatus.Undefined;
            }
            else if (LastRequestOK)
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

            if (IsTerminated)
            {
                DeviceStatus = DeviceStatus.Undefined;
            }
            else if (LastRequestOK)
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
        /// Suspends for the delay specified in the polling options.
        /// </summary>
        protected void SleepPollingDelay()
        {
            if (PollingOptions.Delay > 0)
                Thread.Sleep(PollingOptions.Delay);
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
        /// Binds the device tags to the configuration database.
        /// </summary>
        public virtual void BindDeviceTags(ConfigDataset configDataset)
        {
            foreach (Cnl cnl in configDataset.CnlTable.SelectItems(new TableFilter("DeviceNum", DeviceNum), true))
            {
                if (cnl.Active && cnl.IsInput())
                {
                    DeviceTag deviceTag = null;

                    if (!string.IsNullOrEmpty(cnl.TagCode))
                    {
                        // find tag by code
                        DeviceTags.TryGetTag(cnl.TagCode, out deviceTag);
                    }
                    else if (cnl.TagNum > 0)
                    {
                        // find tag by index
                        DeviceTags.TryGetTag(cnl.TagNum.Value - 1, out deviceTag);
                    }

                    // check match and bind tag
                    if (deviceTag != null &&
                        (int)deviceTag.DataType == (cnl.DataTypeID ?? DataTypeID.Double) &&
                        deviceTag.DataLength == Math.Max(cnl.DataLen ?? 1, 1))
                    {
                        deviceTag.Cnl = cnl;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the device data.
        /// </summary>
        public virtual void InitDeviceData()
        {
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
        /// Gets a slice of the current data to transfer.
        /// </summary>
        public virtual DeviceSlice GetCurrentData(bool allData)
        {
            return allData 
                ? DeviceData.GetCurrentData() 
                : DeviceData.GetModifiedData();
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public virtual void Session()
        {
            LastSessionTime = DateTime.UtcNow;
            LastRequestOK = true;

            Log.WriteLine();
            Log.WriteAction(Locale.IsRussian ?
                "Сеанс связи с устройством {0}" :
                "Session with the device {0}", Title);
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public virtual void SendCommand(TeleCommand cmd)
        {
            LastCommandTime = DateTime.UtcNow;
            LastRequestOK = true;
            DeviceData.RegisterCommand(cmd);

            Log.WriteLine();
            Log.WriteAction(Locale.IsRussian ?
                "Команда устройству {0}" :
                "Command to the device {0}", Title);
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
            StringBuilder sb = new StringBuilder((int)(lastInfoLength * 1.1));
            sb.AppendLine(Title);
            sb.Append('-', Title.Length).AppendLine();

            string TimeToStringRu(DateTime dateTime)
            {
                return dateTime > DateTime.MinValue ? dateTime.ToLocalTime().ToLocalizedString() : "не определено";
            };

            string TimeToStringEn(DateTime dateTime)
            {
                return dateTime > DateTime.MinValue ? dateTime.ToLocalTime().ToLocalizedString() : "Undefined";
            };

            void AppendStats(string caption, int total, int errors)
            {
                sb.Append(caption).Append(total).Append(" / ").Append(errors).AppendLine();
            }

            if (Locale.IsRussian)
            {
                sb.Append("Драйвер       : ").AppendLine(DriverName);
                sb.Append("Статус        : ").AppendLine(DeviceStatus.ToString(true));
                sb.Append("Время сеанса  : ").AppendLine(TimeToStringRu(LastSessionTime));
                sb.Append("Время команды : ").AppendLine(TimeToStringRu(LastCommandTime));
                sb.AppendLine();
                AppendStats("Сеансы  (всего / ошибок) : ", DeviceStats.SessionCount, DeviceStats.SessionErrors);
                AppendStats("Команды (всего / ошибок) : ", DeviceStats.CommandCount, DeviceStats.CommandErrors);
                AppendStats("Запросы (всего / ошибок) : ", DeviceStats.RequestCount, DeviceStats.RequestErrors);
            }
            else
            {
                sb.Append("Driver       : ").AppendLine(DriverName);
                sb.Append("Status       : ").AppendLine(DeviceStatus.ToString(false));
                sb.Append("Session time : ").AppendLine(TimeToStringEn(LastSessionTime));
                sb.Append("Command time : ").AppendLine(TimeToStringEn(LastCommandTime));
                sb.AppendLine();
                AppendStats("Sessions (total / errors) : ", DeviceStats.SessionCount, DeviceStats.SessionErrors);
                AppendStats("Commands (total / errors) : ", DeviceStats.CommandCount, DeviceStats.CommandErrors);
                AppendStats("Requests (total / errors) : ", DeviceStats.RequestCount, DeviceStats.RequestErrors);
            }

            sb.AppendLine();
            DeviceData.AppendInfo(sb);
            lastInfoLength = sb.Length;
            return sb.ToString();
        }

        /// <summary>
        /// Prompts to terminate the current operation.
        /// </summary>
        public void Terminate()
        {
            terminated = true;
            DeviceStatus = DeviceStatus.Undefined;
        }
    }
}
