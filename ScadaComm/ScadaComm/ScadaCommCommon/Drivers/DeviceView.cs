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
 * Summary  : Represents the base class for device user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2020
 */

using Scada.Agent;
using Scada.Client;
using Scada.Comm.Config;
using Scada.Data.Models;
using System.Collections.Generic;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Represents the base class for device user interface.
    /// <para>Представляет базовый класс пользовательского интерфейса КП.</para>
    /// </summary>
    public abstract class DeviceView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceView(int deviceNum)
        {
            DeviceNum = deviceNum;
            BaseDataSet = null;
            AppConfig = null;
            LineConfig = null;
            DeviceConfig = null;
            AppDirs = null;
            ScadaClient = null;
            AgentClient = null;
            CanShowProperties = false;
            LineConfigModified = false;
            DeviceConfigModified = false;
        }


        /// <summary>
        /// Gets the device number.
        /// </summary>
        public int DeviceNum { get; protected set; }

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
        /// Gets or sets the communication line configuration.
        /// </summary>
        public LineConfig LineConfig { get; set; }

        /// <summary>
        /// Gets or sets the device configuration.
        /// </summary>
        public DeviceConfig DeviceConfig { get; set; }

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
        /// Gets a value indicating whether the device can show a properties dialog box.
        /// </summary>
        public bool CanShowProperties { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the communication line configuration was modified by a user.
        /// </summary>
        public bool LineConfigModified { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the device configuration was modified by a user.
        /// </summary>
        public bool DeviceConfigModified { get; protected set; }


        /// <summary>
        /// Gets the default polling options for the device.
        /// </summary>
        public virtual PollingOptions GetPollingOptions()
        {
            return PollingOptions.CreateDefault();
        }

        /// <summary>
        /// Gets the input channel prototypes for the device.
        /// </summary>
        public virtual ICollection<InCnlPrototype> GetInCnls()
        {
            return null;
        }

        /// <summary>
        /// Gets the output channel prototypes for the device.
        /// </summary>
        public virtual ICollection<OutCnlPrototype> GetOutCnls()
        {
            return null;
        }

        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public virtual bool ShowProperties()
        {
            LineConfigModified = false;
            DeviceConfigModified = false;
            return false;
        }
    }
}
