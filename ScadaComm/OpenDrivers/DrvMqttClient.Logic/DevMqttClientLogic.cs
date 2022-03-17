// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Protocol;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Comm.Lang;
using Scada.Data.Models;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvMqttClient.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevMqttClientLogic : DeviceLogic, ISubscriber
    {
        private readonly MqttClientDeviceConfig config; // the device configuration
        private IMqttClientChannel mqttClientChannel;   // the communication channel reference


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevMqttClientLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = new MqttClientDeviceConfig();
            mqttClientChannel = null;

            ConnectionRequired = false;
        }


        /// <summary>
        /// Creates subscriptions according to the configuration.
        /// </summary>
        private void CreateSubscriptions()
        {
            foreach (SubscriptionConfig subscriptionConfig in config.Subscriptions)
            {
                mqttClientChannel.Subscribe(new SubscriptionRecord
                {
                    Subscriber = this,
                    TopicFilter = new MqttTopicFilter
                    {
                        Topic = subscriptionConfig.Topic,
                        QualityOfServiceLevel = (MqttQualityOfServiceLevel)subscriptionConfig.QosLevel
                    },
                    AuxData = null
                });
            }
        }

        /// <summary>
        /// Handles the received message.
        /// </summary>
        void ISubscriber.HandleReceivedMessage(ReceivedMessage message)
        {

        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            if (!config.Load(Storage, MqttClientDeviceConfig.GetFileName(DeviceNum), out string errMsg))
                Log.WriteLine(CommPhrases.DeviceMessage, Title, errMsg);

            if (LineContext.Channel is IMqttClientChannel channel)
            {
                mqttClientChannel = channel;
                CreateSubscriptions();
            }
            else
            {
                Log.WriteLine(CommPhrases.DeviceMessage, Title, Locale.IsRussian ?
                    "Требуется канал связи MQTT-клиент" :
                    "MQTT client communication channel required");
            }
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {

        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            //base.Session();
            //FinishSession();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {

        }
    }
}
