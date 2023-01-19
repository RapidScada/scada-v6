// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvDbImport.Config;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvDbImport.View.Controls
{
    /// <summary>
    /// Represetns a control for editing a command options.
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
            }
            else
            {
                txtName.Text = commandConfig.Name;
                txtCmdCode.Text = commandConfig.CmdCode;
                
                txtSql.Clear();
                txtSql.AppendText(commandConfig.Sql.Replace("\n", Environment.NewLine));
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
                commandConfig.Name = txtName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtCmdCode_TextChanged(object sender, EventArgs e)
        {
            if (commandConfig != null)
            {
                commandConfig.CmdCode = txtCmdCode.Text;
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
