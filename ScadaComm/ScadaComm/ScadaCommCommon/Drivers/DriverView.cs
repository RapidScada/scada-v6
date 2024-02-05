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
 * Summary  : Represents the base class for driver user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using System.Collections.Generic;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Represents the base class for driver user interface.
    /// <para>Представляет базовый класс пользовательского интерфейса драйвера.</para>
    /// </summary>
    public abstract class DriverView : LibraryView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DriverView()
            : base()
        {
            AppConfig = null;
            CanCreateDataSource = false;
            CanCreateChannel = false;
            CanCreateDevice = false;
        }


        /// <summary>
        /// Gets or sets the application configuration.
        /// </summary>
        /// <remarks>Do not modify the configuration.</remarks>
        public CommConfig AppConfig { get; set; }

        /// <summary>
        /// Gets a value indicating whether the driver can create a data source.
        /// </summary>
        public bool CanCreateDataSource { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the driver can create a communication channel.
        /// </summary>
        public bool CanCreateChannel { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the driver can create a device.
        /// </summary>
        public bool CanCreateDevice { get; protected set; }

        /// <summary>
        /// Gets the communication channel types provided by the driver.
        /// </summary>
        public virtual ICollection<ChannelTypeName> ChannelTypes => null;


        /// <summary>
        /// Creates a new data source user interface.
        /// </summary>
        public virtual DataSourceView CreateDataSourceView(DataSourceConfig dataSourceConfig)
        {
            return null;
        }

        /// <summary>
        /// Creates a new communication channel user interface.
        /// </summary>
        public virtual ChannelView CreateChannelView(ChannelConfig channelConfig)
        {
            return null;
        }

        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public virtual DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return null;
        }
    }
}
