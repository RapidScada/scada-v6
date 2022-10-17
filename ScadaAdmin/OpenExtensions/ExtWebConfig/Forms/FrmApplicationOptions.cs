// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWebConfig.Code;
using Scada.Admin.Project;
using Scada.Forms;
using Scada.Web.Config;
using WinControl;

namespace Scada.Admin.Extensions.ExtWebConfig.Forms
{
    /// <summary>
    /// Represents a form for editing a web config.
    /// <para>Представляет форму для редактирования настроек web-интерфейса.</para>
    /// </summary>
    public partial class FrmApplicationOptions : Form, IChildForm
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly WebApp webApp;              // the web application in a project
        private readonly WebConfig webConfig;        // the web configuration
        private bool generalOptionsReady;            // the general options control is displaying actual options
        private bool connectionOptionsReady;         // the connection options control is displaying actual options
        private bool loginOptionsReady;              // the login options control is displaying actual options
        private bool displayOptionsReady;            // the display control is displaying actual options
        private bool pluginAssigmentReady;           // the polling assigment control is displaying actual options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmApplicationOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmApplicationOptions(IAdminContext adminContext, WebApp webApp)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.webApp = webApp ?? throw new ArgumentNullException(nameof(webApp));
            webConfig = webApp.AppConfig;
            generalOptionsReady = false;
            connectionOptionsReady = false;
            loginOptionsReady = false;
            displayOptionsReady = false;
            pluginAssigmentReady = false;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfig()
        {
            if (generalOptionsReady)
                ctrlGeneralOptions.ControlsToOptions(webConfig.GeneralOptions);
            
            if (connectionOptionsReady)
                ctrlConnectionOptions.ControlsToOptions(webConfig.ConnectionOptions);

            if (loginOptionsReady)
                ctrlLoginOptions.ControlsToOptions(webConfig.LoginOptions);

            if (displayOptionsReady)
                ctrlDisplayOptions.ControlsToOptions(webConfig.DisplayOptions);
            
            if (pluginAssigmentReady)
                ctrlPluginAssignment.ControlsToOptions(webConfig.PluginAssignment);
        }
        

        /// <summary>
        /// Saves the file.
        /// </summary>
        public void Save()
        {
            ControlsToConfig();

            if (webApp.SaveConfig(out string errMsg))
            { 
                Text = ExtensionPhrases.ApplicationConfigTitle;
                ChildFormTag.Modified = false;
            }
            else
            {
                adminContext.ErrLog.HandleError(errMsg);
            }
        }


        private void FrmApplicationOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            Text = ExtensionPhrases.ApplicationConfigTitle;

            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
            ctrlGeneralOptions.Init(adminContext, webApp);
            ctrlConnectionOptions.Init(adminContext, webApp);
            ctrlLoginOptions.Init(adminContext, webApp);
            ctrlDisplayOptions.Init(adminContext, webApp);
            ctrlPluginAssignment.Init(adminContext, webApp);
            lbTabs.SelectedIndex = 0;
        }

        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            // refresh displayed configuration
            if (e.Message == AdminMessage.RefreshData)
            {
                if (generalOptionsReady)
                    ctrlGeneralOptions.OptionsToControls(webConfig.GeneralOptions);
                
                if (connectionOptionsReady)
                    ctrlConnectionOptions.OptionsToControls(webConfig.ConnectionOptions);

                if (loginOptionsReady)
                    ctrlLoginOptions.OptionsToControls(webConfig.LoginOptions);

                if (displayOptionsReady)
                    ctrlDisplayOptions.OptionsToControls(webConfig.DisplayOptions);

                if (pluginAssigmentReady)
                    ctrlPluginAssignment.OptionsToControls(webConfig.PluginAssignment);

                ChildFormTag.Modified = false;
            }
        }

        private void lbTabs_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            SizeF textSize = e.Graphics.MeasureString("0", lbTabs.Font);
            e.ItemHeight = (int)(textSize.Height * 1.5);
        }

        private void lbTabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            lbTabs.DrawTabItem(e);
        }

        private void lbTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tabIndex = lbTabs.SelectedIndex;
            ctrlGeneralOptions.Visible = tabIndex == 0;
            ctrlConnectionOptions.Visible = tabIndex == 1;
            ctrlLoginOptions.Visible = tabIndex == 2;
            ctrlDisplayOptions.Visible = tabIndex == 3;
            ctrlPluginAssignment.Visible = tabIndex == 4;

            if (ctrlGeneralOptions.Visible && !generalOptionsReady)
            {
                ctrlGeneralOptions.OptionsToControls(webConfig.GeneralOptions);
                generalOptionsReady = true;
            }

            if (ctrlConnectionOptions.Visible && !connectionOptionsReady)
            {
                ctrlConnectionOptions.OptionsToControls(webConfig.ConnectionOptions);
                connectionOptionsReady = true;
            }

            if (ctrlLoginOptions.Visible && !loginOptionsReady)
            {
                ctrlLoginOptions.OptionsToControls(webConfig.LoginOptions);
                loginOptionsReady = true;
            }
            
            if (ctrlDisplayOptions.Visible && !displayOptionsReady)
            {
                ctrlDisplayOptions.OptionsToControls(webConfig.DisplayOptions);
                displayOptionsReady = true;
            }

            if (ctrlPluginAssignment.Visible && !pluginAssigmentReady)
            {
                ctrlPluginAssignment.OptionsToControls(webConfig.PluginAssignment);
                pluginAssigmentReady = true;
            }
        }

        private void ctrlPluginAssignment_OptionsChanged(object sender, EventArgs e)
        {
            ChildFormTag.Modified = true;
        }

        private void ctrlLoginOptions_OptionsChanged(object sender, EventArgs e)
        {
            ChildFormTag.Modified = true;
        }

        private void ctrlGeneralOptions_OptionsChanged(object sender, EventArgs e)
        {
            ChildFormTag.Modified = true;
        }

        private void ctrlDisplayOptions_OptionsChanged(object sender, EventArgs e)
        {
            ChildFormTag.Modified = true;
        }

        private void ctrlConnectionOptions_OptionsChanged(object sender, EventArgs e)
        {
            ChildFormTag.Modified = true;
        }
    }
}
