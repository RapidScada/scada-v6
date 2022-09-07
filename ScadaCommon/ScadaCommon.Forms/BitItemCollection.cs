// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.Collections.Generic;

namespace Scada.Forms
{
    /// <summary>
    /// Represents a collection of bitmask items.
    /// <para>Представляет коллекцию элементов битовой маски.</para>
    /// </summary>
    public class BitItemCollection : List<BitItem>
    {
        /// <summary>
        /// Creates a collection of archive bits.
        /// </summary>
        public static BitItemCollection Create(BaseTable<Archive> archiveTable)
        {
            ArgumentNullException.ThrowIfNull(archiveTable, nameof(archiveTable));
            BitItemCollection bitItems = new();

            foreach (Archive archive in archiveTable)
            {
                bitItems.Add(new BitItem(archive.Bit, archive.Name));
            }

            return bitItems;
        }

        /// <summary>
        /// Creates a collection of event bits.
        /// </summary>
        public static BitItemCollection CreateFromEvents()
        {
            return new BitItemCollection
            {
                new BitItem(EventBit.Enabled, CommonPhrases.EventEnabled),
                new BitItem(EventBit.Beep, CommonPhrases.EventBeep),
                new BitItem(EventBit.DataChange, CommonPhrases.DataChangeEvent),
                new BitItem(EventBit.ValueChange, CommonPhrases.ValueChangeEvent),
                new BitItem(EventBit.StatusChange, CommonPhrases.StatusChangeEvent),
                new BitItem(EventBit.CnlUndefined, CommonPhrases.CnlUndefinedEvent),
                new BitItem(EventBit.Command, CommonPhrases.CommandEvent)
            };
        }
    }
}
