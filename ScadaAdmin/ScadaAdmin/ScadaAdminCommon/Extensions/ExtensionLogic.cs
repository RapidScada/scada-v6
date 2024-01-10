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
 * Module   : ScadaAdminCommon
 * Summary  : Represents the base class for extension logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Admin.Config;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Dbms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Admin.Extensions
{
    /// <summary>
    /// Represents the base class for extension logic.
    /// <para>Представляет базовый класс логики расширения.</para>
    /// </summary>
    public abstract class ExtensionLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtensionLogic(IAdminContext adminContext)
        {
            AdminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            CanShowProperties = false;
            CanDeploy = false;
        }


        /// <summary>
        /// Gets the Administrator context.
        /// </summary>
        protected IAdminContext AdminContext { get; }

        /// <summary>
        /// Gets the extension code.
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// Gets the extension name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the extension description.
        /// </summary>
        public abstract string Descr { get; }

        /// <summary>
        /// Gets the extension version.
        /// </summary>
        public virtual string Version
        {
            get
            {
                return GetType().Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets the file extensions for which the extension provides an editor.
        /// </summary>
        /// <remarks>The period is not included.</remarks>
        public virtual ICollection<string> FileExtensions => null;

        /// <summary>
        /// Gets a value indicating whether the extension can show a properties dialog box.
        /// </summary>
        public bool CanShowProperties { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the extension supports project deployment.
        /// </summary>
        public bool CanDeploy { get; protected set; }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public virtual void LoadDictionaries()
        {
        }

        /// <summary>
        /// Loads configuration.
        /// </summary>
        public virtual void LoadConfig()
        {
        }

        /// <summary>
        /// Shows a modal dialog box for editing extension properties.
        /// </summary>
        public virtual void ShowProperties(AdminConfig adminConfig)
        {
        }

        /// <summary>
        /// Gets menu items to add to the main menu.
        /// </summary>
        public virtual ToolStripItem[] GetMainMenuItems()
        {
            return null;
        }

        /// <summary>
        /// Gets tool buttons to add to the toolbar.
        /// </summary>
        public virtual ToolStripItem[] GetToobarButtons()
        {
            return null;
        }

        /// <summary>
        /// Gets tree nodes to add to the explorer tree.
        /// </summary>
        public virtual TreeNode[] GetTreeNodes(object relatedObject)
        {
            return null;
        }

        /// <summary>
        /// Gets images used by the explorer tree.
        /// </summary>
        public virtual Dictionary<string, Image> GetTreeViewImages()
        {
            return null;
        }

        /// <summary>
        /// Gets a form to edit the specified file.
        /// </summary>
        public virtual Form GetEditorForm(string fileName)
        {
            return null;
        }

        /// <summary>
        /// Tests a database connection.
        /// </summary>
        public virtual bool TestDbConnection(DbConnectionOptions connectionOptions, out string errMsg)
        {
            errMsg = CommonPhrases.DatabaseNotSupported;
            return false;
        }

        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        public virtual void DownloadConfig(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            throw new ScadaException(CommonPhrases.OperationNotSupported);
        }

        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        public virtual void UploadConfig(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            throw new ScadaException(CommonPhrases.OperationNotSupported);
        }
    }
}
