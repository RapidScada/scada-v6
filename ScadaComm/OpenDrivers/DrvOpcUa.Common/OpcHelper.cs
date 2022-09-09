// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Lang;
using Scada.Log;
using System.Reflection;
using System.Text;

namespace Scada.Comm.Drivers.DrvOpcUa
{
    /// <summary>
    /// Helper methods to work using OPC UA.
    /// <para>Вспомогательные методы для работы с OPC UA.</para>
    /// </summary>
    public class OpcHelper
    {
        /// <summary>
        /// The OPC UA configuration file name for the logic runtime.
        /// </summary>
        private const string LogicOpcConfig = "DrvOpcUa.Logic.xml";
        /// <summary>
        /// The OPC UA configuration file name for the view runtime.
        /// </summary>
        private const string ViewOpcConfig = "DrvOpcUa.View.xml";

        private readonly AppDirs appDirs;     // the application directories
        private readonly ILog log;            // the communication line log
        private readonly string deviceNumStr; // the device number as a string
        private readonly bool isLogicRuntime; // the runtime kind


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcHelper(AppDirs appDirs, ILog log, int deviceNum, bool isLogicRuntime)
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            deviceNumStr = deviceNum.ToString("D3");
            this.isLogicRuntime = isLogicRuntime;

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
        public Session OpcSession { get; private set; }


        /// <summary>
        /// Writes an OPC UA configuration file depending on operating system and runtime kind.
        /// </summary>
        private void WriteConfigFile(out string fileName)
        {
            fileName = Path.Combine(appDirs.ConfigDir, isLogicRuntime ? LogicOpcConfig : ViewOpcConfig);

            if (!File.Exists(fileName))
            {
                string resourceName = ScadaUtils.IsRunningOnWin ?
                    "Scada.Comm.Drivers.DrvOpcUa.Config.DrvOpcUa.Win.xml" :
                    "Scada.Comm.Drivers.DrvOpcUa.Config.DrvOpcUa.Linux.xml";
                string fileContents;
                Stream stream = null;

                try
                {
                    stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ??
                        throw new ScadaException(String.Format(Locale.IsRussian ?
                            "Ресурс {0} не найден." :
                            "Resource {0} not found.", resourceName));

                    using StreamReader reader = new(stream);
                    fileContents = reader.ReadToEnd();
                }
                finally
                {
                    stream?.Close();
                }

                File.WriteAllText(fileName, fileContents, Encoding.UTF8);
            }
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
        /// Connects to the OPC server asynchronously.
        /// </summary>
        public async Task<bool> ConnectAsync(OpcConnectionOptions connectionOptions, int operationTimeout = -1)
        {
            ApplicationInstance application = new()
            {
                ApplicationName = string.Format("DrvOpcUa_{0} Driver", deviceNumStr),
                ApplicationType = ApplicationType.Client,
                ConfigSectionName = "Scada.Comm.Drivers.DrvOpcUa"
            };

            // load the application configuration
            // TODO: use stream instead of file after updating OPCFoundation.NetStandard.Opc.Ua
            WriteConfigFile(out string configFileName);
            ApplicationConfiguration config = await application.LoadApplicationConfiguration(configFileName, false);

            // check the application certificate
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
            EndpointDescription selectedEndpoint = operationTimeout > 0
                ? CoreClientUtils.SelectEndpoint(connectionOptions.ServerUrl, haveAppCertificate, operationTimeout)
                : CoreClientUtils.SelectEndpoint(connectionOptions.ServerUrl, haveAppCertificate);
            selectedEndpoint.SecurityMode = connectionOptions.SecurityMode;
            selectedEndpoint.SecurityPolicyUri = connectionOptions.GetSecurityPolicy();
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(config);
            ConfiguredEndpoint endpoint = new(null, selectedEndpoint, endpointConfiguration);
            UserIdentity userIdentity = connectionOptions.AuthenticationMode == AuthenticationMode.Username ?
                new UserIdentity(connectionOptions.Username, connectionOptions.Password) :
                new UserIdentity(new AnonymousIdentityToken());

            OpcSession = await Session.Create(config, endpoint, false,
                "Rapid SCADA DrvOpcUa_" + deviceNumStr,
                (uint)config.ClientConfiguration.DefaultSessionTimeout, userIdentity, null);

            log.WriteLine(Locale.IsRussian ?
                "OPC сессия успешно создана: {0}" :
                "OPC session created successfully: {0}", connectionOptions.ServerUrl);
            return true;
        }
    }
}
