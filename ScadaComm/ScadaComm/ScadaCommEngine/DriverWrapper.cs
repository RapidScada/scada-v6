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
 * Module   : ScadaCommEngine
 * Summary  : Represents a wrapper for safely calling methods of driver logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Comm.Drivers;
using Scada.Comm.Lang;
using Scada.Log;
using System;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Represents a wrapper for safely calling methods of driver logic.
    /// <param>Представляет обёртку для безопасного выполнения методов логики драйвера.</param>
    /// </summary>
    internal class DriverWrapper
    {
        private readonly ILog log; // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DriverWrapper(DriverLogic driverLogic, ILog log)
        {
            DriverLogic = driverLogic ?? throw new ArgumentNullException(nameof(driverLogic));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }


        /// <summary>
        /// Gets the device logic.
        /// </summary>
        public DriverLogic DriverLogic { get; }


        /// <summary>
        /// Calls the CreateDataSource method of the driver.
        /// </summary>
        public bool CreateDataSource(ICommContext commContext, DataSourceConfig dataSourceConfig, 
            out DataSourceLogic dataSourceLogic)
        {
            try
            {
                dataSourceLogic = DriverLogic.CreateDataSource(commContext, dataSourceConfig);
                return dataSourceLogic != null;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.ErrorInDriver, nameof(CreateDataSource), DriverLogic.Code);
                dataSourceLogic = null;
                return false;
            }
        }

        /// <summary>
        /// Calls the CreateChannel method of the driver.
        /// </summary>
        public bool CreateChannel(ILineContext lineContext, ChannelConfig channelConfig, out ChannelLogic channelLogic)
        {
            try
            {
                channelLogic = DriverLogic.CreateChannel(lineContext, channelConfig);
                return channelLogic != null;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.ErrorInDriver, nameof(CreateChannel), DriverLogic.Code);
                channelLogic = null;
                return false;
            }
        }

        /// <summary>
        /// Calls the CreateDevice method of the driver.
        /// </summary>
        public bool CreateDevice(ILineContext lineContext, DeviceConfig deviceConfig, out DeviceLogic deviceLogic)
        {
            try
            {
                deviceLogic = DriverLogic.CreateDevice(lineContext, deviceConfig);
                return deviceLogic != null;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.ErrorInDriver, nameof(CreateDevice), DriverLogic.Code);
                deviceLogic = null;
                return false;
            }
        }

        /// <summary>
        /// Calls the OnServiceStart method of the driver.
        /// </summary>
        public void OnServiceStart()
        {
            try
            {
                DriverLogic.OnServiceStart();
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.ErrorInDriver, nameof(OnServiceStart), DriverLogic.Code);
            }
        }
        
        /// <summary>
        /// Calls the OnServiceStop method of the driver.
        /// </summary>
        public void OnServiceStop()
        {
            try
            {
                DriverLogic.OnServiceStop();
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.ErrorInDriver, nameof(OnServiceStop), DriverLogic.Code);
            }
        }
    }
}
