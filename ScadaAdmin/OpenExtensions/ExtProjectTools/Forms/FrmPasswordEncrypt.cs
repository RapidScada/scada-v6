// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents a form for encrypting passwords.
    /// <para>Представляет форму для шифрования паролей.</para>
    /// </summary>
    public partial class FrmPasswordEncrypt : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPasswordEncrypt()
        {
            InitializeComponent();
        }

        private void FrmPasswordEncrypt_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPasswordEncrypted.Text = ScadaUtils.Encrypt(txtPassword.Text);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (txtPasswordEncrypted.Text != "")
                Clipboard.SetText(txtPasswordEncrypted.Text);
        }
    }
}
