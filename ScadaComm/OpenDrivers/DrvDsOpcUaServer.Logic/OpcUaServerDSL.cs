// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Configuration;
using Opc.Ua.Server;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Lang;
using Scada.Log;
using Scada.Storages;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // TODO: use stream instead of file after updating OPCFoundation.NetStandard.Opc.Ua
            PrepareConfigFile(out string configFileName);
            ApplicationConfiguration opcConfig = await opcApp.LoadApplicationConfiguration(configFileName, false);

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
        /// Creates an OPC UA configuration file if does not exist or is outdated.
        /// </summary>
        private void PrepareConfigFile(out string configFileName)
        {
            string shortFileName = string.IsNullOrEmpty(options.ConfigFileName) ? 
                DriverUtils.DefaultConfigFileName : options.ConfigFileName;
            configFileName = Path.Combine(CommContext.AppDirs.ConfigDir, shortFileName);

            if (!CommContext.Storage.IsFileStorage &&
                CommContext.Storage.GetFileInfo(DataCategory.Config, shortFileName).Exists)
            {
                // get configuration from database
                string contents = CommContext.Storage.ReadText(DataCategory.Config, shortFileName);
                File.WriteAllText(configFileName, contents, Encoding.UTF8);
            }
            else if (!File.Exists(configFileName))
            {
                // create configration file 
                DriverUtils.WriteConfigFile(configFileName, ScadaUtils.IsRunningOnWin);
            }
        }

        /// <summary>
        /// Starts OPC UA server.
        /// </summary>
        private async Task StartOpcServer()
        {
            await opcApp.Start(opcServer);

            StringBuilder sbStartInfo = new StringBuilder(Locale.IsRussian ? 
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
