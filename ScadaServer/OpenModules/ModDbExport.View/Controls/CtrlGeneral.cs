// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Server.Modules.ModDbExport.Config;
using System.ComponentModel;

namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    /// <summary>
    /// Represents a control for editing a general options.
    /// <para>Представляет элемент управления для редактирования общих настроек.</para>
    /// </summary>
    public partial class CtrlGeneral : UserControl
    {
        private GeneralOptions generalOptions;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlGeneral()
        {
            InitializeComponent();

            generalOptions = null;
            ConfigDataset = null;
        }


        /// <summary>
        /// Gets or sets the general options for editing.
        /// </summary>
        internal GeneralOptions GeneralOptions
        {
            get
            {
                return generalOptions;
            }
            set
            {
                generalOptions = null;
                ShowOptions(value);
                generalOptions = value;
            }
        }

        /// <summary>
        /// Gets or sets the configuration database.
        /// </summary>
        public ConfigDataset ConfigDataset { get; set; }


        /// <summary>
        /// Shows the options.
        /// </summary>
        private void ShowOptions(GeneralOptions options)
        {
            if (options == null)
            {
                chkActive.Checked = false;
                txtTargetID.Text = "";
                txtName.Text = "";
                txtCmdCode.Text = "";
                numStatusCnlNum.Value = numStatusCnlNum.Minimum;
                numMaxQueueSize.Value = numMaxQueueSize.Minimum;                
                numDataLifetime.Value = numDataLifetime.Minimum;
            }
            else
            {
                chkActive.Checked = options.Active;
                txtTargetID.Text = options.ID.ToString();
                txtName.Text = options.Name;
                txtCmdCode.Text = options.CmdCode;
                numStatusCnlNum.SetValue(options.StatusCnlNum);
                numMaxQueueSize.SetValue(options.MaxQueueSize);
                numDataLifetime.SetValue(options.DataLifetime);
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(generalOptions, changeArgument));
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


        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.Active = chkActive.Checked;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.Name = txtName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtCmdCode_TextChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.CmdCode = txtCmdCode.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numStatusCnlNum_ValueChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.StatusCnlNum = Convert.ToInt32(numStatusCnlNum.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numMaxQueueSize_ValueChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.MaxQueueSize = Convert.ToInt32(numMaxQueueSize.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numDataLifetime_ValueChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.DataLifetime = Convert.ToInt32(numDataLifetime.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void btnSelectCnlStatus_Click(object sender, EventArgs e)
        {
            if (generalOptions != null && ConfigDataset != null)
            {
                FrmEntitySelect frmEntitySelect = new(ConfigDataset.CnlStatusTable)
                {
                    MultiSelect = false,
                    SelectedID = generalOptions.StatusCnlNum
                };

                if (frmEntitySelect.ShowDialog() == DialogResult.OK)
                {
                    numStatusCnlNum.Value = frmEntitySelect.SelectedID;
                    OnObjectChanged(TreeUpdateTypes.None);
                }
            }
        }
    }
}
