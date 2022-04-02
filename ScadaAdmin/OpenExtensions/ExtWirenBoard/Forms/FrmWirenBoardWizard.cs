// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code;
using Scada.Admin.Extensions.ExtWirenBoard.Controls;
using Scada.Admin.Project;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtWirenBoard.Forms
{
    /// <summary>
    /// Represents a wizard form for creating a project configuration.
    /// <para>Представляет форму мастера для создания конфигурации проекта.</para>
    /// </summary>
    internal partial class FrmWirenBoardWizard : Form
    {
        /// <summary>
        /// Specifies the wizard steps.
        /// </summary>
        private enum Step { SelectLine, ReadTopics, SelectDevices, SetEntityIDs, CheckConfig }

        /// <summary>
        /// Specifies the step offsets.
        /// </summary>
        private enum StepOffset { Back = -1, None = 0, Next = 1 }

        private readonly IAdminContext adminContext;      // the Administrator context
        private readonly ScadaProject project;            // the project under development
        private readonly RecentSelection recentSelection; // the recently selected parameters

        private readonly CtrlLineSelect ctrlLineSelect;
        private readonly CtrlLog ctrlLog;
        private readonly CtrlDeviceTree ctrlDeviceTree;
        private readonly CtrlEntityID ctrlEntityID;
        private readonly RichTextBoxHelper logHelper;

        private Step step;               // the current waizard step
        private TopicReader topicReader; // reads Wiren Board topics


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmWirenBoardWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmWirenBoardWizard(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));

            ctrlLineSelect = new CtrlLineSelect(adminContext, project, recentSelection) 
            { 
                Dock = DockStyle.Fill, 
                Visible = false 
            };

            ctrlLog = new CtrlLog { Dock = DockStyle.Fill, Visible = false };
            ctrlDeviceTree = new CtrlDeviceTree { Dock = DockStyle.Fill, Visible = false };
            ctrlEntityID = new CtrlEntityID(adminContext, project) { Dock = DockStyle.Fill, Visible = false };
            logHelper = new RichTextBoxHelper(ctrlLog.RichTextBox);

            pnlMain.Controls.Add(ctrlLineSelect);
            pnlMain.Controls.Add(ctrlLog);
            pnlMain.Controls.Add(ctrlDeviceTree);
            pnlMain.Controls.Add(ctrlEntityID);

            step = Step.SelectLine;
            topicReader = null;
        }


        /// <summary>
        /// Validates the current step.
        /// </summary>
        private bool ValidateStep()
        {
            return step switch
            {
                Step.SelectLine => ctrlLineSelect.ValidateControl(),
                Step.SelectDevices => ctrlDeviceTree.ValidateControl(),
                _ => true
            };
        }

        /// <summary>
        /// Makes a wizard step.
        /// </summary>
        private void MakeStep(StepOffset stepOffset)
        {
            if (stepOffset == StepOffset.Back && step > Step.SelectLine)
                step--;
            if (stepOffset == StepOffset.Next && step < Step.CheckConfig)
                step++;

            ctrlLineSelect.Visible = false;
            ctrlLog.Visible = false;
            ctrlDeviceTree.Visible = false;
            ctrlEntityID.Visible = false;
            btnBack.Visible = true;
            btnBack.Enabled = true;
            btnNext.Visible = true;
            btnNext.Enabled = true;
            btnCreate.Visible = false;

            switch (step)
            {
                case Step.SelectLine:
                    lblStep.Text = ExtensionPhrases.Step1Descr;
                    ctrlLineSelect.Visible = true;
                    ctrlLineSelect.SetFocus();
                    btnBack.Visible = false;
                    break;

                case Step.ReadTopics:
                    lblStep.Text = ExtensionPhrases.Step2Descr;
                    ctrlLineSelect.RememberRecentSelection();
                    ctrlLog.Visible = true;
                    ctrlLog.SetFocus();
                    logHelper.Clear();
                    btnBack.Enabled = false;
                    btnNext.Enabled = false;

                    // start reading topics
                    topicReader = new TopicReader(ctrlLineSelect.GetMqttConnectionOptions(), logHelper);
                    topicReader.Completed += TopicReader_Completed;
                    topicReader.Start();
                    break;

                case Step.SelectDevices:
                    lblStep.Text = ExtensionPhrases.Step3Descr;
                    ctrlDeviceTree.Visible = true;
                    ctrlDeviceTree.SetFocus();

                    if (stepOffset == StepOffset.Next)
                        ctrlDeviceTree.ShowModel(topicReader.WirenBoardModel);
                    break;

                case Step.SetEntityIDs:
                    lblStep.Text = ExtensionPhrases.Step4Descr;
                    ctrlEntityID.Visible = true;
                    ctrlEntityID.SetFocus();

                    if (stepOffset == StepOffset.Next)
                        ctrlEntityID.AssignIDs();
                    break;

                case Step.CheckConfig:
                    lblStep.Text = ExtensionPhrases.Step5Descr;
                    btnNext.Visible = false;
                    btnCreate.Visible = true;
                    break;
            }
        }


        private void TopicReader_Completed(object sender, EventArgs e)
        {
            btnBack.Enabled = true;
            btnNext.Enabled = ((TopicReader)sender).ReadResult;
        }

        private void FrmWirenBoardWizard_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlLineSelect, ctrlLineSelect.GetType().FullName);
            FormTranslator.Translate(ctrlLog, ctrlLog.GetType().FullName);
            FormTranslator.Translate(ctrlDeviceTree, ctrlDeviceTree.GetType().FullName);
            FormTranslator.Translate(ctrlEntityID, ctrlEntityID.GetType().FullName);

            MakeStep(StepOffset.None);
        }

        private void FrmWirenBoardWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            topicReader?.Break();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MakeStep(StepOffset.Back);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ValidateStep())
                MakeStep(StepOffset.Next);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

        }
    }
}
