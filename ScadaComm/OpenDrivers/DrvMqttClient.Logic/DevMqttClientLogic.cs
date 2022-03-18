// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Protocol;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Comm.Lang;
using Scada.Data.Models;
using Scada.Lang;
using System.Globalization;
using System.Text;

namespace Scada.Comm.Drivers.DrvMqttClient.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevMqttClientLogic : DeviceLogic, ISubscriber
    {
        /// <summary>
        /// Represents metadata about a subscription.
        /// </summary>
        private class SubscriptionTag
        {
            public SubscriptionConfig SubscriptionConfig { get; init; }
            public DeviceTag DeviceTag { get; init; }
        }

        private readonly MqttClientDeviceConfig config;               // the device configuration
        private readonly Dictionary<string, CommandConfig> cmdByCode; // the commands accessed by code
        private IMqttClientChannel mqttClientChannel;                 // the communication channel reference


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevMqttClientLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = new MqttClientDeviceConfig();
            cmdByCode = new Dictionary<string, CommandConfig>();
            mqttClientChannel = null;

            CanSendCommands = true;
            ConnectionRequired = false;
        }


        /// <summary>
        /// Fills the command map.
        /// </summary>
        private void FillCmdMap()
        {
            // explicit commands
            foreach (CommandConfig commandConfig in config.Commands)
            {
                if (!string.IsNullOrEmpty(commandConfig.CmdCode) && !cmdByCode.ContainsKey(commandConfig.CmdCode))
                    cmdByCode.Add(commandConfig.CmdCode, commandConfig);
            }

            // commands from subscriptions
            foreach (SubscriptionConfig subscriptionConfig in config.Subscriptions)
            {
                if (!subscriptionConfig.ReadOnly &&
                    !string.IsNullOrEmpty(subscriptionConfig.TagCode) &&
                    !cmdByCode.ContainsKey(subscriptionConfig.TagCode))
                {
                    cmdByCode.Add(subscriptionConfig.TagCode, new CommandConfig
                    {
                        Topic = subscriptionConfig.Topic,
                        DisplayName = subscriptionConfig.DisplayName,
                        CmdCode = subscriptionConfig.TagCode,
                        QosLevel = subscriptionConfig.QosLevel
                    });
                }
            }
        }

        /// <summary>
        /// Creates a subscription for the device tag.
        /// </summary>
        private void CreateSubscription(SubscriptionConfig subscriptionConfig, DeviceTag deviceTag)
        {
            mqttClientChannel.Subscribe(new SubscriptionRecord
            {
                Subscriber = this,
                TopicFilter = new MqttTopicFilter
                {
                    Topic = subscriptionConfig.Topic,
                    QualityOfServiceLevel = (MqttQualityOfServiceLevel)subscriptionConfig.QosLevel
                },
                AuxData = new SubscriptionTag
                {
                    SubscriptionConfig = subscriptionConfig,
                    DeviceTag = deviceTag
                }
            });
        }

        /// <summary>
        /// Creates a message to be published according to the command.
        /// </summary>
        private static MqttApplicationMessage CreateMessage(TeleCommand cmd, CommandConfig cmdConfig, out string valStr)
        {
            MqttApplicationMessage message = new()
            {
                Topic = cmdConfig.Topic,
                QualityOfServiceLevel = (MqttQualityOfServiceLevel)cmdConfig.QosLevel
            };

            if (cmd.CmdData == null)
            {
                valStr = Locale.IsRussian ? "<данные>" : "<data>";
                message.Payload = cmd.CmdData;
            }
            else
            {
                valStr = cmd.CmdVal.ToString(NumberFormatInfo.InvariantInfo);
                message.Payload = Encoding.UTF8.GetBytes(valStr);
            }

            return message;
        }

        /// <summary>
        /// Handles the received message.
        /// </summary>
        void ISubscriber.HandleReceivedMessage(ReceivedMessage message)
        {
            if (message.AuxData is SubscriptionTag subscriptionTag)
            {
                if (subscriptionTag.SubscriptionConfig.JsEnabled)
                {

                }
                else if (ScadaUtils.TryParseDouble(message.Content, out double val))
                {
                    DeviceData.Set(subscriptionTag.DeviceTag.Index, val);
                }
                else
                {
                    DeviceData.Invalidate(subscriptionTag.DeviceTag.Index);
                }
            }
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
                FillCmdMap();
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
            TagGroup tagGroup = new();

            foreach (SubscriptionConfig subscriptionConfig in config.Subscriptions)
            {
                DeviceTag deviceTag = tagGroup.AddTag(subscriptionConfig.TagCode, subscriptionConfig.DisplayName);
                CreateSubscription(subscriptionConfig, deviceTag);
            }

            DeviceTags.AddGroup(tagGroup);
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
            base.SendCommand(cmd);
            LastRequestOK = false;

            if (mqttClientChannel == null || !mqttClientChannel.IsConnected)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка: соединение с MQTT-брокером не установлено" :
                    "Error: connection with the MQTT broker is not established");
            }
            else if (string.IsNullOrEmpty(cmd.CmdCode) || 
                !cmdByCode.TryGetValue(cmd.CmdCode, out CommandConfig commandConfig))
            {
                Log.WriteLine(CommPhrases.InvalidCommand);
            }
            else
            {
                try
                {
                    MqttApplicationMessage message = CreateMessage(cmd, commandConfig, out string valStr);
                    Log.WriteLine("{0} {1} = {2}", CommPhrases.SendNotation, commandConfig.Topic, valStr);
                    MqttClientPublishResult result = mqttClientChannel.Publish(message);

                    if (result.ReasonCode == MqttClientPublishReasonCode.Success)
                    {
                        Log.WriteLine(CommPhrases.ResponseOK);
                        LastRequestOK = true;
                    }
                    else
                    {
                        Log.WriteLine(CommPhrases.ErrorPrefix + result.ReasonCode);
                    }
                }
                catch (ScadaException ex)
                {
                    Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                }
            }

            FinishCommand();
        }
    }
}
