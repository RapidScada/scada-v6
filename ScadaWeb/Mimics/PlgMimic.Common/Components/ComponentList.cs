// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.Components
{
    /// <summary>
    /// Represents a list of components available for adding to a mimic.
    /// <para>Представляет список компонентов, доступных для добавления на мнемосхему.</para>
    /// </summary>
    public class ComponentList
    {
        /// <summary>
        /// Gets the groups of components.
        /// </summary>
        public List<ComponentGroup> Groups { get; } = [];
    }
}
