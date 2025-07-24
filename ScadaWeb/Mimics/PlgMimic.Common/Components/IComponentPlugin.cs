// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.Components
{
    /// <summary>
    /// Indicates that a plugin provides mimic components.
    /// <para>Показывает, что плагин предоставляет компоненты мнемосхем.</para>
    /// </summary>
    public interface IComponentPlugin
    {
        /// <summary>
        /// Gets the component library specification.
        /// </summary>
        public IComponentLibrary ComponentLibrary { get; }
    }
}
