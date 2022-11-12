// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Doc.Code
{
    /// <summary>
    /// Represents a menu item.
    /// <para>Представляет элемент меню.</para>
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets the item text.
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// Gets or sets the item URL.
        /// </summary>
        public string Url { get; set; } = "";

        /// <summary>
        /// Gets the subitems.
        /// </summary>
        public List<MenuItem> Subitems { get; } = new();
    }
}
