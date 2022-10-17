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
                btnProperties.Enabled = false;
                btnRegister.Visible = false;
            }
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        public void Save()
        {
        }


        private void FrmPlugins_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ConfigToControls();
            FillUnusedModules();
            SetButtonsEnabled();
        }
    }
}
