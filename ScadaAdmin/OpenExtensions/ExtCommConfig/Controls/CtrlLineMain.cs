// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control for editing main communication line options.
    /// <para>Представляет элемент управления для редактирования основных параметров линии связи.</para>
    /// </summary>
    public partial class CtrlLineMain : UserControl
    {
        private bool changing; // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLineMain()
        {
            InitializeComponent();
            changing = false;
        }


        /// <summary>
        /// Raises a ConfigChanged event.
        /// </summary>
        private void OnConfigChanged()
        {
            ConfigChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        public void ConfigToControls(LineConfig lineConfig)
        {
            if (lineConfig == null)
                throw new ArgumentNullException(nameof(lineConfig));

            changing = true;

            chkActive.Checked = lineConfig.Active;
            chkIsBound.Checked = lineConfig.IsBound;
            numCommLineNum.SetValue(lineConfig.CommLineNum);
            txtName.Text = lineConfig.Name;

            LineOptions lineOptions = lineConfig.LineOptions;
            numReqRetries.SetValue(lineOptions.ReqRetries);
            numCycleDelay.SetValue(lineOptions.CycleDelay);
            chkCmdEnabled.Checked = lineOptions.CmdEnabled;
            chkPollAfterCmd.Checked = lineOptions.PollAfterCmd;
            chkDetailedLog.Checked = lineOptions.DetailedLog;

            changing = false;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        public void ControlsToConfig(LineConfig lineConfig)
        {
            if (lineConfig == null)
                throw new ArgumentNullException(nameof(lineConfig));

            lineConfig.Active = chkActive.Checked;
            lineConfig.IsBound = chkIsBound.Checked;
            lineConfig.CommLineNum = Convert.ToInt32(numCommLineNum.Value);
            lineConfig.Name = txtName.Text;

            LineOptions lineOptions = lineConfig.LineOptions;
            lineOptions.ReqRetries = Convert.ToInt32(numReqRetries.Value);
            lineOptions.CycleDelay = Convert.ToInt32(numCycleDelay.Value);
            lineOptions.CmdEnabled = chkCmdEnabled.Checked;
            lineOptions.PollAfterCmd = chkPollAfterCmd.Checked;
            lineOptions.DetailedLog = chkDetailedLog.Checked;
        }


        /// <summary>
        /// Occurs when the configuration changes.
        /// </summary>
        public event EventHandler ConfigChanged;

        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                OnConfigChanged();
        }
    }
}
