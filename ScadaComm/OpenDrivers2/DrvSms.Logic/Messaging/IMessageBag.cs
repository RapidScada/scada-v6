// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvSms.Logic.Messaging
{
    /// <summary>
    /// Provides access to message items grouped by recipient address.
    /// <para>Предоставляет доступ к сообщениям, сгруппированным по адресу получателя.</para>
    /// </summary>
    public interface IMessageBag
    {
        /// <summary>
        /// Gets the message items having the specified recipient address.
        /// </summary>
        IEnumerable<IMessageItem> GetMessageItems(string address);
    }
}
