// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Lang;
using Scada.Log;
using System.Reflection;

namespace Scada.Comm.Drivers.DrvOpcUa
{
    /// <summary>
    /// Provides a base class for using an OPC client.
    /// <para>Предоставляет базовый класс для использования OPC-клиента.</para>
    /// </summary>
    public abstract class OpcClientHelperBase
    {
        protected readonly OpcConnectionOptions connectionOptions; // the connection options
        protected readonly ILog log;                               // implements logging
        protected readonly long helperID;                          // the random ID


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcClientHelperBase(OpcConnectionOptions connectionOptions, ILog log)
        {
            this.connectionOptions = connectionOptions ?? throw new ArgumentNullException(nameof(connectionOptions));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            helperID = ScadaUtils.GenerateUniqueID();

            AutoAccept = false;
            OpcSession = null;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to automatically accept server certificates.
        /// </summary>
        public bool AutoAccept { get; set; }

        /// <summary>
        /// Gets the OPC session
        /// </summary>
        public Session OpcSession { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether a connection the OPC server is established.
        /// </summary>
        public bool IsConnected => OpcSession != null && OpcSession.Connected;


        /// <summary>
        /// Gets the OPC application name.
        /// </summary>
        private string GetAppName()
        {
            return $"{DriverUtils.DriverCode}_{helperID} Application";
        }

        /// <summary>
        /// Gets the OPC session name.
        /// </summary>
        private string GetSessionName()
        {
            return $"{DriverUtils.DriverCode}_{helperID} Session";
        }

        /// <summary>
        /// Validates the certificate.
        /// </summary>
        private void CertificateValidator_CertificateValidation(CertificateValidator validator,
            CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
            {
                e.Accept = AutoAccept;

                if (AutoAccept)
                {
                    log.WriteLine(Locale.IsRussian ?
                        "Принятый сертификат: {0}" :
                        "Accepted certificate: {0}", e.Certificate.Subject);
                }
                else
                {
                    log.WriteLine(Locale.IsRussian ?
                        "Отклоненный сертификат: {0}" :
                        "Rejected certificate: {0}", e.Certificate.Subject);
                }
            }
        }

        /// <summary>
        /// Reads the OPC configuration.
        /// </summary>
        protected abstract Stream ReadConfiguration();

        /// <summary>
        /// Gets the resource stream that contains the default OPC configuration.
        /// </summary>
        protected static Stream GetConfigResourceStream()
        {
            string resourceName = ScadaUtils.IsRunningOnWin ?
                "Scada.Comm.Drivers.DrvOpcUa.Config.DrvOpcUa.Win.xml" :
                "Scada.Comm.Drivers.DrvOpcUa.Config.DrvOpcUa.Linux.xml";
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ??
                throw new ScadaException(string.Format(Locale.IsRussian ?
                    "Ресурс {0} не найден." :
                    "Resource {0} not found.", resourceName));
        }

        /// <summary>
        /// Connects to the OPC server asynchronously.
        /// </summary>
        public async Task ConnectAsync()
        {
            ApplicationInstance application = new()
            {
                ApplicationName = GetAppName(),
                ApplicationType = ApplicationType.Client,
                ConfigSectionName = "Scada.Comm.Drivers.DrvOpcUa"
            };

            // load application configuration
            ApplicationConfiguration config;

            using (Stream stream = ReadConfiguration())
            {
                config = await application.LoadApplicationConfiguration(stream, false);
            }

            // check application certificate
            bool haveAppCertificate = await application.CheckApplicationInstanceCertificate(false, 0);

            if (!haveAppCertificate)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Сертификат экземпляра приложения недействителен!" :
                    "Application instance certificate invalid!");
            }

            if (haveAppCertificate)
            {
                config.ApplicationUri = X509Utils.GetApplicationUriFromCertificate(
                    config.SecurityConfiguration.ApplicationCertificate.Certificate);

                if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
                    AutoAccept = true;

                config.CertificateValidator.CertificateValidation += CertificateValidator_CertificateValidation;
            }
            else
            {
                log.WriteLine(Locale.IsRussian ?
                    "Предупреждение: отсутствует сертификат приложения, используется незащищенное соединение." :
                    "Warning: missing application certificate, using unsecure connection.");
            }

            // create session
            EndpointDescription selectedEndpoint = CoreClientUtils.SelectEndpoint(
                connectionOptions.ServerUrl, haveAppCertificate);
            selectedEndpoint.SecurityMode = connectionOptions.SecurityMode;
            selectedEndpoint.SecurityPolicyUri = connectionOptions.GetSecurityPolicy();
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(config);
            ConfiguredEndpoint endpoint = new(null, selectedEndpoint, endpointConfiguration);
            UserIdentity userIdentity = connectionOptions.AuthenticationMode == AuthenticationMode.Username ?
                new UserIdentity(connectionOptions.Username, connectionOptions.Password) :
                new UserIdentity(new AnonymousIdentityToken());

            OpcSession = await Session.Create(config, endpoint, false, GetSessionName(),
                (uint)config.ClientConfiguration.DefaultSessionTimeout, userIdentity, null);

            log.WriteLine(Locale.IsRussian ?
                "OPC сессия создана успешно" :
                "OPC session created successfully");
        }
    }
}
