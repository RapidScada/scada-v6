// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Forms;
using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvCnlBasic.View.Forms
{
    /// <summary>
    /// Represents a form for editing serial port options.
    /// <para>Представляет форму для редактирования параметров последовательного порта.</para>
    /// </summary>
    internal partial class FrmSerialPortChannelOptions : Form
    {
        private readonly ChannelConfig channelConfig;      // the communication channel configuration
        private readonly SerialPortChannelOptions options; // the communication channel options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmSerialPortChannelOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmSerialPortChannelOptions(ChannelConfig channelConfig)
            : this()
        {
            this.channelConfig = channelConfig ?? throw new ArgumentNullException(nameof(channelConfig));
            options = new SerialPortChannelOptions(channelConfig.CustomOptions);
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            cbPortName.Text = options.PortName;
            cbBaudRate.Text = options.BaudRate.ToString();
            cbDataBits.Text = options.DataBits.ToString();
            cbParity.SelectedIndex = (int)options.Parity;
            cbStopBits.SelectedIndex = (int)options.StopBits - 1;
            chkDtrEnable.Checked = options.DtrEnable;
            chkRtsEnable.Checked = options.RtsEnable;
            cbBehavior.SelectedIndex = (int)options.Behavior;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            options.PortName = cbPortName.Text;

            if (int.TryParse(cbBaudRate.Text, out int baudRate))
                options.BaudRate = baudRate;

            if (int.TryParse(cbDataBits.Text, out int dataBits))
                options.DataBits = dataBits;

            options.Parity = (Parity)cbParity.SelectedIndex;
            options.StopBits = (StopBits)(cbStopBits.SelectedIndex + 1);
            options.DtrEnable = chkDtrEnable.Checked;
            options.RtsEnable = chkRtsEnable.Checked;
            options.Behavior = (ChannelBehavior)cbBehavior.SelectedIndex;

            options.AddToOptionList(channelConfig.CustomOptions);
        }


        private void FrmCommSerialProps_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            OptionsToControls();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToOptions();
            DialogResult = DialogResult.OK;
        }
    }
}
