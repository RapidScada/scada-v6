// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Forms;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvOpcUa.View.Controls
{
    /// <summary>
    /// Represents a control for editing a method calling command.
    /// <para>Представляет элемент управления для редактирования команды вызова метода.</para>
    /// </summary>
    public partial class CtrlCallMethodCommand : UserControl
    {
        private CallMethodCommandConfig commandConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCallMethodCommand()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the edited command.
        /// </summary>
        public CallMethodCommandConfig CommandConfig
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
        /// Shows the command properties.
        /// </summary>
        private void ShowCommandProps(CallMethodCommandConfig commandConfig)
        {
            if (commandConfig != null)
            {
                txtDisplayName.Text = commandConfig.DisplayName;
                txtCmdCode.Text = commandConfig.CmdCode;
                numCmdNum.SetValue(commandConfig.CmdNum);
                txtNodeID.Text = commandConfig.NodeID;
                txtParentNodeID.Text = commandConfig.ParentNodeID;
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
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            txtDisplayName.Select();
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (commandConfig != null)
            {
                commandConfig.DisplayName = txtDisplayName.Text;
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

        private void numCmdNum_ValueChanged(object sender, EventArgs e)
        {
            if (commandConfig != null)
            {
                commandConfig.CmdNum = Convert.ToInt32(numCmdNum.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
