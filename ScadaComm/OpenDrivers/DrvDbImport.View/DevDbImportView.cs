// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvDbImport.Config;
using Scada.Data.Const;
using Scada.Data.Models;

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
            CanShowProperties = false;
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
            DbDeviceConfig config = new();

            if (!config.Load(Path.Combine(AppDirs.ConfigDir, DbDeviceConfig.GetFileName(DeviceNum)),
                out string errMsg))
            {
                throw new ScadaException(errMsg);
            }

            // create channel prototypes
            List<CnlPrototype> cnlPrototypes = new();
            int eventMask = new EventMask { Enabled = true, StatusChange = true }.Value;
            int cmdEventMask = new EventMask { Enabled = true, Command = true }.Value;

            // queues
            foreach (QueryConfig queryConfig in config.Queries)
            {
                foreach (string tagCode in queryConfig.TagCodes)
                {
                    cnlPrototypes.Add(new CnlPrototype
                    {
                        Name = tagCode,
                        CnlTypeID = CnlTypeID.Input,
                        TagCode = tagCode,
                        EventMask = eventMask
                    });
                }
            }

            // commands
            foreach (CommandConfig commandConfig in config.Commands)
            {
                cnlPrototypes.Add(new CnlPrototype
                {
                    Name = commandConfig.Name,
                    CnlTypeID = CnlTypeID.Output,
                    TagCode = commandConfig.CmdCode,
                    EventMask = cmdEventMask
                });
            }

            return cnlPrototypes;
        }
    }
}
