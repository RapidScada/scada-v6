// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Subscribing;
using Scada.Lang;
using Scada.Log;

namespace Scada.Comm.Drivers.DrvMqtt
{
    /// <summary>
    /// Provides helper methods for using MQTT.
    /// <para>Предоставляет вспомогательные методы для использования MQTT.</para>
    /// </summary>
    public class MqttClientHelper
    {
        /// <summary>
        /// The delay before reconnect.
        /// </summary>
        private static readonly TimeSpan ReconnectDelay = TimeSpan.FromSeconds(5);

        private readonly MqttConnectionOptions connectionOptions; // the connection options
        private readonly ILog log;                                // implements logging
        private readonly IMqttClient mqttClient;                  // interacts with an MQTT broker
        private readonly IMqttClientOptions clientOptions;        // the client options
        private DateTime connAttemptDT;                           // the timestamp of a connection attempt


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttClientHelper(MqttConnectionOptions connectionOptions, ILog log)
        {
            this.connectionOptions = connectionOptions ?? throw new ArgumentNullException(nameof(connectionOptions));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            mqttClient = new MqttFactory().CreateMqttClient();
            clientOptions = connectionOptions.ToMqttClientOptions();

            connAttemptDT = DateTime.MinValue;
        }


        /// <summary>
        /// Gets the MQTT client.
        /// </summary>
        public IMqttClient Client => mqttClient;

        /// <summary>
        /// Gets a value indicating whether the MQTT client is connected to an MQTT broker.
        /// </summary>
        public bool IsConnected => mqttClient.IsConnected;


        /// <summary>
        /// Connects to the MQTT broker.
        /// </summary>
        public bool Connect()
        {
            try
            {
                log.WriteLine();

                // delay before connecting
                DateTime utcNow = DateTime.UtcNow;
                TimeSpan connectDelay = ReconnectDelay - (utcNow - connAttemptDT);

                if (connectDelay > TimeSpan.Zero)
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Задержка перед соединением {0} с" :
                        "Delay before connecting {0} sec",
                        connectDelay.TotalSeconds.ToString("N1"));
                    Thread.Sleep(connectDelay);
                }

                // connect to MQTT broker
                log.WriteAction(Locale.IsRussian ?
                    "Соединение с {0}:{1}" :
                    "Connect to {0}:{1}",
                    connectionOptions.Server, connectionOptions.Port);

                connAttemptDT = DateTime.UtcNow;
                MqttClientConnectResultCode resultCode =
                    mqttClient.ConnectAsync(clientOptions, CancellationToken.None).Result.ResultCode;

                if (resultCode == MqttClientConnectResultCode.Success)
                {
                    log.WriteLine(Locale.IsRussian ?
                        "Соединение установлено успешно" :
                        "Connected successfully");
                    return true;
                }
                else
                {
                    log.WriteLine(Locale.IsRussian ?
                        "Не удалось установить соединение: {0}" :
                        "Unable to connect: {0}", resultCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.WriteError(Locale.IsRussian ?
                    "Ошибка при установке соединения: {0}" :
                    "Error connecting: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Disconnects from the MQTT broker.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                log.WriteLine();
                log.WriteAction(Locale.IsRussian ?
                    "Отключение от {0}:{1}" :
                    "Disconnect from {0}:{1}",
                    connectionOptions.Server, connectionOptions.Port);
                mqttClient.DisconnectAsync().Wait();
            }
            catch (Exception ex)
            {
                log.WriteError(Locale.IsRussian ?
                    "Ошибка при отключении: {0}" :
                    "Error disconnecting: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Disconnects and disposes the MQTT client.
        /// </summary>
        public void Close()
        {
            if (mqttClient.IsConnected)
                Disconnect();

            mqttClient.Dispose();
        }

        /// <summary>
        /// Subscribes to the topics.
        /// </summary>
        public MqttClientSubscribeResult Subscribe(params MqttTopicFilter[] topicFilters)
        {
            ArgumentNullException.ThrowIfNull(topicFilters, nameof(topicFilters));
            return mqttClient.SubscribeAsync(topicFilters).Result;
        }

        /// <summary>
        /// Publishes the message.
        /// </summary>
        public MqttClientPublishResult Publish(MqttApplicationMessage message)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));
            return mqttClient.PublishAsync(message).Result;
        }

        /// <summary>
        /// Gets the current connection state as a string.
        /// </summary>
        public string GetConnectionState(bool isRussian)
        {
            return isRussian
                ? (mqttClient.IsConnected ? "соединение не установлено" : "соединение установлено")
                : (mqttClient.IsConnected ? "Connected" : "Disconnected");
        }
    }
}
