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
 * Summary  : Represents a form for adding and editing a file association
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Admin.App.Code;
using Scada.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Represents a form for adding and editing a file association.
    /// <para>Представляет форму для добавления или редактирования сопоставления файла.</para>
    /// </summary>
    public partial class FrmFileAssociation : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmFileAssociation()
        {
            InitializeComponent();
            btnOK.Enabled = false;

            FileExtension = "";
            ExecutablePath = "";
        }


        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the executable path.
        /// </summary>
        public string ExecutablePath { get; set; }


        /// <summary>
        /// Shows a file open dialog to select a file.
        /// </summary>
        private void SelectFile(TextBox textBox)
        {
            if (File.Exists(textBox.Text))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(textBox.Text);
                openFileDialog.FileName = Path.GetFileName(textBox.Text);
            }
            else
            {
                openFileDialog.FileName = "";
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                textBox.Text = openFileDialog.FileName;
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            return !string.IsNullOrWhiteSpace(txtExt.Text) && !string.IsNullOrWhiteSpace(txtPath.Text);
        }


        private void FrmFileAssociation_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            openFileDialog.SetFilter(AppPhrases.ExecutableFileFilter);

            txtExt.Text = FileExtension;
            txtPath.Text = ExecutablePath;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = ValidateControls();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SelectFile(txtPath);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FileExtension = txtExt.Text.Trim().ToLowerInvariant();
            ExecutablePath = txtPath.Text.Trim();
            DialogResult = DialogResult.OK;
        }
    }
}
