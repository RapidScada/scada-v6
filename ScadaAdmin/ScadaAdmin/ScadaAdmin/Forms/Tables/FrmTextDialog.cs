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
 * Summary  : Represents a modal form for editing text
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Forms;
using System;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tables
{
    /// <summary>
    /// Represents a modal form for editing text.
    /// <para>Представляет модальную форму для редактирования текста.</para>
    /// </summary>
    public partial class FrmTextDialog : Form
    {
        /// <summary>
        /// The maximum length of the source code by default.
        /// </summary>
        private const int DefaultMaxLength = 1000;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTextDialog()
        {
            InitializeComponent();

            MaxLength = DefaultMaxLength;
            PlainText = "";
        }


        /// <summary>
        /// Gets or sets the maximum length of the plain text.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the plain text to edit.
        /// </summary>
        public string PlainText { get; set; }


        /// <summary>
        /// Shows the current line.
        /// </summary>
        private void ShowCurrentLine()
        {
            lblLine.Text = string.Format(AppPhrases.TextLine,
                txtPlainText.GetLineFromCharIndex(txtPlainText.SelectionStart) + 1);
        }

        /// <summary>
        /// Shows the current and maximum text length.
        /// </summary>
        private void ShowTextLength()
        {
            lblLength.Text = string.Format(AppPhrases.TextLength, txtPlainText.Text.Length, txtPlainText.MaxLength);
        }

        /// <summary>
        /// Normalizes line endings of the specified string.
        /// </summary>
        private static string Normalize(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            else
            {
                StringBuilder stringBuilder = new();

                foreach (char c in s)
                {
                    switch (c)
                    {
                        case '\r':
                            break;
                        case '\n':
                            stringBuilder.AppendLine();
                            break;
                        default:
                            stringBuilder.Append(c);
                            break;
                    }
                }

                return stringBuilder.ToString();
            }
        }


        private void FrmEditSource_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            txtPlainText.MaxLength = MaxLength;
            string text = Normalize(PlainText);
            txtPlainText.Text = text.Length <= MaxLength ? text : text.Substring(0, MaxLength);
            ShowCurrentLine();
            ShowTextLength();
        }

        private void txtPlainText_TextChanged(object sender, EventArgs e)
        {
            ShowTextLength();
        }

        private void txtPlainText_SelectionChanged(object sender, EventArgs e)
        {
            ShowCurrentLine();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PlainText = txtPlainText.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
