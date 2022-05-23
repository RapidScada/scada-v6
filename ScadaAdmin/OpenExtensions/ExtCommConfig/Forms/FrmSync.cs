// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
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
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly ScadaProject project;       // the project under development
        private readonly CommApp commApp;            // the Communicator application in a project
        private readonly CommConfig commConfig;      // the Communicator configuration
        private string lastErrorMessage;             // the last error message during sync


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
        public FrmSync(IAdminContext adminContext, ScadaProject project, CommApp commApp)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.commApp = commApp ?? throw new ArgumentNullException(nameof(commApp));
            commConfig = commApp.AppConfig;
            lastErrorMessage = "";

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
        /// Finds the communication line in the Communicator configuration, or returns an insertion index.
        /// </summary>
        private bool FindCommLine(int commLineNum, out LineConfig lineConfig, out int insertIndex)
        {
            int index = 0;
            insertIndex = -1;

            foreach (LineConfig line in commConfig.Lines)
            {
                if (line.CommLineNum == commLineNum)
                {
                    lineConfig = line;
                    return true;
                }

                if (commLineNum < line.CommLineNum && insertIndex < 0)
                    insertIndex = index;

                index++;
            }

            if (insertIndex < 0)
                insertIndex = commConfig.Lines.Count;

            lineConfig = null;
            return false;
        }

        /// <summary>
        /// Finds the device within the communication line, or returns an insertion index.
        /// </summary>
        private static bool FindDevice(int deviceNum, LineConfig lineConfig, out DeviceConfig deviceConfig, 
            out int insertIndex)
        {
            int index = 0;
            insertIndex = -1;

            foreach (DeviceConfig device in lineConfig.DevicePolling)
            {
                if (device.DeviceNum == deviceNum)
                {
                    deviceConfig = device;
                    return true;
                }

                if (deviceNum < device.DeviceNum && insertIndex < 0)
                    insertIndex = index;

                index++;
            }

            if (insertIndex < 0)
                insertIndex = lineConfig.DevicePolling.Count;

            deviceConfig = null;
            return false;
        }

        /// <summary>
        /// Sets the default polling options for the specified device.
        /// </summary>
        private void SetPollingOptions(DeviceConfig deviceConfig)
        {
            if (!string.IsNullOrEmpty(deviceConfig.Driver))
            {
                if (ExtensionUtils.GetDriverView(adminContext, commApp, deviceConfig.Driver,
                    out DriverView driverView, out string message))
                {
                    if (driverView.CanCreateDevice)
                    {
                        DeviceView deviceView = driverView.CreateDeviceView(deviceConfig.ParentLine, deviceConfig);
                        PollingOptions pollingOptions = deviceView?.GetPollingOptions();

                        if (pollingOptions != null)
                            pollingOptions.CopyTo(deviceConfig.PollingOptions);
                    }
                }
                else
                {
                    lastErrorMessage = message;
                }
            }
        }

        /// <summary>
        /// Imports communication lines and devices to the Communicator configuration.
        /// </summary>
        private void ImportToComm(List<TreeNode> selectedLineNodes)
        {
            foreach (TreeNode lineNode in selectedLineNodes)
            {
                if (lineNode.GetRelatedObject() is CommLine commLine)
                {
                    // communication line
                    if (FindCommLine(commLine.CommLineNum, out LineConfig lineConfig, out int insertIndex))
                    {
                        // update existing line
                        lineConfig.Name = commLine.Name;
                    }
                    else
                    {
                        // add new line
                        lineConfig = CommConfigConverter.CreateLineConfig(commLine);
                        lineConfig.Parent = commConfig;
                        commConfig.Lines.Insert(insertIndex, lineConfig);
                        AddedToComm = true;
                    }

                    // devices
                    foreach (TreeNode deviceNode in lineNode.Nodes)
                    {
                        if (deviceNode.Checked && deviceNode.GetRelatedObject() is Device device)
                        {
                            if (FindDevice(device.DeviceNum, lineConfig, out DeviceConfig deviceConfig, out insertIndex))
                            {
                                // update existing device
                                CommConfigConverter.CopyDeviceProps(device, deviceConfig, 
                                    project.ConfigDatabase.DevTypeTable);
                            }
                            else
                            {
                                // add new device
                                deviceConfig = CommConfigConverter.CreateDeviceConfig(device, 
                                    project.ConfigDatabase.DevTypeTable);
                                deviceConfig.Parent = lineConfig;
                                SetPollingOptions(deviceConfig);
                                lineConfig.DevicePolling.Insert(insertIndex, deviceConfig);
                                AddedToComm = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Imports communication lines and devices to the configuration database.
        /// </summary>
        private void ImportToBase(List<TreeNode> selectedLineNodes)
        {
            BaseTable<CommLine> commLineTable = project.ConfigDatabase.CommLineTable;
            BaseTable<Device> deviceTable = project.ConfigDatabase.DeviceTable;
            BaseTable<DevType> devTypeTable = project.ConfigDatabase.DevTypeTable;

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

                    commLineTable.Modified = true;

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

                            deviceTable.Modified = true;
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
                ctrlSync2.FillTreeView(project.ConfigDatabase, SelectedLineNum);
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

                if (string.IsNullOrEmpty(lastErrorMessage))
                    ScadaUiUtils.ShowInfo(ExtensionPhrases.SyncCompleted);
                else
                    ScadaUiUtils.ShowError(ExtensionPhrases.SyncCompletedWithError, lastErrorMessage);

                DialogResult = DialogResult.OK;
            }
            else
            {
                ScadaUiUtils.ShowWarning(ExtensionPhrases.NoDataToSync);
            }
        }
    }
}
