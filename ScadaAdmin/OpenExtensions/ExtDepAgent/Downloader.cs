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
    /// Downloads configuration.
    /// <para>Скачивает конфигурацию.</para>
    /// </summary>
    internal class Downloader
    {
        /// <summary>
        /// The number of download tasks.
        /// </summary>
        private const int TaskCount = 1;

        private readonly ScadaProject project;
        private readonly ProjectInstance instance;
        private readonly DeploymentProfile profile;
        private readonly ITransferControl transferControl;
        private readonly DownloadOptions downloadOptions;
        private readonly ProgressTracker progressTracker;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Downloader(ScadaProject project, ProjectInstance instance, DeploymentProfile profile,
            ITransferControl transferControl)
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
            this.transferControl = transferControl ?? throw new ArgumentNullException(nameof(transferControl));
            downloadOptions = profile.DownloadOptions;
            progressTracker = new ProgressTracker(transferControl) { TaskCount = TaskCount };
        }


        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        public void Download()
        {

        }
    }
}
