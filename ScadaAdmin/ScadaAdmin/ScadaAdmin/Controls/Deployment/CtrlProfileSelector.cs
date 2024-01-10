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
 * Summary  : Represents a control for selecting a deployment profile
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Admin.App.Forms.Deployment;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scada.Admin.App.Controls.Deployment
{
    /// <summary>
    /// Represents a control for selecting a deployment profile.
    /// <para>Представляет элемент управления для выбора профиля развёртывания.</para>
    /// </summary>
    public partial class CtrlProfileSelector : UserControl
    {
        /// <summary>
        /// The default profile name format.
        /// </summary>
        private const string ProfileNameFormat = "{0} Profile";

        private AppData appData;                   // the common data of the application
        private DeploymentConfig deploymentConfig; // the deployment settings to select or edit
        private ProjectInstance projectInstance;   // the instance which profile is selected


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlProfileSelector()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets the currently selected profile.
        /// </summary>
        public DeploymentProfile SelectedProfile
        {
            get
            {
                return cbProfile.SelectedItem as DeploymentProfile;
            }
        }


        /// <summary>
        /// Fills the profile combo box.
        /// </summary>
        private void FillProfileList()
        {
            try
            {
                cbProfile.BeginUpdate();
                cbProfile.Items.Clear();
                cbProfile.Items.Add(AppPhrases.ProfileNotSet);

                int selectedIndex = 0;
                int instanceID = projectInstance.ID;
                string selectedName = projectInstance.DeploymentProfile;

                foreach (DeploymentProfile profile in deploymentConfig.Profiles.Values)
                {
                    if (profile.InstanceID <= 0 || profile.InstanceID == instanceID)
                    {
                        int index = cbProfile.Items.Add(profile);
                        if (profile.Name == selectedName)
                            selectedIndex = index;
                    }
                }

                cbProfile.SelectedIndex = selectedIndex;
            }
            finally
            {
                cbProfile.EndUpdate();
            }
        }

        /// <summary>
        /// Adds the profile to the deployment configuration and combo box.
        /// </summary>
        private void AddProfileToLists(DeploymentProfile profile)
        {
            // add to deployment configuration
            deploymentConfig.Profiles.Add(profile.Name, profile);

            // add to combo box
            int indexToInsert = cbProfile.Items.Count;

            for (int i = 1, cnt = cbProfile.Items.Count; i < cnt; i++)
            {
                if (string.Compare(cbProfile.Items[i].ToString(), profile.Name) > 0)
                {
                    indexToInsert = i;
                    break;
                }
            }

            cbProfile.Items.Insert(indexToInsert, profile);
            cbProfile.SelectedIndex = indexToInsert;
        }

        /// <summary>
        /// Saves the deployment configuration.
        /// </summary>
        private void SaveDeploymentConfig()
        {
            if (!deploymentConfig.Save(out string errMsg))
                appData.ErrLog.HandleError(errMsg);
        }

        /// <summary>
        /// Raises a SelectedProfileChanged event.
        /// </summary>
        private void OnSelectedProfileChanged()
        {
            SelectedProfileChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises a ProfileEdited event.
        /// </summary>
        private void OnProfileEdited()
        {
            ProfileEdited?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(AppData appData, DeploymentConfig deploymentConfig, ProjectInstance projectInstance)
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            this.deploymentConfig = deploymentConfig ?? throw new ArgumentNullException(nameof(deploymentConfig));
            this.projectInstance = projectInstance ?? throw new ArgumentNullException(nameof(projectInstance));

            txtInstanceName.Text = projectInstance.Name;
            FillProfileList();
        }


        /// <summary>
        /// Occurs when the selected profile changes.
        /// </summary>
        public event EventHandler SelectedProfileChanged;

        /// <summary>
        /// Occurs after a profile has been edited.
        /// </summary>
        public event EventHandler ProfileEdited;


        private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditProfile.Enabled = btnDeleteProfile.Enabled = SelectedProfile != null;
            OnSelectedProfileChanged();
        }

        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            // create new profile
            HashSet<string> existingNames = deploymentConfig.GetExistingProfileNames();
            string defaultProfileName = string.Format(ProfileNameFormat, projectInstance.Name);

            DeploymentProfile profile = new()
            {
                InstanceID = projectInstance.ID,
                Name = existingNames.Contains(defaultProfileName) ? "" : defaultProfileName
            };

            profile.AgentConnectionOptions.Instance = projectInstance.Name;

            FrmProfileEdit frmProfileEdit = new(appData, profile) 
            { 
                ExistingProfileNames = existingNames 
            };

            if (frmProfileEdit.ShowDialog() == DialogResult.OK)
            {
                AddProfileToLists(profile);
                SaveDeploymentConfig();
            }
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            // edit selected profile
            DeploymentProfile profile = SelectedProfile;
            string oldName = profile.Name;

            FrmProfileEdit frmProfileEdit = new(appData, profile)
            {
                ExistingProfileNames = deploymentConfig.GetExistingProfileNames(oldName)
            };

            if (frmProfileEdit.ShowDialog() == DialogResult.OK)
            {
                // update profile name if it changed
                if (oldName != profile.Name)
                {
                    deploymentConfig.Profiles.Remove(oldName);

                    try
                    {
                        cbProfile.BeginUpdate();
                        cbProfile.Items.RemoveAt(cbProfile.SelectedIndex);
                        AddProfileToLists(profile);
                    }
                    finally
                    {
                        cbProfile.EndUpdate();
                    }
                }

                // fix instance reference
                if (profile.InstanceID <= 0)
                    profile.InstanceID = projectInstance.ID;

                // save deployment settings
                SaveDeploymentConfig();
                OnProfileEdited();
            }
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            // delete selected profile
            if (SelectedProfile is DeploymentProfile profile &&
                MessageBox.Show(AppPhrases.ConfirmDeleteProfile, CommonPhrases.QuestionCaption,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // remove from deployment configuration
                deploymentConfig.Profiles.Remove(profile.Name);

                // remove from combo box
                try
                {
                    cbProfile.BeginUpdate();
                    int selectedIndex = cbProfile.SelectedIndex;
                    cbProfile.Items.RemoveAt(selectedIndex);

                    if (cbProfile.Items.Count > 0)
                    {
                        cbProfile.SelectedIndex = selectedIndex >= cbProfile.Items.Count ?
                            cbProfile.Items.Count - 1 : selectedIndex;
                    }
                }
                finally
                {
                    cbProfile.EndUpdate();
                }

                // save deployment settings
                SaveDeploymentConfig();
            }
        }
    }
}
