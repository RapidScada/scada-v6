// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using Scada.Comm.Drivers.DrvModbus.View.Forms;
using Scada.Data.Const;
using Scada.Data.Models;

namespace Scada.Comm.Drivers.DrvModbus.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс устройства.</para>
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
        /// Loads a device template from the configuration file.
        /// </summary>
        protected DeviceTemplate LoadDeviceTemplate()
        {
            DeviceTemplate deviceTemplate = null;
            string fileName = DeviceConfig.PollingOptions.CmdLine.Trim();

            if (!string.IsNullOrEmpty(fileName))
            {
                deviceTemplate = customUi.CreateDeviceTemplate();

                if (!deviceTemplate.Load(Path.Combine(AppDirs.ConfigDir, fileName), out string errMsg))
                    throw new ScadaException(errMsg);
            }

            return deviceTemplate;
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

        /// <summary>
        /// Gets the channel prototypes for the device.
        /// </summary>
        public override ICollection<CnlPrototype> GetCnlPrototypes()
        {
            if (LoadDeviceTemplate() is not DeviceTemplate deviceTemplate)
                return null;

            List<CnlPrototype> cnlPrototypes = new();
            int tagNum = 1;

            foreach (ElemGroupConfig elemGroupConfig in deviceTemplate.ElemGroups)
            {
                foreach (ElemConfig elemConfig in elemGroupConfig.Elems)
                {
                    // create channel for element
                    bool isBool = elemConfig.ElemType == ElemType.Bool;
                    int eventMask = new EventMask
                    {
                        Enabled = true,
                        DataChange = isBool,
                        StatusChange = !isBool,
                        Command = !elemConfig.ReadOnly
                    }.Value;

                    cnlPrototypes.Add(new CnlPrototype
                    {
                        Active = elemGroupConfig.Active,
                        Name = elemConfig.Name,
                        CnlTypeID = elemConfig.ReadOnly ? CnlTypeID.Input : CnlTypeID.InputOutput,
                        TagNum = string.IsNullOrEmpty(elemConfig.TagCode) ? tagNum : null,
                        TagCode = elemConfig.TagCode,
                        FormatCode = isBool
                            ? FormatCode.OffOn
                            : elemConfig.IsBitMask ? FormatCode.X : null,
                        EventMask = eventMask
                    });

                    // create channels for bit mask
                    if (elemConfig.IsBitMask && elemConfig.ElemType != ElemType.Bool)
                    {
                        eventMask = new EventMask
                        {
                            Enabled = true,
                            DataChange = true,
                            Command = !elemConfig.ReadOnly
                        }.Value;

                        for (int bit = 0, bitCnt = ModbusUtils.GetDataLength(elemConfig.ElemType) * 8;
                            bit < bitCnt; bit++)
                        {
                            cnlPrototypes.Add(new CnlPrototype
                            {
                                Active = elemGroupConfig.Active,
                                Name = elemConfig.Name + "[" + bit + "]",
                                CnlTypeID = elemConfig.ReadOnly ? CnlTypeID.Calculated : CnlTypeID.CalculatedOutput,
                                TagNum = string.IsNullOrEmpty(elemConfig.TagCode) ? tagNum : null,
                                TagCode = elemConfig.TagCode,
                                FormatCode = FormatCode.OffOn,
                                FormulaEnabled = true,
                                InFormula = $"GetBit(DataRel({-bit - 1}), {bit})",
                                OutFormula = elemConfig.ReadOnly ? null : $"SetBit(DataRel({-bit - 1}), {bit}, Cmd)",
                                EventMask = eventMask
                            });
                        }
                    }

                    tagNum++;
                }
            }

            // create channels for commands
            int cmdEventMask = new EventMask { Enabled = true, Command = true }.Value;

            foreach (CmdConfig cmdConfig in deviceTemplate.Cmds)
            {
                cnlPrototypes.Add(new CnlPrototype
                {
                    Name = cmdConfig.Name,
                    CnlTypeID = CnlTypeID.Output,
                    TagNum = string.IsNullOrEmpty(cmdConfig.CmdCode) ? cmdConfig.CmdNum : null,
                    TagCode = cmdConfig.CmdCode,
                    FormatCode = cmdConfig.DataBlock == DataBlock.Coils && !cmdConfig.Multiple ?
                        FormatCode.OffOn : null,
                    EventMask = cmdEventMask
                });
            }

            return cnlPrototypes;
        }
    }
}
