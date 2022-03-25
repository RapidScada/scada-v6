// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Jint;
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
            public int TagIndex => DeviceTag.Index;
            public string JsSource { get; set; }
            public double[] JsValues { get; set; }
        }

        /// <summary>
        /// The default data lifetime in seconds. Zero means no lifetime is used.
        /// </summary>
        private const int DefautDataLifetime = 0;

        private readonly MqttClientDeviceConfig config;               // the device configuration
        private readonly Dictionary<string, CommandConfig> cmdByCode; // the commands accessed by code

        private IMqttClientChannel mqttClientChannel; // the communication channel reference
        private Engine jsEngine;                      // executes JavaScript
        private TimeSpan dataLifetime;                // specifies when tag values should be invalidated
        private bool useDataLifetime;                 // indicates that lifetime is used
        private DateTime[] updateTimestamps;          // the update timestamps by device tag


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevMqttClientLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = new MqttClientDeviceConfig();
            cmdByCode = new Dictionary<string, CommandConfig>();

            mqttClientChannel = null;
            jsEngine = null;
            dataLifetime = TimeSpan.Zero;
            useDataLifetime = false;
            updateTimestamps = null;

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
                        QosLevel = subscriptionConfig.QosLevel,
                        Retain = subscriptionConfig.Retain
                    });
                }
            }
        }

        /// <summary>
        /// Subscribes to the device tag.
        /// </summary>
        private void Subscribe(SubscriptionConfig subscriptionConfig, DeviceTag deviceTag)
        {
            mqttClientChannel.Subscribe(new SubscriptionRecord
            {
                Subscriber = this,
                TopicFilter = new MqttTopicFilter
                {
                    Topic = config.DeviceOptions.RootTopic + subscriptionConfig.Topic,
                    QualityOfServiceLevel = (MqttQualityOfServiceLevel)subscriptionConfig.QosLevel
                },
                AuxData = new SubscriptionTag
                {
                    SubscriptionConfig = subscriptionConfig,
                    DeviceTag = deviceTag,
                    JsSource = null,
                    JsValues = null
                }
            });
        }

        /// <summary>
        /// Creates a message to be published according to the command.
        /// </summary>
        private MqttApplicationMessage CreateMessage(TeleCommand cmd, CommandConfig cmdConfig, out string valStr)
        {
            MqttApplicationMessage message = new()
            {
                Topic = config.DeviceOptions.RootTopic + cmdConfig.Topic,
                QualityOfServiceLevel = (MqttQualityOfServiceLevel)cmdConfig.QosLevel,
                Retain = cmdConfig.Retain
            };

            if (!double.IsNaN(cmd.CmdVal))
            {
                valStr = cmd.CmdVal.ToString(NumberFormatInfo.InvariantInfo);
                message.Payload = Encoding.UTF8.GetBytes(valStr);
            }
            else if (cmd.CmdData != null)
            {
                valStr = Locale.IsRussian ? "<данные>" : "<data>";
                message.Payload = cmd.CmdData;
            }
            else
            {
                valStr = "null";
            }

            return message;
        }

        /// <summary>
        /// Executes the JavaScript corresponding to the received message.
        /// </summary>
        private void ExecuteJavaScript(ReceivedMessage message, SubscriptionTag subscriptionTag)
        {
            // initialize engine
            if (jsEngine == null)
            {
                jsEngine = new Engine()
                    .SetValue("log", new Action<string>(s => Log.WriteLine(s)))
                    .SetValue("setValue", new Action<int, double>((i, x) => { subscriptionTag.JsValues[i] = x; }));
            }

            // load source code
            if (subscriptionTag.JsSource == null)
            {
                subscriptionTag.JsSource =
                    Storage.ReadText(Storages.DataCategory.Config, subscriptionTag.SubscriptionConfig.JsFileName);
            }

            // initialize tag data
            int tagCount = Math.Max(1, subscriptionTag.SubscriptionConfig.SubItems.Count);

            if (subscriptionTag.JsValues == null)
                subscriptionTag.JsValues = new double[tagCount];

            for (int i = 0; i < tagCount; i++)
            {
                subscriptionTag.JsValues[i] = double.NaN;
            }

            // set script variables
            jsEngine.SetValue("topic", message.Topic);
            jsEngine.SetValue("payload", message.Payload);

            try
            {
                // execute script
                jsEngine.Execute(subscriptionTag.JsSource);

                // get tag values
                for (int i = 0; i < tagCount; i++)
                {
                    int tagIndex = subscriptionTag.TagIndex + i;
                    double tagValue = subscriptionTag.JsValues[i];

                    if (double.IsNaN(tagValue))
                        DeviceData.Invalidate(tagIndex);
                    else
                        DeviceData.Set(tagIndex, tagValue);

                    updateTimestamps[tagIndex] = LastSessionTime;
                }
            }
            catch
            {
                // invalidate tag values
                for (int i = 0; i < tagCount; i++)
                {
                    int tagIndex = subscriptionTag.TagIndex + i;
                    DeviceData.Invalidate(tagIndex);
                    updateTimestamps[tagIndex] = LastSessionTime;
                }

                throw;
            }
        }

        /// <summary>
        /// Invalidates device tags that have not been updated for longer than the data lifetime.
        /// </summary>
        private void InvalidateOutdatedData()
        {
            DateTime utcNow = DateTime.UtcNow;

            for (int i = 0, len = updateTimestamps.Length; i < len; i++)
            {
                if (utcNow - updateTimestamps[i] > dataLifetime)
                {
                    DeviceData.Invalidate(i);
                    updateTimestamps[i] = utcNow;
                }
            }
        }

        /// <summary>
        /// Handles the received message.
        /// </summary>
        void ISubscriber.HandleReceivedMessage(ReceivedMessage message)
        {
            try
            {
                LastSessionTime = DateTime.UtcNow;

                if (message.AuxData is SubscriptionTag subscriptionTag)
                {
                    if (subscriptionTag.SubscriptionConfig.JsEnabled)
                    {
                        ExecuteJavaScript(message, subscriptionTag);
                    }
                    else
                    {
                        if (ScadaUtils.TryParseDouble(message.Payload, out double val))
                            DeviceData.Set(subscriptionTag.TagIndex, val);
                        else
                            DeviceData.Invalidate(subscriptionTag.TagIndex);

                        updateTimestamps[subscriptionTag.TagIndex] = LastSessionTime;
                    }

                    LastRequestOK = true;
                }
                else
                {
                    LastRequestOK = false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                LastRequestOK = false;
            }
            finally
            {
                FinishSession();
            }
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            dataLifetime = TimeSpan.FromSeconds(
                LineContext.LineConfig.CustomOptions.GetValueAsInt("DataLifetime", DefautDataLifetime));
            useDataLifetime = dataLifetime > TimeSpan.Zero;

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
                if (subscriptionConfig.JsEnabled && subscriptionConfig.SubItems.Count > 0)
                {
                    for (int i = 0, cnt = subscriptionConfig.SubItems.Count; i < cnt; i++)
                    {
                        string suffix = "." + subscriptionConfig.SubItems[i];
                        DeviceTag deviceTag = tagGroup.AddTag(
                            subscriptionConfig.TagCode + suffix, 
                            subscriptionConfig.DisplayName + suffix);

                        if (i == 0)
                            Subscribe(subscriptionConfig, deviceTag);
                    }
                }
                else
                {
                    DeviceTag deviceTag = tagGroup.AddTag(subscriptionConfig.TagCode, subscriptionConfig.DisplayName);
                    Subscribe(subscriptionConfig, deviceTag);
                }
            }

            DeviceTags.AddGroup(tagGroup);
            DeviceTags.FlattenGroups = true;
            updateTimestamps = new DateTime[tagGroup.DeviceTags.Count];
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            if (useDataLifetime)
                InvalidateOutdatedData();

            if (mqttClientChannel == null || !mqttClientChannel.IsConnected)
                DeviceStatus = DeviceStatus.Error;

            SleepPollingDelay();
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
                catch (Exception ex)
                {
                    Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                }
            }

            FinishCommand();
        }
    }
}
