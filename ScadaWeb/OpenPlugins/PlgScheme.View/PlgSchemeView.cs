// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgScheme.View
{
    /// <summary>
    /// Implements the plugin user interface for the Administrator application.
    /// <para>Реализует пользовательский интерфейс плагина для приложения Администратор.</para>
    /// </summary>
    public class PlgSchemeView : PluginView
    {
        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ?
                    "Схемы" :
                    "Schemes";
            }
        }

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Плагин обеспечивает отображение мнемосхем." :
                    "The plugin provides displaying schemes.";
            }
        }
    }
}
