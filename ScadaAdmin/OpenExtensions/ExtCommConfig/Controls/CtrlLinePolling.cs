// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Comm.Devices;
using System.IO;
using Scada.Comm.Config;
using Scada.Lang;
using Scada.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control for editing a polling sequence of a communication line options.
    /// <para>Представляет элемент управления для редактирования основных последовательности опроса линии связи.</para>
    /// </summary>
    public partial class CtrlLinePolling : UserControl
    {
        private IAdminContext adminContext; // the Administrator context
        private bool changing;              // controls are being changed programmatically
        private Settings.KP deviceBuf; // buffer to copy device


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLinePolling()
        {
            InitializeComponent();
            numNumAddress.Maximum = int.MaxValue;

            SetColumnNames();
            adminContext = null;
            changing = false;
            deviceBuf = null;
            CommLine = null;
            Environment = null;
            CustomParams = null;
        }


        /// <summary>
        /// Gets or sets the communication line settings to edit.
        /// </summary>
        public Settings.CommLine CommLine { get; set; }

        /// <summary>
        /// Gets or sets the application environment.
        /// </summary>
        public CommEnvironment Environment { get; set; }

        /// <summary>
        /// Gets or sets the working copy of the custom parameters.
        /// </summary>
        public SortedList<string, string> CustomParams { get; set; }


        /// <summary>
        /// Validates the required control properties.
        /// </summary>
        private void ValidateProps()
        {
            if (CommLine == null)
                throw new InvalidOperationException("CommLine must not be null.");

            if (Environment == null)
                throw new InvalidOperationException("Environment must not be null.");

            if (CustomParams == null)
                throw new InvalidOperationException("CustomParams must not be null.");
        }

        /// <summary>
        /// Sets the column names as needed for translation.
        /// </summary>
        private void SetColumnNames()
        {
            colOrder.Name = nameof(colOrder);
            colActive.Name = nameof(colActive);
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
                numDeviceNum.SetValue(deviceConfig. DeviceNum);
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
        /// Adds an item to the list view according to the specified device.
        /// </summary>
        private void AddDeviceItem(DeviceConfig deviceConfig)
        {
            int index = 0;
            lvDevicePolling.InsertItem(CreateDeviceItem(deviceConfig, ref index), true);
            numDeviceNum.Focus();
            OnSettingsChanged();
        }

        /// <summary>
        /// Raises a SettingsChanged event.
        /// </summary>
        private void OnSettingsChanged()
        {
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises a CustomParamsChanged event.
        /// </summary>
        private void OnCustomParamsChanged()
        {
            CustomParamsChanged?.Invoke(this, EventArgs.Empty);
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
        /// Setup the controls according to the settings.
        /// </summary>
        public void SettingsToControls()
        {
            ValidateProps();
            FillDriverComboBox();

            try
            {
                lvDevicePolling.BeginUpdate();
                lvDevicePolling.Items.Clear();
                int index = 0;

                foreach (Settings.KP kp in CommLine.ReqSequence)
                {
                    lvDevicePolling.Items.Add(CreateDeviceItem(kp.Clone(), ref index));
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
        /// Sets the settings according to the controls.
        /// </summary>
        public void ControlsToSettings()
        {
            ValidateProps();
            CommLine.ReqSequence.Clear();

            foreach (ListViewItem item in lvDevicePolling.Items)
            {
                CommLine.ReqSequence.Add((Settings.KP)item.Tag);
            }
        }


        /// <summary>
        /// Occurs when the settings changes.
        /// </summary>
        public event EventHandler SettingsChanged;

        /// <summary>
        /// Occurs when the custom line parameters changes.
        /// </summary>
        public event EventHandler CustomParamsChanged;


        private void CtrlLineReqSequence_Load(object sender, EventArgs e)
        {
            SetControlsEnabled();
            btnPasteDevice.Enabled = false;
        }

        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            // add a new device
            AddDevice(new Settings.KP());
        }

        private void btnMoveUpDevice_Click(object sender, EventArgs e)
        {
            // move up the selected item
            if (lvDevicePolling.SelectedIndices.Count > 0)
            {
                int index = lvDevicePolling.SelectedIndices[0];

                if (index > 0)
                {
                    try
                    {
                        lvDevicePolling.BeginUpdate();
                        ListViewItem item = lvDevicePolling.Items[index];
                        ListViewItem prevItem = lvDevicePolling.Items[index - 1];

                        lvDevicePolling.Items.RemoveAt(index);
                        lvDevicePolling.Items.Insert(index - 1, item);

                        item.Text = index.ToString();
                        prevItem.Text = (index + 1).ToString();
                    }
                    finally
                    {
                        lvDevicePolling.EndUpdate();
                        lvDevicePolling.Focus();
                    }
                }
            }
        }

        private void btnMoveDownDevice_Click(object sender, EventArgs e)
        {
            // move down the selected item
            if (lvDevicePolling.SelectedIndices.Count > 0)
            {
                int index = lvDevicePolling.SelectedIndices[0];

                if (index < lvDevicePolling.Items.Count - 1)
                {
                    try
                    {
                        lvDevicePolling.BeginUpdate();
                        ListViewItem item = lvDevicePolling.Items[index];
                        ListViewItem nextItem = lvDevicePolling.Items[index + 1];

                        lvDevicePolling.Items.RemoveAt(index);
                        lvDevicePolling.Items.Insert(index + 1, item);

                        item.Text = (index + 2).ToString();
                        nextItem.Text = (index + 1).ToString();
                    }
                    finally
                    {
                        lvDevicePolling.EndUpdate();
                        lvDevicePolling.Focus();
                    }
                }
            }
        }

        private void btnDeleteDevice_Click(object sender, EventArgs e)
        {
            if (lvDevicePolling.SelectedIndices.Count > 0)
            {
                try
                {
                    // delete the selected device
                    lvDevicePolling.BeginUpdate();
                    int index = lvDevicePolling.SelectedIndices[0];
                    lvDevicePolling.Items.RemoveAt(index);

                    if (lvDevicePolling.Items.Count > 0)
                    {
                        // select an item
                        if (index >= lvDevicePolling.Items.Count)
                            index = lvDevicePolling.Items.Count - 1;
                        lvDevicePolling.Items[index].Selected = true;

                        // update item numbers
                        for (int i = index, cnt = lvDevicePolling.Items.Count; i < cnt; i++)
                        {
                            lvDevicePolling.Items[i].Text = (i + 1).ToString();
                        }
                    }
                }
                finally
                {
                    lvDevicePolling.EndUpdate();
                    lvDevicePolling.Focus();
                    OnSettingsChanged();
                }
            }
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
            if (GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                btnPasteDevice.Enabled = true;
                deviceBuf = kp.Clone();
            }

            lvDevicePolling.Focus();
        }

        private void btnPasteDevice_Click(object sender, EventArgs e)
        {
            // paste the copied device
            if (deviceBuf == null)
                lvDevicePolling.Focus();
            else
                AddDevice(deviceBuf.Clone());
        }

        private void lvReqSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            // display the selected item properties
            changing = true;

            Settings.KP kp = lvDevicePolling.SelectedItems.Count > 0 ?
                (Settings.KP)lvDevicePolling.SelectedItems[0].Tag : null;

            DisplayDevice(kp);
            SetControlsEnabled();
            changing = false;
        }

        private void chkDeviceActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Active = chkActive.Checked;
                item.SubItems[1].Text = chkActive.Checked ? "V" : " ";
                OnSettingsChanged();
            }
        }

        private void chkDeviceBound_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Bind = chkIsBound.Checked;
                item.SubItems[2].Text = chkIsBound.Checked ? "V" : " ";
                OnSettingsChanged();
            }
        }

        private void numDeviceNumber_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Number = decimal.ToInt32(numDeviceNum.Value);
                item.SubItems[3].Text = numDeviceNum.Value.ToString();
                OnSettingsChanged();
            }
        }

        private void txtDeviceName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Name = txtName.Text;
                item.SubItems[4].Text = txtName.Text;
                OnSettingsChanged();
            }
        }

        private void cbDeviceDll_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Dll = cbDriver.Text;
                item.SubItems[5].Text = cbDriver.Text;
                OnSettingsChanged();
            }
        }

        private void numDeviceAddress_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Address = decimal.ToInt32(numNumAddress.Value);
                item.SubItems[6].Text = numNumAddress.Value.ToString();
                OnSettingsChanged();
            }
        }

        private void txtDeviceCallNum_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.CallNum = txtStrAddress.Text;
                item.SubItems[7].Text = txtStrAddress.Text;
                OnSettingsChanged();
            }
        }

        private void numDeviceTimeout_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Timeout = decimal.ToInt32(numTimeout.Value);
                item.SubItems[8].Text = numTimeout.Value.ToString();
                OnSettingsChanged();
            }
        }

        private void numDeviceDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Delay = decimal.ToInt32(numDelay.Value);
                item.SubItems[9].Text = numDelay.Value.ToString();
                OnSettingsChanged();
            }
        }

        private void dtpDeviceTime_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Time = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day,
                    dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second);
                item.SubItems[10].Text = kp.Time.ToString("T", Localization.Culture);
                OnSettingsChanged();
            }
        }

        private void dtpDevicePeriod_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.Period = new TimeSpan(dtpPeriod.Value.Hour, dtpPeriod.Value.Minute, 
                    dtpPeriod.Value.Second);
                item.SubItems[11].Text = kp.Period.ToString();
                OnSettingsChanged();
            }
        }

        private void txtDeviceCmdLine_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                kp.CmdLine = txtCmdLine.Text;
                item.SubItems[12].Text = txtCmdLine.Text;
                OnSettingsChanged();
            }
        }

        private void btnResetReqParams_Click(object sender, EventArgs e)
        {
            // set the request parameters of the selected device by default
            if (GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                if (Environment.TryGetKPView(kp, true, null, out KPView kpView, out string errMsg))
                {
                    KPReqParams reqParams = kpView.DefaultReqParams;
                    numTimeout.SetValue(reqParams.Timeout);
                    numDelay.SetValue(reqParams.Delay);
                    dtpTime.SetTime(reqParams.Time);
                    dtpPeriod.SetTime(reqParams.Period);
                    txtCmdLine.Text = reqParams.CmdLine;
                    OnSettingsChanged();
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }
        }

        private void btnDeviceProps_Click(object sender, EventArgs e)
        {
            // show the properties of the selected device
            if (GetSelectedItem(out ListViewItem item, out Settings.KP kp))
            {
                if (Environment.TryGetKPView(kp, false, CustomParams, out KPView kpView, out string errMsg))
                {
                    if (kpView.CanShowProps)
                    {
                        kpView.ShowProps();

                        if (kpView.KPProps.Modified)
                        {
                            txtCmdLine.Text = kpView.KPProps.CmdLine;
                            OnCustomParamsChanged();
                            OnSettingsChanged();
                        }
                    }
                    else
                    {
                        ScadaUiUtils.ShowWarning(CommShellPhrases.NoDeviceProps);
                    }
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }
        }
    }
}
