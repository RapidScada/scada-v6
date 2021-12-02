// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvOpcUa.View.Forms;

namespace Scada.Comm.Drivers.DrvOpcUa.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс КП.</para>
    /// </summary>
    internal class DevOpcUaView : DeviceView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevOpcUaView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig)
            : base(parentView, lineConfig, deviceConfig)
        {
            CanShowProperties = true;
        }

        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public override bool ShowProperties()
        {
            new FrmConfig(AppDirs, DeviceNum).ShowDialog();
            return false;
        }
    }
}
