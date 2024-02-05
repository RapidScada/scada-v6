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
 * Summary  : Represents a form for editing a text file
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Forms;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Represents a form for editing a text file.
    /// <para>Представляет форму для редактирования текстового файла.</para>
    /// </summary>
    public partial class FrmTextEditor : Form, IChildForm
    {
        private readonly AppData appData; // the common data of the application
        private string fileName;          // the full name of the edited file
        private bool changing;            // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTextEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTextEditor(AppData appData, string fileName)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            this.fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            changing = false;
            Text = Path.GetFileName(fileName);
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Loads the file.
        /// </summary>
        public void LoadFile()
        {
            try
            {
                changing = true;

                using FileStream fileStream = new(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader reader = new(fileStream, Encoding.UTF8);
                richTextBox.Text = reader.ReadToEnd(); // RichTextBox faster than TextBox
                ChildFormTag.Modified = false;
            }
            catch (Exception ex)
            {
                appData.ErrLog.HandleError(ex, AppPhrases.OpenTextFileError);
            }
            finally
            {
                changing = false;
            }
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        public void Save()
        {
            try
            {
                using FileStream fileStream = new(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                using StreamWriter writer = new(fileStream, Encoding.UTF8);
                writer.Write(richTextBox.Text);
                ChildFormTag.Modified = false;
            }
            catch (Exception ex)
            {
                appData.ErrLog.HandleError(ex, AppPhrases.SaveTextFileError);
            }
        }


        private void FrmTextEditor_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
            LoadFile();
        }

        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            // update file name in case of renaming a file or its parent directory
            if (e.Message == AdminMessage.UpdateFileName &&
                e.GetArgument("FileName") is string newFileName && newFileName != "")
            {
                fileName = newFileName;
                Text = Path.GetFileName(fileName);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!changing)
                ChildFormTag.Modified = true;
        }
    }
}
