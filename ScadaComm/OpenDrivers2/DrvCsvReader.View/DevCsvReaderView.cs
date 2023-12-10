// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvSms;
using Scada.Forms.Forms;

namespace Scada.Comm.Drivers.DrvCsvReader.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс устройства.</para>
    /// </summary>
    internal class DevCsvReaderView : DeviceView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevCsvReaderView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig)
            : base(parentView, lineConfig, deviceConfig)
        {
            CanShowProperties = true;
        }


        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public override bool ShowProperties()
        {
            CsvReaderOptions options = new(DeviceConfig.PollingOptions.CustomOptions);
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

        /// <summary>
        /// Gets the default polling options for the device.
        /// </summary>
        public override PollingOptions GetPollingOptions()
        {
            PollingOptions pollingOptions = PollingOptions.CreateDefault();
            new CsvReaderOptions().AddToOptionList(pollingOptions.CustomOptions);
            return pollingOptions;
        }

        /// <summary>
        /// Gets the channel prototypes for the device.
        /// </summary>
        public override ICollection<CnlPrototype> GetCnlPrototypes()
        {
            List<CnlPrototype> cnlPrototypes = new();
            CsvReaderOptions options = new(DeviceConfig.PollingOptions.CustomOptions);

            for (int tagNum = 1; tagNum <= options.TagCount; tagNum++)
            {
                string tagCode = TagCode.GetMainTagCode(tagNum);
                cnlPrototypes.Add(new CnlPrototype
                {
                    Name = tagCode,
                    TagCode = tagCode
                });
            }

            return cnlPrototypes;
        }
    }
}
