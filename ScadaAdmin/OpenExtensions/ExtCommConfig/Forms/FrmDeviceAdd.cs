// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers;
using Scada.Data.Entities;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for adding a device.
    /// <para>Представляет форму для добавления устройства.</para>
    /// </summary>
    public partial class FrmDeviceAdd : Form
    {
        private readonly IAdminContext adminContext;      // the Administrator context
        private readonly ScadaProject project;            // the project under development
        private readonly RecentSelection recentSelection; // the recently selected objects


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceAdd()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceAdd(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));
            
            Instance = null;
            DeviceConfig = null;
            LineConfig = null;

            numDeviceNum.Maximum = ConfigDatabase.MaxID;
            txtName.MaxLength = ExtensionUtils.NameLength;
            txtStrAddress.MaxLength = ExtensionUtils.DefaultLength;
            txtDescr.MaxLength = ExtensionUtils.DescrLength;
        }


        /// <summary>
        /// Gets the instance affected in Communicator.
        /// </summary>
        public ProjectInstance Instance { get; private set; }

        /// <summary>
        /// Gets the device configuration added to Communicator.
        /// </summary>
        public DeviceConfig DeviceConfig { get; private set; }

        /// <summary>
        /// Gets the communication line configuration of the device added to Communicator.
        /// </summary>
        public LineConfig LineConfig { get; private set; }

        /// <summary>
        /// Gets a value indicating whether a device has been added to the Communicator configuration.
        /// </summary>
        public bool AddedToComm
        {
            get
            {
                return Instance != null && DeviceConfig != null && LineConfig != null;
            }
        }


        /// <summary>
        /// Fills the combo box with the device types.
        /// </summary>
        private void FillDevTypeList()
        {
            List<DevType> devTypes = new(project.ConfigDatabase.DevTypeTable.ItemCount + 1);
            devTypes.Add(new DevType { DevTypeID = 0, Name = " " });
            devTypes.AddRange(project.ConfigDatabase.DevTypeTable.Enumerate().OrderBy(dt => dt.Name));

            cbDevType.ValueMember = "DevTypeID";
            cbDevType.DisplayMember = "Name";
            cbDevType.DataSource = devTypes;

            try { cbDevType.SelectedValue = recentSelection.DevTypeID; }
            catch { cbDevType.SelectedValue = 0; }
        }

        /// <summary>
        /// Fills the combo box with the communication lines.
        /// </summary>
        private void FillCommLineList()
        {
            List<CommLine> commLines = new(project.ConfigDatabase.CommLineTable.ItemCount + 1);
            commLines.Add(new CommLine { CommLineNum = 0, Name = " " });
            commLines.AddRange(project.ConfigDatabase.CommLineTable.Enumerate().OrderBy(line => line.Name));

            cbCommLine.ValueMember = "CommLineNum";
            cbCommLine.DisplayMember = "Name";
            cbCommLine.DataSource = commLines;

            try { cbCommLine.SelectedValue = recentSelection.CommLineNum; }
            catch { cbCommLine.SelectedValue = 0; }
        }

        /// <summary>
        /// Fills the combo box with the instances.
        /// </summary>
        private void FillInstanceList()
        {
            cbInstance.ValueMember = "Name";
            cbInstance.DisplayMember = "Name";
            cbInstance.DataSource = project.Instances;

            try
            {
                if (!string.IsNullOrEmpty(recentSelection.InstanceName))
                    cbInstance.SelectedValue = recentSelection.InstanceName;
            }
            catch
            {
                if (cbInstance.Items.Count > 0)
                    cbInstance.SelectedIndex = 0;
            }
        }
        
        /// <summary>
        /// Sets the device number by default.
        /// </summary>
        private void SetDeviceNum()
        {
            if (recentSelection.DeviceNum > 0)
                numDeviceNum.SetValue(recentSelection.DeviceNum + 1);
            else if (project.ConfigDatabase.DeviceTable.ItemCount > 0)
                numDeviceNum.SetValue(project.ConfigDatabase.DeviceTable.GetNextPk());
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            StringBuilder sbError = new();

            if (string.IsNullOrWhiteSpace(txtName.Text))
                sbError.AppendError(lblName, CommonPhrases.NonemptyRequired);

            if (txtNumAddress.Text != "" && !int.TryParse(txtNumAddress.Text, out _))
                sbError.AppendError(lblNumAddress, CommonPhrases.IntegerRequired);

            if (chkAddToComm.Checked && cbInstance.SelectedItem == null)
                sbError.AppendError(lblInstance, CommonPhrases.NonemptyRequired);

            if (sbError.Length > 0)
            {
                ScadaUiUtils.ShowError(CommonPhrases.CorrectErrors + Environment.NewLine + sbError);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks feasibility of adding a device.
        /// </summary>
        private bool CheckFeasibility(out LineConfig lineConfig)
        {
            lineConfig = null;
            int deviceNum = Convert.ToInt32(numDeviceNum.Value);
            int commLineNum = (int)cbCommLine.SelectedValue;

            // check that device does not exist in the configuration database
            if (project.ConfigDatabase.DeviceTable.PkExists(deviceNum))
            {
                ScadaUiUtils.ShowError(ExtensionPhrases.DeviceExistsInConfigDatabase);
                return false;
            }

            if (chkAddToComm.Checked &&
                cbInstance.SelectedItem is ProjectInstance instance && instance.CommApp.Enabled)
            {
                // check that communication line is selected
                if (commLineNum <= 0)
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.ChooseLine);
                    return false;
                }

                // load instance configuration
                if (!instance.LoadAppConfig(out string errMsg))
                {
                    ScadaUiUtils.ShowError(errMsg);
                    return false;
                }

                // reverse search for communication line
                lineConfig = instance.CommApp.AppConfig.Lines.FindLast(line => line.CommLineNum == commLineNum);

                if (lineConfig == null)
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.LineNotFoundInCommConfig);
                    return false;
                }

                // check that device does not exist in communication line
                if (lineConfig.DevicePolling.Any(device => device.DeviceNum == deviceNum))
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.DeviceExistsInLineConfig);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Sets the default polling options of the added device.
        /// </summary>
        private void SetPollingOptions()
        {
            if (!string.IsNullOrEmpty(DeviceConfig.Driver))
            {
                if (ExtensionUtils.GetDriverView(adminContext, Instance.CommApp, DeviceConfig.Driver, 
                    out DriverView driverView, out string message))
                {
                    if (driverView.CanCreateDevice)
                    {
                        DeviceView deviceView = driverView.CreateDeviceView(LineConfig, DeviceConfig);
                        PollingOptions pollingOptions = deviceView?.GetPollingOptions();

                        if (pollingOptions != null)
                            pollingOptions.CopyTo(DeviceConfig.PollingOptions);
                    }
                }
                else
                {
                    ScadaUiUtils.ShowError(message);
                }
            }
        }


        private void FrmDeviceAdd_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FillDevTypeList();
            FillCommLineList();
            FillInstanceList();
            SetDeviceNum();
            txtName.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls() && CheckFeasibility(out LineConfig lineConfig))
            {
                // create new device
                int devTypeID = (int)cbDevType.SelectedValue;
                int commLineNum = (int)cbCommLine.SelectedValue;

                Device deviceEntity = new()
                {
                    DeviceNum = Convert.ToInt32(numDeviceNum.Value),
                    Name = txtName.Text,
                    Code = txtCode.Text,
                    DevTypeID = devTypeID > 0 ? devTypeID : null,
                    NumAddress = txtNumAddress.Text == "" ? null : int.Parse(txtNumAddress.Text),
                    StrAddress = txtStrAddress.Text,
                    CommLineNum = commLineNum > 0 ? commLineNum : null,
                    Descr = txtDescr.Text
                };

                // add device to the configuration database
                project.ConfigDatabase.DeviceTable.AddItem(deviceEntity);
                project.ConfigDatabase.DeviceTable.Modified = true;

                // add device to Communicator configuration
                if (chkAddToComm.Checked && cbInstance.SelectedItem is ProjectInstance instance)
                {
                    Instance = instance;
                    recentSelection.InstanceName = instance.Name;

                    if (instance.CommApp.Enabled && lineConfig != null)
                    {
                        DeviceConfig = CommConfigConverter.CreateDeviceConfig(deviceEntity, 
                            project.ConfigDatabase.DevTypeTable);
                        DeviceConfig.Parent = lineConfig;
                        lineConfig.DevicePolling.Add(DeviceConfig);
                        LineConfig = lineConfig;
                        SetPollingOptions();
                    }
                }

                recentSelection.CommLineNum = commLineNum;
                recentSelection.DeviceNum = deviceEntity.DeviceNum;
                recentSelection.DevTypeID = devTypeID;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
