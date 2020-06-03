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
 * Summary  : Represents the base class for driver user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Agent;
using Scada.Client;
using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Data.Models;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Represents the base class for driver user interface.
    /// <para>Представляет базовый класс пользовательского интерфейса драйвера.</para>
    /// </summary>
    public abstract class DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DriverView()
        {
            BaseDataSet = null;
            AppConfig = null;
            AppDirs = null;
            ScadaClient = null;
            AgentClient = null;
            CanCreateChannel = false;
            CanCreateDevice = false;
            CanShowProperties = false;
        }


        /// <summary>
        /// Gets or sets the configuration database cache.
        /// </summary>
        public BaseDataSet BaseDataSet { get; set; }

        /// <summary>
        /// Gets or sets the application configuration.
        /// </summary>
        /// <remarks>Do not modify the configuration.</remarks>
        public CommConfig AppConfig { get; set; }

        /// <summary>
        /// Gets or sets the application directories.
        /// </summary>
        public CommDirs AppDirs { get; set; }

        /// <summary>
        /// Gets or sets the client to interact with the server.
        /// </summary>
        /// <remarks>Null allowed.</remarks>
        public ScadaClient ScadaClient { get; set; }

        /// <summary>
        /// Gets or sets the client to interact with the agent.
        /// </summary>
        /// <remarks>Null allowed.</remarks>
        public IAgentClient AgentClient { get; set; }

        /// <summary>
        /// Gets a value indicating whether the driver can create a communication channel.
        /// </summary>
        public bool CanCreateChannel { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the driver can create a device.
        /// </summary>
        public bool CanCreateDevice { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the driver can show a properties dialog box.
        /// </summary>
        public bool CanShowProperties { get; protected set; }

        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public abstract string Descr { get; }


        /// <summary>
        /// Creates a new user interface of a communication channel.
        /// </summary>
        public virtual ChannelView CreateChannelView()
        {
            return null;
        }

        /// <summary>
        /// Creates a new user interface of a device.
        /// </summary>
        public virtual DeviceView CreateDeviceView(int deviceNum)
        {
            return null;
        }

        /// <summary>
        /// Shows a modal dialog box for editing driver properties.
        /// </summary>
        public virtual bool ShowProperties()
        {
            return false;
        }
    }
}
