// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Publishing;
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
        /// Represents metadata about a topic.
        /// </summary>
        private class TopicTag
        {
            private string subscribedDevices = null;

            public string Topic { get; init; }
            public MqttTopicFilter TopicFilter { get; init; }
            public List<SubscriptionRecord> Subscriptions { get; } = new List<SubscriptionRecord>();
            public string SubscribedDevices
            {
                get
                {
                    if (subscribedDevices == null)
                    {
                        subscribedDevices = string.Format(Locale.IsRussian ?
                            "Подписанные устройства: {0}" :
                            "Subscribed devices: {0}",
                            string.Join(", ", Subscriptions.Select(s => s.Subscriber.DeviceNum)));
                    }

                    return subscribedDevices;
                }
            }
        }

        private readonly MqttClientHelper mqttClientHelper;      // encapsulates an MQTT client
        private readonly Dictionary<string, TopicTag> topicTags; // the topic metadata accessed by topic name
        private readonly object sessionLock;                     // synchronizes sessions and incoming messages


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttClientChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            mqttClientHelper = new MqttClientHelper(new MqttConnectionOptions(channelConfig.CustomOptions), Log);
            topicTags = new Dictionary<string, TopicTag>();
            sessionLock = new object();
        }


        /// <summary>
        /// Reconnects the MQTT client to the MQTT broker if it is disconnected.
        /// </summary>
        private void ReconnectIfRequired()
        {
            if (!mqttClientHelper.IsConnected && mqttClientHelper.Connect())
                Subscribe();
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

                if (topicTags.Count > 0)
                {
                    MqttTopicFilter[] topicFilters = topicTags.Select(kvp => kvp.Value.TopicFilter)
                        .OrderBy(filter => filter.Topic).ToArray();
                    mqttClientHelper.Subscribe(topicFilters);

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
                Monitor.Enter(sessionLock);

                ReceivedMessage message = new()
                {
                    Topic = e.ApplicationMessage.Topic,
                    Payload = e.ApplicationMessage.ConvertPayloadToString() ?? "",
                    PayloadData = e.ApplicationMessage.Payload
                };

                Log.WriteLine();
                Log.WriteAction("{0} {1} = {2}", CommPhrases.ReceiveNotation,
                    message.Topic, message.Payload.GetPreview(MqttUtils.MessagePreviewLength));

                if (topicTags.TryGetValue(message.Topic, out TopicTag topicTag))
                {
                    Log.WriteLine(topicTag.SubscribedDevices);

                    foreach (SubscriptionRecord subscription in topicTag.Subscriptions)
                    {
                        message.AuxData = subscription.AuxData;
                        subscription.Subscriber.HandleReceivedMessage(message);
                    }
                }
                else
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Подписанные устройства отсутствуют" :
                        "Subscribed devices missing");
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при обработке полученного сообщения. Топик: {0}" :
                    "Error handling the received message. Topic: {0}", e?.ApplicationMessage?.Topic);
            }
            finally
            {
                Monitor.Exit(sessionLock);
            }
        }


        /// <summary>
        /// Gets a value indicating whether the MQTT client is connected to an MQTT broker.
        /// </summary>
        bool IMqttClientChannel.IsConnected => mqttClientHelper.IsConnected;

        /// <summary>
        /// Subscribes to the topic.
        /// </summary>
        void IMqttClientChannel.Subscribe(SubscriptionRecord subscriptionRecord)
        {
            ArgumentNullException.ThrowIfNull(subscriptionRecord, nameof(subscriptionRecord));
            string topic = subscriptionRecord.TopicFilter?.Topic;

            if (!string.IsNullOrEmpty(topic))
            {
                if (!topicTags.TryGetValue(topic, out TopicTag topicTag))
                {
                    topicTag = new TopicTag
                    {
                        Topic = topic,
                        TopicFilter = subscriptionRecord.TopicFilter
                    };
                    topicTags.Add(topic, topicTag);
                }

                topicTag.Subscriptions.Add(subscriptionRecord);
            }
        }

        /// <summary>
        /// Publishes the message to the MQTT broker.
        /// </summary>
        MqttClientPublishResult IMqttClientChannel.Publish(MqttApplicationMessage message)
        {
            return mqttClientHelper.Publish(message);
        }


        /// <summary>
        /// Makes the communication channel ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            mqttClientHelper.Client.UseConnectedHandler(MqttClient_Connected);
            mqttClientHelper.Client.UseDisconnectedHandler(MqttClient_Disconnected);
            mqttClientHelper.Client.UseApplicationMessageReceivedHandler(MqttClient_ApplicationMessageReceived);
        }

        /// <summary>
        /// Stops the communication channel.
        /// </summary>
        public override void Stop()
        {
            mqttClientHelper.Close();
        }

        /// <summary>
        /// Performs actions before polling the specified device.
        /// </summary>
        public override void BeforeSession(DeviceLogic deviceLogic)
        {
            ReconnectIfRequired();
            Monitor.Enter(sessionLock);
        }

        /// <summary>
        /// Performs actions after polling the specified device.
        /// </summary>
        public override void AfterSession(DeviceLogic deviceLogic)
        {
            Monitor.Exit(sessionLock);
        }
    }
}
