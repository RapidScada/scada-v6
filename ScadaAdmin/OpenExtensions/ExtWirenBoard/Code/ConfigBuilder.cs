// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Config;
using Scada.Admin.Extensions.ExtWirenBoard.Code.Meta;
using Scada.Admin.Extensions.ExtWirenBoard.Code.Models;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Admin.Extensions.ExtWirenBoard.Code
{
    /// <summary>
    /// Builds a project configuration.
    /// <para>Создает конфигурацию проекта.</para>
    /// </summary>
    internal class ConfigBuilder
    {
        /// <summary>
        /// The name of the driver for the devices being created.
        /// </summary>
        private const string MqttDriverName = "DrvMqttClient";
        /// <summary>
        /// The event mask for the channels being created.
        /// </summary>
        private static readonly int ControlEventMask = 
            new EventMask { Enabled = true, StatusChange = true, Command = true }.Value;

        private readonly IAdminContext adminContext;  // the Administrator context
        private readonly ScadaProject project;        // the project under development
        private readonly RichTextBoxHelper logHelper; // provides access to log
        
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigBuilder(IAdminContext adminContext, ScadaProject project, RichTextBoxHelper logHelper)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.logHelper = logHelper ?? throw new ArgumentNullException(nameof(logHelper));
            
            BuildResult = false;
            DeviceConfigs = new List<DeviceConfigEntry>();
        }


        /// <summary>
        /// Gets the result of building configuration.
        /// </summary>
        public bool BuildResult { get; private set; }

        /// <summary>
        /// Gets the created configuration of the devices.
        /// </summary>
        public List<DeviceConfigEntry> DeviceConfigs { get; }


        /// <summary>
        /// Builds the channels.
        /// </summary>
        private static void BuildCnls(DeviceConfigEntry entry, DeviceModel deviceModel,
            int? objNum, int deviceNum, ref int cnlNum)
        {
            // device error
            entry.Cnls.Add(new Cnl
            {
                CnlNum = cnlNum++,
                Active = true,
                Name = deviceModel.Meta.Name + " Error",
                CnlTypeID = CnlTypeID.Input,
                ObjNum = objNum,
                DeviceNum = deviceNum,
                TagCode = deviceModel.Code + "_error"
            });
        }

        /// <summary>
        /// Builds the channels.
        /// </summary>
        private void BuildCnls(DeviceConfigEntry entry, ControlModel controlModel, 
            ChannelNumberingOptions options, int? objNum, int deviceNum, ref int cnlNum)
        {
            string namePrefix = options.PrependDeviceName ? entry.DeviceEntity.Name + " - " : "";
            ControlMeta controlMeta = controlModel.Meta;
            int? formatID = GetFormatID(controlMeta);
            int? quantityID = GetQuantityID(controlMeta);
            int? unitID = GetUnitID(controlMeta);

            // control value
            entry.Cnls.Add(new Cnl
            {
                CnlNum = cnlNum++,
                Active = true,
                Name = namePrefix + controlMeta.Name + " Last",
                CnlTypeID = CnlTypeID.Input,
                ObjNum = objNum,
                DeviceNum = deviceNum,
                TagCode = controlModel.Code,
                FormatID = formatID,
                QuantityID = quantityID,
                UnitID = unitID
            });

            // control error
            entry.Cnls.Add(new Cnl
            {
                CnlNum = cnlNum++,
                Active = true,
                Name = namePrefix + controlMeta.Name + " Error",
                CnlTypeID = CnlTypeID.Input,
                ObjNum = objNum,
                DeviceNum = deviceNum,
                TagCode = controlModel.Code + "_error"
            });

            // calculated channel
            entry.Cnls.Add(new Cnl
            {
                CnlNum = cnlNum++,
                Active = true,
                Name = namePrefix + controlMeta.Name,
                CnlTypeID = controlMeta.ReadOnly ? CnlTypeID.Calculated : CnlTypeID.CalculatedOutput,
                ObjNum = objNum,
                DeviceNum = deviceNum,
                TagCode = controlModel.Code,
                FormulaEnabled = true,
                InFormula = "WB_CalcControl()",
                FormatID = formatID,
                QuantityID = quantityID,
                UnitID = unitID,
                EventMask = ControlEventMask
            });
        }

        /// <summary>
        /// Gets the control format.
        /// </summary>
        private int? GetFormatID(ControlMeta controlMeta)
        {
            Format format = controlMeta.Type switch
            {
                "pushbutton" => project.ConfigBase.GetFormatByCode(FormatCode.Execute),
                "switch" => project.ConfigBase.GetFormatByCode(FormatCode.OffOn),
                _ => null
            };
            return format?.FormatID;
        }

        /// <summary>
        /// Gets the control quantity.
        /// </summary>
        private int? GetQuantityID(ControlMeta controlMeta)
        {
            Quantity quantity = controlMeta.Type switch
            {
                "temperature" => project.ConfigBase.GetQuantityByCode(QuantityCode.Temperature),
                "voltage" => project.ConfigBase.GetQuantityByCode(QuantityCode.Voltage),
                _ => null
            };
            return quantity?.QuantityID;
        }

        /// <summary>
        /// Gets the control unit.
        /// </summary>
        private int? GetUnitID(ControlMeta controlMeta)
        {
            Unit unit = controlMeta.Type switch
            {
                "temperature" => project.ConfigBase.GetUnitByCode(UnitCode.Celsius),
                "voltage" => project.ConfigBase.GetUnitByCode(UnitCode.Volt),
                _ => null
            };
            return unit?.UnitID;
        }

        /// <summary>
        /// Builds the MQTT device configuration.
        /// </summary>
        private static void BuildMqttDeviceConfig(DeviceConfigEntry entry, DeviceModel deviceModel)
        {
            // subscription to device error
            entry.MqttDeviceConfig.Subscriptions.Add(new SubscriptionConfig
            {
                Topic = deviceModel.Topic + "/meta/error",
                DisplayName = deviceModel.Meta.Name + " Error",
                TagCode = deviceModel.Code + "_error",
                JsEnabled = true,
                JsFileName = "wb_error.js"
            });
        }

        /// <summary>
        /// Builds the MQTT device configuration.
        /// </summary>
        private static void BuildMqttDeviceConfig(DeviceConfigEntry entry, ControlModel controlModel)
        {
            // subscription to control value
            entry.MqttDeviceConfig.Subscriptions.Add(new SubscriptionConfig
            {
                Topic = controlModel.Topic,
                DisplayName = controlModel.Meta.Name,
                TagCode = controlModel.Code
            });

            // subscription to control error
            entry.MqttDeviceConfig.Subscriptions.Add(new SubscriptionConfig
            {
                Topic = controlModel.Topic + "/meta/error",
                DisplayName = controlModel.Meta.Name + " Error",
                TagCode = controlModel.Code + "_error",
                JsEnabled = true,
                JsFileName = "wb_error.js"
            });

            // command
            if (!controlModel.Meta.ReadOnly)
            {
                entry.MqttDeviceConfig.Commands.Add(new CommandConfig
                {
                    Topic = controlModel.Topic + "/on",
                    DisplayName = controlModel.Meta.Name,
                    CmdCode = controlModel.Code
                });
            }
        }


        /// <summary>
        /// Builds a project configuration.
        /// </summary>
        public void Build(List<DeviceModel> selectedDevices, int commLineNum, 
            int startDeviceNum, int startCnlNum, int? objNum)
        {
            ArgumentNullException.ThrowIfNull(selectedDevices, nameof(selectedDevices));

            try
            {
                ChannelNumberingOptions options = adminContext.AppConfig.ChannelNumberingOptions;
                int deviceNum = startDeviceNum;
                int cnlNum = startCnlNum;
                int? deviceTypeID = project.ConfigBase.DevTypeTable.Enumerate()
                    .Where(devType => devType.Driver == MqttDriverName).FirstOrDefault()?.DevTypeID;

                if (deviceTypeID == null)
                {
                    logHelper.WriteError(string.Format(Locale.IsRussian ?
                        "Драйвер {0} не найден в таблице Типы устройств" :
                        "Driver {0} not found in the Device Types table", MqttDriverName));
                    return;
                }

                foreach (DeviceModel deviceModel in selectedDevices)
                {
                    bool deviceIgnored = true;

                    if (deviceModel.Controls.Count > 0)
                    {
                        DeviceConfigEntry entry = new();

                        // device entity
                        Device deviceEntity = entry.DeviceEntity;
                        deviceEntity.DeviceNum = deviceNum;
                        deviceEntity.Name = deviceModel.Meta.Name;
                        deviceEntity.Code = deviceModel.Code;
                        deviceEntity.DevTypeID = deviceTypeID;
                        deviceEntity.CommLineNum = commLineNum;

                        // device configuration
                        DeviceConfig deviceConfig = entry.DeviceConfig;
                        deviceConfig.DeviceNum = deviceNum;
                        deviceConfig.Name = deviceModel.Meta.Name;
                        deviceConfig.Driver = MqttDriverName;

                        // channels and subscriptions
                        BuildCnls(entry, deviceModel, objNum, deviceNum, ref cnlNum);
                        BuildMqttDeviceConfig(entry, deviceModel);

                        foreach (ControlModel controlModel in deviceModel.Controls)
                        {
                            BuildCnls(entry, controlModel, options, objNum, deviceNum, ref cnlNum);
                            BuildMqttDeviceConfig(entry, controlModel);
                        }

                        if (entry.Cnls.Count > 0)
                        {
                            logHelper.WriteMessage(string.Format(Locale.IsRussian ?
                                "Устройство [{0}] {1}" :
                                "Device [{0}] {1}", deviceNum, deviceModel.Meta.Name));

                            logHelper.WriteMessage(string.Format(Locale.IsRussian ?
                                "Каналы: {0}" :
                                "Channels: {0}", entry.Cnls.Select(cnl => cnl.CnlNum).ToArray().ToRangeString()));

                            DeviceConfigs.Add(entry);
                            deviceIgnored = false;
                        }

                        deviceNum++;
                    }

                    if (deviceIgnored)
                    {
                        logHelper.WriteWarning(string.Format(Locale.IsRussian ?
                            "Устройство {0} игнорируется" :
                            "Device {0} ignored", deviceModel.Meta.Name));
                    }

                    logHelper.WriteLine();
                    cnlNum = AdjustCnlNum(options, cnlNum);
                }

                BuildResult = DeviceConfigs.Count > 0;
            }
            catch (Exception ex)
            {
                logHelper.WriteError(ex.Message);
            }
        }

        /// <summary>
        /// Gets a starting channel number for a next device.
        /// </summary>
        public static int AdjustCnlNum(ChannelNumberingOptions options, int nextCnlNum)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));
            int cnlNum = options.Gap > 0 ? nextCnlNum + options.Gap : nextCnlNum;

            if (options.Multiplicity > 1)
            {
                int remainder = cnlNum % options.Multiplicity;

                if (remainder > 0)
                    cnlNum = cnlNum - remainder + options.Multiplicity;

                return cnlNum + options.Shift;
            }
            else
            {
                return cnlNum;
            }
        }
    }
}
