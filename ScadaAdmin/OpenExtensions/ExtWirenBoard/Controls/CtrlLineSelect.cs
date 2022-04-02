// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Forms;
using System.Data;

namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
{
    /// <summary>
    /// Represents a control for selecting a communication line.
    /// <para>Представляет элемент управления для выбора линии связи.</para>
    /// </summary>
    internal partial class CtrlLineSelect : UserControl
    {
        /// <summary>
        /// The required channel type of a selected communication line.
        /// </summary>
        private const string ChannelTypeCode = "MqttClient";

        private readonly IAdminContext adminContext;      // the Administrator context
        private readonly ScadaProject project;            // the project under development
        private readonly RecentSelection recentSelection; // the recently selected parameters


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private CtrlLineSelect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLineSelect(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));
        }


        /// <summary>
        /// Gets the selected instance.
        /// </summary>
        public ProjectInstance Instance => cbInstance.SelectedItem as ProjectInstance;

        /// <summary>
        /// Gets the selected communication line.
        /// </summary>
        public LineConfig Line => cbLine.SelectedItem as LineConfig;


        /// <summary>
        /// Fills the combo box with the instances.
        /// </summary>
        private void FillInstanceList()
        {
            cbInstance.ValueMember = "Name";
            cbInstance.DisplayMember = "Name";
            cbInstance.DataSource = project.Instances;

            try
            {
                if (!string.IsNullOrEmpty(recentSelection.InstanceName))
                    cbInstance.SelectedValue = recentSelection.InstanceName;
            }
            catch
            {
                if (cbInstance.Items.Count > 0)
                    cbInstance.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Fills the combo box with the communication lines.
        /// </summary>
        private void FillLineList()
        {
            if (cbInstance.SelectedItem is ProjectInstance instance &&
                instance.CommApp.Enabled)
            {
                if (!instance.LoadAppConfig(out string errMsg))
                    adminContext.ErrLog.HandleError(errMsg);

                List<LineConfig> sourceLines = instance.CommApp.AppConfig.Lines;
                List<LineConfig> lines = new(sourceLines.Count);
                lines.AddRange(sourceLines.OrderBy(line => line.Name));

                cbLine.ValueMember = "CommLineNum";
                cbLine.DisplayMember = "Name";
                cbLine.DataSource = lines;

                try 
                {
                    if (recentSelection.CommLineNum > 0)
                    {
                        cbLine.SelectedValue = recentSelection.CommLineNum;
                    }
                    else
                    {
                        // select MQTT line
                        for (int i = 0, cnt = lines.Count; i < cnt; i++)
                        {
                            if (lines[i].Channel.TypeCode == ChannelTypeCode)
                            {
                                cbLine.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                catch 
                {
                    if (cbLine.Items.Count > 0)
                        cbLine.SelectedIndex = 0;
                }
            }
            else
            {
                cbLine.DataSource = null;
                cbLine.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Retrieves an IP address from the communication line configuration.
        /// </summary>
        private void RetrieveWirenBoardIP()
        {
            if (cbLine.SelectedItem is LineConfig line && line.Channel.TypeCode == ChannelTypeCode)
            {
                string server = line.Channel.CustomOptions.GetValueAsString("Server").Trim().ToLowerInvariant();
                txtWirenBoardIP.Text = server == "localhost" || server == "127.0.0.1"
                    ? recentSelection.WirenBoardIP
                    : server;
            }
            else
            {
                txtWirenBoardIP.Text = "";
            }
        }


        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            cbInstance.Select();
        }

        /// <summary>
        /// Validates the control.
        /// </summary>
        public bool ValidateControl()
        {
            if (cbLine.SelectedItem is not LineConfig line)
            {
                ScadaUiUtils.ShowError(ExtensionPhrases.CommLineRequired);
                return false;
            }
            else if (line.Channel.TypeCode != ChannelTypeCode)
            {
                ScadaUiUtils.ShowError(ExtensionPhrases.MqttLineRequired);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtWirenBoardIP.Text))
            {
                ScadaUiUtils.ShowError(ExtensionPhrases.WirenBoardIpRequired);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Remembers the selected values.
        /// </summary>
        public void RememberRecentSelection()
        {
            recentSelection.InstanceName = cbInstance.Text;
            recentSelection.CommLineNum = cbLine.SelectedValue is int commLineNum ? commLineNum : 0;
            recentSelection.WirenBoardIP = txtWirenBoardIP.Text;
        }

        /// <summary>
        /// Gets the MQTT connection options.
        /// </summary>
        public MqttConnectionOptions GetMqttConnectionOptions()
        {
            return cbLine.SelectedItem is LineConfig line
                ? new(line.Channel.CustomOptions) { Server = txtWirenBoardIP.Text }
                : new MqttConnectionOptions();
        }


        private void CtrlLineSelect_Load(object sender, EventArgs e)
        {
            FillInstanceList();
        }

        private void cbInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillLineList();
        }

        private void cbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            RetrieveWirenBoardIP();
        }
    }
}
