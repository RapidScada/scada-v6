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
 * Module   : ScadaCommEngine
 * Summary  : Represents a communication line
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2020
 */

using Scada.Comm.Config;
using Scada.Comm.Drivers;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Represents a communication line.
    /// <para>Представляет линию связи.</para>
    /// </summary>
    internal class CommLine
    {
        private readonly CoreLogic coreLogic; // the Communicator logic instance

        private Thread thread;                // the working thread of the communication line
        private volatile bool terminated;     // necessary to stop the thread
        private volatile CommLineStatus lineStatus; // the current communication line status
        private List<DeviceLogic> devices;    // the list of devices


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private CommLine(LineConfig lineConfig, CoreLogic coreLogic)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException(nameof(coreLogic));
            LineConfig = lineConfig ?? throw new ArgumentNullException(nameof(lineConfig));

            thread = null;
            terminated = false;
            lineStatus = CommLineStatus.Undefined;
            devices = new List<DeviceLogic>();
        }


        /// <summary>
        /// Gets the communication line configuration.
        /// </summary>
        public LineConfig LineConfig { get; }

        /// <summary>
        /// Gets a value indicating whether the communication line is terminated.
        /// </summary>
        public bool IsTerminated
        {
            get
            {
                return lineStatus == CommLineStatus.Terminated;
            }
        }


        /// <summary>
        /// Starts the communication line.
        /// </summary>
        public bool Start()
        {
            return false;
        }

        /// <summary>
        /// Begins termination process of the communication line.
        /// </summary>
        public void Terminate()
        {

        }

        /// <summary>
        /// Creates a communication line, communication channel and devices.
        /// </summary>
        public static CommLine Create(LineConfig lineConfig, CoreLogic coreLogic, DriverHolder driverHolder)
        {
            CommLine commLine = new CommLine(lineConfig, coreLogic);
            return commLine;
        }
    }
}
