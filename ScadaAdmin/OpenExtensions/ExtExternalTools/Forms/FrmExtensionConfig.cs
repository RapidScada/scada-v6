// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtExternalTools.Config;
using Scada.Data.Entities;
using Scada.Dbms;
using Scada.Forms;
using Scada.Lang;
using WinControls;

namespace Scada.Admin.Extensions.ExtExternalTools.Forms
{
    /// <summary>
    /// Represents a form for editing an extension configuration.
    /// <para>Представляет форму для редактирования конфигурации расширения.</para>
    /// </summary>
    public partial class FrmExtensionConfig : Form
    {
        private readonly string configFileName;           // the extension configuration file name
        private readonly ExtensionConfig extensionConfig; // the extension configuration
        private bool changing;                            // controls are being changed programmatically
        private bool modified;                            // the configration has been modified


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmExtensionConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        internal FrmExtensionConfig(string configFileName)
            : this()
        {
            ArgumentException.ThrowIfNullOrEmpty(configFileName, nameof(configFileName));
            this.configFileName = configFileName;
            extensionConfig = new ExtensionConfig();
            changing = false;
            modified = false;
        }


        /// <summary>
        /// Loads the extension configuration.
        /// </summary>
        private void LoadConfig()
        {
            if (File.Exists(configFileName) && !extensionConfig.Load(configFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Saves the extension configuration.
        /// </summary>
        private bool SaveConfig()
        {
            RetrieveTools();

            if (extensionConfig.Save(configFileName, out string errMsg))
            {
                modified = false;
                return true;
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Fills the tool list according to the configuration.
        /// </summary>
        private void FillToolList()
        {
            try
            {
                lvTool.BeginUpdate();
                lvTool.Items.Clear();

                foreach (ToolItemConfig toolItemConfig in extensionConfig.ToolItems)
                {
                    lvTool.Items.Add(CreateToolItem(toolItemConfig));
                }

                if (lvTool.Items.Count > 0)
                    lvTool.Items[0].Selected = true;
            }
            finally
            {
                lvTool.EndUpdate();
            }
        }

        /// <summary>
        /// Retrieves the tools from the tool list into the configuration.
        /// </summary>
        private void RetrieveTools()
        {
            extensionConfig.ToolItems.Clear();

            foreach (ListViewItem listViewItem in lvTool.Items)
            {
                ToolItemConfig toolItemConfig = (ToolItemConfig)listViewItem.Tag;
                extensionConfig.ToolItems.Add(toolItemConfig);
            }
        }

        /// <summary>
        /// Enables or disables the controls.
        /// </summary>
        private void SetControlsEnabled()
        {
            if (lvTool.SelectedItems.Count > 0)
            {
                int index = lvTool.SelectedIndices[0];
                btnMoveUp.Enabled = index > 0;
                btnMoveDown.Enabled = index < lvTool.Items.Count - 1;
                btnDelete.Enabled = true;
                gbTool.Enabled = true;
            }
            else
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                btnDelete.Enabled = false;
                gbTool.Enabled = false;
            }
        }

        /// <summary>
        /// Gets the selected list view item and the corresponding tool configuration.
        /// </summary>
        private bool GetSelectedItem(out ListViewItem listViewItem, out ToolItemConfig toolItemConfig)
        {
            if (lvTool.GetSelectedItem() is ListViewItem lvi &&
                lvi.Tag is ToolItemConfig tic)
            {
                listViewItem = lvi;
                toolItemConfig = tic;
                return true;
            }
            else
            {
                listViewItem = null;
                toolItemConfig = null;
                return false;
            }
        }

        /// <summary>
        /// Displays the tool item configration.
        /// </summary>
        private void DisplayToolItemConfig(ToolItemConfig toolItemConfig)
        {
            changing = true;

            if (toolItemConfig == null)
            {
                txtTitle.Text = "";
                txtFileName.Text = "";
                txtArguments.Text = "";
                txtWorkingDirectory.Text = "";
            }
            else
            {
                txtTitle.Text = toolItemConfig.Title;
                txtFileName.Text = toolItemConfig.FileName;
                txtArguments.Text = toolItemConfig.Arguments;
                txtWorkingDirectory.Text = toolItemConfig.WorkingDirectory;
            }

            changing = false;
        }

        /// <summary>
        /// Creates a new list view item that represents the specified tool item.
        /// </summary>
        private static ListViewItem CreateToolItem(ToolItemConfig toolItemConfig)
        {
            return new ListViewItem
            {
                Text = GetToolItemText(toolItemConfig),
                Tag = toolItemConfig
            };
        }

        /// <summary>
        /// Gets the display text for the tool item.
        /// </summary>
        private static string GetToolItemText(ToolItemConfig toolItemConfig)
        {
            return string.IsNullOrEmpty(toolItemConfig.Title)
                ? ExtensionPhrases.UnnamedTool
                : toolItemConfig.Title;
        }


        private void FrmExtensionConfig_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ActiveControl = lvTool;
            LoadConfig();
            FillToolList();
            SetControlsEnabled();
        }

        private void FrmExtensionConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified)
            {
                DialogResult result = MessageBox.Show(CommonPhrases.SaveConfigConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!SaveConfig())
                            e.Cancel = true;
                        break;

                    case DialogResult.No:
                        break;

                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }


        private void lvTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSelectedItem(out _, out ToolItemConfig toolItemConfig);
            DisplayToolItemConfig(toolItemConfig);
            SetControlsEnabled();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lvTool.InsertItem(CreateToolItem(new ToolItemConfig()));
            txtTitle.Focus();
            modified = true;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (lvTool.MoveUpSelectedItem())
                modified = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (lvTool.MoveDownSelectedItem())
                modified = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvTool.RemoveSelectedItem())
                modified = true;
        }


        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (!changing &&
                GetSelectedItem(out ListViewItem listViewItem, out ToolItemConfig toolItemConfig))
            {
                toolItemConfig.Title = txtTitle.Text;
                listViewItem.Text = GetToolItemText(toolItemConfig);
                modified = true;
            }
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out _, out ToolItemConfig toolItemConfig))
            {
                toolItemConfig.FileName = txtFileName.Text;
                modified = true;
            }
        }

        private void txtArguments_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out _, out ToolItemConfig toolItemConfig))
            {
                toolItemConfig.Arguments = txtArguments.Text;
                modified = true;
            }
        }

        private void txtWorkingDirectory_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out _, out ToolItemConfig toolItemConfig))
            {
                toolItemConfig.WorkingDirectory = txtWorkingDirectory.Text;
                modified = true;
            }
        }


        private void btnBrowseFile_Click(object sender, EventArgs e)
        {

        }

        private void btnAddArgument_Click(object sender, EventArgs e)
        {

        }

        private void btnBrowseDir_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SaveConfig())
                DialogResult = DialogResult.OK;
        }
    }
}
