﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtProjectTools.Code;
using Scada.Admin.Extensions.ExtProjectTools.Forms;
using Scada.Data.Entities;

namespace Scada.Admin.Extensions.ExtProjectTools.Controls
{
    /// <summary>
    /// Represents a control that contains a main menu and a toolbar.
    /// <para>Представляет элемент управления, содержащий главное меню и панель инструментов.</para>
    /// </summary>
    public partial class CtrlMainMenu : UserControl
    {
        private readonly IAdminContext adminContext; // the Administrator context


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private CtrlMainMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlMainMenu(IAdminContext adminContext)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            SetMenuItemsEnabled();
            adminContext.CurrentProjectChanged += AdminContext_CurrentProjectChanged;
            adminContext.MessageToExtension += AdminContext_MessageToExtension;
        }


        /// <summary>
        /// Enables or disables main menu items and toolbar buttons.
        /// </summary>
        private void SetMenuItemsEnabled()
        {
            bool projectIsOpen = adminContext.CurrentProject != null;
            miCloneChannels.Enabled = projectIsOpen;
            miChannelMapByDevice.Enabled = projectIsOpen;
            miChannelMapByObject.Enabled = projectIsOpen;
            miDeviceMap.Enabled = projectIsOpen;
            miObjectMap.Enabled = projectIsOpen;
            miCheckIntegrity.Enabled = projectIsOpen;
            miEncryptPassword.Enabled = true;
            miImportTable.Enabled = projectIsOpen;
            miExportTable.Enabled = projectIsOpen;
        }

        /// <summary>
        /// Generates a channel map.
        /// </summary>
        private void GenerateChannelMap(bool groupByDevices)
        {
            if (adminContext.CurrentProject != null)
            {
                new ChannelMap(adminContext.ErrLog, adminContext.CurrentProject.ConfigDatabase)
                {
                    GroupByDevices = groupByDevices
                }
                .Generate(Path.Combine(adminContext.AppDirs.LogDir, ChannelMap.MapFileName));
            }
        }

        /// <summary>
        /// Generates a device map.
        /// </summary>
        private void GenerateDeviceMap()
        {
            if (adminContext.CurrentProject != null)
            {
                new DeviceMap(adminContext.ErrLog, adminContext.CurrentProject.ConfigDatabase)
                    .Generate(Path.Combine(adminContext.AppDirs.LogDir, DeviceMap.MapFileName));
            }
        }

        /// <summary>
        /// Generates an object map.
        /// </summary>
        private void GenerateObjectMap()
        {
            if (adminContext.CurrentProject != null)
            {
                new ObjectMap(adminContext.ErrLog, adminContext.CurrentProject.ConfigDatabase)
                    .Generate(Path.Combine(adminContext.AppDirs.LogDir, ObjectMap.MapFileName));
            }
        }

        /// <summary>
        /// Gets menu items to add to the main menu.
        /// </summary>
        public ToolStripItem[] GetMainMenuItems()
        {
            return new ToolStripItem[] { miProjectTools };
        }


        private void AdminContext_CurrentProjectChanged(object sender, EventArgs e)
        {
            SetMenuItemsEnabled();
        }

        private void AdminContext_MessageToExtension(object sender, MessageEventArgs e)
        {
            if (e.Message == KnownExtensionMessage.GenerateChannelMap)
                GenerateChannelMap((bool)e.Arguments["GroupByDevices"]);
            else if (e.Message == KnownExtensionMessage.GenerateDeviceMap)
                GenerateDeviceMap();
        }

        private void miCloneChannels_Click(object sender, EventArgs e)
        {
            // clone channels
            if (adminContext.CurrentProject != null)
            {
                FrmCnlClone frmCnlClone = new(adminContext, adminContext.CurrentProject.ConfigDatabase);
                frmCnlClone.ShowDialog();

                if (frmCnlClone.ChannelsCloned)
                    adminContext.MainForm.RefreshBaseTables(typeof(Cnl), true);
            }
        }

        private void miChannelMap_Click(object sender, EventArgs e)
        {
            GenerateChannelMap(sender == miChannelMapByDevice);
        }

        private void miDeviceMap_Click(object sender, EventArgs e)
        {
            GenerateDeviceMap();
        }

        private void miObjectMap_Click(object sender, EventArgs e)
        {
            GenerateObjectMap();
        }

        private void miCheckIntegrity_Click(object sender, EventArgs e)
        {
            // check integrity
            if (adminContext.CurrentProject != null)
            {
                new IntegrityCheck(adminContext.ErrLog, adminContext.CurrentProject.ConfigDatabase)
                    .Execute(Path.Combine(adminContext.AppDirs.LogDir, IntegrityCheck.OutputFileName));
            }
        }

        private void miEncryptPassword_Click(object sender, EventArgs e)
        {
            new FrmPasswordEncrypt().ShowDialog();
        }

        private void miImportTable_Click(object sender, EventArgs e)
        {
            // import table
            if (adminContext.CurrentProject != null)
            {
                FrmTableImport frmTableImport = new(adminContext.ErrLog, adminContext.CurrentProject.ConfigDatabase)
                {
                    SelectedItemType = adminContext.MainForm.ActiveBaseTable
                };

                if (frmTableImport.ShowDialog() == DialogResult.OK)
                    adminContext.MainForm.RefreshBaseTables(frmTableImport.SelectedItemType, true);
            }
        }

        private void miExportTable_Click(object sender, EventArgs e)
        {
            // export table
            if (adminContext.CurrentProject != null)
            {
                new FrmTableExport(adminContext.ErrLog, adminContext.CurrentProject.ConfigDatabase)
                {
                    SelectedItemType = adminContext.MainForm.ActiveBaseTable
                }
                .ShowDialog();
            }
        }
    }
}
