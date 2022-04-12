// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvMqttClient.View
{
    /// <summary>
    /// Represents an intermediary between a module configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией модуля и формой конфигурации.</para>
    /// </summary>
    internal class MqttClientConfigProvider : ModuleConfigProvider
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttClientConfigProvider(string configDir, int deviceNum)
            : base()
        {
            ConfigFileName = Path.Combine(configDir, MqttClientDeviceConfig.GetFileName(deviceNum));
            Config = new MqttClientDeviceConfig();
        }
    }
}
