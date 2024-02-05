// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgWebPage
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
        public override string Code => "PlgWebPage";

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public override string Name => Locale.IsRussian ?
            "Веб-страницы" :
            "Web Pages";

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public override string Descr => Locale.IsRussian ?
            "Плагин обеспечивает отображение произвольных веб-страниц." :
            "The plugin provides displaying arbitrary web pages.";
    }
}
