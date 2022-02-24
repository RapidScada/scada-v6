// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions;
using Scada.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Admin.Forms
{
    /// <summary>
    /// Represents a registration form.
    /// <para>Представляет форму регистрации.</para>
    /// </summary>
    public partial class FrmRegistration : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmRegistration()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmRegistration(IAdminContext adminContext, string productCode, string productName)
            : this()
        {
            txtProductName.Text = productName;
        }


        private void FrmRegistration_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);

        }

        private void btnCopyCompCode_Click(object sender, EventArgs e)
        {

        }

        private void btnPasteRegKey_Click(object sender, EventArgs e)
        {

        }

        private void llblGetPermanentKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void llblGetTrialKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
