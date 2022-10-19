// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Forms;
using Scada.Web.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Web;
using WinControl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Scada.Admin.Forms;

namespace Scada.Admin.Extensions.ExtWebConfig.Forms
{
    /// <summary>
    /// Represents a form for editing a web config.
    /// <para>Представляет форму для редактирования настроек web-интерфейса.</para>
    /// </summary>
    public partial class FrmPlugins : Form, IChildForm
    {
        /// <summary>
        /// Reprensents a plugin.
        /// </summary>
        private class PluginItem
        {
            public bool IsInitialized { get; set; }
            public string PluginCode { get; set; }
            public string FileName { get; set; }
            public string Descr { get; set; }
            public override string ToString() => PluginCode;
        }


        private readonly IAdminContext adminContext; // the Administrator context
        private readonly WebApp webApp;              // the web application in a project
        private readonly WebConfig webConfig;        // the web configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPlugins()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmPlugins(IAdminContext adminContext, WebApp webApp)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.webApp = webApp ?? throw new ArgumentNullException(nameof(webApp));
            webConfig = webApp.AppConfig;
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
                lbUnusedPlugins.BeginUpdate();
                lbUnusedPlugins.Items.Clear();

                // read all available plugins
                DirectoryInfo dirInfo = new(adminContext.AppDirs.LibDir);

