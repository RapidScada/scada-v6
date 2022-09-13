// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Configuration;
using Opc.Ua.Server;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvDsOpcUaServer.Config;
using Scada.Lang;
using Scada.Log;
using Scada.Storages;
using System.Text;

namespace Scada.Comm.Drivers.DrvDsOpcUaServer.Logic
{
    /// <summary>
    /// Implements the data source logic.
    /// <para>Реализует логику источника данных.</para>
    /// </summary>
    internal class OpcUaServerDSL : DataSourceLogic
    {
        private readonly OpcUaServerDSO options; // the data source options
        private readonly ILog dsLog;             // the data source log

        private ApplicationInstance opcApp;
        private CustomServer opcServer;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcUaServerDSL(ICommContext commContext, DataSourceConfig dataSourceConfig)
            : base(commContext, dataSourceConfig)
        {
            options = new OpcUaServerDSO(dataSourceConfig.CustomOptions);
            dsLog = CreateLog(DriverUtils.DriverCode);
        }


        /// <summary>
        /// Prepares OPC UA server for operating.
        /// </summary>
        private async Task PrepareOpcServer()
        {
            // create OPC application instance
            opcApp = new ApplicationInstance
            {
                ApplicationName = DataSourceConfig.Name,
                ApplicationType = ApplicationType.Server,
                ConfigSectionName = "Scada.Comm.Drivers.DrvDsOpcUaServer"
            };

            // load the application configuration
            ApplicationConfiguration opcConfig;

            using (Stream stream = PrepareOpcConfig())
            {
                opcConfig = await opcApp.LoadApplicationConfiguration(stream, false);
            }

            // check the application certificate
            bool haveAppCertificate = await opcApp.CheckApplicationInstanceCertificate(false, 
                CertificateFactory.DefaultKeySize, CertificateFactory.DefaultLifeTime);

            if (!haveAppCertificate)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Сертификат экземпляра приложения недействителен!" :
                    "Application instance certificate invalid!");
            }

            if (!opcConfig.SecurityConfiguration.AutoAcceptUntrustedCertificates)
                opcConfig.CertificateValidator.CertificateValidation += CertificateValidator_CertificateValidation;

            // create OPC server
            opcServer = new CustomServer(CommContext, options, dsLog);
        }

        /// <summary>
        /// Reads an OPC UA configuration file, creating it if necessary.
        /// </summary>
        private Stream PrepareOpcConfig()
        {
            string path = ScadaUtils.FirstNonEmpty(options.ConfigFileName, DriverUtils.DefaultConfigFileName);

            if (CommContext.Storage.GetFileInfo(DataCategory.Config, path).Exists)
            {
                byte[] bytes = CommContext.Storage.ReadBytes(DataCategory.Config, path);
                return new MemoryStream(bytes);
            }
            else
            {
                Stream resourceStream = DriverUtils.GetConfigResourceStream(ScadaUtils.IsRunningOnWin);
                WriteOpcConfig(path, resourceStream);
                resourceStream.Position = 0;
                return resourceStream;
            }
        }

        /// <summary>
        /// Writes the OPC UA configuration.
        /// </summary>
        private void WriteOpcConfig(string path, Stream stream)
        {
            BinaryReader reader = new(stream); // do not close reader
            byte[] bytes = reader.ReadBytes((int)stream.Length);
            CommContext.Storage.WriteBytes(DataCategory.Config, path, bytes);
        }

        /// <summary>
        /// Starts OPC UA server.
        /// </summary>
        private async Task StartOpcServer()
        {
            await opcApp.Start(opcServer);

            StringBuilder sbStartInfo = new(Locale.IsRussian ? 
                "Сервер OPC UA запущен" :
                "OPC UA server started");
            EndpointDescriptionCollection endpoints = opcServer.GetEndpoints();

            if (endpoints.Count > 0)
            {
                // print endpoint info
                foreach (string endpointUrl in endpoints.Select(e => e.EndpointUrl).Distinct())
                {
                    sbStartInfo.AppendLine().Append("    ").Append(endpointUrl);
                }
            }
            else
            {
                sbStartInfo.AppendLine().Append("    No endpoints");
            }

            dsLog.WriteAction(sbStartInfo.ToString());

            // add event handlers
            ISessionManager sessionManager = opcServer.CurrentInstance.SessionManager;
            sessionManager.SessionActivated += SessionManager_SessionEvent;
            sessionManager.SessionClosing += SessionManager_SessionEvent;
            sessionManager.SessionCreated += SessionManager_SessionEvent;

            ISubscriptionManager subscriptionManager = opcServer.CurrentInstance.SubscriptionManager;
            subscriptionManager.SubscriptionCreated += SubscriptionManager_SubscriptionEvent;
            subscriptionManager.SubscriptionDeleted += SubscriptionManager_SubscriptionEvent;
        }

        /// <summary>
        /// Stops and disposes OPC UA server.
        /// </summary>
        private void StopOpcServer()
        {
            if (opcServer != null)
            {
                opcServer.Stop();
                dsLog.WriteAction(Locale.IsRussian ?
                    "Сервер OPC UA остановлен" :
                    "OPC UA server stopped");
            }
        }

        /// <summary>
        /// Validates the certificate.
        /// </summary>
        private void CertificateValidator_CertificateValidation(CertificateValidator sender, 
            CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
            {
                e.Accept = options.AutoAccept;

                if (options.AutoAccept)
                {
                    dsLog.WriteAction(Locale.IsRussian ?
                        "Принятый сертификат: {0}" :
                        "Accepted certificate: {0}", e.Certificate.Subject);
                }
                else
                {
                    dsLog.WriteError(Locale.IsRussian ?
                        "Отклоненный сертификат: {0}" :
                        "Rejected certificate: {0}", e.Certificate.Subject);
                }
            }
        }

        /// <summary>
        /// Logs the session event.
        /// </summary>
        private void SessionManager_SessionEvent(Session session, SessionEventReason reason)
        {
            dsLog.WriteAction("{0} {1}", session.SessionDiagnostics.SessionName, reason.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// Logs the subscription event.
        /// </summary>
        private void SubscriptionManager_SubscriptionEvent(Subscription subscription, bool deleted)
        {
            if (deleted)
            {
                dsLog.WriteAction(Locale.IsRussian ?
                    "Подписка с ид. {0} удалена" :
                    "Subscription with ID {0} deleted", subscription.Id);
            }
            else
            {
                dsLog.WriteAction(Locale.IsRussian ?
                    "Подписка с ид. {0} создана" :
                    "Subscription with ID {0} created", subscription.Id);
            }
        }


        /// <summary>
        /// Makes the data source ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            dsLog.WriteBreak();
            PrepareOpcServer().Wait();
        }

        /// <summary>
        /// Starts the data source.
        /// </summary>
        public override void Start()
        {
            try
            {
                StartOpcServer().Wait();
            }
            catch
            {
                IsReady = false;
                throw;
            }
        }

        /// <summary>
        /// Closes the data source.
        /// </summary>
        public override void Close()
        {
            StopOpcServer();
            dsLog.WriteBreak();
        }

        /// <summary>
        /// Writes the slice of the current data.
        /// </summary>
        public override void WriteCurrentData(DeviceSlice deviceSlice)
        {
            opcServer?.NodeManager?.WriteCurrentData(deviceSlice);
        }
    }
}
