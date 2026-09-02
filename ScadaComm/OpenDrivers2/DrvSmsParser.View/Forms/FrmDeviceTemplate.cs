// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using DrvSmsParser.Shared.Config;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvSmsParser.View.Forms
{
    /// <summary>
    /// Represents a form for editing a device template.
    /// <para>Представляет форму для редактирования шаблона устройства.</para>
    /// </summary>
    public partial class FrmDeviceTemplate : Form
    {
        private readonly DeviceTemplate deviceTemplate;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceTemplate()
        {
            InitializeComponent();
            deviceTemplate = new DeviceTemplate();
        }


        /// <summary>
        /// Gets or sets the full file name of the template.
        /// </summary>
        public string FileName { get; set; }


        /// <summary>
        /// Loads the device template from a file.
        /// </summary>
        private void LoadDeviceTemplate()
        {
            if (!deviceTemplate.Load(FileName, out string errMsg))
            {
                ScadaUiUtils.ShowError(errMsg);
            }

            ConfigToControls();
        }

        /// <summary>
        /// Saves the device template to a file.
        /// </summary>
        private void SaveDeviceTemplate()
        {
            ControlsToConfig();

            if (!deviceTemplate.Save(FileName, out string errMsg))
            {
                ScadaUiUtils.ShowError(errMsg);
            }
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            txtTags.Lines = deviceTemplate.Tags.ToArray();
            txtScript.Text = deviceTemplate.Script.Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            deviceTemplate.Tags.Clear();
            deviceTemplate.Tags.AddRange(txtTags.Lines.Where(s => !string.IsNullOrEmpty(s)));
            deviceTemplate.Script = txtScript.Text;
        }


        private void FrmDeviceTemplate_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            LoadDeviceTemplate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveDeviceTemplate();
            DialogResult = DialogResult.OK;
        }
    }
}
