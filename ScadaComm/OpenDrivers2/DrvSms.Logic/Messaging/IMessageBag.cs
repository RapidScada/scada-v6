// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvSms.Logic.Messaging
{
    /// <summary>
    /// Defines methods for retrieving message items by recipient address.
    /// <para>Определяет методы для получения сообщений по адресу получателя.</para>
    /// </summary>
    public interface IMessageBag
    {
        /// <summary>
        /// Gets the message items having the specified recipient address.
        /// </summary>
        IEnumerable<IMessageItem> GetByAddress(string address);
    }
}
