// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Web.Plugins;

namespace Scada.Admin.Extensions.ExtWebConfig.Code
{
    /// <summary>
    /// The class provides helper methods for the extension.
    /// <para>Класс, предоставляющий вспомогательные методы для расширения.</para>
    /// </summary>
    internal static class ExtensionUtils
    {
        /// <summary>
        /// Gets a new instance of the plugin user interface.
        /// </summary>
        public static bool GetPluginView(IAdminContext adminContext, WebApp webApp, string pluginCode,
            out PluginView pluginView, out string message)
        {
            ArgumentNullException.ThrowIfNull(adminContext, nameof(adminContext));
            ArgumentNullException.ThrowIfNull(webApp, nameof(webApp));

            if (PluginViewFactory.GetPluginView(adminContext.AppDirs.LibDir, pluginCode, out pluginView, out message))
            {
                pluginView.ConfigDataset = adminContext.CurrentProject.ConfigDatabase;
                pluginView.AppDirs = adminContext.AppDirs.CreateDirsForView(webApp.ConfigDir);
                pluginView.AppConfig = webApp.AppConfig;
                pluginView.LoadDictionaries();
                return true;
            }
            else
            {
                return false;
            };
        }
    }
}
