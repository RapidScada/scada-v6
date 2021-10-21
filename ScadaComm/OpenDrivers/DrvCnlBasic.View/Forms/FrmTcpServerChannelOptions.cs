// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvCnlBasic.View.Forms
{
    /// <summary>
    /// Represents a form for editing TCP server options.
    /// <para>Представляет форму для редактирования параметров TCP-сервера.</para>
    /// </summary>
    internal partial class FrmTcpServerChannelOptions : Form
    {
        private readonly ChannelConfig channelConfig;     // the communication channel configuration
        private readonly TcpServerChannelOptions options; // the communication channel options

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTcpServerChannelOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTcpServerChannelOptions(ChannelConfig channelConfig)
            : this()
        {
            this.channelConfig = channelConfig ?? throw new ArgumentNullException(nameof(channelConfig));
            options = new TcpServerChannelOptions(channelConfig.CustomOptions);
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            cbBehavior.SelectedIndex = (int)options.Behavior;
            cbConnectionMode.SelectedIndex = (int)options.ConnectionMode;
            cbDeviceMapping.SelectedIndex = (int)options.DeviceMapping;
            numTcpPort.SetValue(options.TcpPort);
            numClientLifetime.SetValue(options.ClientLifetime);
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.Behavior = (ChannelBehavior)cbBehavior.SelectedIndex;
            options.ConnectionMode = (ConnectionMode)cbConnectionMode.SelectedIndex;
            options.DeviceMapping = (DeviceMapping)cbDeviceMapping.SelectedIndex;
            options.TcpPort = Convert.ToInt32(numTcpPort.Value);
            options.ClientLifetime = Convert.ToInt32(numClientLifetime.Value);

            options.AddToOptionList(channelConfig.CustomOptions);
        }


        private void FrmCommTcpServerProps_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, toolTip);
            OptionsToControls();
        }

        private void cbConnectionMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDeviceMapping.Enabled = cbConnectionMode.SelectedIndex == (int)ConnectionMode.Individual;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
