// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgMimBasicComp
{
    /// <summary>
    /// Represents information about a plugin.
    /// <para>Представляет информацию о плагине.</para>
    /// </summary>
    internal class PluginInfo : LibraryInfo
    {
        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public override string Code => "PlgMimBasicComp";

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public override string Name => Locale.IsRussian ?
            "Основные компоненты мнемосхем" :
            "Basic Mimic Components";

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public override string Descr => Locale.IsRussian ?
            "Библиотека основных компонентов мнемосхем." :
            "Library of basic components for mimic diagrams.";
    }
}
