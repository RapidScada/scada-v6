// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Comm.Drivers.DrvOpcUa.View.Forms;
using Scada.Data.Const;
using Scada.Data.Models;

namespace Scada.Comm.Drivers.DrvOpcUa.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс устройства.</para>
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

        /// <summary>
        /// Gets the channel prototypes for the device.
        /// </summary>
        public override ICollection<CnlPrototype> GetCnlPrototypes()
        {
            OpcDeviceConfig opcDeviceConfig = new();

            if (!opcDeviceConfig.Load(Path.Combine(AppDirs.ConfigDir, OpcDeviceConfig.GetFileName(DeviceNum)),
                out string errMsg))
            {
                throw new ScadaException(errMsg);
            }

            // create channels for subscriptions
            List<CnlPrototype> cnlPrototypes = new();
            int tagNum = 1;
            int eventMask = new EventMask { Enabled = true, StatusChange = true, Command = true }.Value;
            int cmdEventMask = new EventMask { Enabled = true, Command = true }.Value;

            foreach (SubscriptionConfig subscriptionConfig in opcDeviceConfig.Subscriptions)
            {
                foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                {
                    CnlPrototype cnl = new()
                    {
                        Active = itemConfig.Active,
                        Name = itemConfig.DisplayName,
                        CnlTypeID = CnlTypeID.InputOutput,
                        TagNum = string.IsNullOrEmpty(itemConfig.TagCode) ? tagNum : null,
                        TagCode = itemConfig.TagCode,
                        EventMask = eventMask
                    };

                    if (itemConfig.IsString)
                    {
                        cnl.DataTypeID = DataTypeID.Unicode;
                        cnl.DataLen = DriverUtils.GetTagDataLength(itemConfig.DataLength);
                        cnl.FormatCode = FormatCode.String;
                    }
                    else if (itemConfig.IsArray)
                    {
                        cnl.DataLen = itemConfig.DataLength;
                    }

                    if (DriverUtils.DataTypeEquals(itemConfig.DataTypeName, typeof(DateTime)))
                        cnl.FormatCode = FormatCode.DateTime;

                    cnlPrototypes.Add(cnl);
                    tagNum++;
                }
            }

            // create channels for commands
            foreach (CommandConfig commandConfig in opcDeviceConfig.Commands)
            {
                CnlPrototype cnl = new()
                {
                    Name = commandConfig.DisplayName,
                    CnlTypeID = CnlTypeID.Output,
                    TagNum = string.IsNullOrEmpty(commandConfig.CmdCode) ? commandConfig.CmdNum : null,
                    TagCode = commandConfig.CmdCode,
                    EventMask = cmdEventMask
                };

                if (commandConfig.IsMethod)
                    cnl.FormatCode = FormatCode.Execute;
                else if (DriverUtils.DataTypeEquals(commandConfig.DataTypeName, typeof(string)))
                    cnl.FormatCode = FormatCode.String;
                else if (DriverUtils.DataTypeEquals(commandConfig.DataTypeName, typeof(DateTime)))
                    cnl.FormatCode = FormatCode.DateTime;

                cnlPrototypes.Add(cnl);
            }

            return cnlPrototypes;
        }
    }
}
