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
            DeviceEntities = new List<Device>();
            DeviceConfigs = new List<DeviceConfig>();
            Cnls = new List<Cnl>();
        }


        /// <summary>
        /// Gets the result of building configuration.
        /// </summary>
        public bool BuildResult { get; private set; }

        /// <summary>
        /// Gets the devices created for the configuration database.
        /// </summary>
        public List<Device> DeviceEntities { get; }

        /// <summary>
        /// Gets the devices created for the Communicator configuration.
        /// </summary>
        public List<DeviceConfig> DeviceConfigs { get; }

        /// <summary>
        /// Gets the channels created for the configuration database.
        /// </summary>
        public List<Cnl> Cnls { get; }


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
                        // add device
                        DeviceEntities.Add(new Device
                        {
                            DeviceNum = deviceNum,
                            Name = deviceModel.Meta.Name,
                            Code = deviceModel.Code,
                            DevTypeID = deviceTypeID,
                            CommLineNum = commLineNum
                        });

                        DeviceConfigs.Add(new DeviceConfig
                        {
                            DeviceNum = deviceNum,
                            Name = deviceModel.Meta.Name,
                            Driver = MqttDriverName
                        });

                        logHelper.WriteMessage(string.Format(Locale.IsRussian ?
                            "Устройство [{0}] {1}" :
                            "Device [{0}] {1}", deviceNum, deviceModel.Meta.Name));
                        deviceNum++;

                        // add channels
                        foreach (ControlModel controlModel in deviceModel.Controls)
                        {

                        }

                        logHelper.WriteLine();
                    }
                }

                BuildResult = DeviceEntities.Count > 0;
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
