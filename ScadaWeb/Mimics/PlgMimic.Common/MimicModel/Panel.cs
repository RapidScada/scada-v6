// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a panel that can contain child components.
    /// <para>Представляет панель, которая может содержать дочерние компоненты.</para>
    /// </summary>
    public class Panel : Component
    {
        /// <summary>
        /// Gets the components contained within the panel.
        /// </summary>
        public List<Component> Components { get; } = [];
    }
}
