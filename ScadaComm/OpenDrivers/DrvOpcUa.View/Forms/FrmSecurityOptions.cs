// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvOpcUa.View.Forms
{
    /// <summary>
    /// Represents a form for editing security options form.
    /// <para>Представляет форма для редактирования параметров безопасности.</para>
    /// </summary>
    public partial class FrmSecurityOptions : Form
    {
        private readonly OpcConnectionOptions connectionOptions; // the OPC server connection options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmSecurityOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmSecurityOptions(OpcConnectionOptions connectionOptions)
            : this()
        {
            this.connectionOptions = connectionOptions;
            FillComboBoxes();
        }


        /// <summary>
        /// Fills the combo boxes.
        /// </summary>
        private void FillComboBoxes()
        {
            cbSecurityMode.Items.AddRange(new object[] {
                MessageSecurityMode.None,
                MessageSecurityMode.Sign,
                MessageSecurityMode.SignAndEncrypt });

            cbSecurityPolicy.Items.AddRange(new object[] {
                SecurityPolicy.None,
                SecurityPolicy.Basic128Rsa15,
                SecurityPolicy.Basic256,
                SecurityPolicy.Basic256Sha256,
                SecurityPolicy.Aes128_Sha256_RsaOaep,
                SecurityPolicy.Aes256_Sha256_RsaPss,
                SecurityPolicy.Https });

            cbAuthenticationMode.Items.AddRange(new object[] {
                AuthenticationMode.Anonymous,
                AuthenticationMode.Username });
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            cbSecurityMode.SelectedIndex = (int)connectionOptions.SecurityMode - 1;
            cbSecurityPolicy.SelectedIndex = (int)connectionOptions.SecurityPolicy;
            cbAuthenticationMode.SelectedIndex = (int)connectionOptions.AuthenticationMode;
            txtUsername.Text = connectionOptions.Username;
            txtPassword.Text = connectionOptions.Password;
        }

        /// <summary>
        /// Sets the configuration parameters according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            connectionOptions.SecurityMode = (MessageSecurityMode)(cbSecurityMode.SelectedIndex + 1);
            connectionOptions.SecurityPolicy = (SecurityPolicy)cbSecurityPolicy.SelectedIndex;
            connectionOptions.AuthenticationMode = (AuthenticationMode)cbAuthenticationMode.SelectedIndex;
            connectionOptions.Username = txtUsername.Text;
            connectionOptions.Password = txtPassword.Text;
        }


        private void FrmSecurityOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ConfigToControls();
        }

        private void cbSecurityMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // fix the security policy
            if (cbSecurityMode.SelectedIndex == 0)
                cbSecurityPolicy.SelectedIndex = 0;
            else if (cbSecurityPolicy.SelectedIndex == 0)
                cbSecurityPolicy.SelectedIndex = 1;
        }

        private void cbAuthenticationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlUsername.Enabled = cbAuthenticationMode.SelectedIndex > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ControlsToConfig();
            DialogResult = DialogResult.OK;
        }
    }
}
