// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control for editing custom communication line options.
    /// <para>Представляет элемент управления для редактирования пользовательских параметров линии связи.</para>
    /// </summary>
    public partial class CtrlLineCustom : UserControl
    {
        private bool changing; // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLineCustom()
        {
            InitializeComponent();
            SetColumnNames();
            changing = false;
        }


        /// <summary>
        /// Sets the column names as needed for translation.
        /// </summary>
        private void SetColumnNames()
        {
            colParamName.Name = nameof(colParamName);
            colParamValue.Name = nameof(colParamValue);
        }
        
        /// <summary>
        /// Enables or disables the controls.
        /// </summary>
        private void SetControlsEnabled()
        {
            if (lvCustomOptions.SelectedItems.Count > 0)
            {
                btnDeleteOption.Enabled = true;
                gbSelectedOption.Enabled = true;
            }
            else
            {
                btnDeleteOption.Enabled = false;
                gbSelectedOption.Enabled = false;
            }
        }

        /// <summary>
        /// Raises an OptionsChanged event.
        /// </summary>
        private void OnOptionsChanged()
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates a new list view item that represents the custom option.
        /// </summary>
        private static ListViewItem CreateOptionItem(string name, string value)
        {
            return new ListViewItem(new string[] { name, value });
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        public void OptionsToControls(OptionList options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            try
            {
                lvCustomOptions.BeginUpdate();
                lvCustomOptions.Items.Clear();

                foreach (KeyValuePair<string, string> pair in options)
                {
                    lvCustomOptions.Items.Add(CreateOptionItem(pair.Key, pair.Value));
                }

                if (lvCustomOptions.Items.Count > 0)
                    lvCustomOptions.Items[0].Selected = true;
            }
            finally
            {
                lvCustomOptions.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        public void ControlsToOptions(OptionList options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Clear();

            foreach (ListViewItem item in lvCustomOptions.Items)
            {
                string name = item.SubItems[0].Text.Trim();
                if (name != "")
                    options[name] = item.SubItems[1].Text.Trim();
            }
        }


        /// <summary>
        /// Occurs when the options change.
        /// </summary>
        public event EventHandler OptionsChanged;


        private void CtrlLineCustomParams_Load(object sender, EventArgs e)
        {
            SetControlsEnabled();
        }

        private void btnAddOption_Click(object sender, EventArgs e)
        {
            // add new option
            ListViewItem item = CreateOptionItem("", "");
            lvCustomOptions.Items.Add(item);
            item.Selected = true;
            txtOptionName.Focus();
            OnOptionsChanged();
        }

        private void btnDeleteOption_Click(object sender, EventArgs e)
        {
            // delete the selected option
            if (lvCustomOptions.RemoveSelectedItem(false))
                OnOptionsChanged();

            lvCustomOptions.Focus();
        }

        private void lvCustomOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            // display the selected option
            changing = true;

            if (lvCustomOptions.SelectedItems.Count > 0)
            {
                ListViewItem item = lvCustomOptions.SelectedItems[0];
                txtOptionName.Text = item.SubItems[0].Text;
                txtOptionValue.Text = item.SubItems[1].Text;
            }
            else
            {
                txtOptionName.Text = "";
                txtOptionValue.Text = "";
            }

            SetControlsEnabled();
            changing = false;
        }

        private void txtOptionName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lvCustomOptions.SelectedItems.Count > 0)
            {
                ListViewItem item = lvCustomOptions.SelectedItems[0];
                item.SubItems[0].Text = txtOptionName.Text;
                OnOptionsChanged();
            }
        }

        private void txtOptionValue_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lvCustomOptions.SelectedItems.Count > 0)
            {
                ListViewItem item = lvCustomOptions.SelectedItems[0];
                item.SubItems[1].Text = txtOptionValue.Text;
                OnOptionsChanged();
            }
        }
    }
}
