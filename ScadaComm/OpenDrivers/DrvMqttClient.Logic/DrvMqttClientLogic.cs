// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Comm.Lang;
using Scada.Storages;

namespace Scada.Comm.Drivers.DrvMqttClient.Logic
{
    /// <summary>
    /// Implements the driver logic.
    /// <para>Реализует логику драйвера.</para>
    /// </summary>
    public class DrvMqttClientLogic : DriverLogic
    {
        private readonly MqttDriverConfig driverConfig; // the driver configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvMqttClientLogic(ICommContext commContext)
            : base(commContext)
        {
            driverConfig = new MqttDriverConfig();
        }


        /// <summary>
        /// Gets the driver code.
        /// </summary>
        public override string Code
        {
            get
            {
                return DriverUtils.DriverCode;
            }
        }


        /// <summary>
        /// Creates a new device.
        /// </summary>
        public override DeviceLogic CreateDevice(ILineContext lineContext, DeviceConfig deviceConfig)
        {
            return new DevMqttClientLogic(CommContext, lineContext, deviceConfig, driverConfig);
        }

        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public override void OnServiceStart()
        {
            if (CommContext.Storage.GetFileInfo(DataCategory.Config, MqttDriverConfig.DefaultFileName).Exists &&
                !driverConfig.Load(CommContext.Storage, MqttDriverConfig.DefaultFileName, out string errMsg))
            {
                Log.WriteError(CommPhrases.DriverMessage, Code, errMsg);
            }
        }
    }
}
