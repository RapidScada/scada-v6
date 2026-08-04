// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvSms.Logic.Protocol
{
    /// <summary>
    /// Represents a message.
    /// <para>Представляет сообщение.</para>
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the index of the message in the device's message list.
        /// </summary>
        public int Index { get; set; } = 0;

        /// <summary>
        /// Gets or sets the message status provided by the device.
        /// </summary>
        public int Status { get; set; } = MessageStatus.Unread;

        /// <summary>
        /// Gets or sets the PDU length. SMS center (SMSC) number is not included.
        /// </summary>
        public int Length { get; set; } = 0;

        /// <summary>
        /// Gets or sets the sender phone number.
        /// </summary>
        public string Phone { get; set; } = "";

        /// <summary>
        /// Gets or sets the timestamp provided by an SMSC.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        public string Text { get; set; } = "";
    }
}
