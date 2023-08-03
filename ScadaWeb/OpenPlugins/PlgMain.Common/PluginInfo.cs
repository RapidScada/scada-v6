// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgMain
{
    /// <summary>
    /// Represents information about a plugin.
    /// <para>Представляет информацию о плагине.</para>
    /// </summary>
    public class PluginInfo : LibraryInfo
    {
        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public override string Code => "PlgMain";

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public override string Name => Locale.IsRussian ?
            "Основной плагин" :
            "Main Plugin";

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public override string Descr => Locale.IsRussian ?
            "Плагин реализует табличное представление, отправку команд, отображение и квитирование событий, " +
            "отчёты по историческим данным и событиям." :
            "The plugin implements a table view, sending commands, displaying and acknowledging events, " +
            "reports on historical data and events.";
    }
}
