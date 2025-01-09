// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Server.Modules.ModDeviceAlarm.Config;
using System.ComponentModel;

namespace Scada.Server.Modules.ModDeviceAlarm.View.Controls
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
                txtEmailAddress.Text = "";
                txtEmailSubject.Text = "";
                txtEmailContent.Text = "";
            }
            else
            {
                chkActive.Checked = options.Active;
                txtTargetID.Text = options.ID.ToString();
                txtName.Text = options.Name;
                txtEmailAddress.Text = options.EmailAddress;
                txtEmailSubject.Text = options.EmailSubject;
                txtEmailContent.Text = options.EmailContent;
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


        private void numSendTimes_ValueChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.SendTimes = Convert.ToInt32(numSendTimes.Value);
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtEmailAddress_TextChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.EmailAddress = txtEmailAddress.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void txtEmailSubject_TextChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.EmailSubject = txtEmailSubject.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void txtEmailContent_TextChanged(object sender, EventArgs e)
        {
            if (generalOptions != null)
            {
                generalOptions.EmailContent = txtEmailContent.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
