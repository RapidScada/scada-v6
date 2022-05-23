// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Controls;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers;
using Scada.Data.Models;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WinControl;

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
        /// Refreshes the corresponding line configuration form.
        /// </summary>
        private static void RefreshLineConfigForm(TreeNode deviceTreeNode)
        {
            if (deviceTreeNode?.FindSibling(CommNodeType.LineOptions) is TreeNode lineOptionsNode &&
                lineOptionsNode.Tag is TreeNodeTag tag && tag.ExistingForm is IChildForm childForm)
            {
                childForm.ChildFormTag.SendMessage(null, AdminMessage.RefreshData);
            }
        }

        /// <summary>
        /// Resets the driver view cache.
        /// </summary>
        public static void ResetDriverViewCache()
        {
            driverViewCache.Clear();
        }

        /// <summary>
        /// Gets a new instance of the module user interface.
        /// </summary>
        public static bool GetDriverView(IAdminContext adminContext, CommApp commApp, string driverCode,
            out DriverView driverView, out string message)
        {
            ArgumentNullException.ThrowIfNull(adminContext, nameof(adminContext));
            ArgumentNullException.ThrowIfNull(commApp, nameof(commApp));

            if (driverViewCache.TryGetValue(driverCode, out driverView))
            {
                driverView.AgentClient = adminContext.MainForm.GetAgentClient(false);
                message = "Driver view has been retrieved from the cache.";
                return true;
            }
            else if (DriverFactory.GetDriverView(adminContext.AppDirs.LibDir, driverCode, out driverView, out message))
            {
                driverView.ConfigDataset = adminContext.CurrentProject.ConfigDatabase;
                driverView.AppDirs = adminContext.AppDirs.CreateDirsForView(commApp.ConfigDir);
                driverView.AgentClient = adminContext.MainForm.GetAgentClient(false);
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
        public static bool GetDeviceView(IAdminContext adminContext, CommApp commApp, DeviceConfig deviceConfig,
            out DeviceView deviceView, out string errMsg)
        {
            ArgumentNullException.ThrowIfNull(adminContext, nameof(adminContext));
            ArgumentNullException.ThrowIfNull(commApp, nameof(commApp));
            ArgumentNullException.ThrowIfNull(deviceConfig, nameof(deviceConfig));
            deviceView = null;

            if (string.IsNullOrEmpty(deviceConfig.Driver))
            {
                errMsg = ExtensionPhrases.DriverNotSpecified;
                return false;
            }
            else if (!GetDriverView(adminContext, commApp, deviceConfig.Driver, 
                out DriverView driverView, out errMsg))
            {
                return false;
            }
            else if (!driverView.CanCreateDevice)
            {
                errMsg = ExtensionPhrases.DeviceNotSupported;
                return false;
            }
            else if (driverView.CreateDeviceView(deviceConfig.ParentLine, deviceConfig) is DeviceView newDeviceView)
            {
                deviceView = newDeviceView;
                errMsg = "";
                return true;
            }
            else
            {
                errMsg = ExtensionPhrases.UnableCreateDeviceView;
                return false;
            }
        }

        /// <summary>
        /// Gets a new instance of the device user interface.
        /// </summary>
        public static DeviceView GetDeviceView(IAdminContext adminContext, CommApp commApp, DeviceConfig deviceConfig)
        {
            if (GetDeviceView(adminContext, commApp, deviceConfig, out DeviceView deviceView, out string message))
            {
                return deviceView;
            }
            else
            {
                ScadaUiUtils.ShowError(message);
                return null;
            }
        }

        /// <summary>
        /// Shows a device properties form.
        /// </summary>
        public static void ShowDeviceProperties(IAdminContext adminContext, CommApp commApp, DeviceConfig deviceConfig,
            TreeNode deviceTreeNode)
        {
            if (GetDeviceView(adminContext, commApp, deviceConfig) is DeviceView deviceView)
            {
                if (deviceView.CanShowProperties)
                {
                    if (deviceView.ShowProperties() ||
                        deviceView.DeviceConfigModified ||
                        deviceView.LineConfigModified)
                    {
                        RefreshLineConfigForm(deviceTreeNode);

                        if (!commApp.SaveConfig(out string errMsg))
                            adminContext.ErrLog.HandleError(errMsg);
                    }
                }
                else
                {
                    ScadaUiUtils.ShowInfo(ExtensionPhrases.NoDeviceProperties);
                }
            }
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public static bool SendCommand(IAdminContext adminContext, IAgentClient agentClient, TeleCommand cmd)
        {
            ArgumentNullException.ThrowIfNull(adminContext, nameof(adminContext));
            ArgumentNullException.ThrowIfNull(agentClient, nameof(agentClient));
            ArgumentNullException.ThrowIfNull(cmd, nameof(cmd));

            try
            {
                adminContext.MainForm.Cursor = Cursors.WaitCursor;

                lock (agentClient.SyncRoot)
                {
                    agentClient.SendCommand(ServiceApp.Comm, cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                adminContext.ErrLog.HandleError(ex, CommonPhrases.SendCommandError);
                return false;
            }
            finally
            {
                adminContext.MainForm.Cursor = Cursors.Default;
            }
        }
    }
}
