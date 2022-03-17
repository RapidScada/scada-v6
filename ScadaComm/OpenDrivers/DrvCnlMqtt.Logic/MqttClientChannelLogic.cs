// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Formatter;
using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Comm.Lang;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvCnlMqtt.Logic
{
    /// <summary>
    /// Implements the MQTT client channel logic.
    /// <para>Реализует логику канала MQTT-клиент.</para>
    /// </summary>
    internal class MqttClientChannelLogic : ChannelLogic, IMqttClientChannel
    {
        /// <summary>
        /// The number of characters to preview message content.
        /// </summary>
        private const int MessagePreviewLength = 20;
        /// <summary>
        /// The delay before reconnect.
        /// </summary>
        private static readonly TimeSpan ReconnectDelay = TimeSpan.FromSeconds(5);

        private readonly MqttClientChannelOptions options;       // the channel options
        private readonly MqttFactory mqttFactory;                // creates MQTT objects
        private readonly List<SubscriptionRecord> subscriptions; // the subscriptions added by devices

        private IMqttClient mqttClient;                    // interacts with an MQTT broker
        private IMqttClientOptions mqttClientOptions;      // the connection options
        private DateTime connAttemptDT;                    // the timestamp of a connection attempt


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttClientChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            options = new MqttClientChannelOptions(channelConfig.CustomOptions);
            mqttFactory = new MqttFactory();
            subscriptions = new List<SubscriptionRecord>();

            mqttClient = null;
            mqttClientOptions = null;
            connAttemptDT = DateTime.MinValue;
        }


        /// <summary>
        /// Handles a client connected event.
        /// </summary>
        private void MqttClient_Connected(MqttClientConnectedEventArgs e)
        {
            //Log.WriteAction("MqttClient_Connected");
        }

        /// <summary>
        /// Handles a client disconnected event.
        /// </summary>
        private void MqttClient_Disconnected(MqttClientDisconnectedEventArgs e)
        {
            //Log.WriteAction("MqttClient_Disconnected");
        }

        /// <summary>
        /// Handles a message received event.
        /// </summary>
        private void MqttClient_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                ReceivedMessage message = new()
                {
                    Topic = e.ApplicationMessage.Topic,
                    Content = e.ApplicationMessage.ConvertPayloadToString() ?? ""
                };

                Log.WriteLine();
                Log.WriteAction("{0} {1} = {2}", CommPhrases.ReceiveNotation, 
                    message.Topic, message.Content.GetPreview(MessagePreviewLength));
                Log.WriteLine(Locale.IsRussian ?
                    "Подписанные устройства: ..." :
                    "Subscribed devices: ...");
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при обработке полученного сообщения" :
                    "Error handling the received message");
            }
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
        /// Reconnects the MQTT client to the MQTT broker if it is disconnected.
        /// </summary>
        private void ReconnectIfRequired()
        {
            if (!mqttClient.IsConnected && Connect())
                Subscribe();
        }

        /// <summary>
        /// Connects the MQTT client to the MQTT broker.
        /// </summary>
        private bool Connect()
        {
            try
            {
                Log.WriteLine();

                // delay before connecting
                DateTime utcNow = DateTime.UtcNow;
                TimeSpan connectDelay = ReconnectDelay - (utcNow - connAttemptDT);

                if (connectDelay > TimeSpan.Zero)
                {
                    Log.WriteAction(Locale.IsRussian ?
                        "Задержка перед соединением {0} с" :
                        "Delay before connecting {0} sec",
                        connectDelay.TotalSeconds.ToString("N1"));
                    Thread.Sleep(connectDelay);
                }

                // connect to MQTT broker
                Log.WriteAction(Locale.IsRussian ?
                    "Соединение с {0}:{1}" :
                    "Connect to {0}:{1}",
                    options.Server, options.Port);

                connAttemptDT = DateTime.UtcNow;
                MqttClientConnectResultCode resultCode = 
                    mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None).Result.ResultCode;

                if (resultCode == MqttClientConnectResultCode.Success)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Соединение установлено успешно" :
                        "Connected successfully");
                    return true;
                }
                else
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Не удалось установить соединение: {0}" :
                        "Unable to connect: {0}", resultCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(Locale.IsRussian ?
                    "Ошибка при установке соединения: {0}" :
                    "Error connecting: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Connects the MQTT client from the MQTT broker.
        /// </summary>
        private void Disconnect()
        {
            try
            {
                Log.WriteLine();
                Log.WriteAction(Locale.IsRussian ?
                    "Отключение от {0}:{1}" :
                    "Disconnect from {0}:{1}",
                    options.Server, options.Port);
                mqttClient.DisconnectAsync().Wait();
            }
            catch (Exception ex)
            {
                Log.WriteError(Locale.IsRussian ?
                    "Ошибка при отключении: {0}" :
                    "Error disconnecting: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Subscribes to the topics previously added by devices.
        /// </summary>
        private void Subscribe()
        {
            try
            {
                Log.WriteLine();
                Log.WriteAction(Locale.IsRussian ?
                    "Подписка на топики" :
                    "Subscribe to topic");

                if (subscriptions.Count > 0)
                {
                    MqttTopicFilter[] topicFilters = subscriptions.Select(s => s.TopicFilter).ToArray();
                    mqttClient.SubscribeAsync(topicFilters).Wait();

                    Log.WriteLine(Locale.IsRussian ?
                        "Подписка выполнена успешно. Количество топиков: {0}" :
                        "Subscribed successfully. Topic count: {0}", topicFilters.Length);
                }
                else
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Топики отсутствуют" :
                        "Topics missing");
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(Locale.IsRussian ?
                    "Ошибка при подписке на топики: {0}" :
                    "Error subscribing to topics: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Subscribes to the topic.
        /// </summary>
        void IMqttClientChannel.Subscribe(SubscriptionRecord subscriptionRecord)
        {
            ArgumentNullException.ThrowIfNull(subscriptionRecord, nameof(subscriptionRecord));
            subscriptions.Add(subscriptionRecord);
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
        }

        /// <summary>
        /// Stops the communication channel.
        /// </summary>
        public override void Stop()
        {
            if (mqttClient != null)
            {
                if (mqttClient.IsConnected)
                    Disconnect();

                mqttClient.Dispose();
                mqttClient = null;
            }
        }

        /// <summary>
        /// Performs actions before polling the specified device.
        /// </summary>
        public override void BeforeSession(DeviceLogic deviceLogic)
        {
            ReconnectIfRequired();
        }
    }
}
