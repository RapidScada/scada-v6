// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvSms.Logic.Messaging
{
    /// <summary>
    /// Defines the properties of a message to be processed by a driver.
    /// <para>Определяет свойства сообщения, предназначенного для обработки драйвером.</para>
    /// </summary>
    public interface IMessageItem
    {
        /// <summary>
        /// Gets the message timestamp.
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the recipient's address or phone number.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the message has been processed by a driver.
        /// </summary>
        bool IsProcessed { get; set; }
    }
}
