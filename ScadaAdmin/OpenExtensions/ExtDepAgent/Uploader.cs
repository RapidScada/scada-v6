// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Deployment;
using Scada.Admin.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Admin.Extensions.ExtDepAgent
{
    /// <summary>
    /// Uploads configuration.
    /// <para>Передаёт конфигурацию.</para>
    /// </summary>
    internal class Uploader
    {
        /// <summary>
        /// The number of upload tasks.
        /// </summary>
        private const int TaskCount = 1;

        private readonly ScadaProject project;
        private readonly ProjectInstance instance;
        private readonly DeploymentProfile profile;
        private readonly ITransferControl transferControl;
        private readonly UploadOptions uploadOptions;
        private readonly ProgressTracker progressTracker;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Uploader(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
            this.transferControl = transferControl ?? throw new ArgumentNullException(nameof(transferControl));
            uploadOptions = profile.UploadOptions;
            progressTracker = new ProgressTracker(transferControl) { TaskCount = TaskCount };
        }


        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        public void Upload()
        {

        }
    }
}
