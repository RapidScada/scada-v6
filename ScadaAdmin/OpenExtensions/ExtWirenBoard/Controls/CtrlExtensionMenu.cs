// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code;
using Scada.Admin.Extensions.ExtWirenBoard.Forms;
using Scada.Data.Entities;

namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
{
    /// <summary>
    /// Represents a control that provides extension menus.
    /// <para>Представляет элемент управления, предоставляющий меню расширения.</para>
    /// </summary>
    internal partial class CtrlExtensionMenu : UserControl
    {
        private readonly IAdminContext adminContext;      // the Administrator context
        private readonly RecentSelection recentSelection; // the recently selected parameters


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private CtrlExtensionMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlExtensionMenu(IAdminContext adminContext)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            recentSelection = new RecentSelection();

            SetMenuItemsEnabled();
            adminContext.CurrentProjectChanged += AdminContext_CurrentProjectChanged;
        }


        /// <summary>
        /// Enables or disables main menu items and toolbar buttons.
        /// </summary>
        private void SetMenuItemsEnabled()
        {
            bool projectIsOpen = adminContext.CurrentProject != null;
            miCreateConfig.Enabled = btnCreateConfig.Enabled = projectIsOpen;
        }


        /// <summary>
        /// Gets menu items to add to the main menu.
        /// </summary>
        public ToolStripItem[] GetMainMenuItems()
        {
            return new ToolStripItem[] { miWirenBoard };
        }

        /// <summary>
        /// Gets tool buttons to add to the toolbar.
        /// </summary>
        public ToolStripItem[] GetToobarButtons()
        {
            return new ToolStripItem[] { btnCreateConfig };
        }


        private void AdminContext_CurrentProjectChanged(object sender, EventArgs e)
        {
            SetMenuItemsEnabled();
            recentSelection.Reset();
        }

        private void miCreateConfig_Click(object sender, EventArgs e)
        {
            if (adminContext.CurrentProject != null)
            {
                FrmWirenBoardWizard frmWirenBoardWizard = new(adminContext, adminContext.CurrentProject, recentSelection);

                if (frmWirenBoardWizard.ShowDialog() == DialogResult.OK)
                {
                    adminContext.MainForm.RefreshBaseTables(typeof(Device));
                    adminContext.MainForm.RefreshBaseTables(typeof(Cnl));
                    // TODO: update explorer and line config form
                }
            }
        }
    }
}
