// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Controls;
using Scada.Admin.Project;
using Scada.Comm.Drivers;
using System;

namespace Scada.Admin.Extensions.ExtCommConfig.Code
{
    /// <summary>
    /// The class provides helper methods for the extension.
    /// <para>Класс, предоставляющий вспомогательные методы для расширения.</para>
    /// </summary>
    internal static class ExtensionUtils
    {
        /// <summary>
        /// Gets or sets the control that contains the context menus.
        /// </summary>
        public static CtrlContextMenu ContextMenuControl { get; set; } = null;

        /// <summary>
        /// Gets a new instance of the module user interface.
        /// </summary>
        public static bool GetDriverView(IAdminContext adminContext, CommApp commApp, string driverCode,
            out DriverView driverView, out string message)
        {
            if (adminContext == null)
                throw new ArgumentNullException(nameof(adminContext));
            if (commApp == null)
                throw new ArgumentNullException(nameof(commApp));

            if (DriverFactory.GetDriverView(adminContext.AppDirs.LibDir, driverCode, out driverView, out message))
            {
                driverView.BaseDataSet = adminContext.CurrentProject.ConfigBase;
                driverView.AppDirs = adminContext.AppDirs.CreateDirsForView(commApp.ConfigDir);
                driverView.AppConfig = commApp.AppConfig;
                driverView.LoadDictionaries();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
