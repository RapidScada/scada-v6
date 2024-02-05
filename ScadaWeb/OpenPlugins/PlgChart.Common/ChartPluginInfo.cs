// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgChart
{
    /// <summary>
    /// Represents information about a plugin.
    /// <para>Представляет информацию о плагине.</para>
    /// </summary>
    public class ChartPluginInfo : LibraryInfo
    {
        /// <summary>
        /// The plugin code.
        /// </summary>
        public const string PluginCode = "PlgChart";
        
        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public override string Code => PluginCode;

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public override string Name => Locale.IsRussian ?
            "Графики" :
            "Chart";

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public override string Descr => Locale.IsRussian ?
            "Плагин обеспечивает отображение графиков." :
            "The plugin provides displaying charts.";
    }
}
