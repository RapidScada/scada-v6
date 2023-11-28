using Scada.Comm.Drivers.DrvGoogle.Common;
using Scada.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGoogle.Common
{
    public class GoogleCloudOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GoogleCloudOptions()
        {
            Server = "";
            ServerKey = "";
            Timeout = 10000;
            ClientID = "";
            ClientSecret = "";
            UseAdcFile = false;
            AdcFilePath = "";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GoogleCloudOptions(OptionList options)
        {
            CredentialType = options.GetValueAsEnum("CredentialType", GoogleCredentialType.CloudScadaAccessToken);
            Server = options.GetValueAsString("Server");
            ServerKey = ScadaUtils.Decrypt(options.GetValueAsString("ServerKey"));
            Timeout = options.GetValueAsInt("Timeout", 10000);
            ClientID = options.GetValueAsString("ClientID");
            ClientSecret = options.GetValueAsString("ClientSecret");
            UseAdcFile = options.GetValueAsBool("UseAdcFile");
            AdcFilePath = options.GetValueAsString("AdcFilePath");
        }

        /// <summary>
        /// 认证方式
        /// </summary>
        public GoogleCredentialType CredentialType { get; set; }

        /// <summary>
        /// Gets or sets the server host.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the server key.
        /// </summary>
        public string ServerKey { get; set; }

        /// <summary>
        /// Gets or sets the send and receive timeout, ms.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the unique client ID.
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// Gets or sets the unique client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use TLS.
        /// </summary>
        public bool UseAdcFile { get; set; }

        /// <summary>
        /// Gets or sets the adc file path.
        /// </summary>
        public string AdcFilePath { get; set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options, bool clearList = true)
        {
            if (clearList)
                options.Clear();

            options["CredentialType"] = CredentialType.ToString();
            options["Server"] = Server;
            options["ServerKey"] = ScadaUtils.Encrypt(ServerKey);
            options["ClientID"] = ClientID;
            options["ClientSecret"] = ClientSecret;
            options["Timeout"] = Timeout.ToString();
            options["UseAdcFile"] = UseAdcFile.ToLowerString();
            options["AdcFilePath"] = AdcFilePath;
        }

        /*
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
                builder.WithTls();

            if (!string.IsNullOrEmpty(ClientID))
                builder.WithClientId(ClientID);

            if (!string.IsNullOrEmpty(Username))
                builder.WithCredentials(Username, Password);

            if (ProtocolVersion > MqttProtocolVersion.Unknown)
                builder.WithProtocolVersion(ProtocolVersion);

            return builder.Build();
        }
        */
    }
}
