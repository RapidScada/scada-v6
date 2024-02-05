﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvDbImport.Config;
using Scada.Forms;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvDbImport.View.Controls
{
    /// <summary>
    /// Represetns a control for editing command options.
    /// <para>Представляет элемент управления для редактирования параметров команды.</para>
    /// </summary>
    public partial class CtrlCommand : UserControl
    {
        private CommandConfig commandConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCommand()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the edited command.
        /// </summary>
        internal CommandConfig CommandConfig
        {
            get
            {
                return commandConfig;
            }
            set
            {
                commandConfig = null;
                ShowCommandProps(value);
                commandConfig = value;
            }
        }
        
        /// <summary>
        /// Gets the tool tip to be accessed on the main form.
        /// </summary>
        internal ToolTip ToolTip
        {
            get
            {
                return toolTip;
            }
            set
            {
                toolTip = value;
            }
        }


        /// <summary>
        /// Shows the command properties.
        /// </summary>
        private void ShowCommandProps(CommandConfig commandConfig)
        {
            if (commandConfig == null)
            {
                txtName.Text = "";
                txtCmdCode.Text = "";
                txtSql.Text = "";
                pnlCmdCodeWarn.Visible = false;
            }
            else
            {
                txtName.Text = commandConfig.Name;
                txtCmdCode.Text = commandConfig.CmdCode;
                pnlCmdCodeWarn.Visible = string.IsNullOrEmpty(commandConfig.CmdCode);
                txtSql.Text = commandConfig.Sql.Replace("\n", Environment.NewLine);
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(commandConfig, changeArgument));
        }

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        public void SetFocus()
        {
            txtName.Select();
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (commandConfig != null)
            {
                bool updateCmdCode = commandConfig.Name == commandConfig.CmdCode;
                commandConfig.Name = txtName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);

                if (updateCmdCode)
                    txtCmdCode.Text = txtName.Text;
            }
        }

        private void txtCmdCode_TextChanged(object sender, EventArgs e)
        {
            if (commandConfig != null)
            {
                commandConfig.CmdCode = txtCmdCode.Text;
                pnlCmdCodeWarn.Visible = txtCmdCode.Text == "";
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void txtSql_TextChanged(object sender, EventArgs e)
        {
            if (commandConfig != null)
            {
                commandConfig.Sql = txtSql.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
