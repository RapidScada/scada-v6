// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Controls;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers;
using Scada.Forms;
using System;
using System.Collections.Generic;

namespace Scada.Admin.Extensions.ExtCommConfig.Code
{
    /// <summary>
    /// The class provides helper methods for the extension.
    /// <para>Класс, предоставляющий вспомогательные методы для расширения.</para>
    /// </summary>
    internal static class ExtensionUtils
    {
        /// <summary>
        /// Gets the maximum length of entity text fields.
        /// </summary>
        public const int DefaultLength = 100;
        /// <summary>
        /// Gets the maximum length of entity names.
        /// </summary>
        public const int NameLength = 100;
        /// <summary>
        /// Gets the maximum length of entity descriptions.
        /// </summary>
        public const int DescrLength = 1000;


        /// <summary>
        /// The driver view cache.
        /// </summary>
        private static readonly Dictionary<string, DriverView> driverViewCache = new();


        /// <summary>
        /// Gets or sets the control that contains the menus.
        /// </summary>
        public static CtrlExtensionMenu MenuControl { get; set; } = null;

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

            if (driverViewCache.TryGetValue(driverCode, out driverView))
            {
                message = "Driver view has been retrieved from the cache.";
                return true;
            }
            else if (DriverFactory.GetDriverView(adminContext.AppDirs.LibDir, driverCode, out driverView, out message))
            {
                driverView.BaseDataSet = adminContext.CurrentProject.ConfigBase;
                driverView.AppDirs = adminContext.AppDirs.CreateDirsForView(commApp.ConfigDir);
                driverView.AppConfig = commApp.AppConfig;
                driverView.LoadDictionaries();
                driverViewCache.Add(driverCode, driverView);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a new instance of the device user interface.
        /// </summary>
        public static DeviceView GetDeviceView(IAdminContext adminContext, CommApp commApp, 
            LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            if (adminContext == null)
                throw new ArgumentNullException(nameof(adminContext));
            if (commApp == null)
                throw new ArgumentNullException(nameof(commApp));
            if (lineConfig == null)
                throw new ArgumentNullException(nameof(lineConfig));
            if (deviceConfig == null)
                throw new ArgumentNullException(nameof(deviceConfig));

            if (string.IsNullOrEmpty(deviceConfig.Driver))
            {
                ScadaUiUtils.ShowError(ExtensionPhrases.DriverNotSpecified);
            }
            else if (!ExtensionUtils.GetDriverView(adminContext, commApp, deviceConfig.Driver,
                out DriverView driverView, out string message))
            {
                ScadaUiUtils.ShowError(message);
            }
            else if (!driverView.CanCreateDevice)
            {
                ScadaUiUtils.ShowError(ExtensionPhrases.DeviceNotSupported);
            }
            else if (driverView.CreateDeviceView(lineConfig, deviceConfig) is not DeviceView deviceView)
            {
                ScadaUiUtils.ShowError(ExtensionPhrases.UnableCreateDeviceView);
            }
            else
            {
                return deviceView;
            }

            return null;
        }
    }
}
