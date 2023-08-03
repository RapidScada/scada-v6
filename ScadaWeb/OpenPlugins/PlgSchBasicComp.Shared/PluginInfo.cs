// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgSchBasicComp
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
        public override string Code => "PlgSchBasicComp";

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public override string Name => Locale.IsRussian ?
            "Основные компоненты схем" :
            "Basic Scheme Components";

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public override string Descr => Locale.IsRussian ?
            "Набор основных компонентов для отображения на мнемосхемах." :
            "A set of basic components for display on schemes.";
    }
}
