// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.Components;

namespace Scada.Web.Plugins.PlgMimBasicComp.Code
{
    /// <summary>
    /// Represents a specification of the basic component library.
    /// <para>Представляет спецификацию библиотеки базовых компонентов.</para>
    /// </summary>
    public class BasicComponentLibrary : IComponentLibrary
    {
        /// <summary>
        /// Gets the groups of components.
        /// </summary>
        public List<ComponentGroup> ComponentGroups => [new BasicComponentGroup()];

        /// <summary>
        /// Gets the groups of subtypes.
        /// </summary>
        public List<SubtypeGroup> SubtypeGroups => [new BasicSubtypeGroup()];

        /// <summary>
        /// Gets the URLs of component stylesheets.
        /// </summary>
        public List<string> StyleUrls => [
            "~/plugins/MimBasicComp/css/basiccomp.min.css"
        ];

        /// <summary>
        /// Gets the URLs of component scripts.
        /// </summary>
        public List<string> ScriptUrls => [
            "~/plugins/MimBasicComp/js/basiccomp-descr.js",
            "~/plugins/MimBasicComp/js/basiccomp-factory.js",
            "~/plugins/MimBasicComp/js/basiccomp-render.js"
        ];
    }
}
