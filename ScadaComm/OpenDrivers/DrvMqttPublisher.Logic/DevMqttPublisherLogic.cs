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
using System.Text;

namespace Scada.Comm.Drivers.DrvMqttPublisher.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevMqttPublisherLogic : DeviceLogic, ISubscriber
    {
        private readonly MqttPublisherDeviceConfig config;       // the device configuration
        private readonly ScadaClient scadaClient;                // interacts with the Server service
        private readonly Dictionary<int, DeviceTag> tagByCnlNum; // the device tags accessed by channel number

        private IMqttClientChannel mqttClientChannel; // the communication channel reference
        private int[] publishCnlNums;                 // the numbers of the published channels
        private string[] publishTopics;               // the published topics according to the channels
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
            scadaClient = new ScadaClient(commContext.AppConfig.ConnectionOptions);
            tagByCnlNum = new Dictionary<int, DeviceTag>();

            mqttClientChannel = null;
            publishCnlNums = null;
            publishTopics = null;
            cnlListID = 0;
            curDataTimestamp = DateTime.MinValue;
            publishPeriod = TimeSpan.Zero;
            usePublishPeriod = false;

            ConnectionRequired = false;
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
        /// Suspends for the delay specified in the polling options.
        /// </summary>
        private void SleepPollingDelay()
        {
            if (PollingOptions.Delay > 0)
                Thread.Sleep(PollingOptions.Delay);
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
                ? scadaClient.GetCurrentData(ref cnlListID)
                : scadaClient.GetCurrentData(publishCnlNums, true, out cnlListID);

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
                            out string valStr);
                        Log.WriteLine("{0} {1} = {2}", CommPhrases.SendNotation, itemConfig.Topic, valStr);
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
        /// 
        /// </summary>
        private MqttApplicationMessage CreateMessage(ItemConfig itemConfig, CnlData cnlData, out string valStr)
        {
            MqttApplicationMessage message = new()
            {
                Topic = config.DeviceOptions.RootTopic + itemConfig.Topic,
                QualityOfServiceLevel = (MqttQualityOfServiceLevel)itemConfig.QosLevel,
                Retain = itemConfig.Retain
            };

            valStr = cnlData.Val.ToString();
            message.Payload = Encoding.UTF8.GetBytes(valStr);
            return message;
        }

        /// <summary>
        /// Handles the received message.
        /// </summary>
        void ISubscriber.HandleReceivedMessage(ReceivedMessage message)
        {
            /*try
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
            }*/
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            if (!config.Load(Storage, MqttPublisherDeviceConfig.GetFileName(DeviceNum), out string errMsg))
                Log.WriteLine(CommPhrases.DeviceMessage, Title, errMsg);

            publishPeriod = TimeSpan.FromSeconds(config.DeviceOptions.PublishPeriod);
            usePublishPeriod = publishPeriod > TimeSpan.Zero;

            if (LineContext.Channel is IMqttClientChannel channel)
            {
                mqttClientChannel = channel;
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
            BaseTable<Cnl> cnlTable = CommContext.BaseDataSet?.CnlTable;

            int itemCnt = config.Items.Count;
            List<int> cnlNumList = new(itemCnt);
            HashSet<int> cnlNumSet = new(itemCnt);
            List<string> topicList = new(itemCnt);

            foreach (ItemConfig itemConfig in config.Items)
            {
                int cnlNum = itemConfig.CnlNum;
                DeviceTag deviceTag;

                if (cnlNum > 0 && cnlNumSet.Add(cnlNum))
                {
                    if (itemConfig.Publish)
                    {
                        cnlNumList.Add(cnlNum);
                        topicList.Add(itemConfig.Topic);
                    }

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
                }
            }

            DeviceTags.AddGroup(tagGroup);
            publishCnlNums = cnlNumList.ToArray();
            publishTopics = topicList.ToArray();
        }

        /// <summary>
        /// Binds the device tags to the configuration database.
        /// </summary>
        public override void BindDeviceTags(BaseDataSet baseDataSet)
        {
            // do nothing
        }

        /// <summary>
        /// Initializes the device data.
        /// </summary>
        public override void InitDeviceData()
        {
            base.InitDeviceData();
            DeviceData.DisableCurrentData = true;
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            if (!RequestToServerNeeded(out bool allItems))
            {
                SleepPollingDelay();
                return;
            }

            base.Session();
            LastRequestOK = false;

            if (publishCnlNums == null || publishCnlNums.Length == 0)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0}Отсутствуют публикуемые каналы" :
                    "{0}Published channels missing", CommPhrases.ErrorPrefix);
            }
            else
            {
                try
                {
                    RequestCurrentData();

                    if (PublishCurrentData(allItems))
                        LastRequestOK = true;
                }
                catch (Exception ex)
                {
                    Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                }
            }

            SleepPollingDelay();
            FinishSession();
        }
    }
}
