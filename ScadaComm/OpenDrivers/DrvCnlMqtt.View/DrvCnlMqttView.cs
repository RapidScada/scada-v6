// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvCnlMqtt.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvCnlMqttView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvCnlMqttView()
            : base()
        {
            CanCreateChannel = true;
        }


        /// <summary>
        /// Gets the driver name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Канал связи MQTT" : "MQTT Communication Channel";
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
                    "Предоставляет канал связи MQTT-клиент." :
                    "Provides MQTT client communication channel.";
            }
        }

        /// <summary>
        /// Gets the communication channel types provided by the driver.
        /// </summary>
        public override ICollection<ChannelTypeName> ChannelTypes
        {
            get
            {
                return new ChannelTypeName[] { new ChannelTypeName("MqttClient", Locale.IsRussian ?
                    "MQTT-клиент" : 
                    "MQTT client") };
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "DrvCnlMqtt", out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Creates a new communication channel user interface.
        /// </summary>
        public override ChannelView CreateChannelView(ChannelConfig channelConfig)
        {
            return new MqttClientChannelView(this, channelConfig);
        }
    }
}
