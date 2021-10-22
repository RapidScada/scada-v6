// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvModbus.View.Forms;
using System;

namespace Scada.Comm.Drivers.DrvModbus.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс КП.</para>
    /// </summary>
    public class DevModbusView : DeviceView
    {
        private readonly UiCustomization uiCustomization;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevModbusView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig, 
            UiCustomization uiCustomization) : base(parentView, lineConfig, deviceConfig)
        {
            this.uiCustomization = uiCustomization ?? throw new ArgumentNullException(nameof(uiCustomization));
            CanShowProperties = true;
        }

        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public override bool ShowProperties()
        {
            FrmDevProps.ShowDialog(DeviceNum, LineConfig, DeviceConfig, AppDirs, uiCustomization);
            return false;
        }
    }
}
