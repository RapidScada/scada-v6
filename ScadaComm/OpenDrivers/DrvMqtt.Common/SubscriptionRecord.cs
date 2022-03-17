// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;

namespace Scada.Comm.Drivers.DrvMqtt
{
    /// <summary>
    /// Represents a data record associated with a subscription.
    /// <para>Представляет запись данных, связанных с подпиской.</para>
    /// </summary>
    public class SubscriptionRecord
    {
        /// <summary>
        /// Gets the subscriber.
        /// </summary>
        public ISubscriber Subscriber { get; init; }

        /// <summary>
        /// Gets the topic filter.
        /// </summary>
        public MqttTopicFilter TopicFilter { get; init; }

        /// <summary>
        /// Gets the auxiliary data.
        /// </summary>
        public object AuxData { get; init; }
    }
}
