// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Data.Models;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvDsScadaServer.View.Forms
{
    /// <summary>
    /// Represents a form for editing data source options.
    /// <para>Представляет форму для редактирования параметров источника данных.</para>
    /// </summary>
    public partial class FrmScadaServerDSO : Form
    {
        private readonly BaseDataSet baseDataSet;           // the configuration database
        private readonly AppDirs appDirs;                   // the application directories
        private readonly DataSourceConfig dataSourceConfig; // the data source configuration
        private readonly ScadaServerDSO options;            // the data source options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmScadaServerDSO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmScadaServerDSO(BaseDataSet baseDataSet, AppDirs appDirs, DataSourceConfig dataSourceConfig)
            : this()
        {
            this.baseDataSet = baseDataSet ?? throw new ArgumentNullException(nameof(baseDataSet));
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.dataSourceConfig = dataSourceConfig ?? throw new ArgumentNullException(nameof(dataSourceConfig));
            options = new ScadaServerDSO(dataSourceConfig.CustomOptions);
        }


        /// <summary>
        /// Fills the connection combo box from a configuration file.
        /// </summary>
        private void FillConnections()
        {
            string configFileName = Path.Combine(appDirs.ConfigDir, DriverConfig.DefaultFileName);

            if (File.Exists(configFileName))
            {
                DriverConfig driverConfig = new();

                if (driverConfig.Load(configFileName, out string errMsg))
                {
                    cbConnection.Items.Clear();
                    cbConnection.Items.AddRange(driverConfig.Connections.Keys.ToArray());
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }
        }

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            chkUseDefaultConn.Checked = options.UseDefaultConn;
            cbConnection.Text = options.Connection;
            numMaxQueueSize.SetValue(options.MaxQueueSize);
            numMaxCurDataAge.SetValue(options.MaxCurDataAge);
            numDataLifetime.SetValue(options.DataLifetime);
            chkClientLogEnabled.Checked = options.ClientLogEnabled;
            txtDeviceFilter.Text = options.DeviceFilter.ToRangeString();
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.UseDefaultConn = chkUseDefaultConn.Checked;
            options.Connection = cbConnection.Text;
            options.MaxQueueSize = Convert.ToInt32(numMaxQueueSize.Value);
            options.MaxCurDataAge = Convert.ToInt32(numMaxCurDataAge.Value);
            options.DataLifetime = Convert.ToInt32(numDataLifetime.Value);
            options.ClientLogEnabled = chkClientLogEnabled.Checked;
            options.DeviceFilter.Clear();
            options.DeviceFilter.AddRange(ScadaUtils.ParseRange(txtDeviceFilter.Text, true, true));

            options.AddToOptionList(dataSourceConfig.CustomOptions);
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            if (!ScadaUtils.ParseRange(txtDeviceFilter.Text, true, true, out _))
            {
                ScadaUiUtils.ShowError(CommonPhrases.ValidRangeRequired);
                return false;
            }

            return true;
        }


        private void FrmScadaServerDSO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FillConnections();
            OptionsToControls();
        }

        private void chkUseDefaultConn_CheckedChanged(object sender, EventArgs e)
        {
            cbConnection.Enabled = !chkUseDefaultConn.Checked;
        }

        private void btnSelectDevices_Click(object sender, EventArgs e)
        {
            // show dialog to select devices
            ScadaUtils.ParseRange(txtDeviceFilter.Text, true, true, out IList<int> deviceNums);
            FrmEntitySelect frmEntitySelect = new(baseDataSet.DeviceTable) { SelectedIDs = deviceNums };

            if (frmEntitySelect.ShowDialog() == DialogResult.OK)
                txtDeviceFilter.Text = frmEntitySelect.SelectedIDs.ToRangeString();
        }

        private void btnManageConn_Click(object sender, EventArgs e)
        {
            FrmConnManager frmConnManager = new(appDirs.ConfigDir);

            if (frmConnManager.ShowDialog() == DialogResult.OK)
            {
                cbConnection.Items.Clear();
                cbConnection.Items.AddRange(frmConnManager.ConnectionNames);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
            {
                ControlsToOptions();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
