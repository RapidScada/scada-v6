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
            txtClientID.Text = options.ClientID;
            txtUsername.Text = options.Username;
            txtPassword.Text = options.Password;
            numTimeout.SetValue(options.Timeout);
            cbProtocolVersion.SelectedIndex = (int)options.ProtocolVersion;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.Server = txtServer.Text;
            options.Port = Convert.ToInt32(numPort.Value);
            options.ClientID = txtClientID.Text;
            options.Username = txtUsername.Text;
            options.Password = txtPassword.Text;
            options.Timeout = Convert.ToInt32(numTimeout.Value);
            options.ProtocolVersion = (MqttProtocolVersion)cbProtocolVersion.SelectedIndex;

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
    }
}
