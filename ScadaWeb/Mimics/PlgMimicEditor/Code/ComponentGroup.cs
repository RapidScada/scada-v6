// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Represents a component list group.
    /// <para>Представляет группу списка компонентов.</para>
    /// </summary>
    public class ComponentGroup
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; init; } = "";

        /// <summary>
        /// Gets the items.
        /// </summary>
        public List<ComponentItem> Items { get; } = [];
    }
}
