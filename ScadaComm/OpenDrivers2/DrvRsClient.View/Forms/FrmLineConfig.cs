// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvRsClient.Config;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvRsClient.View.Forms
{
    /// <summary>
    /// Represents a form for editing options common to a communication line.
    /// <para>Представляет форму для редактирования параметров, общих для линии связи.</para>
    /// </summary>
    public partial class FrmLineConfig : Form
    {
        private readonly string configFileName;
        private readonly RsClientLineConfig config;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmLineConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLineConfig(string configDir, int lineNum)
            : this()
        {
            configFileName = RsClientLineConfig.GetFullFileName(configDir, lineNum);
            config = new RsClientLineConfig();
        }


        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            chkUseDefaultConnection.Checked = config.UseDefaultConnection;
            ctrlClientConnection.Enabled = !config.UseDefaultConnection;
            ctrlClientConnection.ConnectionOptions = config.ConnectionOptions;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            config.UseDefaultConnection = chkUseDefaultConnection.Checked;
        }


        private void FrmLineConfig_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlClientConnection, ctrlClientConnection.GetType().FullName);

            if (!config.Load(configFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            ConfigToControls();
        }

        private void chkUseDefaultConnection_CheckedChanged(object sender, EventArgs e)
        {
            ctrlClientConnection.Enabled = !chkUseDefaultConnection.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToConfig();

            if (config.Save(configFileName, out string errMsg))
                DialogResult = DialogResult.OK;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
