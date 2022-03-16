// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvCnlMqtt.View.Forms;

namespace Scada.Comm.Drivers.DrvCnlMqtt.View
{
    internal class MqttClientChannelView : ChannelView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttClientChannelView(DriverView parentView, ChannelConfig channelConfig)
            : base(parentView, channelConfig)
        {
            CanShowProperties = true;
        }

        /// <summary>
        /// Shows a modal dialog box for editing communication channel properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return new FrmMqttClientChannelOptions(ChannelConfig).ShowDialog() == DialogResult.OK;
        }
    }
}
