// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;
using WinControls;

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
        private bool mainOptionsReady;               // the main options control is displaying actual options
        private bool customOptionsReady;             // the custom options control is displaying actual options
        private bool devicePollingReady;             // the device polling control is displaying actual options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmLineConfig()
        {
            InitializeComponent();
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
            mainOptionsReady = false;
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
            if (mainOptionsReady)
                ctrlLineMain.ControlsToConfig(lineConfig);

            if (customOptionsReady)
                ctrlLineCustom.ControlsToOptions(lineConfig.CustomOptions);

            if (devicePollingReady)
                ctrlLinePolling.ControlsToConfig(lineConfig.DevicePolling);
        }

        /// <summary>
        /// Updates the main form according to the configuration changes.
        /// </summary>
        private void UpdateMainForm()
        {
            if (ChildFormTag?.TreeNode?.FindClosest(CommNodeType.Line) is not TreeNode lineNode)
                return;

            // close or update child forms
            foreach (TreeNode node in lineNode.Nodes)
            {
                if (node.Tag is TreeNodeTag tag && tag.ExistingForm is IChildForm childForm)
                {
                    if (node.TagIs(CommNodeType.Device))
                        adminContext.MainForm.CloseChildForm(tag.ExistingForm, false);
                    else if (node.TagIs(CommNodeType.LineStats))
                        childForm.ChildFormTag.SendMessage(this, AdminMessage.UpdateFileName);
                }
            }

            // update explorer
            new TreeViewBuilder(adminContext, ExtensionUtils.MenuControl).UpdateLineNode(lineNode);

            // update tab hints
            adminContext.MainForm.UpdateChildFormHints(lineNode);
        }

        /// <summary>
        /// Saves the changes of the child form data.
        /// </summary>
        public void Save()
        {
            ControlsToConfig();

            if (commApp.SaveConfig(out string errMsg))
            {
                Text = string.Format(ExtensionPhrases.LineConfigTitle, lineConfig.CommLineNum);
                UpdateMainForm();
                ChildFormTag.Modified = false;
            }
            else
            {
                adminContext.ErrLog.HandleError(errMsg);
            }
        }


        private void FrmLineOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            Text = string.Format(ExtensionPhrases.LineConfigTitle, lineConfig.CommLineNum);

            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
            ctrlLineMain.Init(adminContext, commApp);
            ctrlLinePolling.Init(adminContext, commApp, lineConfig);
            lbTabs.SelectedIndex = 0;
        }

        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            // refresh displayed configuration
            if (e.Message == AdminMessage.RefreshData)
            {
                if (mainOptionsReady)
                    ctrlLineMain.ConfigToControls(lineConfig);

                if (customOptionsReady)
                    ctrlLineCustom.OptionsToControls(lineConfig.CustomOptions);

                if (devicePollingReady)
                    ctrlLinePolling.ConfigToControls(lineConfig.DevicePolling);

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
            ctrlLineMain.Visible = tabIndex == 0;
            ctrlLineCustom.Visible = tabIndex == 1;
            ctrlLinePolling.Visible = tabIndex == 2;

            if (ctrlLineMain.Visible && !mainOptionsReady)
            {
                ctrlLineMain.ConfigToControls(lineConfig);
                mainOptionsReady = true;
            }

            if (ctrlLineCustom.Visible && !customOptionsReady)
            {
                ctrlLineCustom.OptionsToControls(lineConfig.CustomOptions);
                customOptionsReady = true;
            }

            if (ctrlLinePolling.Visible && !devicePollingReady)
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
