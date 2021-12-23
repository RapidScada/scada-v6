// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for synchronizing properties of communication lines and devices.
    /// <para>Представляет форму для синхронизации свойств линий связи и устройств.</para>
    /// </summary>
    public partial class FrmSync : Form
    {
        private readonly ScadaProject project;  // the project under development
        private readonly CommConfig commConfig; // the Communicator configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmSync()
        {
            InitializeComponent();

            ctrlSync2.Visible = false;
            ctrlSync2.Top = ctrlSync1.Top;
            btnNext.Left = btnSync.Left;
            btnSync.Visible = false;
        }
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmSync(ScadaProject project, CommConfig commConfig)
            : this()
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.commConfig = commConfig ?? throw new ArgumentNullException(nameof(commConfig));

            SelectedLineNum = 0;
            AddedToComm = false;
        }


        /// <summary>
        /// Gets or sets the selected line number.
        /// </summary>
        public int SelectedLineNum { get; set; }

        /// <summary>
        /// Gets a value indicating whether the sync is performed from the configuration database to Communicator.
        /// </summary>
        public bool BaseToComm => ctrlSync1.BaseToComm;

        /// <summary>
        /// Gets a value indicating whether a communication line or device has been added to 
        /// the Communicator configuration.
        /// </summary>
        public bool AddedToComm { get; private set; }


        /// <summary>
        /// Imports communication lines and devices to the Communicator configuration.
        /// </summary>
        private void ImportToComm(List<TreeNode> selectedLineNodes)
        {
            foreach (TreeNode lineNode in selectedLineNodes)
            {
                if (lineNode.GetRelatedObject() is CommLine commLine)
                {

                }
            }
        }

        /// <summary>
        /// Imports communication lines and devices to the configuration database.
        /// </summary>
        private void ImportToBase(List<TreeNode> selectedLineNodes)
        {
            BaseTable<CommLine> commLineTable = project.ConfigBase.CommLineTable;
            BaseTable<Device> deviceTable = project.ConfigBase.DeviceTable;
            BaseTable<DevType> devTypeTable = project.ConfigBase.DevTypeTable;

            foreach (TreeNode lineNode in selectedLineNodes)
            {
                if (lineNode.GetRelatedObject() is LineConfig lineConfig)
                {
                    // communication line
                    if (commLineTable.Items.TryGetValue(lineConfig.CommLineNum, out CommLine commLine))
                    {
                        // update existing line
                        commLine.Name = lineConfig.Name;
                    }
                    else
                    {
                        // add new line
                        commLineTable.AddItem(new CommLine
                        {
                            CommLineNum = lineConfig.CommLineNum,
                            Name = lineConfig.Name
                        });
                    }

                    // devices
                    foreach (TreeNode deviceNode in lineNode.Nodes)
                    {
                        if (deviceNode.Checked && deviceNode.GetRelatedObject() is DeviceConfig deviceConfig)
                        {
                            int? devTypeID = devTypeTable.SelectFirst(
                                new TableFilter("Driver", deviceConfig.Driver))?.DevTypeID;
                            int? numAddress = deviceConfig.NumAddress > 0 ? deviceConfig.NumAddress : null;

                            if (deviceTable.Items.TryGetValue(deviceConfig.DeviceNum, out Device device))
                            {
                                // update existing device
                                device.Name = deviceConfig.Name;
                                device.DevTypeID = devTypeID;
                                device.NumAddress = numAddress;
                                device.StrAddress = deviceConfig.StrAddress;
                                device.CommLineNum = lineConfig.CommLineNum;
                            }
                            else
                            {
                                // add new device
                                deviceTable.AddItem(new Device
                                {
                                    DeviceNum = deviceConfig.DeviceNum,
                                    Name = deviceConfig.Name,
                                    DevTypeID = devTypeID,
                                    NumAddress = numAddress,
                                    StrAddress = deviceConfig.StrAddress,
                                    CommLineNum = lineConfig.CommLineNum
                                });
                            }
                        }
                    }
                }
            }
        }


        private void FrmSync_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlSync1, ctrlSync1.GetType().FullName);
            FormTranslator.Translate(ctrlSync2, ctrlSync2.GetType().FullName);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ctrlSync1.Visible = false;
            ctrlSync2.Visible = true;
            btnNext.Visible = false;
            btnSync.Visible = true;

            if (BaseToComm)
                ctrlSync2.FillTreeView(project.ConfigBase, SelectedLineNum);
            else
                ctrlSync2.FillTreeView(commConfig, SelectedLineNum);
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            List<TreeNode> selectedLineNodes = ctrlSync2.GetSelectedLineNodes();

            if (selectedLineNodes.Count > 0)
            {
                if (BaseToComm)
                    ImportToComm(selectedLineNodes);
                else
                    ImportToBase(selectedLineNodes);

                ScadaUiUtils.ShowInfo(ExtensionPhrases.SyncCompleted);
                DialogResult = DialogResult.OK;
            }
            else
            {
                ScadaUiUtils.ShowWarning(ExtensionPhrases.NoDataToSync);
            }
        }
    }
}
