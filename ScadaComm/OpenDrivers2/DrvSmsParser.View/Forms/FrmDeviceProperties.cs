// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvSmsParser.View.Forms
{
    /// <summary>
    /// Represents a form for editing device properties.
    /// <para>Представляет форму для редактирования свойств устройства.</para>
    /// </summary>
    public partial class FrmDeviceProperties : Form
    {
        private readonly AppDirs appDirs;
        private readonly LineConfig lineConfig;
        private readonly DeviceConfig deviceConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceProperties()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceProperties(AppDirs appDirs, LineConfig lineConfig, DeviceConfig deviceConfig)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.lineConfig = lineConfig ?? throw new ArgumentNullException(nameof(lineConfig));
            this.deviceConfig = deviceConfig ?? throw new ArgumentNullException(nameof(deviceConfig));
        }


        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            numDataLifetime.SetValue(lineConfig.CustomOptions.GetValueAsInt(OptionName.DataLifetime));
            txtTemplateFileName.Text = deviceConfig.PollingOptions.CmdLine;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            lineConfig.CustomOptions[OptionName.DataLifetime] = numDataLifetime.Value.ToString();
            deviceConfig.PollingOptions.CmdLine = txtTemplateFileName.Text;
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            if (!File.Exists(GetTemplatePath()))
            {
                ScadaUiUtils.ShowError(DriverPhrases.TemplateNotExists);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the short name of the template file if it is located in the configuration directory.
        /// </summary>
        private bool GetTemplateShortFileName(string fileName, out string shortFileName)
        {
            if (fileName.StartsWith(appDirs.ConfigDir))
            {
                shortFileName = fileName[appDirs.ConfigDir.Length..];
                return true;
            }
            else
            {
                ScadaUiUtils.ShowError(DriverPhrases.ConfigDirRequired, appDirs.ConfigDir);
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
        private void EditDeviceTemplate()
        {
            FrmDeviceTemplate frmDeviceTemplate = new() { FileName = GetTemplatePath() };
            frmDeviceTemplate.ShowDialog();
        }


        private void FrmDeviceProperties_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, new FormTranslatorOptions { ToolTip = toolTip });
            openFileDialog.SetFilter(CommonPhrases.XmlFileFilter);
            saveFileDialog.SetFilter(CommonPhrases.XmlFileFilter);

            Text = string.Format(Text, deviceConfig.DeviceNum);
            ConfigToControls();
        }

        private void btnNewTemplate_Click(object sender, EventArgs e)
        {
            saveFileDialog.InitialDirectory = appDirs.ConfigDir;
            saveFileDialog.FileName = DriverUtils.DefaultTemplateFileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK &&
                GetTemplateShortFileName(saveFileDialog.FileName, out string shortFileName))
            {
                txtTemplateFileName.Text = shortFileName;
                EditDeviceTemplate();
            }
        }

        private void btnEditTemplate_Click(object sender, EventArgs e)
        {
            EditDeviceTemplate();
        }

        private void btnBrowseTemplate_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = appDirs.ConfigDir;
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK &&
                GetTemplateShortFileName(openFileDialog.FileName, out string shortFileName))
            {
                txtTemplateFileName.Text = shortFileName;
            }
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
