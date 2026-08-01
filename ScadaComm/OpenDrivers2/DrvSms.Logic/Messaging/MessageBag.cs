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
        private readonly Dictionary<string, List<IMessageItem>> bag = [];


        public void Add(IMessageItem messageItem)
        {
            ArgumentNullException.ThrowIfNull(messageItem);
            string address = messageItem.Address ?? "";

            if (bag.TryGetValue(address, out List<IMessageItem> messages))
            {
                messages.Add(messageItem);
            }
            else
            {
                bag.Add(address, [messageItem]);
            }
        }

        public IEnumerable<IMessageItem> GetByAddress(string address)
        {
            return !string.IsNullOrEmpty(address) && bag.TryGetValue(address, out List<IMessageItem> messages)
                ? messages
                : [];
        }

        public IEnumerable<IMessageItem> GetUnprocessed()
        {
            return bag.Values
                .SelectMany(list => list)
                .Where(msg => msg != null && !msg.IsProcessed);
        }

        public void Clear()
        {
            bag.Clear();
        }
    }
}
