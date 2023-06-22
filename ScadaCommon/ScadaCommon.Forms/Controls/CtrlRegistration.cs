// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System.ComponentModel;

namespace Scada.Forms.Controls
{
    /// <summary>
    /// Represents a control for module registration.
    /// <para>Представляет элемент управления для регистрации модуля.</para>
    /// </summary>
    public partial class CtrlRegistration : UserControl
    {
        private string productCode;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlRegistration()
        {
            InitializeComponent();

            productCode = "";
            PermanentKeyUrl = "";
            TrialKeyUrl = "";
        }


        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public new string ProductName
        {
            get => txtProductName.Text;
            set => txtProductName.Text = value;
        }

        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        public string ProductCode
        {
            get => productCode;
            set => productCode = value;
        }

        /// <summary>
        /// Gets or sets the computer code.
        /// </summary>
        public string ComputerName
        {
            get => txtCompCode.Text;
            set => txtCompCode.Text = value;
        }

        /// <summary>
        /// Gets the URL to get a permanent key.
        /// </summary>
        public string PermanentKeyUrl { get; set; }

        /// <summary>
        /// Gets the default URL to get a trial key.
        /// </summary>
        public string TrialKeyUrl { get; set; }


        /// <summary>
        /// Gets the default URL to get a permanent key.
        /// </summary>
        private static string GetDefaultPermanentKeyUrl()
        {
            return Locale.IsRussian ?
                "https://rapidscada.ru/download-all-files/purchase-module/" :
                "https://rapidscada.org/download-all-files/purchase-module/";
        }

        /// <summary>
        /// Gets the default URL to get a trial key.
        /// </summary>
        private static string GetDefaultTrialKeyUrl(string productCode)
        {
            return string.Format(Locale.IsRussian ?
                "https://rapidscada.net/trial/?prod={0}&ver=6&lang=ru" :
                "https://rapidscada.net/trial/?prod={0}&ver=6",
                productCode);
        }


        /// <summary>
        /// Occurs when the Refresh button is clicked.
        /// </summary>
        [Category("Action")]
        public event EventHandler RefreshCompCode;


        private void btnCopyCompCode_Click(object sender, EventArgs e)
        {
            if (txtCompCode.Text != "")
                Clipboard.SetText(txtCompCode.Text);
        }

        private void btnRefreshCompCode_Click(object sender, EventArgs e)
        {
            RefreshCompCode?.Invoke(this, EventArgs.Empty);
        }

        private void btnPasteRegKey_Click(object sender, EventArgs e)
        {
            txtRegKey.Text = Clipboard.GetText();
        }

        private void llblGetPermanentKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ScadaUiUtils.StartProcess(string.IsNullOrEmpty(PermanentKeyUrl)
                ? GetDefaultPermanentKeyUrl()
                : PermanentKeyUrl);
        }

        private void llblGetTrialKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ScadaUiUtils.StartProcess(string.IsNullOrEmpty(TrialKeyUrl)
                ? GetDefaultTrialKeyUrl(ProductCode)
                : TrialKeyUrl);
        }
    }
}
