// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code;
using Scada.Admin.Extensions.ExtWirenBoard.Controls;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Data.Entities;
using Scada.Data.Tables;
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

        private readonly CtrlLineSelect ctrlLineSelect;
        private readonly CtrlLog ctrlLog;
        private readonly CtrlDeviceTree ctrlDeviceTree;
        private readonly CtrlEntityID ctrlEntityID;
        private readonly RichTextBoxHelper logHelper;

        private Step step;                   // the current waizard step
        private TopicReader topicReader;     // reads Wiren Board topics
        private ConfigBuilder configBuilder; // builds a project configuration


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

            ctrlLineSelect = new CtrlLineSelect(adminContext, project, recentSelection);
            ctrlLog = new CtrlLog();
            ctrlDeviceTree = new CtrlDeviceTree();
            ctrlEntityID = new CtrlEntityID(adminContext, project, recentSelection);
            logHelper = new RichTextBoxHelper(ctrlLog.RichTextBox);
            AddUserControls();

            step = Step.SelectLine;
            topicReader = null;
            configBuilder = null;
        }


        /// <summary>
        /// Gets the selected instance.
        /// </summary>
        public ProjectInstance Instance => ctrlLineSelect.Instance;

        /// <summary>
        /// Gets the selected communication line.
        /// </summary>
        public LineConfig Line => ctrlLineSelect.Line;


        /// <summary>
        /// Adds the user controls to the form.
        /// </summary>
        private void AddUserControls()
        {
            ctrlLineSelect.Dock = DockStyle.Fill;
            ctrlLog.Dock = DockStyle.Fill;
            ctrlDeviceTree.Dock = DockStyle.Fill;
            ctrlEntityID.Dock = DockStyle.Fill;

            ctrlLineSelect.Visible = false;
            ctrlLog.Visible = false;
            ctrlDeviceTree.Visible = false;
            ctrlEntityID.Visible = false;

            pnlMain.Controls.Add(ctrlLineSelect);
            pnlMain.Controls.Add(ctrlLog);
            pnlMain.Controls.Add(ctrlDeviceTree);
            pnlMain.Controls.Add(ctrlEntityID);
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
                    ctrlEntityID.RememberRecentSelection();
                    ctrlLog.Visible = true;
                    ctrlLog.SetFocus();
                    logHelper.Clear();
                    btnNext.Visible = false;
                    btnCreate.Visible = true;

                    // build project configuration
                    configBuilder = new ConfigBuilder(adminContext, project, logHelper);
                    configBuilder.Build(ctrlDeviceTree.GetSelectedDevices(), ctrlLineSelect.Line.CommLineNum,
                        ctrlEntityID.StartDeviceNum, ctrlEntityID.StartCnlNum, ctrlEntityID.ObjNum);
                    btnCreate.Enabled = configBuilder.BuildResult;
                    break;
            }
        }

        /// <summary>
        /// Creates a project configuration.
        /// </summary>
        private bool CreateConfig()
        {
            try
            {
                if (configBuilder != null && ctrlLineSelect.Instance != null && ctrlLineSelect.Line != null)
                {
                    string commConfigDir = ctrlLineSelect.Instance.CommApp.ConfigDir;
                    CreateScript();
                    CreateJsHandler(commConfigDir);

                    project.ConfigDatabase.DeviceTable.Modified = true;
                    project.ConfigDatabase.CnlTable.Modified = true;

                    foreach (DeviceConfigEntry entry in configBuilder.DeviceConfigs)
                    {
                        project.ConfigDatabase.DeviceTable.AddItem(entry.DeviceEntity);
                        entry.Cnls.ForEach(cnl => project.ConfigDatabase.CnlTable.AddItem(cnl));
                        ctrlLineSelect.Line.DevicePolling.Add(entry.DeviceConfig);
                        string fileName = Path.Combine(commConfigDir, 
                            MqttClientDeviceConfig.GetFileName(entry.DeviceConfig.DeviceNum));

                        if (!entry.MqttDeviceConfig.Save(fileName, out string errMsg))
                            throw new ScadaException(errMsg);
                    }

                    ScadaUiUtils.ShowInfo(ExtensionPhrases.CreateConfigCompleted);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                adminContext.ErrLog.HandleError(ex, ExtensionPhrases.CreateConfigError);
                return false;
            }
        }

        /// <summary>
        /// Creates the necessary script in the configuration database.
        /// </summary>
        private void CreateScript()
        {
            if (project.ConfigDatabase.ScriptTable.SelectFirst(
                new TableFilter("Name", ProjectScript.CsScriptName)) == null)
            {
                project.ConfigDatabase.ScriptTable.AddItem(new Script
                {
                    ScriptID = project.ConfigDatabase.ScriptTable.GetNextPk(),
                    Name = ProjectScript.CsScriptName,
                    Source = ProjectScript.CsScriptSource
                });
                project.ConfigDatabase.ScriptTable.Modified = true;
            }
        }

        /// <summary>
        /// Creates a JavaScript file to handle MQTT subscriptions.
        /// </summary>
        private static void CreateJsHandler(string commConfigDir)
        {
            string fileName = Path.Combine(commConfigDir, ProjectScript.JsFileName);

            if (!File.Exists(fileName))
                File.WriteAllText(fileName, ProjectScript.JsSource);
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
            if (CreateConfig())
                DialogResult = DialogResult.OK;
        }
    }
}
