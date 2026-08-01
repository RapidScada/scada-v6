// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvSms.Logic.Messaging
{
    /// <summary>
    /// Stores message items grouped by recipient address.
    /// <para>Хранит сообщения, сгруппированные по адресу получателя.</para>
    /// </summary>
    internal class MessageBag : IMessageBag
    {
        private readonly Dictionary<string, List<IMessageItem>> messageBag = [];
        private readonly List<IMessageItem> allMessages = [];


        public void Add(IMessageItem messageItem)
        {
            ArgumentNullException.ThrowIfNull(messageItem);
            allMessages.Add(messageItem);
            string address = messageItem.Address ?? "";

            if (messageBag.TryGetValue(address, out List<IMessageItem> messages))
            {
                messages.Add(messageItem);
            }
            else
            {
                messageBag.Add(address, [messageItem]);
            }
        }

        public IEnumerable<IMessageItem> GetByAddress(string address)
        {
            return !string.IsNullOrEmpty(address) && messageBag.TryGetValue(address, out List<IMessageItem> messages)
                ? messages
                : [];
        }

        public IEnumerable<IMessageItem> GetUnprocessed()
        {
            return allMessages.Where(mi => !mi.IsProcessed);
        }

        public void Clear()
        {
            messageBag.Clear();
            allMessages.Clear();
        }
    }
}
