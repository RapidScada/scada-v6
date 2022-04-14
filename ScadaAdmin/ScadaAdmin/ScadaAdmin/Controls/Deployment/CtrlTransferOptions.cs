/*
 * Copyright 2022 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Administrator
 * Summary  : Represents a control for editing configuration transfer options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Forms;
using Scada.Forms.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.App.Controls.Deployment
{
    /// <summary>
    /// Represents a control for editing configuration transfer options.
    /// <para>Представляет элемент управления для редактирования параметров передачи конфигурации.</para>
    /// </summary>
    public partial class CtrlTransferOptions : UserControl
    {
        private ConfigBase configBase; // the configuration database
        private bool changing;         // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlTransferOptions()
        {
            InitializeComponent();
            configBase = null;
            changing = false;
        }


        /// <summary>
        /// Gets a value indicating whether none of the options are selected.
        /// </summary>
        private bool Empty
        {
            get
            {
                return !(chkIncludeBase.Checked || chkIncludeView.Checked ||
                    chkIncludeServer.Checked || chkIncludeComm.Checked || chkIncludeWeb.Checked);
            }
        }


        /// <summary>
        /// Displays or hides the controls that represent upload options.
        /// </summary>
        private void SetUploadOptionsVisible(bool visible)
        {
            chkRestartServer.Visible = visible;
            chkRestartComm.Visible = visible;
            chkRestartWeb.Visible = visible;
            lblObjFilter.Visible = visible;
            txtObjFilter.Visible = visible;
            btnSelectObj.Visible = visible;
        }

        /// <summary>
        /// Raises an OptionsChanged event.
        /// </summary>
        private void OnOptionsChanged()
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(ConfigBase configBase, bool upload)
        {
            this.configBase = configBase;
            SetUploadOptionsVisible(upload);
        }

        /// <summary>
        /// Validates the form controls.
        /// </summary>
        public bool ValidateControls()
        {
            if (Empty)
            {
                ScadaUiUtils.ShowError(AppPhrases.ConfigNotSelected);
                return false;
            }

            if (!ScadaUtils.ParseRange(txtObjFilter.Text, true, true, out _))
            {
                ScadaUiUtils.ShowError(AppPhrases.InvalidObjectFilter);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the controls according to the options.
        /// </summary>
        public void OptionsToControls(TransferOptions transferOptions)
        {
            if (transferOptions == null)
                throw new ArgumentNullException(nameof(transferOptions));

            changing = true;
            gbOptions.Enabled = true;
            chkIncludeBase.Checked = transferOptions.IncludeBase;
            chkIncludeView.Checked = transferOptions.IncludeView;
            chkIncludeServer.Checked = transferOptions.IncludeServer;
            chkIncludeComm.Checked = transferOptions.IncludeComm;
            chkIncludeWeb.Checked = transferOptions.IncludeWeb;
            chkIgnoreRegKeys.Checked = transferOptions.IgnoreRegKeys;

            if (transferOptions is UploadOptions uploadOptions)
            {
                chkRestartServer.Checked = uploadOptions.RestartServer;
                chkRestartComm.Checked = uploadOptions.RestartComm;
                chkRestartWeb.Checked = uploadOptions.RestartWeb;
                txtObjFilter.Text = uploadOptions.ObjectFilter.ToRangeString();
            }

            changing = false;
        }

        /// <summary>
        /// Sets the options according to the controls.
        /// </summary>
        public void ControlsToOptions(TransferOptions transferOptions)
        {
            if (transferOptions == null)
                throw new ArgumentNullException(nameof(transferOptions));

            transferOptions.IncludeBase = chkIncludeBase.Checked;
            transferOptions.IncludeView = chkIncludeView.Checked;
            transferOptions.IncludeServer = chkIncludeServer.Checked;
            transferOptions.IncludeComm = chkIncludeComm.Checked;
            transferOptions.IncludeWeb = chkIncludeWeb.Checked;
            transferOptions.IgnoreRegKeys = chkIgnoreRegKeys.Checked;

            if (transferOptions is UploadOptions uploadOptions)
            {
                uploadOptions.RestartServer = chkRestartServer.Checked;
                uploadOptions.RestartComm = chkRestartComm.Checked;
                uploadOptions.RestartWeb = chkRestartWeb.Checked;
                uploadOptions.SetObjectFilter(ScadaUtils.ParseRange(txtObjFilter.Text, true, true));
            }
        }

        /// <summary>
        /// Clears and disables the control.
        /// </summary>
        public void Disable()
        {
            changing = true;
            chkIncludeBase.Checked = false;
            chkIncludeView.Checked = false;
            chkIncludeServer.Checked = false;
            chkIncludeComm.Checked = false;
            chkIncludeWeb.Checked = false;
            chkIgnoreRegKeys.Checked = false;
            txtObjFilter.Text = "";
            gbOptions.Enabled = false;
            changing = false;
        }


        /// <summary>
        /// Occurs when the options change.
        /// </summary>
        public event EventHandler OptionsChanged;


        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                OnOptionsChanged();
        }

        private void btnSelectObj_Click(object sender, EventArgs e)
        {
            // show a dialog to select objects
            if (configBase != null)
            {
                ScadaUtils.ParseRange(txtObjFilter.Text, true, false, out IList<int> objNums);
                FrmEntitySelect frmEntitySelect = new(configBase.ObjTable) { SelectedIDs = objNums };

                if (frmEntitySelect.ShowDialog() == DialogResult.OK)
                    txtObjFilter.Text = frmEntitySelect.SelectedIDs.ToRangeString();
            }
        }
    }
}
