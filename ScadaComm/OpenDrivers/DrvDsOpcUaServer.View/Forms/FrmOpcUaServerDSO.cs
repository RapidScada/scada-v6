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
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvDsOpcUaServer.View.Forms
{
    /// <summary>
    /// Represents a form for editing data source options.
    /// <para>Представляет форму для редактирования параметров источника данных.</para>
    /// </summary>
    public partial class FrmOpcUaServerDSO : Form
    {
        private readonly ConfigDataset configDataset;       // the configuration database
        private readonly AppDirs appDirs;                   // the application directories
        private readonly DataSourceConfig dataSourceConfig; // the data source configuration
        private readonly OpcUaServerDSO options;            // the data source options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmOpcUaServerDSO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmOpcUaServerDSO(ConfigDataset configDataset, AppDirs appDirs, DataSourceConfig dataSourceConfig)
            : this()
        {
            this.configDataset = configDataset ?? throw new ArgumentNullException(nameof(configDataset));
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.dataSourceConfig = dataSourceConfig ?? throw new ArgumentNullException(nameof(dataSourceConfig));
            options = new OpcUaServerDSO(dataSourceConfig.CustomOptions);
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            chkAutoAccept.Checked = options.AutoAccept;
            txtUsername.Text = options.Username;
            txtPassword.Text = options.Password;
            txtConfigFileName.Text = options.ConfigFileName;
            txtDeviceFilter.Text = options.DeviceFilter.ToRangeString();
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.AutoAccept = chkAutoAccept.Checked;
            options.Username = txtUsername.Text;
            options.Password = txtPassword.Text;
            options.ConfigFileName = txtConfigFileName.Text;
            options.DeviceFilter.Clear();
            options.DeviceFilter.AddRange(ScadaUtils.ParseRange(txtDeviceFilter.Text, true, true));

            options.AddToOptionList(dataSourceConfig.CustomOptions);
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            if (!string.IsNullOrEmpty(txtConfigFileName.Text) &&
                !File.Exists(Path.Combine(appDirs.ConfigDir, txtConfigFileName.Text)))
            {
                ScadaUiUtils.ShowError(CommonPhrases.FileNotFound);
                return false;
            }

            if (!ScadaUtils.ParseRange(txtDeviceFilter.Text, true, true, out _))
            {
                ScadaUiUtils.ShowError(CommonPhrases.ValidRangeRequired);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the path of the configuration file.
        /// </summary>
        private bool ValidateConfigPath(string fileName)
        {
            if (fileName.StartsWith(appDirs.ConfigDir))
            {
                return true;
            }
            else
            {
                ScadaUiUtils.ShowError(DriverPhrases.ConfigDirRequired, appDirs.ConfigDir);
                return false;
            }
        }


        private void FrmOpcUaServerDSO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, new FormTranslatorOptions { ToolTip = toolTip });
            openFileDialog.SetFilter(CommonPhrases.XmlFileFilter);
            saveFileDialog.SetFilter(CommonPhrases.XmlFileFilter);

            OptionsToControls();
        }

        private void btnCreateConfig_Click(object sender, EventArgs e)
        {
            // show dialog to create configuration file
            saveFileDialog.InitialDirectory = appDirs.ConfigDir;
            saveFileDialog.FileName = DriverUtils.DefaultConfigFileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK &&
                ValidateConfigPath(saveFileDialog.FileName))
            {
                txtConfigFileName.Text = saveFileDialog.FileName[appDirs.ConfigDir.Length..];
                DriverUtils.WriteConfigFile(saveFileDialog.FileName, sender == btnCreateConfigWin);
            }
        }

        private void btnBrowseConfig_Click(object sender, EventArgs e)
        {
            // show dialog to select configuration file
            openFileDialog.InitialDirectory = appDirs.ConfigDir;
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK && 
                ValidateConfigPath(openFileDialog.FileName))
            {
                txtConfigFileName.Text = openFileDialog.FileName[appDirs.ConfigDir.Length..];
            }
        }

        private void btnSelectDevices_Click(object sender, EventArgs e)
        {
            // show dialog to select devices
            ScadaUtils.ParseRange(txtDeviceFilter.Text, true, true, out IList<int> deviceNums);
            FrmEntitySelect frmEntitySelect = new(configDataset.DeviceTable) { SelectedIDs = deviceNums };

            if (frmEntitySelect.ShowDialog() == DialogResult.OK)
                txtDeviceFilter.Text = frmEntitySelect.SelectedIDs.ToRangeString();
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
