// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code.Models;
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
                }

                if (treeView.Nodes.Count > 0)
                    treeView.SelectedNode = treeView.Nodes[0];
            }
            finally
            {
                treeView.EndUpdate();
            }
        }
    }
}
