// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtProjectTools.Controls
{
    /// <summary>
    /// Represents a control that contains a main menu and a toolbar.
    /// <para>Представляет элемент управления, содержащий главное меню и панель инструментов.</para>
    /// </summary>
    public partial class CtrlMainMenu : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlMainMenu()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets menu items to add to the main menu.
        /// </summary>
        public ToolStripMenuItem[] GetMainMenuItems()
        {
            return new ToolStripMenuItem[] { miProjectTools };
        }

        /// <summary>
        /// Gets tool buttons to add to the toolbar.
        /// </summary>
        public ToolStripButton[] GetToobarButtons()
        {
            return new ToolStripButton[] { btnAddLine };
        }


        private void miAddLine_Click(object sender, EventArgs e)
        {
            ScadaUiUtils.ShowInfo("Test");
        }
    }
}
