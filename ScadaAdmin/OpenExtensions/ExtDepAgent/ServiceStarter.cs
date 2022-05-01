// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Deployment;
using Scada.Admin.Lang;
using Scada.Agent;
using Scada.Lang;
using System;

namespace Scada.Admin.Extensions
{
    /// <summary>
    /// Restarts services when uploading configuration.
    /// <para>Перезапускает сервисы при передаче конфигурации.</para>
    /// </summary>
    /// <remarks>This class is intended to share with other deployment extensions.</remarks>
    internal class ServiceStarter
    {
        private readonly IAgentClient agentClient;
        private readonly UploadOptions uploadOptions;
        private readonly ITransferControl transferControl;
        private readonly ProgressTracker progressTracker;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServiceStarter(IAgentClient agentClient, UploadOptions uploadOptions, 
            ITransferControl transferControl, ProgressTracker progressTracker)
        {
            this.agentClient = agentClient ?? throw new ArgumentNullException(nameof(agentClient));
            this.uploadOptions = uploadOptions ?? throw new ArgumentNullException(nameof(uploadOptions));
            this.transferControl = transferControl ?? throw new ArgumentNullException(nameof(transferControl));
            this.progressTracker = progressTracker ?? throw new ArgumentNullException(nameof(progressTracker));
            ProcessTimeout = 0;
        }


        /// <summary>
        /// The number of milliseconds to wait for service command process.
        /// </summary>
        public int ProcessTimeout { get; set; }


        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        private void ControlService(ServiceApp serviceApp, ServiceCommand cmd)
        {
            string cmdFormat = cmd switch
            {
                ServiceCommand.Start => AdminPhrases.StartNamedService,
                ServiceCommand.Stop => AdminPhrases.StopNamedService,
                ServiceCommand.Restart => AdminPhrases.RestartNamedService,
                _ => throw new ScadaException("Unknown service control command.")
            };

            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteMessage(string.Format(cmdFormat, ScadaUtils.GetAppName(serviceApp)));

            if (agentClient.ControlService(serviceApp, cmd, ProcessTimeout))
                transferControl.WriteMessage(AdminPhrases.ServiceCommandCompleted);
            else
                transferControl.WriteError(AdminPhrases.ServiceCommandFailed);
        }

        /// <summary>
        /// Sets the process timeout based on the specified connection timeout.
        /// </summary>
        public ServiceStarter SetProcessTimeout(int connectionTimeout)
        {
            ProcessTimeout = Math.Max(0, connectionTimeout - 1000);
            return this;
        }

        /// <summary>
        /// Restarts the services.
        /// </summary>
        public void RestartServices()
        {
            bool restartServer = uploadOptions.RestartServer && 
                (uploadOptions.IncludeBase || uploadOptions.IncludeServer);
            bool restartComm = uploadOptions.RestartComm && 
                (uploadOptions.IncludeBase || uploadOptions.IncludeComm);
            bool smoothRestartWeb = uploadOptions.RestartWeb && !uploadOptions.IncludeWeb &&
                (uploadOptions.IncludeBase || uploadOptions.IncludeView);
            bool fullRestartWeb = uploadOptions.RestartWeb && uploadOptions.IncludeWeb;

            if (restartServer || restartComm || smoothRestartWeb || fullRestartWeb)
            {
                transferControl.WriteLine();

                if (restartComm)
                    ControlService(ServiceApp.Comm, ServiceCommand.Stop);

                if (fullRestartWeb)
                    ControlService(ServiceApp.Web, ServiceCommand.Stop);

                if (restartServer)
                    ControlService(ServiceApp.Server, ServiceCommand.Restart);

                if (restartComm)
                    ControlService(ServiceApp.Comm, ServiceCommand.Start);

                if (smoothRestartWeb)
                    ControlService(ServiceApp.Web, ServiceCommand.Restart);

                if (fullRestartWeb)
                    ControlService(ServiceApp.Web, ServiceCommand.Start);
            }

            progressTracker.TaskIndex++;
        }
    }
}
