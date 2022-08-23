// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvCnlBasic.View.Forms
{
    /// <summary>
    /// Represents a form for editing TCP client options.
    /// <para>Представляет форму для редактирования параметров TCP-клиента.</para>
    /// </summary>
    internal partial class FrmTcpClientChannelOptions : Form
    {
        private readonly ChannelConfig channelConfig;     // the communication channel configuration
        private readonly TcpClientChannelOptions options; // the communication channel options

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTcpClientChannelOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTcpClientChannelOptions(ChannelConfig channelConfig)
            : this()
        {
            this.channelConfig = channelConfig ?? throw new ArgumentNullException(nameof(channelConfig));
            options = new TcpClientChannelOptions(channelConfig.CustomOptions);
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            cbBehavior.SelectedIndex = (int)options.Behavior;
            cbConnectionMode.SelectedIndex = (int)options.ConnectionMode;
            txtHost.Text = options.Host;
            numTcpPort.SetValue(options.TcpPort);
            numReconnectAfter.SetValue(options.ReconnectAfter);
            chkStayConnected.Checked = options.StayConnected;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.Behavior = (ChannelBehavior)cbBehavior.SelectedIndex;
            options.ConnectionMode = (ConnectionMode)cbConnectionMode.SelectedIndex;
            options.Host = txtHost.Text;
            options.TcpPort = Convert.ToInt32(numTcpPort.Value);
            options.ReconnectAfter = Convert.ToInt32(numReconnectAfter.Value);
            options.StayConnected = chkStayConnected.Checked;

            options.AddToOptionList(channelConfig.CustomOptions);
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            if ((ConnectionMode)cbConnectionMode.SelectedIndex == ConnectionMode.Shared &&
                string.IsNullOrWhiteSpace(txtHost.Text))
            {
                ScadaUiUtils.ShowError(DriverPhrases.HostRequired);
                return false;
            }

            return true;
        }


        private void FrmCommTcpClientProps_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, new FormTranslatorOptions { ToolTip = toolTip });
            OptionsToControls();
        }

        private void cbConnectionMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtHost.Enabled = cbConnectionMode.SelectedIndex == (int)ConnectionMode.Shared;
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
