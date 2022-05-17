// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// The class provides helper methods for the plugin.
    /// <para>Класс, предоставляющий вспомогательные методы для плагина.</para>
    /// </summary>
    internal static class PluginUtils
    {
        /// <summary>
        /// The plugin code.
        /// </summary>
        public const string PluginCode = "PlgMain";

        /// <summary>
        /// Gets the cache key for the plugin.
        /// </summary>
        public static string GetCacheKey(string typeName, object objectID)
        {
            return WebUtils.GetCacheKey(PluginCode, typeName, objectID);
        }

        /// <summary>
        /// Gets the cache key for the plugin.
        /// </summary>
        public static string GetCacheKey(string typeName, params object[] args)
        {
            return WebUtils.GetCacheKey(PluginCode, typeName, args);
        }
    }
}
