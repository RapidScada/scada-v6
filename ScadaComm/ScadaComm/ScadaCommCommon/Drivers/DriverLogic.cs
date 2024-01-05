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
 * Summary  : Represents the base class for driver logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Log;
using Scada.Storages;
using System;

namespace Scada.Comm.Drivers
{
    /// <summary>
    /// Represents the base class for driver logic.
    /// <para>Представляет базовый класс логики драйвера.</para>
    /// </summary>
    public abstract class DriverLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DriverLogic(ICommContext commContext)
        {
            CommContext = commContext ?? throw new ArgumentNullException(nameof(commContext));
            AppDirs = commContext.AppDirs;
            Storage = commContext.Storage;
            Log = commContext.Log;
        }


        /// <summary>
        /// Gets the Communicator context.
        /// </summary>
        protected ICommContext CommContext { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        protected CommDirs AppDirs { get; }

        /// <summary>
        /// Gets the application storage.
        /// </summary>
        protected IStorage Storage { get; }

        /// <summary>
        /// Gets or sets the driver log.
        /// </summary>
        protected ILog Log { get; set; }

        /// <summary>
        /// Gets the driver code.
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// Gets the driver version.
        /// </summary>
        public virtual string Version
        {
            get
            {
                return GetType().Assembly.GetName().Version.ToString();
            }
        }


        /// <summary>
        /// Creates a new data source.
        /// </summary>
        public virtual DataSourceLogic CreateDataSource(ICommContext commContext, DataSourceConfig dataSourceConfig)
        {
            return null;
        }

        /// <summary>
        /// Creates a new communication channel.
        /// </summary>
        public virtual ChannelLogic CreateChannel(ILineContext lineContext, ChannelConfig channelConfig)
        {
            return null;
        }

        /// <summary>
        /// Creates a new device.
        /// </summary>
        public virtual DeviceLogic CreateDevice(ILineContext lineContext, DeviceConfig deviceConfig)
        {
            return null;
        }

        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public virtual void OnServiceStart()
        {
        }

        /// <summary>
        /// Performs actions when the service stops.
        /// </summary>
        public virtual void OnServiceStop()
        {
        }
    }
}
