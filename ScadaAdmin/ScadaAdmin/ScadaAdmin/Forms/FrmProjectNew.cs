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
 * Summary  : Represents a form for creating a new project
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Forms;
using Scada.Lang;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Represents a form for creating a new project.
    /// <para>Представляет форму для создания нового проекта.</para>
    /// </summary>
    public partial class FrmProjectNew : Form
    {
        /// <summary>
        /// Represents an item of the project template list.
        /// </summary>
        private class TemplateItem
        {
            public string Name { get; set; }
            public string FileName { get; set; }
            public string Descr { get; set; }
            public override string ToString() => Name;
        }

        private readonly AppData appData; // the common data of the application


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmProjectNew()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmProjectNew(AppData appData)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
        }


        /// <summary>
        /// Gets the project name.
        /// </summary>
        public string ProjectName
        {
            get
            {
                return txtName.Text.Trim();
            }
        }
        
        /// <summary>
        /// Gets the project location.
        /// </summary>
        public string ProjectLocation
        {
            get
            {
                return txtLocation.Text.Trim();
            }
        }

        /// <summary>
        /// Gets the file name of the project template.
        /// </summary>
        public string ProjectTemplate
        {
            get
            {
                return cbTemplate.SelectedItem is TemplateItem templateItem ?
                    templateItem.FileName : cbTemplate.Text.Trim();
            }
        }


        /// <summary>
        /// Fills the list of available templates.
        /// </summary>
        private void FillTemplateList()
        {
            try
            {
                cbTemplate.BeginUpdate();
                DirectoryInfo templateDirInfo = new(appData.AppDirs.TemplateDir);

                if (templateDirInfo.Exists)
                {
                    int selectedIndex = 0;
                    string cultureTemplate = "." + Locale.Culture.Name + ".";

                    // search for project files
                    foreach (DirectoryInfo projectDirInfo in
                        templateDirInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                    {
                        foreach (FileInfo projectFileInfo in
                            projectDirInfo.EnumerateFiles("*" + AdminUtils.ProjectExt, SearchOption.TopDirectoryOnly))
                        {
                            if (ScadaProject.LoadDescription(projectFileInfo.FullName,
                                out string description, out string errMsg))
                            {
                                cbTemplate.Items.Add(new TemplateItem()
                                {
                                    Name = projectFileInfo.Name,
                                    FileName = projectFileInfo.FullName,
                                    Descr = description
                                });

                                if (projectFileInfo.Name.Contains(cultureTemplate))
                                    selectedIndex = cbTemplate.Items.Count - 1;
                            }
                            else
                            {
                                appData.ErrLog.WriteError(errMsg);
                            }
                        }
                    }

                    // select the template by default
                    if (cbTemplate.Items.Count > 0)
                        cbTemplate.SelectedIndex = selectedIndex;
                }
            }
            finally
            {
                cbTemplate.EndUpdate();
            }
        }

        /// <summary>
        /// Shows a description of the selected template.
        /// </summary>
        private void ShowTemplateDescr()
        {
            if (cbTemplate.SelectedItem is TemplateItem templateItem)
            {
                txtTemplateDescr.Text = templateItem.Descr;
            }
            else if (File.Exists(cbTemplate.Text))
            {
                txtTemplateDescr.Text =
                    ScadaProject.LoadDescription(cbTemplate.Text, out string description, out string errMsg) ?
                    description : errMsg;
            }
            else
            {
                txtTemplateDescr.Text = "";
            }
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            string name = ProjectName;
            string location = ProjectLocation;
            string template = ProjectTemplate;

            // validate name
            if (name == "")
            {
                ScadaUiUtils.ShowError(AppPhrases.ProjectNameEmpty);
                return false;
            }

            if (!AdminUtils.NameIsValid(name))
            {
                ScadaUiUtils.ShowError(AppPhrases.ProjectNameInvalid);
                return false;
            }

            // validate location
            if (location == "")
            {
                ScadaUiUtils.ShowError(AppPhrases.ProjectLocationEmpty);
                return false;
            }

            if (location.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                ScadaUiUtils.ShowError(AppPhrases.ProjectLocationInvalid);
                return false;
            }

            if (Directory.Exists(Path.Combine(location, name)))
            {
                ScadaUiUtils.ShowError(AppPhrases.ProjectAlreadyExists);
                return false;
            }

            // validate template
            if (template == "")
            {
                if (MessageBox.Show(AppPhrases.ProjectTemplateEmpty, CommonPhrases.QuestionCaption, 
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return false;
                }
            }
            else if (!File.Exists(template))
            {
                ScadaUiUtils.ShowError(AppPhrases.ProjectTemplateNotFound);
                return false;
            }

            return true;
        }


        private void FrmNewProject_Load(object sender, EventArgs e)
        {
            // translate the form
            FormTranslator.Translate(this, GetType().FullName);
            fbdLocation.Description = AppPhrases.ChooseProjectLocation;
            ofdTemplate.SetFilter(AppPhrases.ProjectFileFilter);

            // setup the controls
            txtName.Text = ScadaProject.DefaultName;
            txtLocation.Text = appData.State.ProjectDir;
            ofdTemplate.InitialDirectory = appData.State.ProjectDir;
            FillTemplateList();
        }

        private void cbTemplate_TextChanged(object sender, EventArgs e)
        {
            ShowTemplateDescr();
        }

        private void btnBrowseLocation_Click(object sender, EventArgs e)
        {
            fbdLocation.SelectedPath = txtLocation.Text.Trim();

            if (fbdLocation.ShowDialog() == DialogResult.OK)
                txtLocation.Text = ScadaUtils.NormalDir(fbdLocation.SelectedPath);
        }

        private void btnBrowseTemplate_Click(object sender, EventArgs e)
        {
            ofdTemplate.FileName = "";

            if (ofdTemplate.ShowDialog() == DialogResult.OK)
            {
                ofdTemplate.InitialDirectory = Path.GetDirectoryName(ofdTemplate.FileName);
                cbTemplate.SelectedIndex = -1;
                cbTemplate.Text = ofdTemplate.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
            {
                appData.State.ProjectDir = ProjectLocation;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
