// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using Scada.Forms;
using Scada.Lang;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvModbus.View.Forms
{
    /// <summary>
    /// Represents a form for configuring device and communication line properties.
    /// <para>Представляет форму для настройки свойств устройства и линии связи.</para>
    /// </summary>
    public partial class FrmDeviceProps : Form
    {
        private readonly AppDirs appDirs;           // the application directories
        private readonly LineConfig lineConfig;     // the communication line configuration
        private readonly DeviceConfig deviceConfig; // the device configuration
        private readonly CustomUi customUi;         // the UI customization object


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceProps()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceProps(AppDirs appDirs, LineConfig lineConfig, DeviceConfig deviceConfig, CustomUi customUi)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.lineConfig = lineConfig ?? throw new ArgumentNullException(nameof(lineConfig));
            this.deviceConfig = deviceConfig ?? throw new ArgumentNullException(nameof(deviceConfig));
            this.customUi = customUi ?? throw new ArgumentNullException(nameof(customUi));
        }


        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            cbTransMode.SelectedIndex = (int)lineConfig.CustomOptions.GetValueAsEnum("TransMode", TransMode.RTU);
            txtTemplateFileName.Text = deviceConfig.PollingOptions.CmdLine;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            lineConfig.CustomOptions["TransMode"] = ((TransMode)cbTransMode.SelectedIndex).ToString();
            deviceConfig.PollingOptions.CmdLine = txtTemplateFileName.Text;
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            if (!File.Exists(GetTemplatePath()))
            {
                ScadaUiUtils.ShowError(ModbusDriverPhrases.TemplateNotExists);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the path of the device template file.
        /// </summary>
        private bool ValidateTemplatePath(string fileName, out string shortFileName)
        {
            if (fileName.StartsWith(appDirs.ConfigDir))
            {
                shortFileName = fileName[appDirs.ConfigDir.Length..];
                return true;
            }
            else
            {
                ScadaUiUtils.ShowError(ModbusDriverPhrases.ConfigDirRequired, appDirs.ConfigDir);
                shortFileName = "";
                return false;
            }
        }

        /// <summary>
        /// Gets the file path of the device template.
        /// </summary>
        private string GetTemplatePath()
        {
            return Path.Combine(appDirs.ConfigDir, txtTemplateFileName.Text);
        }

        /// <summary>
        /// Shows a form for editing the device template.
        /// </summary>
        private void EditDeviceTemplate(string fileName = "")
        {
            FrmDeviceTemplate frmDeviceTemplate = new(appDirs, customUi)
            {
                SaveOnly = true,
                FileName = fileName
            };

            frmDeviceTemplate.ShowDialog();
            fileName = frmDeviceTemplate.FileName;

            if (!string.IsNullOrEmpty(fileName) && ValidateTemplatePath(fileName, out string shortFileName))
                txtTemplateFileName.Text = shortFileName;
        }


        private void FrmDevProps_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            openFileDialog.SetFilter(CommonPhrases.XmlFileFilter);

            Text = string.Format(Text, deviceConfig.DeviceNum);
            ConfigToControls();
        }

        private void txtTemplate_TextChanged(object sender, EventArgs e)
        {
            btnEditTemplate.Enabled = !string.IsNullOrWhiteSpace(txtTemplateFileName.Text);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // show dialog to select template file
            openFileDialog.InitialDirectory = appDirs.ConfigDir;
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK &&
                ValidateTemplatePath(openFileDialog.FileName, out string shortFileName))
            {
                txtTemplateFileName.Text = shortFileName;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            EditDeviceTemplate();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditDeviceTemplate(GetTemplatePath());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
            {
                ControlsToConfig();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
