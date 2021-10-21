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
    /// Represents a form for editing UDP channel options.
    /// <para>Представляет форму для редактирования параметров канала UDP.</para>
    /// </summary>
    internal partial class FrmUdpChannelOptions : Form
    {
        private readonly ChannelConfig channelConfig; // the communication channel configuration
        private readonly UdpChannelOptions options;   // the communication channel options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmUdpChannelOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmUdpChannelOptions(ChannelConfig channelConfig)
            : this()
        {
            this.channelConfig = channelConfig ?? throw new ArgumentNullException(nameof(channelConfig));
            options = new UdpChannelOptions(channelConfig.CustomOptions);
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            cbBehavior.SelectedIndex = (int)options.Behavior;
            cbDeviceMapping.SelectedIndex = (int)options.DeviceMapping;
            numLocalUdpPort.SetValue(options.LocalUdpPort);
            numRemoteUdpPort.SetValue(options.RemoteUdpPort);
            txtRemoteIpAddress.Text = options.RemoteIpAddress;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.Behavior = (ChannelBehavior)cbBehavior.SelectedIndex;
            options.DeviceMapping = (DeviceMapping)cbDeviceMapping.SelectedIndex;
            options.LocalUdpPort = Convert.ToInt32(numLocalUdpPort.Value);
            options.RemoteUdpPort = Convert.ToInt32(numRemoteUdpPort.Value);
            options.RemoteIpAddress = txtRemoteIpAddress.Text;

            options.AddToOptionList(channelConfig.CustomOptions);
        }


        private void FrmCommUdpProps_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, toolTip);
            OptionsToControls();
        }

        private void cbBehavior_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDeviceMapping.Enabled = cbBehavior.SelectedIndex == (int)ChannelBehavior.Slave;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
