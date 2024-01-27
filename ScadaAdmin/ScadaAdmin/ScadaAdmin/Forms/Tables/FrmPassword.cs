/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Administrator
 * Summary  : Represents a form for setting a password
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Represents a form for setting a password hash.
    /// <para>Представляет форму для установки хэша пароля.</para>
    /// </summary>
    public partial class FrmPassword : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPassword()
        {
            InitializeComponent();

            UserID = 0;
            PasswordHash = "";
        }


        /// <summary>
        /// Gets or sets the user ID that affect hash generation.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Get the password hash.
        /// </summary>
        public string PasswordHash { get; private set; }


        /// <summary>
        /// Disables or enables the OK button.
        /// </summary>
        private void SetOkEnabled()
        {
            btnOK.Enabled = UserID > 0 && txtPassword.Text != "";
        }


        private void FrmPasswordSet_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            txtUserID.Text = UserID.ToString();
            txtPassword.Select();
            btnHidePassword.Top = btnShowPassword.Top;
            SetOkEnabled();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            SetOkEnabled();
        }

        private void btnShowHidePassword_Click(object sender, EventArgs e)
        {
            bool hide = sender == btnHidePassword;
            txtPassword.UseSystemPasswordChar = hide;
            btnShowPassword.Visible = hide;
            btnHidePassword.Visible = !hide;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PasswordHash = ScadaUtils.GetPasswordHash(UserID, txtPassword.Text);
            DialogResult = DialogResult.OK;
        }
    }
}
