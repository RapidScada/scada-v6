/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Summary  : Represents a form for transfer configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Admin.Extensions;
using Scada.Admin.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Represents a form for transfer configuration.
    /// <para>Представляет форму для передачи конфигурации.</para>
    /// </summary>
    public partial class FrmTransfer : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTransfer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTransfer(AppData appData, ScadaProject scadaProject, ProjectInstance projectInstance,
            DeploymentProfile deploymentProfile) : this()
        {
        }


        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        public bool DownloadConfig()
        {
            return false;
        }

        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        public bool UploadConfig()
        {
            /*if (!appData.ExtensionHolder.GetExtension(deploymentProfile.Extension, out ExtensionLogic extensionLogic))
            {
                ScadaUiUtils.ShowError(AppPhrases.ExtensionNotFound);
                return false;
            }

            if (!extensionLogic.CanDeploy)
            {
                ScadaUiUtils.ShowError(AppPhrases.ExtensionCannotDeploy);
                return false;
            }*/

            return false;
        }
    }
}
