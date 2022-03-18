// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client.Publishing;

namespace Scada.Comm.Drivers.DrvMqtt
{
    /// <summary>
    /// Defines functionality for using an MQTT client.
    /// <para>Определяет функциональность для использования MQTT-клиента.</para>
    /// </summary>
    public interface IMqttClientChannel
    {
        /// <summary>
        /// Gets a value indicating whether the MQTT client is connected to an MQTT broker.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Subscribes to the topic.
        /// </summary>
        void Subscribe(SubscriptionRecord subscriptionRecord);

        /// <summary>
        /// Publishes the message to the MQTT broker.
        /// </summary>
        MqttClientPublishResult Publish(MqttApplicationMessage message);
    }
}
