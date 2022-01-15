/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Represents a form for editing an application configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Admin.App.Code;
using Scada.Admin.Config;
using Scada.Admin.Extensions;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Represents a form for editing an application configuration.
    /// <para>Представляет форму для редактирования конфигурации приложения.</para>
    /// </summary>
    public partial class FrmConfig : Form
    {
        /// <summary>
        /// Reprensents an extension.
        /// </summary>
        private class ExtentionItem
        {
            public bool IsInitialized { get; set; }
            public string ExtentionCode { get; set; }
            public string FileName { get; set; }
            public string Descr { get; set; }
            public ExtensionLogic ExtensionLogic { get; set; }
            public override string ToString() => ExtentionCode;
        }


        private readonly AppData appData;    // the common data of the application
        private readonly AdminConfig config; // the application configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmConfig()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConfig(AppData appData)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            config = new AdminConfig();
        }


        /// <summary>
        /// Gets the full name of the configuration file.
        /// </summary>
        private string ConfigFileName => Path.Combine(appData.AppDirs.ConfigDir, AdminConfig.DefaultFileName);


        /// <summary>
        /// Fills the list of unused extensions.
        /// </summary>
        private void FillUnusedExtensions()
        {
            try
            {
                lbUnusedExt.BeginUpdate();
                lbUnusedExt.Items.Clear();

                // read all available extensions
                DirectoryInfo dirInfo = new(appData.AppDirs.LibDir);

                if (dirInfo.Exists)
                {
                    foreach (FileInfo fileInfo in
                        dirInfo.EnumerateFiles("Ext*.dll", SearchOption.TopDirectoryOnly))
                    {
                        string extentionCode = ScadaUtils.RemoveFileNameSuffixes(fileInfo.Name);

                        if (!config.ExtensionCodes.Contains(extentionCode))
                        {
                            lbUnusedExt.Items.Add(new ExtentionItem
                            {
                                IsInitialized = false,
                                ExtentionCode = extentionCode,
                                FileName = fileInfo.FullName
                            });
                        }
                    }
                }
            }
            finally
            {
                lbUnusedExt.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            // fill active extensions
            try
            {
                lbActiveExt.BeginUpdate();
                lbActiveExt.Items.Clear();

                foreach (string extentionCode in config.ExtensionCodes)
                {
                    lbActiveExt.Items.Add(new ExtentionItem
                    {
                        IsInitialized = false,
                        ExtentionCode = extentionCode,
                        FileName = Path.Combine(appData.AppDirs.LibDir, extentionCode + ".dll")
                    });
                }

                if (lbActiveExt.Items.Count > 0)
                    lbActiveExt.SelectedIndex = 0;

            }
            finally
            {
                lbActiveExt.EndUpdate();
            }

            // fill file associations
            try
            {
                lvAssoc.BeginUpdate();
                lvAssoc.Items.Clear();

                foreach (KeyValuePair<string, string> pair in config.FileAssociations)
                {
                    lvAssoc.Items.Add(CreateAssocItem(pair.Key, pair.Value));
                }

                if (lvAssoc.Items.Count > 0)
                    lvAssoc.Items[0].Selected = true;
            }
            finally
            {
                lvAssoc.EndUpdate();
            }
        }

        /// <summary>
        /// Creates a new list view item represents a file association.
        /// </summary>
        private static ListViewItem CreateAssocItem(string ext, string path, bool selected = false)
        {
            return new ListViewItem(new string[] { ext, path }) { Selected = selected };
        }


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);

            if (!config.Load(ConfigFileName, out string errMsg))
                appData.ErrLog.HandleError(errMsg);

            ConfigToControls();
            FillUnusedExtensions();
        }

        private void btnActivateExt_Click(object sender, EventArgs e)
        {

        }

        private void btnDeactivateExt_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveUpExt_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveDownExt_Click(object sender, EventArgs e)
        {

        }

        private void btnExtProperties_Click(object sender, EventArgs e)
        {

        }

        private void lbUnusedExt_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbUnusedExt_DoubleClick(object sender, EventArgs e)
        {

        }

        private void lbActiveExt_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbActiveExt_DoubleClick(object sender, EventArgs e)
        {

        }


        private void btnAddAssoc_Click(object sender, EventArgs e)
        {

        }

        private void btnEditAssoc_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteAssoc_Click(object sender, EventArgs e)
        {

        }

        private void btnRegisterRsproj_Click(object sender, EventArgs e)
        {

        }

        private void lvAssoc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lvAssoc_DoubleClick(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (config.Save(ConfigFileName, out string errMsg))
                DialogResult = DialogResult.OK;
            else
                appData.ErrLog.HandleError(errMsg);
        }
    }
}
