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
 * Summary  : Represents the base class for device user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2021
 */

using Scada.Comm.Config;
using Scada.Comm.Drivers;
using System;
using System.Collections.Generic;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents the base class for device user interface.
    /// <para>Представляет базовый класс пользовательского интерфейса устройства.</para>
    /// </summary>
    public abstract class DeviceView : LibraryView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig)
            : base(parentView)
        {
            AppConfig = parentView.AppConfig;
            LineConfig = lineConfig ?? throw new ArgumentNullException(nameof(lineConfig));
            DeviceConfig = deviceConfig ?? throw new ArgumentNullException(nameof(deviceConfig));
            DeviceNum = deviceConfig.DeviceNum;
            LineConfigModified = false;
            DeviceConfigModified = false;
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        /// <remarks>Do not modify the configuration.</remarks>
        public CommConfig AppConfig { get; }

        /// <summary>
        /// Gets the communication line configuration.
        /// </summary>
        public LineConfig LineConfig { get; }

        /// <summary>
        /// Gets the device configuration.
        /// </summary>
        public DeviceConfig DeviceConfig { get; }

        /// <summary>
        /// Gets the device number.
        /// </summary>
        public int DeviceNum { get; }

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
        /// Gets the channel prototypes for the device.
        /// </summary>
        public virtual ICollection<CnlPrototype> GetCnlPrototypes()
        {
            return null;
        }
    }
}
