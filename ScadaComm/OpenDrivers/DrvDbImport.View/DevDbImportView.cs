// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;

namespace Scada.Comm.Drivers.DrvDbImport.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс устройства.</para>
    /// </summary>
    internal class DevDbImportView : DeviceView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevDbImportView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig)
            : base(parentView, lineConfig, deviceConfig)
        {
        }

        /// <summary>
        /// Gets the default polling options for the device.
        /// </summary>
        public override PollingOptions GetPollingOptions()
        {
            return PollingOptions.CreateWithDefaultDelay();
        }
    }
}
