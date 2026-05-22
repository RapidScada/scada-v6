// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvRsClient.Config;
using Scada.Data.Const;
using Scada.Data.Models;

namespace Scada.Comm.Drivers.DrvRsClient.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс устройства.</para>
    /// </summary>
    internal class DevRsClientView : DeviceView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevRsClientView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig)
            : base(parentView, lineConfig, deviceConfig)
        {
            CanShowProperties = true;
        }


        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return false;
        }

        /// <summary>
        /// Gets the default polling options for the device.
        /// </summary>
        public override PollingOptions GetPollingOptions()
        {
            return new PollingOptions(0, 1000);
        }

        /// <summary>
        /// Gets the channel prototypes for the device.
        /// </summary>
        public override ICollection<CnlPrototype> GetCnlPrototypes()
        {
            // load device configuration
            RsClientDeviceConfig config = new();

            if (!config.Load(AppDirs.ConfigDir, DeviceNum, out string errMsg))
                throw new ScadaException(errMsg);

            // create channel prototypes
            List<CnlPrototype> cnlPrototypes = [];

            foreach (ItemGroupConfig itemGroupConfig in config.ItemGroups)
            {
                foreach (ItemConfig itemConfig in itemGroupConfig.Items)
                {
                    int eventMask = new EventMask
                    {
                        Enabled = true,
                        StatusChange = true,
                        Command = !itemConfig.ReadOnly
                    }.Value;

                    cnlPrototypes.Add(new CnlPrototype
                    {
                        Active = itemGroupConfig.Active,
                        Name = itemConfig.Name,
                        CnlTypeID = itemConfig.ReadOnly ? CnlTypeID.Input : CnlTypeID.InputOutput,
                        TagCode = "Cnl" + itemConfig.CnlNum,
                        EventMask = eventMask,
                    });
                }
            }

            return cnlPrototypes;
        }
    }
}