                if (dirInfo.Exists)
                {
                    foreach (FileInfo fileInfo in
                        dirInfo.EnumerateFiles("Plg*.dll", SearchOption.TopDirectoryOnly))
                    {
                        string pluginCode = ScadaUtils.RemoveFileNameSuffixes(fileInfo.Name);
                        
                        if (!webConfig.PluginCodes.Contains(pluginCode))
                        {
                            lbUnusedPlugins.Items.Add(new PluginItem
                            {
                                IsInitialized = false,
                                PluginCode = pluginCode,
                                FileName = fileInfo.FullName
                            });
                        }
                    }
                }
            }
            finally
            {
                lbUnusedPlugins.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            try
            {
                lbActivePlugins.BeginUpdate();
                lbActivePlugins.Items.Clear();

                // fill the list of active plugins
                foreach (string pluginCode in webConfig.PluginCodes)
                {
                    lbActivePlugins.Items.Add(new PluginItem
                    {
                        IsInitialized = false,
                        PluginCode = pluginCode,
                        FileName = Path.Combine(adminContext.AppDirs.LibDir, pluginCode + ".dll")
                    });
                }

                // select an item
                if (lbActivePlugins.Items.Count > 0)
                    lbActivePlugins.SelectedIndex = 0;
            }
            finally
            {
                lbActivePlugins.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            webConfig.PluginCodes.Clear();

            foreach (PluginItem item in lbActivePlugins.Items)
            {
                webConfig.PluginCodes.Add(item.PluginCode);
            }
        }

        /// <summary>
        /// Initializes the plugin item if needed.
        /// </summary>
        private void InitPluginItem(PluginItem pluginItem)
        {
            /*if (!pluginItem.IsInitialized)
            {
                pluginItem.IsInitialized = true;

                if (ExtensionUtils.GetModuleView(adminContext, webApp, pluginItem.PluginCode,
                    out ModuleView moduleView, out string message))
                {
                    pluginItem.Descr = BuildModuleDescr(moduleView);
                    //pluginItem.ModuleView = moduleView;
                }
                else
                {
                    pluginItem.Descr = message;
                    //pluginItem.ModuleView = null;
                }
            }*/
        }

        /// <summary>
        /// Shows a description of the specified item.
        /// </summary>
        private void ShowItemDescr(object item)
        {
            if (item is PluginItem pluginItem)
            {
                InitPluginItem(pluginItem);
                txtDescr.Text = pluginItem.Descr;
            }
        }

        /// <summary>
        /// Enables or disables the buttons.
        /// </summary>
        private void SetButtonsEnabled()
        {
            btnActivate.Enabled = lbUnusedPlugins.SelectedItem is PluginItem;

            if (lbActivePlugins.SelectedItem is PluginItem pluginItem)
            {
                btnDeactivate.Enabled = true;
                btnMoveUp.Enabled = lbActivePlugins.SelectedIndex > 0;
                btnMoveDown.Enabled = lbActivePlugins.SelectedIndex < lbActivePlugins.Items.Count - 1;
                //btnProperties.Enabled = pluginItem.ModuleView != null && pluginItem.ModuleView.CanShowProperties;
                //btnRegister.Visible = pluginItem.ModuleView != null && pluginItem.ModuleView.RequireRegistration;
            }
            else
            {
                btnDeactivate.Enabled = false;
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                //btnRegister.Visible = false;
            }
        }

        // <summary>
        /// Build the plugin description.
        /// </summary>
        /**private static string BuildModuleDescr(ModuleView moduleView)
        {
            string title = string.Format("{0} {1}",
                moduleView.Name,
                moduleView.Version);

            return new StringBuilder()
                .AppendLine(title)
                .AppendLine(new string('-', title.Length))
                .Append(moduleView.Descr?.Replace("\n", Environment.NewLine))
                .ToString();
        }*/


        /// <summary>
        /// Saves the file.
        /// </summary>
        public void Save()
        {
            ControlsToConfig();

            if (webApp.SaveConfig(out string errMsg))
                ChildFormTag.Modified = false;
            else
                adminContext.ErrLog.HandleError(errMsg);
        }


        private void FrmPlugins_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ConfigToControls();
            FillUnusedModules();
            SetButtonsEnabled();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            // move the selected plugin from unused plugins to active plugins
            if (lbUnusedPlugins.SelectedItem is PluginItem pluginItem)
            {
                lbUnusedPlugins.Items.RemoveAt(lbUnusedPlugins.SelectedIndex);
                lbActivePlugins.SelectedIndex = lbActivePlugins.Items.Add(pluginItem);
                lbActivePlugins.Focus();
                ChildFormTag.Modified = true;
            }
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            // move the selected plugin from active plugins to unused plugins
            if (lbActivePlugins.SelectedItem is PluginItem pluginItem)
            {
                lbActivePlugins.Items.RemoveAt(lbActivePlugins.SelectedIndex);
                lbUnusedPlugins.SelectedIndex = lbUnusedPlugins.Items.Add(pluginItem);
                lbUnusedPlugins.Focus();
                ChildFormTag.Modified = true;
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            // move up the selected plugin
            if (lbActivePlugins.SelectedItem is PluginItem pluginItem)
            {
                int curInd = lbActivePlugins.SelectedIndex;
                int prevInd = curInd - 1;

                if (prevInd >= 0)
                {
                    lbActivePlugins.Items.RemoveAt(curInd);
                    lbActivePlugins.Items.Insert(prevInd, pluginItem);
                    lbActivePlugins.SelectedIndex = prevInd;
                    lbActivePlugins.Focus();
                    ChildFormTag.Modified = true;
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            // move down the selected module
            if (lbActivePlugins.SelectedItem is PluginItem pluginItem)
            {
                int curInd = lbActivePlugins.SelectedIndex;
                int nextInd = curInd + 1;

                if (nextInd < lbActivePlugins.Items.Count)
                {
                    lbActivePlugins.Items.RemoveAt(curInd);
                    lbActivePlugins.Items.Insert(nextInd, pluginItem);
                    lbActivePlugins.SelectedIndex = nextInd;
                    lbActivePlugins.Focus();
                    ChildFormTag.Modified = true;
                }
            }
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            // show properties of the selected plugin
            /*if (lbActivePlugins.SelectedItem is PluginItem pluginItem &&
                pluginItem.ModuleView != null && pluginItem.ModuleView.CanShowProperties)
            {
                lbActivePlugins.Focus();
                pluginItem.ModuleView.ShowProperties();
            }*/
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // show registration form for the selected plugin
            /*if (lbActivePlugins.SelectedItem is PluginItem pluginItem &&
                pluginItem.ModuleView != null && moduleItem.ModuleView.RequireRegistration)
            {
                lbActivePlugins.Focus();
                new FrmRegistration(adminContext, webApp,
                    pluginItem.ModuleView.ProductCode, pluginItem.ModuleView.Name).ShowDialog();
            }*/
        }

        private void lbUnusedPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowItemDescr(lbUnusedPlugins.SelectedItem);
            SetButtonsEnabled();
        }

        private void lbUnusedPlugins_DoubleClick(object sender, EventArgs e)
        {
            btnActivate_Click(null, null);
        }

        private void lbActivePlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowItemDescr(lbActivePlugins.SelectedItem);
            SetButtonsEnabled();
        }

        private void lbActivePlugins_DoubleClick(object sender, EventArgs e)
        {
            btnProperties_Click(null, null);
        }
    }
}
