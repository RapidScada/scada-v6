// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Config;
using Scada.Admin.Extensions.ExtWirenBoard.Code.Models;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Data.Entities;
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

        private readonly ScadaProject project;        // the project under development
        private readonly RichTextBoxHelper logHelper; // provides access to log
        
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigBuilder(ScadaProject project, RichTextBoxHelper logHelper)
        {
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
        /// Builds a project configuration.
        /// </summary>
        public void Build(List<DeviceModel> selectedDevices, int commLineNum, int startDeviceNum, int startCnlNum)
        {
            ArgumentNullException.ThrowIfNull(selectedDevices, nameof(selectedDevices));

            try
            {
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
                    if (deviceModel.Controls.Count > 0)
                    {
                        logHelper.WriteMessage(string.Format(Locale.IsRussian ?
                            "Устройство [{0}] {1}" :
                            "Device [{0}] {1}", deviceNum, deviceModel.Meta.Name));

                        DeviceConfigEntry entry = new();
                        DeviceConfigs.Add(entry);

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
                        deviceNum++;

                        // channels
                        foreach (ControlModel controlModel in deviceModel.Controls)
                        {

                        }

                        logHelper.WriteLine();
                    }
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
        public static int AdjustCnlNum(ChannelNumberingOptions options, int lastCnlNum, int cnlNumToAdjust)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));
            int cnlNum = cnlNumToAdjust - lastCnlNum <= options.Gap 
                ? lastCnlNum + options.Gap + 1
                : cnlNumToAdjust;

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
