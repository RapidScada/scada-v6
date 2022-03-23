// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvMqtt
{
    /// <summary>
    /// Represents a message received from an MQTT broker.
    /// <para>Представляет сообщение, полученное от MQTT-брокера.</para>
    /// </summary>
    public class ReceivedMessage
    {
        /// <summary>
        /// Gets the topic.
        /// </summary>
        public string Topic { get; init; }

        /// <summary>
        /// Gets the message payload as a string.
        /// </summary>
        public string Payload { get; init; }

        /// <summary>
        /// Gets the message payload as an array of bytes.
        /// </summary>
        public byte[] PayloadData { get; init; }

        /// <summary>
        /// Gets or sets the auxiliary data associated with the topic, originally provided by the subscriber.
        /// </summary>
        public object AuxData { get; set; }
    }
}
