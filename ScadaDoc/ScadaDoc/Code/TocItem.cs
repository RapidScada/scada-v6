// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Doc.Code
{
    /// <summary>
    /// Represents a table of contents item.
    /// <para>Представляет элемент оглавления.</para>
    /// </summary>
    public class TocItem
    {
        /// <summary>
        /// Gets or sets the item text.
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// Gets or sets the article URL.
        /// </summary>
        public string Url { get; set; } = "";

        /// <summary>
        /// Gets or sets the subitems.
        /// </summary>
        public List<TocItem> Subitems { get; } = new();
    }
}
