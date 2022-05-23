// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Data.Entities;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    /// <summary>
    /// Represents a form for adding a communication line.
    /// <para>Представляет форму для добавления линии связи.</para>
    /// </summary>
    public partial class FrmLineAdd : Form
    {
        private readonly ScadaProject project;            // the project under development
        private readonly RecentSelection recentSelection; // the recently selected objects


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmLineAdd()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmLineAdd(ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));

            Instance = null;
            LineConfig = null;

            numCommLineNum.Maximum = ConfigDatabase.MaxID;
            txtName.MaxLength = ExtensionUtils.NameLength;
            txtDescr.MaxLength = ExtensionUtils.DescrLength;
        }


        /// <summary>
        /// Gets the instance affected in Communicator.
        /// </summary>
        public ProjectInstance Instance { get; private set; }

        /// <summary>
        /// Gets the communication line configuration added to Communicator.
        /// </summary>
        public LineConfig LineConfig { get; private set; }

        /// <summary>
        /// Gets a value indicating whether a communication line has been added to the Communicator configuration.
        /// </summary>
        public bool AddedToComm
        {
            get
            {
                return Instance != null && LineConfig != null;
            }
        }


        /// <summary>
        /// Fills the combo box with the instances.
        /// </summary>
        private void FillInstanceList()
        {
            cbInstance.ValueMember = "Name";
            cbInstance.DisplayMember = "Name";
            cbInstance.DataSource = project.Instances;

            try
            {
                if (!string.IsNullOrEmpty(recentSelection.InstanceName))
                    cbInstance.SelectedValue = recentSelection.InstanceName;
            }
            catch
            {
                if (cbInstance.Items.Count > 0)
                    cbInstance.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Sets the communication line number by default.
        /// </summary>
        private void SetLineNum()
        {
            if (recentSelection.CommLineNum > 0)
                numCommLineNum.SetValue(recentSelection.CommLineNum + 1);
            else if (project.ConfigDatabase.CommLineTable.ItemCount > 0)
                numCommLineNum.SetValue(project.ConfigDatabase.CommLineTable.GetNextPk());
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            StringBuilder sbError = new();

            if (string.IsNullOrWhiteSpace(txtName.Text))
                sbError.AppendError(lblName, CommonPhrases.NonemptyRequired);

            if (chkAddToComm.Checked && cbInstance.SelectedItem == null)
                sbError.AppendError(lblInstance, CommonPhrases.NonemptyRequired);

            if (sbError.Length > 0)
            {
                ScadaUiUtils.ShowError(CommonPhrases.CorrectErrors + Environment.NewLine + sbError);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks feasibility of adding a line.
        /// </summary>
        private bool CheckFeasibility()
        {
            int commLineNum = Convert.ToInt32(numCommLineNum.Value);

            if (project.ConfigDatabase.CommLineTable.PkExists(commLineNum))
            {
                ScadaUiUtils.ShowError(ExtensionPhrases.LineExistsInConfigDatabase);
                return false;
            }

            if (chkAddToComm.Checked && 
                cbInstance.SelectedItem is ProjectInstance instance && instance.CommApp.Enabled)
            {
                if (!instance.LoadAppConfig(out string errMsg))
                {
                    ScadaUiUtils.ShowError(errMsg);
                    return false;
                }

                if (instance.CommApp.AppConfig.Lines.Any(line => line.CommLineNum == commLineNum))
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.LineExistsInCommConfig);
                    return false;
                }
            }

            return true;
        }


        private void FrmLineAdd_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FillInstanceList();
            SetLineNum();
            txtName.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls() && CheckFeasibility())
            {
                // create new communication line
                CommLine commLineEntity = new()
                {
                    CommLineNum = Convert.ToInt32(numCommLineNum.Value),
                    Name = txtName.Text,
                    Descr = txtDescr.Text
                };

                // add line to the configuration database
                project.ConfigDatabase.CommLineTable.AddItem(commLineEntity);
                project.ConfigDatabase.CommLineTable.Modified = true;

                // add line to Communicator configuration
                if (chkAddToComm.Checked && cbInstance.SelectedItem is ProjectInstance instance)
                {
                    Instance = instance;
                    recentSelection.InstanceName = instance.Name;

                    if (instance.CommApp.Enabled)
                    {
                        LineConfig = CommConfigConverter.CreateLineConfig(commLineEntity);
                        LineConfig.Parent = instance.CommApp.AppConfig;
                        instance.CommApp.AppConfig.Lines.Add(LineConfig);
                    }
                }

                recentSelection.CommLineNum = commLineEntity.CommLineNum;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
