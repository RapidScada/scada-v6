// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;

namespace Scada.Server.Modules.ModDbExport.Logic
{
    /// <summary>
    /// Represents a data queue item that contains a slice.
    /// <para>Представляет элемент очереди данных, который содержит срез.</para>
    /// </summary>
    internal class SliceItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SliceItem(Slice slice)
        {
            Slice = slice ?? throw new ArgumentNullException(nameof(slice));
            QueryID = 0;
            SingleQuery = null;
        }


        /// <summary>
        /// Gets the slice.
        /// </summary>
        public Slice Slice { get; }

        /// <summary>
        /// Gets the ID of the query that exports the slice.
        /// </summary>
        public int QueryID { get; init; }

        /// <summary>
        /// Gets a value indicating whether the item is intended for the specified type of query.
        /// </summary>
        public bool? SingleQuery { get; init; }
    }
}
