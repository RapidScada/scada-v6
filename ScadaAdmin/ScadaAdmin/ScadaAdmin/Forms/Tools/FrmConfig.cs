/*
 * Copyright 2023 Rapid Software LLC
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

using Microsoft.Win32;
using Scada.Admin.App.Code;
using Scada.Admin.Config;
using Scada.Admin.Extensions;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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

            colExt.Name = nameof(colExt);
            colPath.Name = nameof(colPath);
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
            // active extensions
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

            // file associations
            try
            {
                lvAssoc.BeginUpdate();
                lvAssoc.Items.Clear();

                foreach (KeyValuePair<string, string> pair in config.FileAssociations)
                {
                    lvAssoc.Items.Add(CreateAssociationItem(pair.Key, pair.Value));
                }

                if (lvAssoc.Items.Count > 0)
                    lvAssoc.Items[0].Selected = true;
            }
            finally
            {
                lvAssoc.EndUpdate();
            }

            // channel numbering
            ChannelNumberingOptions options = config.ChannelNumberingOptions;
            numMultiplicity.SetValue(options.Multiplicity);
            numShift.SetValue(options.Shift);
            numGap.SetValue(options.Gap);
            chkPrependDeviceName.Checked = options.PrependDeviceName;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            // active extensions
            config.ExtensionCodes.Clear();

            foreach (ExtentionItem item in lbActiveExt.Items)
            {
                config.ExtensionCodes.Add(item.ExtentionCode);
            }

            // file associations
            config.FileAssociations.Clear();

            foreach (ListViewItem item in lvAssoc.Items)
            {
                config.FileAssociations[item.SubItems[0].Text] = item.SubItems[1].Text;
            }

            // channel numbering
            ChannelNumberingOptions options = config.ChannelNumberingOptions;
            options.Multiplicity = Convert.ToInt32(numMultiplicity.Value);
            options.Shift = Convert.ToInt32(numShift.Value);
            options.Gap = Convert.ToInt32(numGap.Value);
            options.PrependDeviceName = chkPrependDeviceName.Checked;
        }

        /// <summary>
        /// Initializes the extension item if needed.
        /// </summary>
        private void InitExtensionItem(ExtentionItem extensionItem)
        {
            if (!extensionItem.IsInitialized)
            {
                extensionItem.IsInitialized = true;

                if (ExtensionFactory.GetExtensionLogic(appData.AppDirs.LibDir, extensionItem.ExtentionCode, appData,
                    out ExtensionLogic extensionLogic, out string message))
                {
                    extensionItem.Descr = BuildExtensionDescr(extensionLogic);
                    extensionItem.ExtensionLogic = extensionLogic;
                }
                else
                {
                    extensionItem.Descr = message;
                    extensionItem.ExtensionLogic = null;
                }
            }
        }

        /// <summary>
        /// Shows a description of the specified extension item.
        /// </summary>
        private void ShowExtensionDescr(object item)
        {
            if (item is ExtentionItem extensionItem)
            {
                InitExtensionItem(extensionItem);
                txtExtDescr.Text = extensionItem.Descr;
            }
        }

        /// <summary>
        /// Enables or disables the extension buttons.
        /// </summary>
        private void SetExtensionButtonsEnabled()
        {
            btnActivateExt.Enabled = lbUnusedExt.SelectedItem is ExtentionItem;

            if (lbActiveExt.SelectedItem is ExtentionItem extensionItem)
            {
                btnDeactivateExt.Enabled = true;
                btnMoveUpExt.Enabled = lbActiveExt.SelectedIndex > 0;
                btnMoveDownExt.Enabled = lbActiveExt.SelectedIndex < lbActiveExt.Items.Count - 1;
                btnExtProperties.Enabled = extensionItem.ExtensionLogic != null && 
                    extensionItem.ExtensionLogic.CanShowProperties;
            }
            else
            {
                btnDeactivateExt.Enabled = false;
                btnMoveUpExt.Enabled = false;
                btnMoveDownExt.Enabled = false;
                btnExtProperties.Enabled = false;
            }
        }

        /// <summary>
        /// Enables or disables the file association buttons.
        /// </summary>
        private void SetAssociationButtonsEnabled()
        {
            btnEditAssoc.Enabled = btnDeleteAssoc.Enabled = lvAssoc.SelectedItems.Count > 0;
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            // warnings
            StringBuilder sbWarn = new();

            foreach (ListViewItem item in lvAssoc.Items)
            {
                string path = item.SubItems[1].Text;
                if (!string.IsNullOrWhiteSpace(path) && !File.Exists(path))
                    sbWarn.AppendLine(string.Format(CommonPhrases.NamedFileNotFound, path));
            }

            if (sbWarn.Length > 0)
                ScadaUiUtils.ShowWarning(sbWarn.ToString());

            return true;
        }

        /// <summary>
        /// Creates a new list view item represents a file association.
        /// </summary>
        private static ListViewItem CreateAssociationItem(string ext, string path, bool selected = false)
        {
            return new ListViewItem(new string[] { ext, path }) { Selected = selected };
        }

        /// <summary>
        /// Build the extension description.
        /// </summary>
        private static string BuildExtensionDescr(ExtensionLogic extensionLogic)
        {
            string title = string.Format("{0} {1}",
                extensionLogic.Name,
                extensionLogic.GetType().Assembly.GetName().Version);

            return new StringBuilder()
                .AppendLine(title)
                .AppendLine(new string('-', title.Length))
                .Append(extensionLogic.Descr?.Replace("\n", Environment.NewLine))
                .ToString();
        }

        /// <summary>
        /// Registers the file association.
        /// </summary>
        private static void RegisterAssociation(string ext, string path)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + ext))
            {
                key.CreateSubKey("shell\\open\\command").SetValue("", $"\"{path}\" \"%1\"");
            }

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);

            if (!config.Load(ConfigFileName, out string errMsg))
                appData.ErrLog.HandleError(errMsg);

            ConfigToControls();
            FillUnusedExtensions();
            SetExtensionButtonsEnabled();
            SetAssociationButtonsEnabled();
        }

        private void btnActivateExt_Click(object sender, EventArgs e)
        {
            // move the selected extension from unused extensions to active extensions
            if (lbUnusedExt.SelectedItem is ExtentionItem extentionItem)
            {
                if (extentionItem.ExtentionCode == "ExtSubFolder") config.EnableSubFolder(true);
                if (extentionItem.ExtentionCode == "ExtBitReader") config.EnableBitReader(true);
                lbUnusedExt.Items.RemoveAt(lbUnusedExt.SelectedIndex);
                lbActiveExt.SelectedIndex = lbActiveExt.Items.Add(extentionItem);
                lbActiveExt.Focus();
            }
        }

        private void btnDeactivateExt_Click(object sender, EventArgs e)
        {
            // move the selected extension from active extensions to unused extensions
            if (lbActiveExt.SelectedItem is ExtentionItem extentionItem)
            {
                if (extentionItem.ExtentionCode == "ExtSubFolder") config.EnableSubFolder(true);
                if (extentionItem.ExtentionCode == "ExtBitReader") config.EnableBitReader(true);
                lbActiveExt.Items.RemoveAt(lbActiveExt.SelectedIndex);
                lbUnusedExt.SelectedIndex = lbUnusedExt.Items.Add(extentionItem);
                lbUnusedExt.Focus();
            }
        }

        private void btnMoveUpExt_Click(object sender, EventArgs e)
        {
            // move up the selected extension
            if (lbActiveExt.SelectedItem is ExtentionItem extentionItem)
            {
                int curInd = lbActiveExt.SelectedIndex;
                int prevInd = curInd - 1;

                if (prevInd >= 0)
                {
                    lbActiveExt.Items.RemoveAt(curInd);
                    lbActiveExt.Items.Insert(prevInd, extentionItem);
                    lbActiveExt.SelectedIndex = prevInd;
                    lbActiveExt.Focus();
                }
            }
        }

        private void btnMoveDownExt_Click(object sender, EventArgs e)
        {
            // move down the selected extension
            if (lbActiveExt.SelectedItem is ExtentionItem extentionItem)
            {
                int curInd = lbActiveExt.SelectedIndex;
                int nextInd = curInd + 1;

                if (nextInd < lbActiveExt.Items.Count)
                {
                    lbActiveExt.Items.RemoveAt(curInd);
                    lbActiveExt.Items.Insert(nextInd, extentionItem);
                    lbActiveExt.SelectedIndex = nextInd;
                    lbActiveExt.Focus();
                }
            }
        }

        private void btnExtProperties_Click(object sender, EventArgs e)
        {
            // show properties of the selected extension
            if (lbActiveExt.SelectedItem is ExtentionItem extentionItem &&
                extentionItem.ExtensionLogic != null && extentionItem.ExtensionLogic.CanShowProperties)
            {
                lbActiveExt.Focus();
                extentionItem.ExtensionLogic.ShowProperties(config);
            }
        }

        private void lbUnusedExt_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowExtensionDescr(lbUnusedExt.SelectedItem);
            SetExtensionButtonsEnabled();
        }

        private void lbUnusedExt_DoubleClick(object sender, EventArgs e)
        {
            btnActivateExt_Click(null, null);
        }

        private void lbActiveExt_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowExtensionDescr(lbActiveExt.SelectedItem);
            SetExtensionButtonsEnabled();
        }

        private void lbActiveExt_DoubleClick(object sender, EventArgs e)
        {
            btnExtProperties_Click(null, null);
        }


        private void btnAddAssoc_Click(object sender, EventArgs e)
        {
            // add new file association
            FrmFileAssociation form = new();

            if (form.ShowDialog() == DialogResult.OK)
            {
                lvAssoc.Items.Add(CreateAssociationItem(form.FileExtension, form.ExecutablePath, true));
                lvAssoc.Focus();
            }
        }

        private void btnEditAssoc_Click(object sender, EventArgs e)
        {
            // edit selected file association
            if (lvAssoc.SelectedItems.Count > 0)
            {
                ListViewItem item = lvAssoc.SelectedItems[0];
                FrmFileAssociation form = new()
                {
                    FileExtension = item.SubItems[0].Text,
                    ExecutablePath = item.SubItems[1].Text
                };

                if (form.ShowDialog() == DialogResult.OK)
                {
                    lvAssoc.Items.RemoveAt(item.Index);
                    lvAssoc.Items.Add(CreateAssociationItem(form.FileExtension, form.ExecutablePath, true));
                    lvAssoc.Focus();
                }
            }
        }

        private void btnDeleteAssoc_Click(object sender, EventArgs e)
        {
            // delete selected file association
            if (lvAssoc.SelectedItems.Count > 0)
            {
                int index = lvAssoc.SelectedIndices[0];
                lvAssoc.Items.RemoveAt(index);

                if (lvAssoc.Items.Count > 0)
                {
                    index = Math.Min(index, lvAssoc.Items.Count - 1);
                    lvAssoc.Items[index].Selected = true;
                }

                lvAssoc.Focus();
            }
        }

        private void btnRegisterProjectExt_Click(object sender, EventArgs e)
        {
            RegisterAssociation(AdminUtils.ProjectExt, Application.ExecutablePath);
            ScadaUiUtils.ShowInfo(string.Format(AppPhrases.ProjectExtRegistered, AdminUtils.ProjectExt));
        }

        private void lvAssoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAssociationButtonsEnabled();
        }

        private void lvAssoc_DoubleClick(object sender, EventArgs e)
        {
            btnEditAssoc_Click(null, null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
            {
                ControlsToConfig();

                if (config.Save(ConfigFileName, out string errMsg))
                    DialogResult = DialogResult.OK;
                else
                    appData.ErrLog.HandleError(errMsg);
            }
        }
    }
}
