// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvTester.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс КП.</para>
    /// </summary>
    internal class DevTesterView : DeviceView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevTesterView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig)
            : base(parentView, lineConfig, deviceConfig)
        {
            CanShowProperties = true;
        }

        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public override bool ShowProperties()
        {
            TesterOptions options = new(DeviceConfig.PollingOptions.CustomOptions);
            FrmOptions frmOptions = new() { Options = options };

            if (frmOptions.ShowDialog() == DialogResult.OK)
            {
                options.AddToOptionList(DeviceConfig.PollingOptions.CustomOptions);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
