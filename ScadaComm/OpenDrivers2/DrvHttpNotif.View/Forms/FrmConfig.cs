// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.AB.Forms;
using Scada.Comm.Drivers.DrvHttpNotif.Config;
using Scada.Comm.Lang;
using Scada.Forms;
using Scada.Lang;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvHttpNotif.View.Forms
{
    /// <summary>
    /// Represents a device configuration form.
    /// <para>Представляет форму конфигурации КП.</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        private readonly AppDirs appDirs;          // the application directories
        private readonly int deviceNum;            // the device number
        private readonly NotifDeviceConfig config; // the device configuration
        private readonly string configFileName;    // the configuration file name
        private bool modified;                     // the configuration was modified


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConfig(AppDirs appDirs, int deviceNum)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.deviceNum = deviceNum;
            config = new NotifDeviceConfig();
            configFileName = Path.Combine(appDirs.ConfigDir, NotifDeviceConfig.GetFileName(deviceNum));
            modified = false;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the configuration was modified.
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                btnSave.Enabled = modified;
            }
        }

        /// <summary>
        /// Gets or sets the default request URI.
        /// </summary>
        public string DefaultUri { get; set; }


        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            cbMethod.SelectedIndex = (int)config.Method;
            txtUri.Text = string.IsNullOrEmpty(config.Uri) ? DefaultUri : config.Uri;
            chkParamEnabled.Checked = config.ParamEnabled;
            txtParamBegin.Text = config.ParamBegin.ToString();
            txtParamEnd.Text = config.ParamEnd.ToString();
            txtAddrSep.Text = config.AddrSep;
            cbContentType.Text = config.ContentType;
            cbContentEscaping.SelectedIndex = (int)config.ContentEscaping;
            txtContent.Text = config.Content;
            dgvHeaders.DataSource = new BindingList<Header>(config.Headers);
        }

        /// <summary>
        /// Sets the configuration parameters according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            config.Method = (RequestMethod)cbMethod.SelectedIndex;
            config.Uri = txtUri.Text;
            config.ParamEnabled = chkParamEnabled.Checked;
            config.SetParamBegin(txtParamBegin.Text);
            config.SetParamEnd(txtParamEnd.Text);
            config.AddrSep = txtAddrSep.Text;
            config.ContentType = cbContentType.Text;
            config.ContentEscaping = (EscapingMethod)cbContentEscaping.SelectedIndex;
            config.Content = txtContent.Text;
        }


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            // translate form
            FormTranslator.Translate(this, GetType().FullName);
            Text = string.Format(Text, deviceNum);

            // load configuration
            if (File.Exists(configFileName) && !config.Load(configFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // display configuration
            ConfigToControls();
            Modified = false;
        }

        private void FrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(CommPhrases.SaveDeviceConfigConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!config.Save(configFileName, out string errMsg))
                        {
                            ScadaUiUtils.ShowError(errMsg);
                            e.Cancel = true;
                        }
                        break;

                    case DialogResult.No:
                        break;

                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void control_Changed(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void dgvHeaders_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                Modified = true;
        }

        private void dgvHeaders_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            Modified = true;
        }

        private void btnAddressBook_Click(object sender, EventArgs e)
        {
            new FrmAddressBook(appDirs).ShowDialog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // retrieve configuration
            ControlsToConfig();

            // save configuration
            if (config.Save(configFileName, out string errMsg))
                Modified = false;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
