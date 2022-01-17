// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Deployment;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Lang;

namespace Scada.Admin.Extensions.ExtDepAgent
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtDepAgentLogic : ExtensionLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtDepAgentLogic(IAdminContext adminContext)
            : base(adminContext)
        {
            CanDeploy = true;
        }


        /// <summary>
        /// Gets the extension code.
        /// </summary>
        public override string Code
        {
            get
            {
                return "ExtDepAgent";
            }
        }

        /// <summary>
        /// Gets the extension name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Развёртывание с Агентом" : "Deployment with Agent";
            }
        }

        /// <summary>
        /// Gets the extension description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Расширение обеспечивает передачу проекта с помощью службы Агента." :
                    "The extension provides project transfer using the Agent service.";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AdminContext.AppDirs.LangDir, Code, out string errMsg))
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);

            ExtensionPhrases.Init();
        }

        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        public override void DownloadConfig(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            new Downloader(AdminContext.AppDirs, project, instance, profile, transferControl).Download();
        }

        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        public override void UploadConfig(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            new Uploader(AdminContext.AppDirs, project, instance, profile, transferControl).Upload();
        }
    }
}
