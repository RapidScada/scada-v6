// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Deployment;
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
        }


        /// <summary>
        /// Sends the command to the service.
        /// </summary>
        private void ControlService(ServiceApp serviceApp, ServiceCommand cmd)
        {
            string cmdFormat = cmd switch
            {
                ServiceCommand.Start => Locale.IsRussian ? "Запуск службы {0}" : "Start the {0} service",
                ServiceCommand.Stop => Locale.IsRussian ? "Остановка службы {0}" : "Stop the {0} service",
                ServiceCommand.Restart => Locale.IsRussian ? "Перезапуск службы {0}" : "Restart the {0} service",
                _ => throw new ScadaException("Unknown service control command.")
            };

            transferControl.ThrowIfCancellationRequested();
            transferControl.WriteMessage(string.Format(cmdFormat, ScadaUtils.GetAppName(serviceApp)));

            if (agentClient.ControlService(serviceApp, cmd))
            {
                transferControl.WriteMessage(Locale.IsRussian ?
                    "Команда управления службой выполнена успешно" :
                    "Service control command completed successfully");
            }
            else
            {
                transferControl.WriteMessage(Locale.IsRussian ?
                    "Команда управления службой не выполнена" :
                    "Service control command failed");
            }
        }

        /// <summary>
        /// Restarts the services.
        /// </summary>
        public void RestartServices()
        {
            bool restartServer = uploadOptions.RestartServer && (uploadOptions.IncludeBase || uploadOptions.IncludeServer);
            bool restartComm = uploadOptions.RestartComm && (uploadOptions.IncludeBase || uploadOptions.IncludeComm);
            bool restartWeb = uploadOptions.RestartWeb && (uploadOptions.IncludeBase || uploadOptions.IncludeWeb);

            if (restartServer || restartComm || restartWeb)
            {
                transferControl.WriteLine();

                if (restartComm)
                    ControlService(ServiceApp.Comm, ServiceCommand.Stop);

                if (restartServer)
                    ControlService(ServiceApp.Server, ServiceCommand.Restart);

                if (restartComm)
                    ControlService(ServiceApp.Comm, ServiceCommand.Start);

                if (restartWeb)
                    ControlService(ServiceApp.Web, ServiceCommand.Restart);
            }

            progressTracker.TaskIndex++;
        }
    }
}
