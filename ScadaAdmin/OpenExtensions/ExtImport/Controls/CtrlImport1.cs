// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Data.Entities;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Controls
{
    /// <summary>
    /// Represents a control for selecting a device when creating channels.
    /// <para>Представляет элемент управления для выбора устройства при создании каналов.</para>
    /// </summary>
    public partial class CtrlImport1 : UserControl
    {
        /// <summary>
        /// Represents an item corresponding to a device.
        /// </summary>
        private class DeviceItem
        {
            public Device DeviceEntity { get; set; }
            public DeviceConfig DeviceConfig { get; set; }
            public ProjectInstance Instance { get; set; }
            public DeviceView DeviceView { get; set; }
            public ICollection<CnlPrototype> CnlPrototypes { get; set; }
        }


        private IAdminContext adminContext;              // the Administrator context
        private ScadaProject project;                    // the project under development
        private RecentSelection recentSelection;         // the recently selected objects
        private Dictionary<int, DeviceItem> deviceItems; // the device items accessed by device number


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlImport1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets the selected device.
        /// </summary>
        public Device SelectedDevice => cbDevice.SelectedItem as Device;

        /// <summary>
        /// Gets the channel prototypes.
        /// </summary>
        public ICollection<CnlPrototype> CnlPrototypes { get; private set; }

        /// <summary>
        /// Gets a value indicating whether channels can be created.
        /// </summary>
        public bool StatusOK => SelectedDevice != null && CnlPrototypes != null && CnlPrototypes.Count > 0;


        /// <summary>
        /// Scans the Communicator configurations of all instances in the project.
        /// </summary>
        private void ScanCommSettings()
        {
            deviceItems = new Dictionary<int, DeviceItem>(project.ConfigDatabase.DeviceTable.ItemCount);

            foreach (Device deviceEntity in project.ConfigDatabase.DeviceTable.Enumerate())
            {
                deviceItems.Add(deviceEntity.DeviceNum, new DeviceItem { DeviceEntity = deviceEntity });
            }

            foreach (ProjectInstance instance in project.Instances)
            {
                if (instance.LoadAppConfig(out _) && instance.CommApp.Enabled)
                {
                    foreach (LineConfig lineConfig in instance.CommApp.AppConfig.Lines)
                    {
                        foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
                        {
                            if (deviceItems.TryGetValue(deviceConfig.DeviceNum, out DeviceItem deviceItem))
                            {
                                deviceItem.DeviceConfig = deviceConfig;
                                deviceItem.Instance = instance;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fills the combo box with the communication lines.
        /// </summary>
        private void FillCommLineList()
        {
            List<CommLine> commLines = new(project.ConfigDatabase.CommLineTable.ItemCount + 1);
            commLines.Add(new CommLine { CommLineNum = 0, Name = ExtensionPhrases.AllCommLines });
            commLines.AddRange(project.ConfigDatabase.CommLineTable.Enumerate().OrderBy(line => line.Name));
            cbCommLine.DataSource = commLines;

            try { cbCommLine.SelectedValue = recentSelection.CommLineNum; }
            catch { cbCommLine.SelectedValue = 0; }
        }

        /// <summary>
        /// Raises a SelectedDeviceChanged event.
        /// </summary>
        private void OnSelectedDeviceChanged()
        {
            SelectedDeviceChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));

            CnlPrototypes = null;
            ScanCommSettings();
            FillCommLineList();
        }
        
        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            cbCommLine.Select();
        }


        /// <summary>
        /// Occurs when the selected device changes.
        /// </summary>
        public event EventHandler SelectedDeviceChanged;


        private void cbCommLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            int commLineNum = (int)cbCommLine.SelectedValue;
            IEnumerable<Device> devices = commLineNum > 0 
                ? project.ConfigDatabase.DeviceTable.Select(new TableFilter("CommLineNum", commLineNum), true)
                : project.ConfigDatabase.DeviceTable.Enumerate();
            cbDevice.DataSource = devices.OrderBy(device => device.Name).ToList();

            try { cbDevice.SelectedValue = recentSelection.DeviceNum; }
            catch { cbDevice.SelectedValue = null; }
        }

        private void cbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            CnlPrototypes = null;

            if (cbDevice.SelectedItem is Device device)
            {
                if (deviceItems.TryGetValue(device.DeviceNum, out DeviceItem deviceItem) && 
                    deviceItem.Instance != null)
                {
                    try
                    {
                        if (deviceItem.DeviceView == null)
                        {
                            if (ExtensionUtils.GetDeviceView(adminContext, deviceItem.Instance.CommApp, 
                                    deviceItem.DeviceConfig, out DeviceView deviceView, out string errMsg))
                            {
                                deviceItem.DeviceView = deviceView;
                            }
                            else
                            {
                                throw new ScadaException(errMsg);
                            }
                        }

                        deviceItem.CnlPrototypes = deviceItem.DeviceView.GetCnlPrototypes();
                        int cnlCnt = deviceItem.CnlPrototypes?.Count ?? 0;

                        txtInfo.Text = string.Format(ExtensionPhrases.DeviceInfo, 
                            deviceItem.DeviceConfig.Driver, deviceItem.Instance.Name, cnlCnt);
                        pbStatus.Image = cnlCnt > 0 ? Properties.Resources.success : Properties.Resources.warning;
                    }
                    catch (Exception ex)
                    {
                        txtInfo.Text = ex.Message;
                        pbStatus.Image = Properties.Resources.error;
                    }
                    finally
                    {
                        CnlPrototypes = deviceItem.CnlPrototypes;
                    }
                }
                else
                {
                    txtInfo.Text = ExtensionPhrases.DeviceNotFound;
                    pbStatus.Image = Properties.Resources.warning;
                }
            }
            else
            {
                txtInfo.Text = ExtensionPhrases.NoDeviceSelected;
                pbStatus.Image = Properties.Resources.warning;
            }

            OnSelectedDeviceChanged();
        }
    }
}
