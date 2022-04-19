// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.AB.Forms;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvEmail.View.Forms
{
    /// <summary>
    /// Device properties form.
    /// <para>Форма настройки свойств КП.</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        private readonly AppDirs appDirs;          // the application directories
        private readonly int deviceNum;            // the device number
        private readonly EmailDeviceConfig config; // the device configuration
        private readonly string configFileName;    // the configuration file name
        private string prevUsername;               // the previous username


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
            config = new EmailDeviceConfig();
            configFileName = Path.Combine(appDirs.ConfigDir, EmailDeviceConfig.GetFileName(deviceNum));
            prevUsername = "";
        }


        /// <summary>
        /// Установить элементы управления в соответствии с конфигурацией
        /// </summary>
        private void ConfigToControls()
        {
            txtHost.Text = config.Host;
            numPort.SetValue(config.Port);
            txtUsername.Text = config.Username;
            txtPassword.Text = config.Password;
            chkEnableSsl.Checked = config.EnableSsl;
            txtSenderAddress.Text = config.SenderAddress;
            txtSenderDisplayName.Text = config.SenderDisplayName;
        }

        /// <summary>
        /// Перенести значения элементов управления в конфигурацию
        /// </summary>
        private void ControlsToConfig()
        {
            config.Host = txtHost.Text;
            config.Port = Convert.ToInt32(numPort.Value);
            config.Username = txtUsername.Text;
            config.Password = txtPassword.Text;
            config.EnableSsl = chkEnableSsl.Checked;
            config.SenderAddress = txtSenderAddress.Text;
            config.SenderDisplayName = txtSenderDisplayName.Text;
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
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            // set sender same as username
            if (string.IsNullOrWhiteSpace(txtSenderAddress.Text) || txtSenderAddress.Text == prevUsername)
                txtSenderAddress.Text = txtUsername.Text;

            prevUsername = txtUsername.Text;
        }

        private void btnAddressBook_Click(object sender, EventArgs e)
        {
            new FrmAddressBook(appDirs).ShowDialog();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // retrieve configuration
            ControlsToConfig();

            // save configuration
            if (config.Save(configFileName, out string errMsg))
                DialogResult = DialogResult.OK;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
