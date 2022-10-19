// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgMain.View
{
    /// <summary>
    /// Implements the plugin user interface for the Administrator application..
    /// <para>Реализует пользовательский интерфейс плагина для приложения Администратор.</para>
    /// </summary>
    public class PlgMainView : PluginView
    {
        /// <summary>
        /// Gets the module name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ?
                    "Основной плагин" :
                    "Main Plugin";
            }
        }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Плагин реализует табличное представление, отправку команд, отображение и квитирование событий, " +
                    "отчёты по историческим данным и событиям." :
                    "The plugin implements a table view, sending commands, displaying and acknowledging events, " + 
                    "reports on historical data and events.";
            }
        }
    }
}
