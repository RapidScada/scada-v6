// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet.Formatter;
using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Data.Models;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Lang;
using System.Text;

namespace Scada.Comm.Drivers.DrvDsMqtt.View.Forms
{
    /// <summary>
    /// Represents a form for editing data source options.
    /// <para>Представляет форму для редактирования параметров источника данных.</para>
    /// </summary>
    public partial class FrmMqttDSO : Form
    {
        private readonly ConfigDataset configDataset;       // the configuration database
        private readonly DataSourceConfig dataSourceConfig; // the data source configuration
        private readonly MqttDSO options;                   // the data source options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmMqttDSO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmMqttDSO(ConfigDataset configDataset, DataSourceConfig dataSourceConfig)
            : this()
        {
            this.configDataset = configDataset ?? throw new ArgumentNullException(nameof(configDataset));
            this.dataSourceConfig = dataSourceConfig ?? throw new ArgumentNullException(nameof(dataSourceConfig));
            options = new MqttDSO(dataSourceConfig.CustomOptions);
        }


        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        private void OptionsToControls()
        {
            // connection
            MqttConnectionOptions connectionOptions = options.ConnectionOptions;
            txtServer.Text = connectionOptions.Server;
            numPort.SetValue(connectionOptions.Port);
            txtClientID.Text = connectionOptions.ClientID;
            txtUsername.Text = connectionOptions.Username;
            txtPassword.Text = connectionOptions.Password;
            numTimeout.SetValue(connectionOptions.Timeout);
            cbProtocolVersion.SelectedIndex = (int)connectionOptions.ProtocolVersion;

            // publishing
            PublishOptions publishOptions = options.PublishOptions;
            txtRootTopic.Text = publishOptions.RootTopic;
            txtUndefinedValue.Text = publishOptions.UndefinedValue;
            txtPublishFormat.Text = publishOptions.PublishFormat;
            cbQosLevel.SelectedIndex = publishOptions.QosLevel;
            chkRetain.Checked = publishOptions.Retain;
            numMaxQueueSize.SetValue(publishOptions.MaxQueueSize);
            numDataLifetime.SetValue(publishOptions.DataLifetime);
            chkDetailedLog.Checked = publishOptions.DetailedLog;
            txtDeviceFilter.Text = publishOptions.DeviceFilter.ToRangeString();
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        private void ControlsToOptions()
        {
            // connection
            MqttConnectionOptions connectionOptions = options.ConnectionOptions;
            connectionOptions.Server = txtServer.Text;
            connectionOptions.Port = Convert.ToInt32(numPort.Value);
            connectionOptions.ClientID = txtClientID.Text;
            connectionOptions.Username = txtUsername.Text;
            connectionOptions.Password = txtPassword.Text;
            connectionOptions.Timeout = Convert.ToInt32(numTimeout.Value);
            connectionOptions.ProtocolVersion = (MqttProtocolVersion)cbProtocolVersion.SelectedIndex;

            // publishing
            PublishOptions publishOptions = options.PublishOptions;
            publishOptions.RootTopic = txtRootTopic.Text;
            publishOptions.UndefinedValue = txtUndefinedValue.Text;
            publishOptions.PublishFormat = txtPublishFormat.Text;
            publishOptions.QosLevel = cbQosLevel.SelectedIndex;
            publishOptions.Retain = chkRetain.Checked;
            publishOptions.MaxQueueSize = Convert.ToInt32(numMaxQueueSize.Value);
            publishOptions.DataLifetime = Convert.ToInt32(numDataLifetime.Value);
            publishOptions.DetailedLog = publishOptions.DetailedLog;
            publishOptions.DeviceFilter.Clear();
            publishOptions.DeviceFilter.AddRange(ScadaUtils.ParseRange(txtDeviceFilter.Text, true, true));

            options.AddToOptionList(dataSourceConfig.CustomOptions);
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            StringBuilder sbError = new();

            if (string.IsNullOrWhiteSpace(txtServer.Text))
                sbError.AppendError(lblServer, CommonPhrases.NonemptyRequired);

            if (!ScadaUtils.ParseRange(txtDeviceFilter.Text, true, true, out _))
                sbError.AppendError(lblDeviceFilter, CommonPhrases.ValidRangeRequired);

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


        private void FrmMqttDSO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            OptionsToControls();
        }

        private void btnSelectDevices_Click(object sender, EventArgs e)
        {
            // show dialog to select devices
            ScadaUtils.ParseRange(txtDeviceFilter.Text, true, true, out IList<int> deviceNums);
            FrmEntitySelect frmEntitySelect = new(configDataset.DeviceTable) { SelectedIDs = deviceNums };

            if (frmEntitySelect.ShowDialog() == DialogResult.OK)
                txtDeviceFilter.Text = frmEntitySelect.SelectedIDs.ToRangeString();
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
