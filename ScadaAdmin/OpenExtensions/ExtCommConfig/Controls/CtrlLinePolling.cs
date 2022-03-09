// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Config;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control for editing a polling sequence of a communication line options.
    /// <para>Представляет элемент управления для редактирования основных последовательности опроса линии связи.</para>
    /// </summary>
    public partial class CtrlLinePolling : UserControl
    {
        private IAdminContext adminContext;   // the Administrator context
        private CommApp commApp;              // the Communicator application in a project
        private LineConfig lineConfig;        // the communication line configuration
        private bool changing;                // controls are being changed programmatically
        private DeviceConfig deviceClipboard; // contains the copied device


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLinePolling()
        {
            InitializeComponent();
            numNumAddress.Maximum = int.MaxValue;

            SetColumnNames();
            adminContext = null;
            commApp = null;
            lineConfig = null;
            changing = false;
            deviceClipboard = null;
        }


        /// <summary>
        /// Validates that the control is initialized.
        /// </summary>
        private void ValidateInit()
        {
            if (adminContext == null)
                throw new InvalidOperationException("Administrator context must not be null.");

            if (commApp == null)
                throw new InvalidOperationException("Communicator application must not be null.");

            if (lineConfig == null)
                throw new InvalidOperationException("Communication line configuration must not be null.");
        }

        /// <summary>
        /// Sets the column names as needed for translation.
        /// </summary>
        private void SetColumnNames()
        {
            colOrder.Name = nameof(colOrder);
            colActive.Name = nameof(colActive);
            colPollOnCmd.Name = nameof(colPollOnCmd);
            colIsBound.Name = nameof(colIsBound);
            colNumber.Name = nameof(colNumber);
            colName.Name = nameof(colName);
            colDriver.Name = nameof(colDriver);
            colNumAddress.Name = nameof(colNumAddress);
            colStrAddress.Name = nameof(colStrAddress);
            colTimeout.Name = nameof(colTimeout);
            colDelay.Name = nameof(colDelay);
            colTime.Name = nameof(colTime);
            colPeriod.Name = nameof(colPeriod);
            colCmdLine.Name = nameof(colCmdLine);
        }

        /// <summary>
        /// Fills the combo box that specifies a device driver.
        /// </summary>
        private void FillDriverComboBox()
        {
            try
            {
                cbDriver.BeginUpdate();
                cbDriver.Items.Clear();
                DirectoryInfo dirInfo = new(adminContext.AppDirs.LibDir);

                foreach (FileInfo fileInfo in
                    dirInfo.EnumerateFiles("Drv*.View.dll", SearchOption.TopDirectoryOnly))
                {
                    cbDriver.Items.Add(ScadaUtils.RemoveFileNameSuffixes(fileInfo.Name));
                }
            }
            finally
            {
                cbDriver.EndUpdate();
            }
        }

        /// <summary>
        /// Enables or disables the controls.
        /// </summary>
        private void SetControlsEnabled()
        {
            if (lvDevicePolling.SelectedItems.Count > 0)
            {
                int index = lvDevicePolling.SelectedIndices[0];
                btnMoveUpDevice.Enabled = index > 0;
                btnMoveDownDevice.Enabled = index < lvDevicePolling.Items.Count - 1;
                btnDeleteDevice.Enabled = true;
                btnCutDevice.Enabled = true;
                btnCopyDevice.Enabled = true;
                gbSelectedDevice.Enabled = true;
            }
            else
            {
                btnMoveUpDevice.Enabled = false;
                btnMoveDownDevice.Enabled = false;
                btnDeleteDevice.Enabled = false;
                btnCutDevice.Enabled = false;
                btnCopyDevice.Enabled = false;
                gbSelectedDevice.Enabled = false;
            }
        }

        /// <summary>
        /// Gets the selected list view item and the corresponding device configuration.
        /// </summary>
        private bool GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig)
        {
            if (lvDevicePolling.SelectedItems.Count > 0)
            {
                item = lvDevicePolling.SelectedItems[0];
                deviceConfig = (DeviceConfig)item.Tag;
                return true;
            }
            else
            {
                item = null;
                deviceConfig = null;
                return false;
            }
        }

        /// <summary>
        /// Displays the specified device properties.
        /// </summary>
        private void DisplayDevice(DeviceConfig deviceConfig)
        {
            if (deviceConfig == null)
            {
                chkActive.Checked = false;
                chkPollOnCmd.Checked = false;
                chkIsBound.Checked = false;
                numDeviceNum.Value = 0;
                txtName.Text = "";
                cbDriver.Text = "";
                numNumAddress.Value = 0;
                txtStrAddress.Text = "";
                numTimeout.Value = 0;
                numDelay.Value = 0;
                dtpTime.Value = dtpTime.MinDate;
                dtpPeriod.Value = dtpPeriod.MinDate;
                txtCmdLine.Text = "";
                txtCustomOptions.Text = "";
            }
            else
            {
                PollingOptions pollingOptions = deviceConfig.PollingOptions;
                chkActive.Checked = deviceConfig.Active;
                chkPollOnCmd.Checked = pollingOptions.PollOnCmd;
                chkIsBound.Checked = deviceConfig.IsBound;
                numDeviceNum.SetValue(deviceConfig.DeviceNum);
                txtName.Text = deviceConfig.Name;
                cbDriver.Text = deviceConfig.Driver;
                numNumAddress.SetValue(deviceConfig.NumAddress);
                txtStrAddress.Text = deviceConfig.StrAddress;
                numTimeout.SetValue(pollingOptions.Timeout);
                numDelay.SetValue(pollingOptions.Delay);
                dtpTime.SetTime(pollingOptions.Time);
                dtpPeriod.SetTime(pollingOptions.Period);
                txtCmdLine.Text = pollingOptions.CmdLine;
                txtCustomOptions.Text = pollingOptions.CustomOptions.ToString();
            }
        }

        /// <summary>
        /// Replaces the existing custom options of the device with the new custom options.
        /// </summary>
        private void ReplaceDeviceOptions(OptionList existingCustomOptions, OptionList newCustomOptions)
        {
            existingCustomOptions.Clear();
            newCustomOptions.CopyTo(existingCustomOptions);
            txtCustomOptions.Text = existingCustomOptions.ToString();
        }

        /// <summary>
        /// Adds an item to the list view according to the specified device.
        /// </summary>
        private void AddDeviceItem(DeviceConfig deviceConfig)
        {
            int index = 0;
            lvDevicePolling.InsertItem(CreateDeviceItem(deviceConfig, ref index), true);
            numDeviceNum.Focus();
            OnConfigChanged();
        }

        /// <summary>
        /// Gets a new instance of the device user interface.
        /// </summary>
        private DeviceView GetDeviceView(DeviceConfig deviceConfig)
        {
            ValidateInit();
            return ExtensionUtils.GetDeviceView(adminContext, commApp, deviceConfig);
        }

        /// <summary>
        /// Raises a ConfigChanged event.
        /// </summary>
        private void OnConfigChanged()
        {
            ConfigChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises a LineConfigChanged event.
        /// </summary>
        private void OnLineConfigChanged()
        {
            LineConfigChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates a new list view item that represents the specified device.
        /// </summary>
        private static ListViewItem CreateDeviceItem(DeviceConfig deviceConfig, ref int index)
        {
            return new ListViewItem(new string[]
            {
                (++index ).ToString(),
                AdminUtils.GetCheckedString(deviceConfig.Active),
                AdminUtils.GetCheckedString(deviceConfig.PollingOptions.PollOnCmd),
                AdminUtils.GetCheckedString(deviceConfig.IsBound),
                deviceConfig.DeviceNum.ToString(),
                deviceConfig.Name,
                deviceConfig.Driver,
                deviceConfig.NumAddress.ToString(),
                deviceConfig.StrAddress,
                deviceConfig.PollingOptions.Timeout.ToString(),
                deviceConfig.PollingOptions.Delay.ToString(),
                deviceConfig.PollingOptions.Time.ToString("T", Locale.Culture),
                deviceConfig.PollingOptions.Period.ToString(),
                deviceConfig.PollingOptions.CmdLine
            })
            {
                Tag = deviceConfig
            };
        }


        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(IAdminContext adminContext, CommApp commApp, LineConfig lineConfig)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.commApp = commApp ?? throw new ArgumentNullException(nameof(commApp));
            this.lineConfig = lineConfig ?? throw new ArgumentNullException(nameof(lineConfig));
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        public void ConfigToControls(List<DeviceConfig> devicePolling)
        {
            ValidateInit();
            FillDriverComboBox();

            try
            {
                lvDevicePolling.BeginUpdate();
                lvDevicePolling.Items.Clear();
                int index = 0;

                foreach (DeviceConfig deviceConfig in devicePolling)
                {
                    DeviceConfig deviceConfigCopy = deviceConfig.DeepClone();
                    deviceConfigCopy.Parent = deviceConfig.Parent; // line reference
                    lvDevicePolling.Items.Add(CreateDeviceItem(deviceConfigCopy, ref index));
                }

                if (lvDevicePolling.Items.Count > 0)
                    lvDevicePolling.Items[0].Selected = true;
            }
            finally
            {
                lvDevicePolling.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        public void ControlsToConfig(List<DeviceConfig> devicePolling)
        {
            ValidateInit();
            devicePolling.Clear();

            foreach (ListViewItem item in lvDevicePolling.Items)
            {
                devicePolling.Add((DeviceConfig)item.Tag);
            }
        }


        /// <summary>
        /// Occurs when any configuration changes.
        /// </summary>
        public event EventHandler ConfigChanged;

        /// <summary>
        /// Occurs when the communication line configuration changes.
        /// </summary>
        public event EventHandler LineConfigChanged;


        private void CtrlLineReqSequence_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, 
                new FormTranslatorOptions { ToolTip = toolTip, SkipUserControls = false });
            SetControlsEnabled();
            btnPasteDevice.Enabled = false;
        }

        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            AddDeviceItem(new DeviceConfig { Parent = lineConfig });
        }

        private void btnMoveUpDevice_Click(object sender, EventArgs e)
        {
            if (lvDevicePolling.MoveUpSelectedItem(true))
                OnConfigChanged();
        }

        private void btnMoveDownDevice_Click(object sender, EventArgs e)
        {
            if (lvDevicePolling.MoveDownSelectedItem(true))
                OnConfigChanged();
        }

        private void btnDeleteDevice_Click(object sender, EventArgs e)
        {
            if (lvDevicePolling.RemoveSelectedItem(true))
                OnConfigChanged();
        }

        private void btnCutDevice_Click(object sender, EventArgs e)
        {
            // cut the selected device
            btnCopyDevice_Click(null, null);
            btnDeleteDevice_Click(null, null);
        }

        private void btnCopyDevice_Click(object sender, EventArgs e)
        {
            // copy the selected device
            if (GetSelectedItem(out _, out DeviceConfig deviceConfig))
            {
                btnPasteDevice.Enabled = true;
                deviceClipboard = deviceConfig.DeepClone();
            }

            lvDevicePolling.Focus();
        }

        private void btnPasteDevice_Click(object sender, EventArgs e)
        {
            // paste the copied device
            if (deviceClipboard == null)
                lvDevicePolling.Focus();
            else
                AddDeviceItem(deviceClipboard.DeepClone());
        }

        private void lvDevicePolling_SelectedIndexChanged(object sender, EventArgs e)
        {
            // display the selected item properties
            changing = true;
            GetSelectedItem(out _, out DeviceConfig deviceConfig);
            DisplayDevice(deviceConfig);
            SetControlsEnabled();
            changing = false;
        }

        private void lvDevicePolling_DoubleClick(object sender, EventArgs e)
        {
            btnDeviceProperties_Click(null, null);
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.Active = chkActive.Checked;
                item.SubItems[1].Text = AdminUtils.GetCheckedString(chkActive.Checked);
                OnConfigChanged();
            }
        }

        private void chkPollOnCmd_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.PollingOptions.PollOnCmd = chkPollOnCmd.Checked;
                item.SubItems[2].Text = AdminUtils.GetCheckedString(chkPollOnCmd.Checked);
                OnConfigChanged();
            }
        }

        private void chkIsBound_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.IsBound = chkIsBound.Checked;
                item.SubItems[3].Text = AdminUtils.GetCheckedString(chkIsBound.Checked);
                OnConfigChanged();
            }
        }

        private void numDeviceNum_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.DeviceNum = decimal.ToInt32(numDeviceNum.Value);
                item.SubItems[4].Text = numDeviceNum.Value.ToString();
                OnConfigChanged();
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.Name = txtName.Text;
                item.SubItems[5].Text = txtName.Text;
                OnConfigChanged();
            }
        }

        private void cbDriver_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.Driver = cbDriver.Text;
                item.SubItems[6].Text = cbDriver.Text;
                OnConfigChanged();
            }
        }

        private void numNumAddress_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.NumAddress = decimal.ToInt32(numNumAddress.Value);
                item.SubItems[7].Text = numNumAddress.Value.ToString();
                OnConfigChanged();
            }
        }

        private void txtStrAddress_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.StrAddress = txtStrAddress.Text;
                item.SubItems[8].Text = txtStrAddress.Text;
                OnConfigChanged();
            }
        }

        private void numTimeout_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.PollingOptions.Timeout = decimal.ToInt32(numTimeout.Value);
                item.SubItems[9].Text = numTimeout.Value.ToString();
                OnConfigChanged();
            }
        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.PollingOptions.Delay = decimal.ToInt32(numDelay.Value);
                item.SubItems[10].Text = numDelay.Value.ToString();
                OnConfigChanged();
            }
        }

        private void dtpTime_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.PollingOptions.Time = 
                    new TimeSpan(dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second);
                item.SubItems[11].Text = deviceConfig.PollingOptions.Time.ToString();
                OnConfigChanged();
            }
        }

        private void dtpPeriod_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.PollingOptions.Period = 
                    new TimeSpan(dtpPeriod.Value.Hour, dtpPeriod.Value.Minute, dtpPeriod.Value.Second);
                item.SubItems[12].Text = deviceConfig.PollingOptions.Period.ToString();
                OnConfigChanged();
            }
        }

        private void txtCmdLine_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DeviceConfig deviceConfig))
            {
                deviceConfig.PollingOptions.CmdLine = txtCmdLine.Text;
                item.SubItems[13].Text = txtCmdLine.Text;
                OnConfigChanged();
            }
        }

        private void btnDeviceProperties_Click(object sender, EventArgs e)
        {
            // show device properties
            if (GetSelectedItem(out _, out DeviceConfig deviceConfig) &&
                GetDeviceView(deviceConfig) is DeviceView deviceView)
            {
                if (deviceView.CanShowProperties)
                {
                    if (deviceView.ShowProperties() || deviceView.DeviceConfigModified)
                    {
                        DisplayDevice(deviceConfig);
                        OnConfigChanged();
                    }

                    if (deviceView.LineConfigModified)
                    {
                        OnLineConfigChanged();
                    }
                }
                else
                {
                    ScadaUiUtils.ShowInfo(ExtensionPhrases.NoDeviceProperties);
                }
            }
        }

        private void btnResetPollingOptions_Click(object sender, EventArgs e)
        {
            // set polling options to default
            if (GetSelectedItem(out _, out DeviceConfig deviceConfig) &&
                GetDeviceView(deviceConfig) is DeviceView deviceView)
            {
                PollingOptions pollingOptions = deviceView.GetPollingOptions();
                chkPollOnCmd.Checked = pollingOptions.PollOnCmd;
                numTimeout.SetValue(pollingOptions.Timeout);
                numDelay.SetValue(pollingOptions.Delay);
                dtpTime.SetTime(pollingOptions.Time);
                dtpPeriod.SetTime(pollingOptions.Period);
                txtCmdLine.Text = pollingOptions.CmdLine;
                ReplaceDeviceOptions(deviceConfig.PollingOptions.CustomOptions, pollingOptions.CustomOptions);
                OnConfigChanged();
            }
        }
    }
}
