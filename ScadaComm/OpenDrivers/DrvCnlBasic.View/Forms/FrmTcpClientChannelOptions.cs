// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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


        private void FrmCommTcpClientProps_Load(object sender, EventArgs e)
        {
            /*
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Channels.FrmCommTcpClientProps", toolTip);

            // инициализация настроек канала связи
            settings = new CommTcpClientLogic.Settings();
            settings.Init(commCnlParams, false);

            // установка элементов управления в соответствии с параметрами канала связи
            cbBehavior.Text = settings.Behavior.ToString();
            cbConnMode.SetSelectedItem(settings.ConnMode, 
                new Dictionary<string, int>() { { "Individual", 0 }, { "Shared", 1 } });
            txtHost.Text = settings.Host;
            numTcpPort.SetValue(settings.TcpPort);
            numReconnectAfter.SetValue(settings.ReconnectAfter);
            chkStayConnected.Checked = settings.StayConnected;

            modified = false;*/
        }

        private void control_Changed(object sender, EventArgs e)
        {
        }

        private void cbConnMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtHost.Enabled = cbConnMode.SelectedIndex == 1; // Shared
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            /*CommTcpChannelLogic.ConnectionModes newConnMode = 
                (CommTcpChannelLogic.ConnectionModes)cbConnMode.GetSelectedItem(
                    new Dictionary<int, object>() {
                        { 0, CommTcpChannelLogic.ConnectionModes.Individual },
                        { 1, CommTcpChannelLogic.ConnectionModes.Shared } });

            if (newConnMode == CommTcpChannelLogic.ConnectionModes.Shared &&
                string.IsNullOrWhiteSpace(txtHost.Text)) // проверка настроек
            {
                ScadaUiUtils.ShowError(CommPhrases.HostRequired);
            }
            else
            {
                // изменение настроек в соответствии с элементами управления
                if (modified)
                {
                    settings.Behavior = cbBehavior.ParseText<CommChannelLogic.OperatingBehaviors>();
                    settings.ConnMode = newConnMode;
                    settings.Host = txtHost.Text;
                    settings.TcpPort = Convert.ToInt32(numTcpPort.Value);
                    settings.ReconnectAfter = Convert.ToInt32(numReconnectAfter.Value);
                    settings.StayConnected = chkStayConnected.Checked;

                    settings.SetCommCnlParams(commCnlParams);
                }

                DialogResult = DialogResult.OK;
            }*/
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
