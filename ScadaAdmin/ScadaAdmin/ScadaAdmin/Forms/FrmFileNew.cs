﻿/*
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
 * Summary  : Represents a form for creating a new file
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Represents a form for creating a new file.
    /// <para>Представляет форму для создания нового файла.</para>
    /// </summary>
    public partial class FrmFileNew : Form
    {
        /// <summary>
        /// The default file name without extension.
        /// </summary>
        private const string DefaultFileName = "NewFile";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmFileNew()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets the short file name.
        /// </summary>
        public string FileName
        {
            get
            {
                return txtFileName.Text.Trim();
            }
        }

        /// <summary>
        /// Gets the file type.
        /// </summary>
        public KnownFileType FileType
        {
            get
            {
                return GetSelectedFileType();
            }
        }


        /// <summary>
        /// Gets the selected file type.
        /// </summary>
        private KnownFileType GetSelectedFileType()
        {
            return lbFileType.SelectedIndex switch
            {
                1 => KnownFileType.TableView,
                2 => KnownFileType.TextFile,
                3 => KnownFileType.XmlFile,
                _ => KnownFileType.SchemeView
            };
        }

        /// <summary>
        /// Corrects the file extension according to the file type.
        /// </summary>
        private void FixFileExtenstion()
        {
            if (!string.IsNullOrWhiteSpace(txtFileName.Text))
            {
                string ext = FileCreator.GetExtension(GetSelectedFileType());
                txtFileName.Text = Path.ChangeExtension(txtFileName.Text, ext);
            }
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            string fileName = FileName;

            if (fileName == "")
            {
                ScadaUiUtils.ShowError(AppPhrases.FileNameEmpty);
                return false;
            }

            if (!AdminUtils.NameIsValid(fileName))
            {
                ScadaUiUtils.ShowError(AppPhrases.FileNameInvalid);
                return false;
            }

            return true;
        }


        private void FrmFileNew_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            txtFileName.Text = DefaultFileName;
            lbFileType.SelectedIndex = 0;
        }

        private void lbFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FixFileExtenstion();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
                DialogResult = DialogResult.OK;
        }
    }
}
