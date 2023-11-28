// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvGoogle.Common;
using Scada.Forms;
using Scada.Lang;
using System.Text;

namespace Scada.Comm.Drivers.DrvCnlGoogle.View.Forms
{
    /// <summary>
    /// Represents a form for editing Google Cloud options.
    /// </summary>
    public partial class FrmGoogleCloudChannelOptions : Form
    {
        private readonly ChannelConfig channelConfig;   // the communication channel configuration
        private readonly GoogleCloudOptions options; // the connection options

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmGoogleCloudChannelOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmGoogleCloudChannelOptions(ChannelConfig channelConfig)
            : this()
        {
            this.channelConfig = channelConfig ?? throw new ArgumentNullException(nameof(channelConfig));
            options = new GoogleCloudOptions(channelConfig.CustomOptions);
        }

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            cbCredentialType.SelectedIndex = (int)options.CredentialType;

            txtServer.Text = options.Server;
            txtServerKey.Text = options.ServerKey;
            txtClientID.Text = options.ClientID;
            txtClientSecret.Text = options.ClientSecret;
            numTimeout.SetValue(options.Timeout);

            chkUseAdcFile.Checked = options.UseAdcFile;
            txtAdcFilePath.Text = options.AdcFilePath;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.CredentialType = (GoogleCredentialType)cbCredentialType.SelectedIndex;

            options.Server = txtServer.Text;
            options.ServerKey = txtServerKey.Text;
            options.Timeout = Convert.ToInt32(numTimeout.Value);
            options.ClientID = txtClientID.Text;
            options.ClientSecret = txtClientSecret.Text;

            options.UseAdcFile = chkUseAdcFile.Checked;
            options.AdcFilePath = txtAdcFilePath.Text;
            options.AddToOptionList(channelConfig.CustomOptions);
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            StringBuilder sbError = new();

            if (string.IsNullOrWhiteSpace(txtServer.Text))
                sbError.AppendError(lblServer, CommonPhrases.NonemptyRequired);

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


        private void FrmMqttClientChannelOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            OptionsToControls();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
            {
                ControlsToOptions();
                DialogResult = DialogResult.OK;
            }
        }

        private void cbCredentialType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
