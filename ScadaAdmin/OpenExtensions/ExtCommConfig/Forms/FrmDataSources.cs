// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Drivers;
using Scada.Forms;
using System;
using System.IO;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for editing data sources.
    /// <para>Представляет форму для редактирования источников данных.</para>
    /// </summary>
    public partial class FrmDataSources : Form, IChildForm
    {
        private readonly IAdminContext adminContext;  // the Administrator context
        private readonly CommApp commApp;             // the Communicator application in a project
        private readonly CommConfig commConfig;       // the Communicator configuration
        private bool changing;                        // controls are being changed programmatically
        private DataSourceConfig dataSourceClipboard; // contains the copied data source


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDataSources()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDataSources(IAdminContext adminContext, CommApp commApp)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.commApp = commApp ?? throw new ArgumentNullException(nameof(commApp));
            commConfig = commApp.AppConfig;
            changing = false;
            dataSourceClipboard = null;
            SetColumnNames();
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Sets the column names as needed for translation.
        /// </summary>
        private void SetColumnNames()
        {
            colOrder.Name = nameof(colOrder);
            colActive.Name = nameof(colActive);
            colCode.Name = nameof(colCode);
            colName.Name = nameof(colName);
            colDriver.Name = nameof(colDriver);
        }

        /// <summary>
        /// Fills the combo box by the drivers that support data sources.
        /// </summary>
        private void FillDriverComboBox()
        {
            try
            {
                cbDriver.BeginUpdate();
                cbDriver.Items.Clear();
                DirectoryInfo dirInfo = new(adminContext.AppDirs.LibDir);

                foreach (FileInfo fileInfo in
                    dirInfo.EnumerateFiles("DrvDs*.View.dll", SearchOption.TopDirectoryOnly))
                {
                    cbDriver.Items.Add(ScadaUtils.RemoveFileNameSuffixes(fileInfo.Name));
                }
            }
            finally
            {
                cbDriver.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            try
            {
                lvDataSource.BeginUpdate();
                lvDataSource.Items.Clear();
                int index = 0;

                foreach (DataSourceConfig dataSourceConfig in commConfig.DataSources)
                {
                    lvDataSource.Items.Add(CreateDataSourceItem(dataSourceConfig.DeepClone(), ref index));
                }

                if (lvDataSource.Items.Count > 0)
                    lvDataSource.Items[0].Selected = true;
            }
            finally
            {
                lvDataSource.EndUpdate();
            }
        }
        
        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            commConfig.DataSources.Clear();

            foreach (ListViewItem item in lvDataSource.Items)
            {
                commConfig.DataSources.Add((DataSourceConfig)item.Tag);
            }
        }

        /// <summary>
        /// Enables or disables the controls.
        /// </summary>
        private void SetControlsEnabled()
        {
            if (lvDataSource.SelectedItems.Count > 0)
            {
                int index = lvDataSource.SelectedIndices[0];
                btnMoveUp.Enabled = index > 0;
                btnMoveDown.Enabled = index < lvDataSource.Items.Count - 1;
                btnDelete.Enabled = true;
                btnCut.Enabled = true;
                btnCopy.Enabled = true;
                gbDataSource.Enabled = true;
            }
            else
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                btnDelete.Enabled = false;
                btnCut.Enabled = false;
                btnCopy.Enabled = false;
                gbDataSource.Enabled = false;
            }
        }

        /// <summary>
        /// Gets the selected list view item and the corresponding data source configuration.
        /// </summary>
        private bool GetSelectedItem(out ListViewItem item, out DataSourceConfig dataSourceConfig)
        {
            if (lvDataSource.SelectedItems.Count > 0)
            {
                item = lvDataSource.SelectedItems[0];
                dataSourceConfig = (DataSourceConfig)item.Tag;
                return true;
            }
            else
            {
                item = null;
                dataSourceConfig = null;
                return false;
            }
        }

        /// <summary>
        /// Displays the specified data source properties.
        /// </summary>
        private void DisplayDataSource(DataSourceConfig dataSourceConfig)
        {
            if (dataSourceConfig == null)
            {
                chkActive.Checked = false;
                txtCode.Text = "";
                txtName.Text = "";
                cbDriver.Text = "";
                txtOptions.Text = "";
            }
            else
            {
                chkActive.Checked = dataSourceConfig.Active;
                txtCode.Text = dataSourceConfig.Code;
                txtName.Text = dataSourceConfig.Name;
                cbDriver.Text = dataSourceConfig.Driver;
                txtOptions.Text = dataSourceConfig.CustomOptions.ToString();
            }
        }

        /// <summary>
        /// Adds an item to the list view according to the specified data source.
        /// </summary>
        private void AddDataSourceItem(DataSourceConfig dataSourceConfig)
        {
            int index = 0;
            lvDataSource.InsertItem(CreateDataSourceItem(dataSourceConfig, ref index), true);
            txtCode.Focus();
            ChildFormTag.Modified = true;
        }

        /// <summary>
        /// Creates a new list view item that represents the specified data source.
        /// </summary>
        private static ListViewItem CreateDataSourceItem(DataSourceConfig dataSourceConfig, ref int index)
        {
            return new ListViewItem(new string[]
            {
                (++index).ToString(),
                AdminUtils.GetCheckedString(dataSourceConfig.Active),
                dataSourceConfig.Code,
                dataSourceConfig.Name,
                dataSourceConfig.Driver
            })
            {
                Tag = dataSourceConfig
            };
        }

        /// <summary>
        /// Saves the changes of the child form data.
        /// </summary>
        public void Save()
        {
            ControlsToConfig();

            if (commApp.SaveConfig(out string errMsg))
                ChildFormTag.Modified = false;
            else
                adminContext.ErrLog.HandleError(errMsg);
        }


        private void FrmDataSources_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FillDriverComboBox();
            SetControlsEnabled();
            ConfigToControls();
            btnPaste.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddDataSourceItem(new DataSourceConfig { Active = true });
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (lvDataSource.MoveUpSelectedItem(true))
                ChildFormTag.Modified = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (lvDataSource.MoveDownSelectedItem(true))
                ChildFormTag.Modified = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvDataSource.RemoveSelectedItem(true))
                ChildFormTag.Modified = true;
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
            // cut the selected data source
            btnCopy_Click(null, null);
            btnDelete_Click(null, null);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            // copy the selected data source
            if (GetSelectedItem(out _, out DataSourceConfig dataSourceConfig))
            {
                btnPaste.Enabled = true;
                dataSourceClipboard = dataSourceConfig.DeepClone();
            }

            lvDataSource.Focus();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            // paste the copied data source
            if (dataSourceClipboard == null)
                lvDataSource.Focus();
            else
                AddDataSourceItem(dataSourceClipboard.DeepClone());
        }

        private void lvDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            // display the selected item properties
            changing = true;
            GetSelectedItem(out _, out DataSourceConfig dataSourceConfig);
            DisplayDataSource(dataSourceConfig);
            SetControlsEnabled();
            changing = false;
        }

        private void lvDataSource_DoubleClick(object sender, EventArgs e)
        {
            btnProperties_Click(null, null);
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DataSourceConfig dataSourceConfig))
            {
                dataSourceConfig.Active = chkActive.Checked;
                item.SubItems[1].Text = AdminUtils.GetCheckedString(chkActive.Checked);
                ChildFormTag.Modified = true;
            }
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DataSourceConfig dataSourceConfig))
            {
                dataSourceConfig.Code = txtCode.Text;
                item.SubItems[2].Text = txtCode.Text;
                ChildFormTag.Modified = true;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DataSourceConfig dataSourceConfig))
            {
                dataSourceConfig.Name = txtName.Text;
                item.SubItems[3].Text = txtName.Text;
                ChildFormTag.Modified = true;
            }
        }

        private void cbDriver_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out ListViewItem item, out DataSourceConfig dataSourceConfig))
            {
                dataSourceConfig.Driver = cbDriver.Text;
                item.SubItems[4].Text = cbDriver.Text;
                ChildFormTag.Modified = true;
            }
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            // show data source properties
            if (GetSelectedItem(out _, out DataSourceConfig dataSourceConfig))
            {
                if (string.IsNullOrEmpty(dataSourceConfig.Driver))
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.DriverNotSpecified);
                }
                else if (!ExtensionUtils.GetDriverView(adminContext, commApp, dataSourceConfig.Driver,
                    out DriverView driverView, out string message))
                {
                    ScadaUiUtils.ShowError(message);
                }
                else if (!driverView.CanCreateDataSource)
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.DataSourceNotSupported);
                }
                else if (driverView.CreateDataSourceView(dataSourceConfig) is not DataSourceView dataSourceView)
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.UnableCreateDataSourceView);
                }
                else if (!dataSourceView.CanShowProperties)
                {
                    ScadaUiUtils.ShowInfo(ExtensionPhrases.NoDataSourceProperties);
                }
                else if (dataSourceView.ShowProperties())
                {
                    DisplayDataSource(dataSourceConfig);
                    ChildFormTag.Modified = true;
                }
            }
        }
    }
}
