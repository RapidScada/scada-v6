// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet.Formatter;
using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Forms;
using Scada.Lang;
using System.Text;

namespace Scada.Comm.Drivers.DrvCnlMqtt.View.Forms
{
    /// <summary>
    /// Represents a form for editing MQTT client options.
    /// <para>Представляет форму для редактирования параметров MQTT-клиента.</para>
    /// </summary>
    public partial class FrmMqttClientChannelOptions : Form
    {
        private readonly ChannelConfig channelConfig;   // the communication channel configuration
        private readonly MqttConnectionOptions options; // the connection options

        // Maps the combo box index to the corresponding MQTTnet enum value which is NOT contiguous.
        private static readonly MqttProtocolVersion[] ProtocolVersionByIndex =
        [
            MqttProtocolVersion.Unknown,
            MqttProtocolVersion.V310,
            MqttProtocolVersion.V311,
            MqttProtocolVersion.V500
        ];


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmMqttClientChannelOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmMqttClientChannelOptions(ChannelConfig channelConfig)
            : this()
        {
            this.channelConfig = channelConfig ?? throw new ArgumentNullException(nameof(channelConfig));
            options = new MqttConnectionOptions(channelConfig.CustomOptions);
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            txtServer.Text = options.Server;
            numPort.SetValue(options.Port);
            numTimeout.SetValue(options.Timeout);
            chkUseTls.Checked = options.UseTls;
            txtClientID.Text = options.ClientID;
            txtUsername.Text = options.Username;
            txtPassword.Text = options.Password;
            int versionIndex = Array.IndexOf(ProtocolVersionByIndex, options.ProtocolVersion);
            cbProtocolVersion.SelectedIndex = versionIndex >= 0 ? versionIndex : 0;
            txtCaCertFile.Text = options.CaCertFile;
            txtClientCertFile.Text = options.ClientCertFile;
            txtClientCertPassword.Text = options.ClientCertPassword;
            chkAllowUntrustedCertificates.Checked = options.AllowUntrustedCertificates;
            chkIgnoreCertificateRevocationErrors.Checked = options.IgnoreCertificateRevocationErrors;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.Server = txtServer.Text;
            options.Port = Convert.ToInt32(numPort.Value);
            options.Timeout = Convert.ToInt32(numTimeout.Value);
            options.UseTls = chkUseTls.Checked;
            options.ClientID = txtClientID.Text;
            options.Username = txtUsername.Text;
            options.Password = txtPassword.Text;
            options.ProtocolVersion = ProtocolVersionByIndex[cbProtocolVersion.SelectedIndex];
            options.CaCertFile = txtCaCertFile.Text;
            options.ClientCertFile = txtClientCertFile.Text;
            options.ClientCertPassword = txtClientCertPassword.Text;
            options.AllowUntrustedCertificates = chkAllowUntrustedCertificates.Checked;
            options.IgnoreCertificateRevocationErrors = chkIgnoreCertificateRevocationErrors.Checked;

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

            if (!string.IsNullOrEmpty(txtCaCertFile.Text) && !File.Exists(txtCaCertFile.Text))
                sbError.AppendError(lblCaCertFile, CommonPhrases.FileNotFound);

            if (!string.IsNullOrEmpty(txtClientCertFile.Text) && !File.Exists(txtClientCertFile.Text))
                sbError.AppendError(lblClientCertFile, CommonPhrases.FileNotFound);

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

        private void btnCaCertFileBrowse_Click(object sender, EventArgs e)
        {
            openCertFileDialog.FileName = txtCaCertFile.Text;

            if (openCertFileDialog.ShowDialog() == DialogResult.OK)
                txtCaCertFile.Text = openCertFileDialog.FileName;
        }

        private void btnClientCertFileBrowse_Click(object sender, EventArgs e)
        {
            openCertFileDialog.FileName = txtClientCertFile.Text;

            if (openCertFileDialog.ShowDialog() == DialogResult.OK)
                txtClientCertFile.Text = openCertFileDialog.FileName;
        }
    }
}
