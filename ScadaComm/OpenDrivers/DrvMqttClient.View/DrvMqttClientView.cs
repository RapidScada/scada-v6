// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.ComponentModel;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvMqttClient.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvMqttClientView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvMqttClientView()
        {
            CanCreateDevice = true;
        }


        /// <summary>
        /// Gets the driver name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "MQTT-клиент" : "MQTT Client";
            }
        }

        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Получает данные устройств от MQTT-брокера и публикует команды." :
                    "Receives device data from MQTT broker and publishes commands.";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, DriverUtils.DriverCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            DriverPhrases.Init();
            AttrTranslator.Translate(typeof(DeviceOptions));
            AttrTranslator.Translate(typeof(BaseItemConfig));
            AttrTranslator.Translate(typeof(SubscriptionConfig));
            AttrTranslator.Translate(typeof(CommandConfig));
        }

        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevMqttClientView(this, lineConfig, deviceConfig);
        }
    }
}
