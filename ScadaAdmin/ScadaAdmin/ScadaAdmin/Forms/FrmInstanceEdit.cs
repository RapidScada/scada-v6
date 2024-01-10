/*
 * Copyright 2024 Rapid Software LLC
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
 * Summary  : Represents a form for creating or editing an instance
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Forms;
using System;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms
{
    /// <summary>
    /// Represents a form for creating or editing an instance.
    /// <para>Представляет форму для создания или редактирования экземпляра.</para>
    /// </summary>
    public partial class FrmInstanceEdit : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmInstanceEdit()
        {
            InitializeComponent();
            EditMode = false;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the form is in edit mode.
        /// </summary>
        public bool EditMode { get; set; }

        /// <summary>
        /// Gets or sets the instance name.
        /// </summary>
        public string InstanceName
        {
            get
            {
                return txtName.Text.Trim();
            }
            set
            {
                txtName.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Server is present in the instance.
        /// </summary>
        public bool ServerAppEnabled
        {
            get
            {
                return chkServerApp.Checked;
            }
            set
            {
                chkServerApp.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Communicator is present in the instance.
        /// </summary>
        public bool CommAppEnabled
        {
            get
            {
                return chkCommApp.Checked;
            }
            set
            {
                chkCommApp.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Webstation is present in the instance.
        /// </summary>
        public bool WebAppEnabled
        {
            get
            {
                return chkWebApp.Checked;
            }
            set
            {
                chkWebApp.Checked = value;
            }
        }


        /// <summary>
        /// Validates the form controls.
        /// </summary>
        private bool ValidateControls()
        {
            // validate the name when creating a new instance
            if (!EditMode)
            {
                string name = InstanceName;

                if (name == "")
                {
                    ScadaUiUtils.ShowError(AppPhrases.InstanceNameEmpty);
                    return false;
                }

                if (!AdminUtils.NameIsValid(name))
                {
                    ScadaUiUtils.ShowError(AppPhrases.InstanceNameInvalid);
                    return false;
                }
            }

            // validate the applications
            if (!(ServerAppEnabled || CommAppEnabled || WebAppEnabled))
            {
                ScadaUiUtils.ShowError(AppPhrases.InstanceSelectApps);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Initializes the properties based on the specified instance.
        /// </summary>
        public void Init(ProjectInstance projectInstance)
        {
            ArgumentNullException.ThrowIfNull(projectInstance, nameof(projectInstance));
            InstanceName = projectInstance.Name;
            ServerAppEnabled = projectInstance.ServerApp.Enabled;
            CommAppEnabled = projectInstance.CommApp.Enabled;
            WebAppEnabled = projectInstance.WebApp.Enabled;
        }

        /// <summary>
        /// Determines whether the application is enabled or not.
        /// </summary>
        public bool GetAppEnabled(ProjectApp projectApp)
        {
            return projectApp switch
            {
                ServerApp => ServerAppEnabled,
                CommApp => CommAppEnabled,
                WebApp => WebAppEnabled,
                _ => false
            };
        }


        private void FrmInstanceEdit_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);

            if (EditMode)
            {
                Text = AppPhrases.EditInstanceTitle;
                txtName.ReadOnly = true;
                ActiveControl = gbApplications;
            }
            else
            {
                Text = AppPhrases.NewInstanceTitle;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
                DialogResult = DialogResult.OK;
        }
    }
}
