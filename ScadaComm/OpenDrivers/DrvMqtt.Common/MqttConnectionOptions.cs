// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet.Client;
using MQTTnet.Formatter;
using Scada.Config;
using System.Security.Cryptography.X509Certificates;

namespace Scada.Comm.Drivers.DrvMqtt
{
    /// <summary>
    /// Represents options for connecting to an MQTT broker.
    /// <para>Представляет параметры подключения к MQTT-брокеру.</para>
    /// </summary>
    public class MqttConnectionOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttConnectionOptions()
        {
            Server = "";
            Port = 1883;
            Timeout = 10000;
            UseTls = false;
            ClientID = "";
            Username = "";
            Password = "";
            ProtocolVersion = MqttProtocolVersion.Unknown;
            CaCertFile = "";
            ClientCertFile = "";
            ClientCertPassword = "";
            AllowUntrustedCertificates = false;
            IgnoreCertificateRevocationErrors = false;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttConnectionOptions(OptionList options)
        {
            Server = options.GetValueAsString("Server");
            Port = options.GetValueAsInt("Port", 1883);
            Timeout = options.GetValueAsInt("Timeout", 10000);
            UseTls = options.GetValueAsBool("UseTls");
            ClientID = options.GetValueAsString("ClientID");
            Username = options.GetValueAsString("Username");
            Password = ScadaUtils.Decrypt(options.GetValueAsString("Password"));
            ProtocolVersion = options.GetValueAsEnum("ProtocolVersion", MqttProtocolVersion.Unknown);
            CaCertFile = options.GetValueAsString("CaCertFile");
            ClientCertFile = options.GetValueAsString("ClientCertFile");
            ClientCertPassword = ScadaUtils.Decrypt(options.GetValueAsString("ClientCertPassword"));
            AllowUntrustedCertificates = options.GetValueAsBool("AllowUntrustedCertificates");
            IgnoreCertificateRevocationErrors = options.GetValueAsBool("IgnoreCertificateRevocationErrors");
        }


        /// <summary>
        /// Gets or sets the server host.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the TCP port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the send and receive timeout, ms.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use TLS.
        /// </summary>
        public bool UseTls { get; set; }

        /// <summary>
        /// Gets or sets the unique client ID.
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the protocol version.
        /// </summary>
        public MqttProtocolVersion ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the path to a CA certificate file used to validate the broker,
        /// instead of the operating system trust store. Use for a private or self-signed CA.
        /// </summary>
        public string CaCertFile { get; set; }

        /// <summary>
        /// Gets or sets the path to a client certificate file (PFX/P12) presented to the
        /// broker for mutual TLS authentication.
        /// </summary>
        public string ClientCertFile { get; set; }

        /// <summary>
        /// Gets or sets the password protecting the client certificate file.
        /// </summary>
        public string ClientCertPassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to accept the broker certificate
        /// without validating it. Ignored if CaCertFile is specified.
        /// </summary>
        public bool AllowUntrustedCertificates { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore certificate revocation
        /// check failures, useful for private CAs that don't publish CRL/OCSP endpoints.
        /// </summary>
        public bool IgnoreCertificateRevocationErrors { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options, bool clearList = true)
        {
            if (clearList)
                options.Clear();

            options["Server"] = Server;
            options["Port"] = Port.ToString();
            options["ClientID"] = ClientID;
            options["Username"] = Username;
            options["Password"] = ScadaUtils.Encrypt(Password);
            options["Timeout"] = Timeout.ToString();
            options["UseTls"] = UseTls.ToLowerString();
            options["ProtocolVersion"] = ProtocolVersion.ToString();
            options["CaCertFile"] = CaCertFile;
            options["ClientCertFile"] = ClientCertFile;
            options["ClientCertPassword"] = ScadaUtils.Encrypt(ClientCertPassword);
            options["AllowUntrustedCertificates"] = AllowUntrustedCertificates.ToLowerString();
            options["IgnoreCertificateRevocationErrors"] = IgnoreCertificateRevocationErrors.ToLowerString();
        }

        /// <summary>
        /// Builds a certificate validation handler that trusts only the specified CA certificate,
        /// instead of the operating system trust store. Supports private and self-signed CAs.
        /// </summary>
        private Func<MqttClientCertificateValidationEventArgs, bool> CreateCaValidationHandler()
        {
            X509Certificate2 caCert = new(CaCertFile);

            return context =>
            {
                using X509Certificate2 serverCert = new(context.Certificate);
                using X509Chain chain = new();
                chain.ChainPolicy.RevocationMode = IgnoreCertificateRevocationErrors
                    ? X509RevocationMode.NoCheck
                    : X509RevocationMode.Online;
                chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                chain.ChainPolicy.CustomTrustStore.Add(caCert);
                return chain.Build(serverCert);
            };
        }

        /// <summary>
        /// Loads the client certificate used for mutual TLS authentication.
        /// </summary>
        private X509Certificate2 LoadClientCert()
        {
            return string.IsNullOrEmpty(ClientCertPassword)
                ? new X509Certificate2(ClientCertFile)
                : new X509Certificate2(ClientCertFile, ClientCertPassword);
        }

        /// <summary>
        /// Converts the connection options to client options.
        /// </summary>
        public MqttClientOptions ToMqttClientOptions()
        {
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                .WithTcpServer(Server, Port > 0 ? Port : null);

            if (Timeout > 0)
                builder.WithTimeout(TimeSpan.FromMilliseconds(Timeout));

            if (UseTls)
            {
                builder.WithTlsOptions(o =>
                {
                    o.UseTls();

                    if (!string.IsNullOrEmpty(CaCertFile))
                    {
                        // Pin a private/self-signed CA instead of trusting the OS store.
                        o.WithCertificateValidationHandler(CreateCaValidationHandler());
                    }
                    else if (AllowUntrustedCertificates)
                    {
                        o.WithAllowUntrustedCertificates();
                        o.WithIgnoreCertificateChainErrors();

                        if (IgnoreCertificateRevocationErrors)
                            o.WithIgnoreCertificateRevocationErrors();
                    }
                    else if (IgnoreCertificateRevocationErrors)
                    {
                        o.WithIgnoreCertificateRevocationErrors();
                    }

                    if (!string.IsNullOrEmpty(ClientCertFile))
                        o.WithClientCertificates(new X509Certificate2Collection(LoadClientCert()));
                });
            }

            if (!string.IsNullOrEmpty(ClientID))
                builder.WithClientId(ClientID);

            if (!string.IsNullOrEmpty(Username))
                builder.WithCredentials(Username, Password);

            if (ProtocolVersion > MqttProtocolVersion.Unknown)
                builder.WithProtocolVersion(ProtocolVersion);

            return builder.Build();
        }
    }
}
