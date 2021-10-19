// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Forms;
using System;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for editing communication line options.
    /// <para>Форма для редактирования параметров линии связи.</para>
    /// </summary>
    public partial class FrmLineConfig : Form, IChildForm
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly CommApp commApp;            // the Communicator application in a project
        private readonly LineConfig lineConfig;      // the communication line configuration
        private bool customOptionsReady;             // the custom options control is displaying actual options
        private bool devicePollingReady;             // the device polling control is displaying actual options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmLineConfig()
        {
            InitializeComponent();
            ctrlLineCustom.Dock = DockStyle.Fill;
            ctrlLinePolling.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLineConfig(IAdminContext adminContext, CommApp commApp, LineConfig lineConfig)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.commApp = commApp ?? throw new ArgumentNullException(nameof(commApp));
            this.lineConfig = lineConfig ?? throw new ArgumentNullException(nameof(lineConfig));
            customOptionsReady = false;
            devicePollingReady = false;
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
            ctrlLineMain.ControlsToConfig(lineConfig);

            if (customOptionsReady)
                ctrlLineCustom.ControlsToOptions(lineConfig.CustomOptions);

            if (devicePollingReady)
                ctrlLinePolling.ControlsToConfig(lineConfig.DevicePolling);
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


        private void FrmLineOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ctrlLineMain.ConfigToControls(lineConfig);
            ctrlLinePolling.Init(adminContext, commApp, lineConfig);
            lbTabs.SelectedIndex = 0;
        }

        private void lbTabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            lbTabs.DrawTabItem(e);
        }

        private void lbTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tabIndex = lbTabs.SelectedIndex;
            ctrlLineMain.Visible = tabIndex == 0;
            ctrlLineCustom.Visible = tabIndex == 1;
            ctrlLinePolling.Visible = tabIndex == 2;

            if (ctrlLineCustom.Visible)
            {
                ctrlLineCustom.OptionsToControls(lineConfig.CustomOptions);
                customOptionsReady = true;
            }

            if (ctrlLinePolling.Visible)
            {
                ctrlLinePolling.ConfigToControls(lineConfig.DevicePolling);
                devicePollingReady = true;
            }
        }

        private void ctrlLineMain_ConfigChanged(object sender, EventArgs e)
        {
            ChildFormTag.Modified = true;
        }

        private void ctrlLineCustom_OptionsChanged(object sender, EventArgs e)
        {
            ChildFormTag.Modified = true;
        }

        private void ctrlLinePolling_ConfigChanged(object sender, EventArgs e)
        {
            ChildFormTag.Modified = true;
        }

        private void ctrlLinePolling_LineConfigChanged(object sender, EventArgs e)
        {
            ctrlLineMain.ConfigToControls(lineConfig);
            ctrlLineCustom.OptionsToControls(lineConfig.CustomOptions);
        }
    }
}
