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

using Scada.Comm.Config;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Represents the base class for device logic.
    /// <para>Представляет базовый класс логики КП.</para>
    /// </summary>
    public abstract class DeviceLogic
    {
        private ILineContext lineContext;   // the communication line context
        private DeviceConfig deviceConfig;  // the device configuration

        /// <summary>
        /// Necessary to stop the thread.
        /// </summary>
        public volatile bool Terminated;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceLogic(int deviceNum)
        {
            lineContext = null;
            deviceConfig = null;
            Terminated = false;

            Log = null;
            IsBound = false;
            DeviceNum = deviceNum;
            DeviceName = "";
            NumAddress = 0;
            StrAddress = "";
            PollingOptions = null;
            ReqRetries = 0;
            AppContext = null;
            DriverCode = "";
            CanSendCommands = false;
            ConnectionRequired = true;
        }


        /// <summary>
        /// Gets or sets the communication line log.
        /// </summary>
        protected ILog Log { get; set; }

        /// <summary>
        /// Gets a value indicating whether the device is bound to the server.
        /// </summary>
        public bool IsBound { get; protected set; }

        /// <summary>
        /// Gets the device number.
        /// </summary>
        public int DeviceNum { get; protected set; }

        /// <summary>
        /// Gets the device name.
        /// </summary>
        public string DeviceName { get; protected set; }

        /// <summary>
        /// Gets the numeric address.
        /// </summary>
        public int NumAddress { get; protected set; }

        /// <summary>
        /// Gets the string address.
        /// </summary>
        public string StrAddress { get; protected set; }

        /// <summary>
        /// Gets the polling options.
        /// </summary>
        public PollingOptions PollingOptions { get; protected set; }

        /// <summary>
        /// Gets or sets the number of retries of the request in case of an error.
        /// </summary>
        public int ReqRetries { get; protected set; }

        /// <summary>
        /// Gets or sets the application context.
        /// </summary>
        public ICommContext AppContext { get; set; }

        /// <summary>
        /// Gets or sets the Communicator context.
        /// </summary>
        public ILineContext LineContext
        {
            get
            {
                return lineContext;
            }
            set
            {
                lineContext = value ?? throw new ArgumentNullException();
                LineOptions lineOptions = lineContext.LineConfig.LineOptions;
                Log = lineOptions.DetailedLog ? lineContext.Log : null;
                IsBound = IsBound && lineContext.LineConfig.IsBound;
                ReqRetries = lineOptions.ReqRetries;
            }
        }

        /// <summary>
        /// Gets or sets the device configuration.
        /// </summary>
        public DeviceConfig DeviceConfig
        {
            get
            {
                return deviceConfig;
            }
            set
            {
                deviceConfig = value ?? throw new ArgumentNullException();

                if (DeviceNum != deviceConfig.DeviceNum)
                    throw new ScadaException("Device number mismatch.");

                bool lineIsBound = LineContext == null || LineContext.LineConfig.IsBound;
                IsBound = deviceConfig.IsBound && lineIsBound;
                DeviceName = deviceConfig.Name;
                NumAddress = deviceConfig.NumAddress;
                StrAddress = deviceConfig.StrAddress;
                PollingOptions = deviceConfig.PollingOptions;
            }
        }

        /// <summary>
        /// Gets or sets the driver code.
        /// </summary>
        public string DriverCode { get; set; }

        /// <summary>
        /// Gets a value indicating whether the device can send telecontrol commands.
        /// </summary>
        public bool CanSendCommands { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether a connection is required to communicate with the device.
        /// </summary>
        public bool ConnectionRequired { get; protected set; }


        /// <summary>
        /// Writes the specified line to the log.
        /// </summary>
        protected void WriteToLog(string text = "")
        {
            Log?.WriteLine(text);
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
    }
}
