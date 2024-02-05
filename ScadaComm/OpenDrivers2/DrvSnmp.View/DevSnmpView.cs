// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvSnmp.Config;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Forms.Forms;

namespace Scada.Comm.Drivers.DrvSnmp.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс устройства.</para>
    /// </summary>
    internal class DevSnmpView : DeviceView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevSnmpView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig)
            : base(parentView, lineConfig, deviceConfig)
        {
            CanShowProperties = true;
        }


        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public override bool ShowProperties()
        {
            new FrmModuleConfig(new SnmpConfigProvider(AppDirs.ConfigDir, DeviceNum)).ShowDialog();
            return false;
        }

        /// <summary>
        /// Gets the channel prototypes for the device.
        /// </summary>
        public override ICollection<CnlPrototype> GetCnlPrototypes()
        {
            // load device configuration
            SnmpDeviceConfig config = new();

            if (!config.Load(Path.Combine(AppDirs.ConfigDir, SnmpDeviceConfig.GetFileName(DeviceNum)),
                out string errMsg))
            {
                throw new ScadaException(errMsg);
            }

            // create channel prototypes
            List<CnlPrototype> cnlPrototypes = new();
            int eventMask = new EventMask { Enabled = true, StatusChange = true }.Value;

            foreach (VarGroupConfig varGroupConfig in config.VarGroups)
            {
                foreach (VariableConfig variableConfig in varGroupConfig.Variables)
                {
                    int dataTypeID = (int)variableConfig.DataType;
                    cnlPrototypes.Add(new CnlPrototype
                    {
                        Active = varGroupConfig.Active,
                        Name = variableConfig.Name,
                        DataTypeID = dataTypeID > 0 ? dataTypeID : null,
                        DataLen = variableConfig.DataLen > 0 ? 
                            DeviceTag.CalcDataLength(variableConfig.DataLen, variableConfig.DataType) : null,
                        CnlTypeID = CnlTypeID.Input,
                        TagCode = variableConfig.TagCode,
                        EventMask = eventMask,
                        FormatCode = DataTypeID.IsString(dataTypeID) ? FormatCode.String : null
                    });
                }
            }

            return cnlPrototypes;
        }
    }
}
