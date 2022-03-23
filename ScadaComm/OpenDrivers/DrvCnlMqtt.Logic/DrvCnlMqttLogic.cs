// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;

namespace Scada.Comm.Drivers.DrvCnlMqtt.Logic
{
    /// <summary>
    /// Implements the driver logic.
    /// <para>Реализует логику драйвера.</para>
    /// </summary>
    public class DrvCnlMqttLogic : DriverLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvCnlMqttLogic(ICommContext commContext)
            : base(commContext)
        {
        }

        /// <summary>
        /// Gets the driver code.
        /// </summary>
        public override string Code
        {
            get
            {
                return "DrvCnlMqtt";
            }
        }

        /// <summary>
        /// Creates a new communication channel.
        /// </summary>
        public override ChannelLogic CreateChannel(ILineContext lineContext, ChannelConfig channelConfig)
        {
            return channelConfig.TypeCode == "MqttClient"
                ? new MqttClientChannelLogic(lineContext, channelConfig)
                : null;
        }
    }
}
