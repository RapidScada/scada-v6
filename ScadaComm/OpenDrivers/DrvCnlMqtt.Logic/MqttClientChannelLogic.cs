// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Formatter;
using Scada.Comm.Channels;
using Scada.Comm.Config;

namespace Scada.Comm.Drivers.DrvCnlMqtt.Logic
{
    /// <summary>
    /// Implements the MQTT client channel logic.
    /// <para>Реализует логику канала MQTT-клиент.</para>
    /// </summary>
    internal class MqttClientChannelLogic : ChannelLogic
    {
        private readonly MqttClientChannelOptions options; // the channel options
        private readonly MqttFactory mqttFactory;          // creates MQTT objects
        private IMqttClient mqttClient;                    // interacts with an MQTT broker
        private IMqttClientOptions mqttClientOptions;      // the connection options

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttClientChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            options = new MqttClientChannelOptions(channelConfig.CustomOptions);
            mqttFactory = new MqttFactory();

            mqttClient = null;
            mqttClientOptions = null;
        }


        /// <summary>
        /// Handles a client connected event.
        /// </summary>
        private void MqttClient_Connected(MqttClientConnectedEventArgs e)
        {
            Log.WriteAction("MqttClient_Connected");
        }

        /// <summary>
        /// Handles a client disconnected event.
        /// </summary>
        private void MqttClient_Disconnected(MqttClientDisconnectedEventArgs e)
        {
            Log.WriteAction("MqttClient_Disconnected");
        }

        /// <summary>
        /// Handles a message received event.
        /// </summary>
        private void MqttClient_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            Log.WriteAction("MqttClient_ApplicationMessageReceived topic = " + e.ApplicationMessage.Topic + 
                ", data =" + e.ApplicationMessage.ConvertPayloadToString());
        }

        /// <summary>
        /// Initializes the MQTT client options.
        /// </summary>
        private void InitClientOptions()
        {
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                .WithTcpServer(options.Server, options.Port > 0 ? options.Port : null);

            if (!string.IsNullOrEmpty(options.ClientID))
                builder.WithClientId(options.ClientID);

            if (!string.IsNullOrEmpty(options.Username))
                builder.WithCredentials(options.Username, options.Password);

            if (options.Timeout > 0)
                builder.WithCommunicationTimeout(TimeSpan.FromMilliseconds(options.Timeout));

            if (options.ProtocolVersion > ProtocolVersion.Unknown)
                builder.WithProtocolVersion((MqttProtocolVersion)options.ProtocolVersion);

            mqttClientOptions = builder.Build();
        }


        /// <summary>
        /// Makes the communication channel ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            mqttClient = mqttFactory.CreateMqttClient();
            mqttClient.UseConnectedHandler(MqttClient_Connected);
            mqttClient.UseDisconnectedHandler(MqttClient_Disconnected);
            mqttClient.UseApplicationMessageReceivedHandler(MqttClient_ApplicationMessageReceived);
            InitClientOptions();
        }

        /// <summary>
        /// Starts the communication channel.
        /// </summary>
        public override void Start()
        {
            mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None).Wait();

            MqttClientSubscribeOptions subscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic("/myparam1"); })
                .Build();

            mqttClient.SubscribeAsync(subscribeOptions, CancellationToken.None).Wait();
            Log.WriteLine("!!! MQTT client subscribed to topic.");
        }

        /// <summary>
        /// Stops the communication channel.
        /// </summary>
        public override void Stop()
        {
            if (mqttClient != null)
            {
                mqttClient.DisconnectAsync().Wait();
                mqttClient.Dispose();
            }
        }
    }
}
