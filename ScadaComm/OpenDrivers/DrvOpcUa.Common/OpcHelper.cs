// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Lang;
using Scada.Log;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvOpcUa
{
    /// <summary>
    /// Helper methods to work using OPC UA.
    /// <para>Вспомогательные методы для работы с OPC UA.</para>
    /// </summary>
    public class OpcHelper
    {
        /// <summary>
        /// The runtime kinds.
        /// </summary>
        public enum RuntimeKind { Logic, View };

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
        private readonly RuntimeKind runtime; // the runtime kind


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcHelper(AppDirs appDirs, ILog log, int deviceNum, RuntimeKind runtime)
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            deviceNumStr = deviceNum.ToString("D3");
            this.runtime = runtime;

            AutoAccept = false;
            OpcSession = null;
            CertificateValidation = null;
        }


        /// <summary>
        /// Gets or sets the certificate validation method.
        /// </summary>
        public CertificateValidationEventHandler CertificateValidation { get; set; }

        /// <summary>
        /// Gets a value indicating whether to automatically accept server certificates.
        /// </summary>
        public bool AutoAccept { get; private set; }

        /// <summary>
        /// Gets the OPC session
        /// </summary>
        public Session OpcSession { get; private set; }


        /// <summary>
        /// Writes an OPC UA configuration file depending on operating system and runtime kind.
        /// </summary>
        private void WriteConfigFile(out string fileName)
        {
            fileName = Path.Combine(appDirs.ConfigDir, runtime == RuntimeKind.View ? ViewOpcConfig : LogicOpcConfig);

            if (!File.Exists(fileName))
            {
                string resourceName = ScadaUtils.IsRunningOnWin ?
                    "Scada.Comm.Drivers.DrvOpcUa.Config.Win.xml" :
                    "Scada.Comm.Drivers.DrvOpcUa.Config.Linux.xml";
                string fileContents;

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        fileContents = reader.ReadToEnd();
                    }
                }

                File.WriteAllText(fileName, fileContents, Encoding.UTF8);
            }
        }

        /// <summary>
        /// Connects to the OPC server asynchronously.
        /// </summary>
        public async Task<bool> ConnectAsync(OpcConnectionOptions connectionOptions, int operationTimeout = -1)
        {
            AutoAccept = false;
            OpcSession = null;

            ApplicationInstance application = new ApplicationInstance
            {
                ApplicationName = string.Format("DrvOpcUa_{0} Driver", deviceNumStr),
                ApplicationType = ApplicationType.Client,
                ConfigSectionName = "Scada.Comm.Drivers.DrvOpcUa"
            };

            // load the application configuration
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
                {
                    AutoAccept = true;
                }

                if (CertificateValidation != null)
                    config.CertificateValidator.CertificateValidation += CertificateValidation;
            }
            else
            {
                log.WriteLine(Locale.IsRussian ?
                    "Предупреждение: отсутствует сертификат приложения, используется незащищенное соединение." :
                    "Warning: missing application certificate, using unsecure connection.");
            }

            // create session
            EndpointDescription selectedEndpoint = CoreClientUtils.SelectEndpoint(
                connectionOptions.ServerUrl, haveAppCertificate, operationTimeout);
            selectedEndpoint.SecurityMode = connectionOptions.SecurityMode;
            selectedEndpoint.SecurityPolicyUri = connectionOptions.GetSecurityPolicy();
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(config);
            ConfiguredEndpoint endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);
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
