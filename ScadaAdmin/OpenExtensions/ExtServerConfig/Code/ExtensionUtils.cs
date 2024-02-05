﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Server.Modules;

namespace Scada.Admin.Extensions.ExtServerConfig.Code
{
    /// <summary>
    /// The class provides helper methods for the extension.
    /// <para>Класс, предоставляющий вспомогательные методы для расширения.</para>
    /// </summary>
    internal static class ExtensionUtils
    {
        /// <summary>
        /// Gets a new instance of the module user interface.
        /// </summary>
        public static bool GetModuleView(IAdminContext adminContext, ServerApp serverApp, string moduleCode,
            out ModuleView moduleView, out string message)
        {
            ArgumentNullException.ThrowIfNull(adminContext, nameof(adminContext));
            ArgumentNullException.ThrowIfNull(serverApp, nameof(serverApp));

            if (ModuleFactory.GetModuleView(adminContext.AppDirs.LibDir, moduleCode, out moduleView, out message))
            {
                moduleView.ConfigDataset = adminContext.CurrentProject.ConfigDatabase;
                moduleView.AppDirs = adminContext.AppDirs.CreateDirsForView(serverApp.ConfigDir);
                moduleView.AgentClient = adminContext.MainForm.GetAgentClient(false);
                moduleView.AppConfig = serverApp.AppConfig;
                moduleView.LoadDictionaries();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
