// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvModbus.View.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvModbus.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс КП.</para>
    /// </summary>
    public class DevModbusView : DeviceView
    {
        private readonly CustomUi customUi;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevModbusView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig, 
            CustomUi customUi) : base(parentView, lineConfig, deviceConfig)
        {
            this.customUi = customUi ?? throw new ArgumentNullException(nameof(customUi));
            CanShowProperties = true;
        }

        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public override bool ShowProperties()
        {
            if (new FrmDeviceProps(AppDirs, LineConfig, DeviceConfig, customUi).ShowDialog() == DialogResult.OK)
            {
                LineConfigModified = true;
                DeviceConfigModified = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
