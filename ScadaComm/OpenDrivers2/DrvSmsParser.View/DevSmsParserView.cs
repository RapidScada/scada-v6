// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using DrvSmsParser.Shared.Config;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvSmsParser.View.Forms;

namespace Scada.Comm.Drivers.DrvSmsParser.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс устройства.</para>
    /// </summary>
    internal class DevSmsParserView : DeviceView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevSmsParserView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig)
            : base(parentView, lineConfig, deviceConfig)
        {
            CanShowProperties = true;
        }


        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public override bool ShowProperties()
        {
            if (new FrmDeviceProperties(AppDirs, LineConfig, DeviceConfig).ShowDialog() == DialogResult.OK)
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

        /// <summary>
        /// Gets the default polling options for the device.
        /// </summary>
        public override PollingOptions GetPollingOptions()
        {
            return new PollingOptions(0, 0);
        }

        /// <summary>
        /// Gets the channel prototypes for the device.
        /// </summary>
        public override ICollection<CnlPrototype> GetCnlPrototypes()
        {
            DeviceTemplate deviceTemplate = new();
            string fileName = DeviceConfig.PollingOptions.CmdLine.Trim();

            if (!string.IsNullOrEmpty(fileName) &&
                !deviceTemplate.Load(Path.Combine(AppDirs.ConfigDir, fileName), out string errMsg))
            {
                throw new ScadaException(errMsg);
            }

            return
            [
                ..CnlPrototypeFactory.GetGeneralGroup().CnlPrototypes,
                ..CnlPrototypeFactory.GetCustomGroup(deviceTemplate).CnlPrototypes
            ];
        }
    }
}
