// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvMqtt
{
    /// <summary>
    /// Defines functionality to handle messages from an MQTT broker.
    /// <para>Определяет функциональность для обработки сообщений от MQTT-брокера.</para>
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// Gets the device number.
        /// </summary>
        public int DeviceNum { get; }

        /// <summary>
        /// Handles the received message.
        /// </summary>
        void HandleReceivedMessage(ReceivedMessage message);
    }
}
