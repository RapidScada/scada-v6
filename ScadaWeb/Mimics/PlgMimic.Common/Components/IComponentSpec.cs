// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.Components
{
    /// <summary>
    /// Defines functionality of a component library specification.
    /// <para>Определяет функциональность спецификации библиотеки компонентов.</para>
    /// </summary>
    public interface IComponentSpec
    {
        /// <summary>
        /// Gets the groups of components.
        /// </summary>
        List<ComponentGroup> ComponentGroups { get; }

        /// <summary>
        /// Gets the groups of subtypes.
        /// </summary>
        List<SubtypeGroup> SubtypeGroups { get; }

        /// <summary>
        /// Gets the URLs of component stylesheets.
        /// </summary>
        List<string> StyleUrls { get; }

        /// <summary>
        /// Gets the URLs of component scripts.
        /// </summary>
        List<string> ScriptUrls { get; }
    }
}
