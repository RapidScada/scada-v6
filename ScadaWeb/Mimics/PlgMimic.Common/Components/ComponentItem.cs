// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.Components
{
    /// <summary>
    /// Represents a component list item.
    /// <para>Представляет элемент списка компонентов.</para>
    /// </summary>
    public class ComponentItem : IComparable<ComponentItem>
    {
        /// <summary>
        /// Gets or sets the icon URL.
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        public string TypeName { get; set; }


        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        public int CompareTo(ComponentItem other)
        {
            return string.Compare(DisplayName, other?.DisplayName);
        }
    }
}
