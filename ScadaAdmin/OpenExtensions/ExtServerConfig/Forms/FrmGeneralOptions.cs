// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtServerConfig.Code;
using Scada.Admin.Project;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Lang;
using Scada.Server;
using Scada.Server.Config;
using System.Text;
using WinControl;

namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    /// <summary>
    /// Represents a form for editing general options.
    /// <para>Форма для редактирования основных параметров.</para>
    /// </summary>
    public partial class FrmGeneralOptions : Form, IChildForm
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly ServerApp serverApp;        // the Server application in a project
        private readonly ServerConfig serverConfig;  // the Server configuration
        private bool changing;                       // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmGeneralOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmGeneralOptions(IAdminContext adminContext, ServerApp serverApp)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.serverApp = serverApp ?? throw new ArgumentNullException(nameof(serverApp));
            serverConfig = serverApp.AppConfig;
            changing = false;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            changing = true;

            // general options
            GeneralOptions generalOptions = serverConfig.GeneralOptions;
            numUnrelIfInactive.SetValue(generalOptions.UnrelIfInactive);
            chkUseArchivalStatus.Checked = generalOptions.UseArchivalStatus;
            chkGenerateAckCmd.Checked = generalOptions.GenerateAckCmd;
            numMaxLogSize.SetValue(generalOptions.MaxLogSize);
            chkDisableFormulas.Checked = generalOptions.DisableFormulas;
            txtEnableFormulasObjNums.Text = generalOptions.EnableFormulasObjNums.ToRangeString();

            // listener options
            ListenerOptions listenerOptions = serverConfig.ListenerOptions;
            numPort.SetValue(listenerOptions.Port);
            numTimeout.SetValue(listenerOptions.Timeout);
            txtSecretKey.Text = ScadaUtils.BytesToHex(listenerOptions.SecretKey);

            changing = false;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        private void ControlsToConfing()
        {
            // general options
            GeneralOptions generalOptions = serverConfig.GeneralOptions;
            generalOptions.UnrelIfInactive = decimal.ToInt32(numUnrelIfInactive.Value);
            generalOptions.UseArchivalStatus = chkUseArchivalStatus.Checked;
            generalOptions.GenerateAckCmd = chkGenerateAckCmd.Checked;
            generalOptions.MaxLogSize = decimal.ToInt32(numMaxLogSize.Value);
            generalOptions.DisableFormulas = chkDisableFormulas.Checked;
            generalOptions.EnableFormulasObjNums = ScadaUtils.ParseRange(txtEnableFormulasObjNums.Text, true, true);

            // listener options
            ListenerOptions listenerOptions = serverConfig.ListenerOptions;
            listenerOptions.Port = decimal.ToInt32(numPort.Value);
            listenerOptions.Timeout = decimal.ToInt32(numTimeout.Value);
            listenerOptions.SecretKey = ScadaUtils.HexToBytes(txtSecretKey.Text.Trim());
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            StringBuilder sbError = new();

            if (!ScadaUtils.ParseRange(txtEnableFormulasObjNums.Text, true, true, out _))
                sbError.AppendLine(ExtensionPhrases.InvalidObjectRange);

            if (!ScadaUtils.HexToBytes(txtSecretKey.Text.Trim(), out _))
                sbError.AppendLine(CommonPhrases.InvalidSecretKey);

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
        /// Saves the changes of the child form data.
        /// </summary>
        public void Save()
        {
            if (ValidateControls())
            {
                ControlsToConfing();

                if (serverApp.SaveConfig(out string errMsg))
                    ChildFormTag.Modified = false;
                else
                    adminContext.ErrLog.HandleError(errMsg);
            }
        }


        private void FrmCommonParams_Load(object sender, System.EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ConfigToControls();
        }

        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                ChildFormTag.Modified = true;
        }

        private void chkDisableFormulas_CheckedChanged(object sender, EventArgs e)
        {
            txtEnableFormulasObjNums.Enabled = chkDisableFormulas.Checked;

            if (!changing)
                ChildFormTag.Modified = true;
        }

        private void btnSelectObjects_Click(object sender, EventArgs e)
        {
            // show a dialog to select objects
            if (adminContext.CurrentProject != null)
            {
                ScadaUtils.ParseRange(txtEnableFormulasObjNums.Text, true, false, out IList<int> objNums);
                FrmEntitySelect frmEntitySelect = new(adminContext.CurrentProject.ConfigDatabase.ObjTable)
                {
                    SelectedIDs = objNums
                };

                if (frmEntitySelect.ShowDialog() == DialogResult.OK)
                {
                    chkDisableFormulas.Checked = true;
                    txtEnableFormulasObjNums.Text = frmEntitySelect.SelectedIDs.ToRangeString();
                }
            }
        }

        private void txtSecretKey_Enter(object sender, EventArgs e)
        {
            txtSecretKey.UseSystemPasswordChar = false;
        }

        private void txtSecretKey_Leave(object sender, EventArgs e)
        {
            // otherwise the Tab key does not work
            Action action = () => { txtSecretKey.UseSystemPasswordChar = true; };
            Task.Run(() => { Invoke(action); });
        }

        private void btnGenerateKey_Click(object sender, EventArgs e)
        {
            txtSecretKey.Text = ScadaUtils.BytesToHex(ScadaUtils.GetRandomBytes(ScadaUtils.SecretKeySize));
            txtSecretKey.Focus();
        }

        private void btnCopyKey_Click(object sender, EventArgs e)
        {
            if (txtSecretKey.Text != "")
                Clipboard.SetText(txtSecretKey.Text);
        }
    }
}
