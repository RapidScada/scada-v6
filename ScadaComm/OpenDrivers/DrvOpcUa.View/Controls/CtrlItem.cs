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
                txtNodeID.Text = itemConfig.NodeID;
                chkIsArray.Checked = itemConfig.IsArray;
                numArrayLen.Enabled = itemConfig.IsArray;
                numArrayLen.SetValue(itemConfig.ArrayLen);

                if (itemConfig.Tag is ItemConfigTag tag)
                    txtSignal.Text = tag.GetTagNumInfo();
            }
        }

        /// <summary>
        /// Updates the information in the item tag.
        /// </summary>
        private void UpdateTag()
        {
            if (itemConfig.Tag is ItemConfigTag tag)
            {
                tag.SetLength(itemConfig.IsArray, itemConfig.ArrayLen);
                txtSignal.Text = tag.GetTagNumInfo();
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
        /// Shows a range of tag numbers.
        /// </summary>
        public void ShowTagNum()
        {
            if (itemConfig?.Tag is ItemConfigTag tag)
                txtSignal.Text = tag.GetTagNumInfo();
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

        private void chkIsArray_CheckedChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                if (chkIsArray.Checked)
                {
                    itemConfig.IsArray = true;
                    numArrayLen.Enabled = true;
                }
                else
                {
                    itemConfig.IsArray = false;
                    numArrayLen.SetValue(1);
                    numArrayLen.Enabled = false;
                }

                UpdateTag();
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void numArrayLen_ValueChanged(object sender, EventArgs e)
        {
            if (itemConfig != null)
            {
                itemConfig.ArrayLen = Convert.ToInt32(numArrayLen.Value);
                UpdateTag();
                OnObjectChanged(TreeUpdateTypes.UpdateTagNums);
            }
        }
    }
}
