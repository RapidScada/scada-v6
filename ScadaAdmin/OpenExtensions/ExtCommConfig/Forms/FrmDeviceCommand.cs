// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Lang;
using Scada.Agent;
using Scada.Comm;
using Scada.Comm.Config;
using Scada.Data.Models;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;
using System;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for sending commands to a device.
    /// <para>Представляет форму для отправки команд КП.</para>
    /// </summary>
    public partial class FrmDeviceCommand : Form
    {
        private readonly ILog log;                   // the application log
        private readonly DeviceConfig deviceConfig;  // the device configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceCommand()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceCommand(ILog log, DeviceConfig deviceConfig)
            : this()
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.deviceConfig = deviceConfig ?? throw new ArgumentNullException(nameof(deviceConfig));
            AgentClient = null;

            FormTranslator.Translate(this, GetType().FullName);
            Text = string.Format(Text, CommUtils.GetDeviceTitle(deviceConfig));
            rbNumVal.Checked = true;
        }


        /// <summary>
        /// Gets or sets the client of the Agent service.
        /// </summary>
        public IAgentClient AgentClient { get; set; }


        /// <summary>
        /// Creates a telecontrol command.
        /// </summary>
        private bool CreateCommand(out TeleCommand cmd)
        {
            TeleCommand InitCmd()
            {
                return new TeleCommand
                {
                    CreationTime = DateTime.UtcNow,
                    CmdNum = Convert.ToInt32(numCmdNum.Value),
                    CmdCode = txtCmdCode.Text
                };
            }

            if (rbNumVal.Checked)
            {
                if (ScadaUtils.TryParseDouble(txtCmdVal.Text, out double cmdVal))
                {
                    cmd = InitCmd();
                    cmd.CmdVal = cmdVal;
                    return true;
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.RealRequired);
                }
            }
            else if (rbStrData.Checked)
            {
                cmd = InitCmd();
                cmd.CmdData = TeleCommand.StringToCmdData(txtCmdData.Text);
                return true;
            }
            else if (rbHexData.Checked)
            {
                if (ScadaUtils.HexToBytes(txtCmdData.Text, out byte[] cmdData, true))
                {
                    cmd = InitCmd();
                    cmd.CmdData = cmdData;
                    return true;
                }
                else
                {
                    ScadaUiUtils.ShowError(CommonPhrases.NotHexadecimal);
                }
            }

            cmd = null;
            return false;
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        private bool SendCommand(TeleCommand cmd)
        {
            try
            {
                AgentClient.SendCommand(ServiceApp.Comm, cmd);
                return true;
            }
            catch (Exception ex)
            {
                log.HandleError(ex, ExtensionPhrases.SendCommandError);
                return false;
            }
        }


        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Checked)
            {
                if (radioButton == rbNumVal)
                {
                    pnlNumVal.Visible = true;
                    txtCmdData.Visible = false;
                }
                else
                {
                    pnlNumVal.Visible = false;
                    txtCmdData.Visible = true;
                }
            }
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            txtCmdVal.Text = "0";
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            txtCmdVal.Text = "1";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (AgentClient == null)
                ScadaUiUtils.ShowError(AdminPhrases.AgentNotEnabled);
            else if (CreateCommand(out TeleCommand cmd) && SendCommand(cmd))
                DialogResult = DialogResult.OK;
        }
    }
}
