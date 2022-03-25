// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Protocol;
using Scada.Client;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Comm.Drivers.DrvMqttPublisher.Config;
using Scada.Comm.Lang;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using System.Globalization;
using System.Text;

namespace Scada.Comm.Drivers.DrvMqttPublisher.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevMqttPublisherLogic : DeviceLogic, ISubscriber
    {
        /// <summary>
        /// Contains publisher data common to a communication line.
        /// </summary>
        private class MqttPublisherLineData
        {
            public ScadaClient ScadaClient { get; init; }
            public Queue<TeleCommand> CommandQueue { get; init; }
            public override string ToString() => CommPhrases.SharedObject;
        }

        private readonly MqttPublisherDeviceConfig config;       // the device configuration
        private readonly Dictionary<int, DeviceTag> tagByCnlNum; // the device tags accessed by channel number

        private bool fatalError;                      // normal operation is impossible
        private IMqttClientChannel mqttClientChannel; // the communication channel reference
        private MqttPublisherLineData lineData;       // data common to the communication line
        private int[] publishCnlNums;                 // the numbers of the published channels
        private long cnlListID;                       // the cached channel list ID
        private DateTime curDataTimestamp;            // the timestamp of the last received current data
        private TimeSpan publishPeriod;               // the publishing period for all device items
        private bool usePublishPeriod;                // indicates that publishing period is used


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevMqttPublisherLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = new MqttPublisherDeviceConfig();
            tagByCnlNum = new Dictionary<int, DeviceTag>();

            fatalError = false;
            mqttClientChannel = null;
            lineData = null;
            publishCnlNums = null;
            cnlListID = 0;
            curDataTimestamp = DateTime.MinValue;
            publishPeriod = TimeSpan.Zero;
            usePublishPeriod = false;

            ConnectionRequired = false;
        }


        /// <summary>
        /// Initializes publisher data common to the communication line.
        /// </summary>
        private void InitLineData()
        {
            if (LineContext.SharedData.TryGetValue(nameof(MqttPublisherLineData), out object obj) &&
                obj is MqttPublisherLineData data)
            {
                lineData = data;
            }
            else
            {
                lineData = new MqttPublisherLineData
                {
                    ScadaClient = new ScadaClient(CommContext.AppConfig.ConnectionOptions),
                    CommandQueue = new Queue<TeleCommand>()
                };

                LineContext.SharedData[nameof(MqttPublisherLineData)] = lineData;
            }
        }

        /// <summary>
        /// Checks if is it time to make a request to Server.
        /// </summary>
        private bool RequestToServerNeeded(out bool allItems)
        {
            if (usePublishPeriod && DateTime.UtcNow - curDataTimestamp >= publishPeriod)
            {
                allItems = true;
                return true;
            }

            allItems = false;
            return config.DeviceOptions.PublishOnChange;
        }

        /// <summary>
        /// Requests current data from Server and sets device data.
        /// </summary>
        private void RequestCurrentData()
        {
            // request data
            Log.WriteLine(Locale.IsRussian ?
                "Запрос текущих данных" :
                "Reques current data");

            CnlData[] cnlDataArr = cnlListID > 0
                ? lineData.ScadaClient.GetCurrentData(ref cnlListID)
                : lineData.ScadaClient.GetCurrentData(publishCnlNums, true, out cnlListID);

            Log.WriteLine(CommPhrases.ResponseOK);
            curDataTimestamp = LastSessionTime;

            // set device data
            for (int i = 0, len = publishCnlNums.Length; i < len; i++)
            {
                if (tagByCnlNum.TryGetValue(publishCnlNums[i], out DeviceTag deviceTag))
                {
                    CnlData cnlData = cnlDataArr[i];
                    DeviceData.Set(deviceTag.Index, cnlData.Val, cnlData.Stat);
                }
            }
        }

        /// <summary>
        /// Publishes current data to an MQTT broker.
        /// </summary>
        private bool PublishCurrentData(bool allItems)
        {
            DeviceSlice deviceSlice = allItems
                ? DeviceData.GetCurrentData()
                : DeviceData.GetModifiedData();

            if (deviceSlice.IsEmpty)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Отсутствуют данные для публикации" :
                    "No data to publish");
                return true;
            }
            else
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Публикация данных" :
                    "Publish data");
                bool publishOK = true;

                for (int i = 0, len = deviceSlice.DeviceTags.Length; i < len; i++)
                {
                    if (deviceSlice.DeviceTags[i].Aux is ItemConfig itemConfig)
                    {
                        MqttApplicationMessage message = CreateMessage(itemConfig, deviceSlice.CnlData[i],
                            out string payloadStr);
                        Log.WriteLine("{0} {1} = {2}", CommPhrases.SendNotation, itemConfig.Topic, payloadStr);
                        MqttClientPublishResult result = mqttClientChannel.Publish(message);

                        if (result.ReasonCode != MqttClientPublishReasonCode.Success)
                        {
                            Log.WriteLine(CommPhrases.ErrorPrefix + result.ReasonCode);
                            publishOK = false;
                        }
                    }
                }

                if (publishOK)
                {
                    Log.WriteLine(CommPhrases.ResponseOK);
                    return true;
                }
                else
                {
                    Log.WriteLine(CommPhrases.ResponseError);
                    return false;
                }
            }
        }

        /// <summary>
        /// Dequeues and sends commands to Server.
        /// </summary>
        private bool SendCommands()
        {
            // define user ID
            if (lineData.ScadaClient.UserID == 0)
            {
                lineData.ScadaClient.GetStatus(out _, out bool userIsLoggedIn);

                if (!userIsLoggedIn)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Отправка команд невозможна, потому что пользователь не вошел в систему" :
                        "Unable to send commands because user is not logged in");
                    return false;
                }
            }

            // send commands
            bool sendOK = true;

            lock (lineData.CommandQueue)
            {
                while (lineData.CommandQueue.TryDequeue(out TeleCommand cmd))
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Отправка команды на канал {0}" :
                        "Send command to channel {0}", cmd.CnlNum);

                    cmd.UserID = lineData.ScadaClient.UserID;
                    lineData.ScadaClient.SendCommand(cmd, out CommandResult result);

                    if (result.IsSuccessful)
                    {
                        Log.WriteLine(CommPhrases.ResponseOK);
                    }
                    else
                    {
                        Log.WriteLine(result.ErrorMessage);
                        sendOK = false;
                    }
                }
            }

            return sendOK;
        }
        
        /// <summary>
        /// Subscribes to the device tag.
        /// </summary>
        private void Subscribe(ItemConfig itemConfig, DeviceTag deviceTag)
        {
            mqttClientChannel.Subscribe(new SubscriptionRecord
            {
                Subscriber = this,
                TopicFilter = new MqttTopicFilter
                {
                    Topic = config.DeviceOptions.RootTopic + itemConfig.Topic,
                    QualityOfServiceLevel = (MqttQualityOfServiceLevel)itemConfig.QosLevel
                },
                AuxData = deviceTag
            });
        }

        /// <summary>
        /// Creates a message to publish channel data.
        /// </summary>
        private MqttApplicationMessage CreateMessage(ItemConfig itemConfig, CnlData cnlData, out string payloadStr)
        {
            MqttApplicationMessage message = new()
            {
                Topic = config.DeviceOptions.RootTopic + itemConfig.Topic,
                QualityOfServiceLevel = (MqttQualityOfServiceLevel)itemConfig.QosLevel,
                Retain = itemConfig.Retain
            };

            if (string.IsNullOrEmpty(config.DeviceOptions.PublishFormat))
            {
                payloadStr = cnlData.IsUndefined
                    ? config.DeviceOptions.UndefinedValue
                    : cnlData.Val.ToString(NumberFormatInfo.InvariantInfo);
            }
            else
            {
                string valStr = cnlData.Val.ToString(NumberFormatInfo.InvariantInfo);
                payloadStr = MqttUtils.FormatPayload(config.DeviceOptions.PublishFormat, valStr, cnlData.Stat);
            }

            message.Payload = Encoding.UTF8.GetBytes(payloadStr);
            return message;
        }

        /// <summary>
        /// Handles the received message.
        /// </summary>
        void ISubscriber.HandleReceivedMessage(ReceivedMessage message)
        {
            if (!fatalError &&
                message.AuxData is DeviceTag deviceTag &&
                deviceTag.Aux is ItemConfig itemConfig)
            {
                // create command
                TeleCommand cmd = new()
                {
                    CnlNum = itemConfig.CnlNum
                };

                if (ScadaUtils.TryParseDouble(message.Payload, out double cmdVal))
                    cmd.CmdVal = cmdVal;
                else
                    cmd.CmdData = message.PayloadData;

                // enqueue command
                lock (lineData.CommandQueue)
                {
                    lineData.CommandQueue.Enqueue(cmd);
                }
            }
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            if (!config.Load(Storage, MqttPublisherDeviceConfig.GetFileName(DeviceNum), out string errMsg))
            {
                fatalError = true;
                Log.WriteLine(CommPhrases.DeviceMessage, Title, errMsg);
            }

            publishPeriod = TimeSpan.FromSeconds(config.DeviceOptions.PublishPeriod);
            usePublishPeriod = publishPeriod > TimeSpan.Zero;

            if (LineContext.Channel is IMqttClientChannel channel)
            {
                mqttClientChannel = channel;
            }
            else
            {
                fatalError = true;
                Log.WriteLine(CommPhrases.DeviceMessage, Title, Locale.IsRussian ?
                    "Требуется канал связи MQTT-клиент" :
                    "MQTT client communication channel required");
            }

            InitLineData();

            if (fatalError)
                DeviceStatus = DeviceStatus.Error;
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            TagGroup tagGroup = new();
            BaseTable<Cnl> cnlTable = CommContext.BaseDataSet?.CnlTable;

            int itemCnt = config.Items.Count;
            List<int> cnlNumList = new(itemCnt);
            HashSet<int> cnlNumSet = new(itemCnt);

            foreach (ItemConfig itemConfig in config.Items)
            {
                int cnlNum = itemConfig.CnlNum;
                DeviceTag deviceTag;

                if (cnlNum > 0 && cnlNumSet.Add(cnlNum))
                {
                    if (cnlTable?.GetItem(cnlNum) is Cnl cnl)
                    {
                        deviceTag = tagGroup.AddTag("", cnl.Name);
                        deviceTag.Cnl = cnl;
                    }
                    else
                    {
                        deviceTag = tagGroup.AddTag("", Locale.IsRussian ?
                            "Канал " + cnlNum :
                            "Channel " + cnlNum);
                    }

                    deviceTag.Aux = itemConfig;
                    tagByCnlNum.Add(cnlNum, deviceTag);

                    if (itemConfig.Publish)
                        cnlNumList.Add(cnlNum);

                    if (itemConfig.Subscribe)
                        Subscribe(itemConfig, deviceTag);
                }
            }

            DeviceTags.AddGroup(tagGroup);
            DeviceTags.FlattenGroups = true;
            DeviceTags.UseStatusTag = false;
            publishCnlNums = cnlNumList.ToArray();
        }

        /// <summary>
        /// Binds the device tags to the configuration database.
        /// </summary>
        public override void BindDeviceTags(BaseDataSet baseDataSet)
        {
            // do nothing
        }

        /// <summary>
        /// Gets a slice of the current data to transfer.
        /// </summary>
        public override DeviceSlice GetCurrentData(bool allData)
        {
            return DeviceSlice.Empty;
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            // check fatal error
            if (fatalError)
            {
                SleepPollingDelay();
                return;
            }

            // check if data should be sent or received
            bool publishNeeded = RequestToServerNeeded(out bool allItems) && publishCnlNums.Length > 0;
            bool commandsExist = lineData.CommandQueue.Count > 0;

            if (!publishNeeded && !commandsExist)
            {
                SleepPollingDelay();
                return;
            }

            // publish data and send commands
            base.Session();

            try
            {
                if (publishNeeded)
                {
                    RequestCurrentData();

                    if (!PublishCurrentData(allItems))
                        LastRequestOK = false;
                }

                if (commandsExist && !SendCommands())
                    LastRequestOK = false;
            }
            catch (Exception ex)
            {
                Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                LastRequestOK = false;
            }

            SleepPollingDelay();
            FinishSession();
        }
    }
}
