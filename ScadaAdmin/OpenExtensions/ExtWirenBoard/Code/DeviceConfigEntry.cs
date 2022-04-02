// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Data.Entities;

namespace Scada.Admin.Extensions.ExtWirenBoard.Code
{
    /// <summary>
    /// Represents an entry that aggregates device configuration information.
    /// <para>Представляет запись, объединяющую информацию о конфигурации устройства.</para>
    /// </summary>
    internal class DeviceConfigEntry
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceConfigEntry()
        {
            DeviceEntity = new Device();
            DeviceConfig = new DeviceConfig();
            MqttDeviceConfig = new MqttClientDeviceConfig();
            Cnls = new List<Cnl>();
        }


        /// <summary>
        /// Gets the device entity for the configuration database.
        /// </summary>
        public Device DeviceEntity { get; }

        /// <summary>
        /// Gets the general device configuration for Communicator.
        /// </summary>
        public DeviceConfig DeviceConfig { get; }

        /// <summary>
        /// Gets the MQTT device configuration for Communicator.
        /// </summary>
        public MqttClientDeviceConfig MqttDeviceConfig { get; }

        /// <summary>
        /// Gets the channels for the configuration database.
        /// </summary>
        public List<Cnl> Cnls { get; }
    }
}
