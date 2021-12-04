// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Forms;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvOpcUa.View.Controls
{
    /// <summary>
    /// Represents a control for editing a monitored item.
    /// <para>Представляет элемент управления для редактирования отслеживаемого элемента.</para>
    /// </summary>
    public partial class CtrlItem : UserControl
    {
        private ItemConfig itemConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlItem()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the edited monitored item.
        /// </summary>
        public ItemConfig ItemConfig
        {
            get
            {
                return itemConfig;
            }
            set
            {
                itemConfig = null;
                ShowItemProps(value);
                itemConfig = value;
            }
        }


        /// <summary>
        /// Shows the monitored item properties.
        /// </summary>
        private void ShowItemProps(ItemConfig itemConfig)
        {
            if (itemConfig != null)
            {
                chkItemActive.Checked = itemConfig.Active;
                txtDisplayName.Text = itemConfig.DisplayName;
                txtTagCode.Text = itemConfig.TagCode;
                txtTagNum.Text = itemConfig?.Tag is ItemConfigTag tag ? tag.TagNumStr : "";
                txtNodeID.Text = itemConfig.NodeID;
                chkIsString.Checked = itemConfig.IsString;
                chkIsArray.Checked = itemConfig.IsArray;
                numArrayLen.Enabled = itemConfig.IsString || itemConfig.IsArray;
                numArrayLen.SetValue(itemConfig.ArrayLen);
            }
        }

        /// <summary>
        /// Enables or disables the array length control.
        /// </summary>
        private void SetArrayLengthEnabled()
        {
            if (chkIsString.Checked || chkIsArray.Checked)
            {
                numArrayLen.Enabled = true;
            }
            else
            {
                numArrayLen.SetValue(1);
                numArrayLen.Enabled = false;
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(itemConfig, changeArgument));
        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            txtDisplayName.Select();
        }

        /// <summary>
        /// Refreshes the displayed tag number.
        /// </summary>
        public void RefreshTagNum()
        {
            txtTagNum.Text = itemConfig?.Tag is ItemConfigTag tag ? tag.TagNumStr : "";
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void chkItemActive_CheckedChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.Active = chkItemActive.Checked;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.DisplayName = txtDisplayName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void txtTagCode_TextChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.TagCode = txtTagCode.Text;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void chkIsString_CheckedChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.IsString = chkIsString.Checked;

                chkIsArray.CheckedChanged -= chkIsArray_CheckedChanged;
                itemConfig.IsArray = false;
                chkIsArray.Checked = false;
                chkIsArray.CheckedChanged += chkIsArray_CheckedChanged;

                OnObjectChanged(TreeUpdateTypes.None);
                SetArrayLengthEnabled();
            }
        }

        private void chkIsArray_CheckedChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.IsArray = chkIsArray.Checked;

                chkIsString.CheckedChanged -= chkIsString_CheckedChanged;
                itemConfig.IsString = false;
                chkIsString.Checked = false;
                chkIsString.CheckedChanged += chkIsString_CheckedChanged;

                OnObjectChanged(TreeUpdateTypes.None);
                SetArrayLengthEnabled();
            }
        }

        private void numArrayLen_ValueChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.ArrayLen = Convert.ToInt32(numArrayLen.Value);
                OnObjectChanged(TreeUpdateTypes.UpdateTagNums);
            }
        }
    }
}
