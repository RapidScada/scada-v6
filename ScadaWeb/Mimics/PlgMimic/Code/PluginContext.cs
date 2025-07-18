// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.Config;

namespace Scada.Web.Plugins.PlgMimic.Code
{
    /// <summary>
    /// Contains plugin data.
    /// <para>Содержит данные плагина.</para>
    /// </summary>
    public class PluginContext
    {
        /// <summary>
        /// Gets the plugin configuration.
        /// </summary>
        public MimicPluginConfig PluginConfig { get; } = new();
    }
}
