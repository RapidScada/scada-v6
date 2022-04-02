// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code;
using Scada.Admin.Extensions.ExtWirenBoard.Code.Models;
using Scada.Admin.Extensions.ExtWirenBoard.Properties;
using Scada.Forms;

namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
{
    /// <summary>
    /// Represents a control for selecting devices and controls.
    /// <para>Представляет элемент управления для выбора устройств и элементов.</para>
    /// </summary>
    internal partial class CtrlDeviceTree : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlDeviceTree()
        {
            InitializeComponent();

            ilTree.Images.Add("device.png", Resources.device);
            ilTree.Images.Add("elem.png", Resources.elem);
        }


        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            treeView.Select();
        }

        /// <summary>
        /// Shows the data model.
        /// </summary>
        public void ShowModel(WirenBoardModel wirenBoardModel)
        {
            ArgumentNullException.ThrowIfNull(wirenBoardModel, nameof(wirenBoardModel));

            try
            {
                treeView.BeginUpdate();
                treeView.Nodes.Clear();

                foreach (DeviceModel deviceModel in wirenBoardModel.Devices)
                {
                    TreeNode deviceNode = TreeViewExtensions.CreateNode(deviceModel.Code, "device.png", deviceModel);
                    treeView.Nodes.Add(deviceNode);

                    foreach (ControlModel controlModel in deviceModel.Controls)
                    {
                        TreeNode controlNode = TreeViewExtensions.CreateNode(controlModel.Code, "elem.png", controlModel);
                        deviceNode.Nodes.Add(controlNode);
                    }

                    if (deviceNode.Nodes.Count > 0)
                        deviceNode.Checked = true;
                }

                if (treeView.Nodes.Count > 0)
                    treeView.SelectedNode = treeView.Nodes[0];
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Validates the control.
        /// </summary>
        public bool ValidateControl()
        {
            foreach (TreeNode deviceNode in treeView.Nodes)
            {
                if (deviceNode.Checked)
                    return true;
            }

            ScadaUiUtils.ShowError(ExtensionPhrases.SelectDevice);
            return false;
        }


        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode deviceNode in treeView.Nodes)
            {
                deviceNode.Checked = true;
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            foreach (TreeNode deviceNode in treeView.Nodes)
            {
                deviceNode.Checked = false;
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            object tag = treeView.SelectedNode?.Tag;

            if (tag is DeviceModel deviceModel)
                propertyGrid.SelectedObject = deviceModel.Meta;
            else if (tag is ControlModel controlModel)
                propertyGrid.SelectedObject = controlModel.Meta;
            else
                propertyGrid.SelectedObject = null;
        }

        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeView.AfterCheck -= treeView_AfterCheck;
            TreeNode node = e.Node;
            bool isChecked = node.Checked;

            // select parent node
            if (node.Parent != null && isChecked)
                node.Parent.Checked = true;

            // select child nodes
            foreach (TreeNode childNode in node.Nodes)
            {
                childNode.Checked = isChecked;
            }

            if (isChecked)
                node.Expand();

            treeView.AfterCheck += treeView_AfterCheck;
        }
    }
}
