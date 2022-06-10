// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtServerConfig.Code;
using Scada.Admin.Forms;
using Scada.Admin.Project;
using Scada.Forms;
using Scada.Server.Config;
using Scada.Server.Modules;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    /// <summary>
    /// Represents a form for editing a list of server modules.
    /// <para>Представляет форму для редактирования списка модулей сервера.</para>
    /// </summary>
    public partial class FrmModules : Form, IChildForm
    {
        /// <summary>
        /// Reprensents a module.
        /// </summary>
        private class ModuleItem
        {
            public bool IsInitialized { get; set; }
            public string ModuleCode { get; set; }
            public string FileName { get; set; }
            public string Descr { get; set; }
            public ModuleView ModuleView { get; set; }
            public override string ToString() => ModuleCode;
        }


        private readonly IAdminContext adminContext; // the Administrator context
        private readonly ServerApp serverApp;        // the server application in a project
        private readonly ServerConfig serverConfig;  // the server configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmModules()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmModules(IAdminContext adminContext, ServerApp serverApp)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.serverApp = serverApp ?? throw new ArgumentNullException(nameof(serverApp));
            serverConfig = serverApp.AppConfig;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Fills the list of unused modules.
        /// </summary>
        private void FillUnusedModules()
        {
            try
            {
                lbUnusedModules.BeginUpdate();
                lbUnusedModules.Items.Clear();

                // read all available modules
                DirectoryInfo dirInfo = new(adminContext.AppDirs.LibDir);

                if (dirInfo.Exists)
                {
                    foreach (FileInfo fileInfo in 
                        dirInfo.EnumerateFiles("Mod*.View.dll", SearchOption.TopDirectoryOnly))
                    {
                        string moduleCode = ScadaUtils.RemoveFileNameSuffixes(fileInfo.Name);
                        
                        if (!serverConfig.ModuleCodes.Contains(moduleCode))
                        {
                            lbUnusedModules.Items.Add(new ModuleItem
                            {
                                IsInitialized = false,
                                ModuleCode = moduleCode,
                                FileName = fileInfo.FullName
                            });
                        }
                    }
                }
            }
            finally
            {
                lbUnusedModules.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            try
            {
                lbActiveModules.BeginUpdate();
                lbActiveModules.Items.Clear();

                // fill the list of active modules
                foreach (string moduleCode in serverConfig.ModuleCodes)
                {
                    lbActiveModules.Items.Add(new ModuleItem
                    {
                        IsInitialized = false,
                        ModuleCode = moduleCode,
                        FileName = Path.Combine(adminContext.AppDirs.LibDir, moduleCode + ".View.dll")
                    });
                }

                // select an item
                if (lbActiveModules.Items.Count > 0)
                    lbActiveModules.SelectedIndex = 0;
            }
            finally
            {
                lbActiveModules.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            serverConfig.ModuleCodes.Clear();

            foreach (ModuleItem item in lbActiveModules.Items)
            {
                serverConfig.ModuleCodes.Add(item.ModuleCode);
            }
        }

        /// <summary>
        /// Initializes the module item if needed.
        /// </summary>
        private void InitModuleItem(ModuleItem moduleItem)
        {
            if (!moduleItem.IsInitialized)
            {
                moduleItem.IsInitialized = true;

                if (ExtensionUtils.GetModuleView(adminContext, serverApp, moduleItem.ModuleCode,
                    out ModuleView moduleView, out string message))
                {
                    moduleItem.Descr = BuildModuleDescr(moduleView);
                    moduleItem.ModuleView = moduleView;
                }
                else
                {
                    moduleItem.Descr = message;
                    moduleItem.ModuleView = null;
                }
            }
        }

        /// <summary>
        /// Shows a description of the specified item.
        /// </summary>
        private void ShowItemDescr(object item)
        {
            if (item is ModuleItem moduleItem)
            {
                InitModuleItem(moduleItem);
                txtDescr.Text = moduleItem.Descr;
            }
        }

        /// <summary>
        /// Enables or disables the buttons.
        /// </summary>
        private void SetButtonsEnabled()
        {
            btnActivate.Enabled = lbUnusedModules.SelectedItem is ModuleItem;

            if (lbActiveModules.SelectedItem is ModuleItem moduleItem)
            {
                btnDeactivate.Enabled = true;
                btnMoveUp.Enabled = lbActiveModules.SelectedIndex > 0;
                btnMoveDown.Enabled = lbActiveModules.SelectedIndex < lbActiveModules.Items.Count - 1;
                btnProperties.Enabled = moduleItem.ModuleView != null && moduleItem.ModuleView.CanShowProperties;
                btnRegister.Visible = moduleItem.ModuleView != null && moduleItem.ModuleView.RequireRegistration;
            }
            else
            {
                btnDeactivate.Enabled = false;
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                btnProperties.Enabled = false;
                btnRegister.Visible = false;
            }
        }

        /// <summary>
        /// Build the module description.
        /// </summary>
        private static string BuildModuleDescr(ModuleView moduleView)
        {
            string title = string.Format("{0} {1}",
                moduleView.Name,
                moduleView.Version);

            return new StringBuilder()
                .AppendLine(title)
                .AppendLine(new string('-', title.Length))
                .Append(moduleView.Descr?.Replace("\n", Environment.NewLine))
                .ToString();
        }

        /// <summary>
        /// Saves the changes of the child form data.
        /// </summary>
        public void Save()
        {
            ControlsToConfig();

            if (serverApp.SaveConfig(out string errMsg))
                ChildFormTag.Modified = false;
            else
                adminContext.ErrLog.HandleError(errMsg);
        }


        private void FrmModules_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ConfigToControls();
            FillUnusedModules();
            SetButtonsEnabled();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            // move the selected module from unused modules to active modules
            if (lbUnusedModules.SelectedItem is ModuleItem moduleItem)
            {
                lbUnusedModules.Items.RemoveAt(lbUnusedModules.SelectedIndex);
                lbActiveModules.SelectedIndex = lbActiveModules.Items.Add(moduleItem);
                lbActiveModules.Focus();
                ChildFormTag.Modified = true;
            }
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            // move the selected module from active modules to unused modules
            if (lbActiveModules.SelectedItem is ModuleItem moduleItem)
            {
                lbActiveModules.Items.RemoveAt(lbActiveModules.SelectedIndex);
                lbUnusedModules.SelectedIndex = lbUnusedModules.Items.Add(moduleItem);
                lbUnusedModules.Focus();
                ChildFormTag.Modified = true;
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            // move up the selected module
            if (lbActiveModules.SelectedItem is ModuleItem moduleItem)
            {
                int curInd = lbActiveModules.SelectedIndex;
                int prevInd = curInd - 1;

                if (prevInd >= 0)
                {
                    lbActiveModules.Items.RemoveAt(curInd);
                    lbActiveModules.Items.Insert(prevInd, moduleItem);
                    lbActiveModules.SelectedIndex = prevInd;
                    lbActiveModules.Focus();
                    ChildFormTag.Modified = true;
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            // move down the selected module
            if (lbActiveModules.SelectedItem is ModuleItem moduleItem)
            {
                int curInd = lbActiveModules.SelectedIndex;
                int nextInd = curInd + 1;

                if (nextInd < lbActiveModules.Items.Count)
                {
                    lbActiveModules.Items.RemoveAt(curInd);
                    lbActiveModules.Items.Insert(nextInd, moduleItem);
                    lbActiveModules.SelectedIndex = nextInd;
                    lbActiveModules.Focus();
                    ChildFormTag.Modified = true;
                }
            }
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            // show properties of the selected module
            if (lbActiveModules.SelectedItem is ModuleItem moduleItem &&
                moduleItem.ModuleView != null && moduleItem.ModuleView.CanShowProperties)
            {
                lbActiveModules.Focus();
                moduleItem.ModuleView.ShowProperties();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // show registration form for the selected module
            if (lbActiveModules.SelectedItem is ModuleItem moduleItem &&
                moduleItem.ModuleView != null && moduleItem.ModuleView.RequireRegistration)
            {
                lbActiveModules.Focus();
                new FrmRegistration(adminContext, serverApp, 
                    moduleItem.ModuleView.ProductCode, moduleItem.ModuleView.Name).ShowDialog();
            }
        }

        private void lbUnusedModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowItemDescr(lbUnusedModules.SelectedItem);
            SetButtonsEnabled();
        }

        private void lbUnusedModules_DoubleClick(object sender, EventArgs e)
        {
            btnActivate_Click(null, null);
        }

        private void lbActiveModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowItemDescr(lbActiveModules.SelectedItem);
            SetButtonsEnabled();
        }

        private void lbActiveModules_DoubleClick(object sender, EventArgs e)
        {
            btnProperties_Click(null, null);
        }
    }
}
