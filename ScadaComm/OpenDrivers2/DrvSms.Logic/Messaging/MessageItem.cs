// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvSms.Logic.Protocol;

namespace Scada.Comm.Drivers.DrvSms.Logic.Messaging
{
    /// <summary>
    /// Represents a message to be processed by a driver.
    /// <para>Представляет сообщение для обработки драйвером.</para>
    /// </summary>
    internal class MessageItem : IMessageItem
    {
        private readonly Message message;


        public MessageItem(Message message)
        {
            this.message = message ?? throw new ArgumentNullException(nameof(message));
            IsProcessed = false;
        }


        public DateTime Timestamp => message.Timestamp;

        public string Address => message.Phone;

        public string Text => message.Text;

        public bool IsProcessed { get; set; }
    }
}
