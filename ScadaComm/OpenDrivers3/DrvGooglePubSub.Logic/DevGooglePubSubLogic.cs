// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Newtonsoft.Json;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvGoogle.Config;
using Scada.Comm.Drivers.DrvGooglePubSub.Config;
using Scada.Comm.Drivers.DrvGooglePubSub.Models;
using Scada.Comm.Lang;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Jint;
using Scada.Comm.Drivers.DrvGooglePubSub.Logic.GoogleAuth;

namespace Scada.Comm.Drivers.DrvGooglePubSub.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// </summary>
    internal class DevGooglePubSubLogic : DeviceLogic
    {
        /// <summary>
        /// The default data lifetime in seconds. Zero means no lifetime is used.
        /// </summary>
        private const int DefautDataLifetime = 0;

        private readonly GooglePubSubDeviceConfig config;               // the device configuration

        private TimeSpan dataLifetime;                // specifies when tag values should be invalidated
        private bool useDataLifetime;                 // indicates that lifetime is used
        private DateTime[] updateTimestamps;          // the update timestamps by device tag
        private IGoogleCloudChannel googleCloudChannel; //
        private readonly object sessionLock;                     // synchronizes sessions and incoming messages
        private Engine jsEngine;                      // executes JavaScript
        private int MaxTimeLimitDay = 5;//最大数据时限
        private ConcurrentDictionary<string, DateTime> DataTimeCache = null;
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevGooglePubSubLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = new GooglePubSubDeviceConfig();

            dataLifetime = TimeSpan.Zero;
            useDataLifetime = false;
            updateTimestamps = null;

            CanSendCommands = true;
            ConnectionRequired = false;

            sessionLock = new object();
            DataTimeCache = new ConcurrentDictionary<string, DateTime>();
            LogUtil.Logger = Log;
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
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            dataLifetime = TimeSpan.FromSeconds(
                LineContext.LineConfig.CustomOptions.GetValueAsInt("DataLifetime", DefautDataLifetime));
            useDataLifetime = dataLifetime > TimeSpan.Zero;

            if (!config.Load(Storage, GooglePubSubDeviceConfig.GetFileName(DeviceNum), out string errMsg))
                Log.WriteLine(CommPhrases.DeviceMessage, Title, errMsg);

            if (LineContext.Channel is IGoogleCloudChannel channel)
            {
                this.googleCloudChannel = channel;
            }
            else
            {
                Log.WriteLine(CommPhrases.DeviceMessage, Title, "Google cloud channel is required");
            }
        }

        /// <summary>
        /// Performs actions when stoping a communication line.
        /// </summary>
        public override void OnCommLineTerminate()
        {
            try
            {
                for (int i = 0; i < SubscriptionRecordList.Count; i++)
                {
                    var subscriptionRecord = SubscriptionRecordList[i];
                    try
                    {
                        subscriptionRecord.SubscriberClient.StopAsync(CancellationToken.None).ConfigureAwait(false);
                        subscriptionRecord.SubscribeTask.Wait();
                        Log.WriteLine($"[UnSubscribe]SubId: {subscriptionRecord.SubscriptionId}");
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLine($"[UnSubscribe][Err]SubId: {subscriptionRecord.SubscriptionId}, err: {ex.Message}");
                    }
                }
            }
            catch (Exception ex2)
            {
                Log.WriteLine($"[OnCommLineTerminate][Err]err: {ex2.Message}");
            }
            base.OnCommLineTerminate();
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            TagGroup tagGroup = new();
            MaxTimeLimitDay = config.DeviceOptions.MaximumTimeLimit;
            foreach (SubscriptionConfig subscriptionConfig in config.Subscriptions)
            {
                if (subscriptionConfig.SubItems.Count > 0)
                {
                    for (int i = 0, cnt = subscriptionConfig.SubItems.Count; i < cnt; i++)
                    {
                        string suffix = "." + subscriptionConfig.SubItems[i];
                        DeviceTag deviceTag = tagGroup.AddTag(
                            CalcTagCode(subscriptionConfig.TagCode, subscriptionConfig.SubItems[i]), 
                            subscriptionConfig.DisplayName + suffix);
                        DeviceTag deviceTagTime = tagGroup.AddTag(
                            DriverUtils.GetPointTime(CalcTagCode(subscriptionConfig.TagCode, subscriptionConfig.SubItems[i])), 
                            subscriptionConfig.DisplayName + suffix + ".time");
                        deviceTagTime.SetDataType(TagDataType.Int64);
                        deviceTagTime.SetFormat(TagFormat.DateTime);
                        if (i == 0)
                            Subscribe(subscriptionConfig).ConfigureAwait(false);
                    }
                }
                else
                {
                    DeviceTag deviceTag = tagGroup.AddTag(subscriptionConfig.TagCode, subscriptionConfig.DisplayName);
                    DeviceTag deviceTagTime = tagGroup.AddTag(DriverUtils.GetPointTime(subscriptionConfig.TagCode), subscriptionConfig.DisplayName + ".time");
                    deviceTagTime.SetDataType(TagDataType.Int64);
                    deviceTagTime.SetFormat(TagFormat.DateTime);
                    Subscribe(subscriptionConfig).ConfigureAwait(false);
                }
            }

            DeviceTags.AddGroup(tagGroup);
            DeviceTags.FlattenGroups = true;
            updateTimestamps = new DateTime[tagGroup.DeviceTags.Count];
        }
        
        private Queue<PubsubMessage> DataQueue = new Queue<PubsubMessage>(65535);
        private static object queueLock = new object();

        private List<SubscriptionRecord> SubscriptionRecordList = new List<SubscriptionRecord>();
        private async Task Subscribe(SubscriptionConfig subscriptionConfig)
        {
            if (SubscriptionRecordList.Any(x => subscriptionConfig.SubscriptionId.Equals(x.SubscriptionId, StringComparison.OrdinalIgnoreCase)))
            {
                Log.WriteLine($"[Subscribe] {subscriptionConfig.SubscriptionId} already subscripte.");
                return;
            }
            var newRecord = new SubscriptionRecord
            {
                ProjectId = config.DeviceOptions.ProjectId,
                SubscriptionId = subscriptionConfig.SubscriptionId
            };
            SubscriberClientBuilder subscriberClientBuilder = CreateSubscriberClient(newRecord);
            newRecord.SubscriberClient = await subscriberClientBuilder.BuildAsync();
            newRecord.SubscribeTask = newRecord.SubscriberClient.StartAsync(ReceivePullMessageHandler);
            SubscriptionRecordList.Add(newRecord);
        }

        /// <summary>
        /// 创建订阅客户端
        /// </summary>
        private SubscriberClientBuilder CreateSubscriberClient(SubscriptionRecord newRecord)
        {
            SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(newRecord.ProjectId, newRecord.SubscriptionId);
            Log.WriteLine($"[Subscribe]Name: {subscriptionName}");
            ICredential credential = null;
            var googleCloudOptions = this.googleCloudChannel.GoogleCloudOptions;
            switch (googleCloudOptions.CredentialType)
            {
                case DrvGoogle.Common.GoogleCredentialType.CloudScadaAccessToken:
                    credential = new CloudScadaCredential(this.googleCloudChannel);
                    break;
                case DrvGoogle.Common.GoogleCredentialType.ApplicationDefaultCredential:
                    if (googleCloudOptions.UseAdcFile)
                        credential = GoogleCredential.FromFile(googleCloudOptions.AdcFilePath);
                    else
                        credential = GoogleCredential.GetApplicationDefault();
                    break;
            }
            SubscriberClientBuilder subscriberClientBuilder = new SubscriberClientBuilder
            {
                SubscriptionName = subscriptionName,
                Credential = credential,
            };

            return subscriberClientBuilder;
        }

        /// <summary>
        /// 接收推送数据方法
        /// </summary>
        private Task<SubscriberClient.Reply> ReceivePullMessageHandler(PubsubMessage message, CancellationToken cancel)
        {
            string text = System.Text.Encoding.UTF8.GetString(message.Data.ToArray());
            //Log.WriteLine($"Message {message.MessageId}: {text}");
            if (base.IsTerminated) return Task.FromResult(SubscriberClient.Reply.Nack);
            try
            {
                string dataBody = System.Text.Encoding.UTF8.GetString(message.Data.ToArray());
                var dataInfo = JsonConvert.DeserializeObject<SubDataContent>(dataBody);
                if (DeviceTags.ContainsTag(dataInfo.timeserie_name))
                {
                    Log.WriteLine($"Message {message.MessageId}: {text}");
                    //未超过最长设定时限，默认5天
                    if (DateTime.UtcNow.Subtract(dataInfo.timestamp).Days <= MaxTimeLimitDay)
                        HandlerReceivedMessage(message, dataInfo);
                    return Task.FromResult(SubscriberClient.Reply.Ack);
                }
            }
            catch (Exception ex) 
            {
                Log.WriteError($"HandlerReceivedMessage error: {ex.Message}.");
            }
            // Interlocked.Increment(ref messageCount);
            return Task.FromResult(SubscriberClient.Reply.Ack);
        }

        private void HandlerReceivedMessage(PubsubMessage message, SubDataContent dataInfo)
        {
            try
            {
                Monitor.Enter(sessionLock);
                Log.WriteLine();
                //Log.WriteAction("{0} {1} = {2}", CommPhrases.ReceiveNotation, message.Topic, message.Payload.GetPreview(MqttUtils.MessagePreviewLength));

                //实时数据 or 历史数据
                var timePoint = DriverUtils.GetPointTime(dataInfo.timeserie_name);
                var utcNow = DateTime.UtcNow;
                //create a historical data slice
                //判断测点记录时间是否存在并且是最新的
                if (!(DataTimeCache.TryGetValue(dataInfo.timeserie_name, out var dataTime) && dataTime > dataInfo.timestamp))
                {
                    DeviceData.Set(dataInfo.timeserie_name, dataInfo.value);
                    DeviceData.SetDateTime(timePoint, utcNow, 1);
                    DataTimeCache.AddOrUpdate(dataInfo.timeserie_name, dataInfo.timestamp, (k, v) => dataInfo.timestamp);
                }
                //覆盖历史数据
                DeviceSlice deviceSlice = new DeviceSlice(dataInfo.timestamp, 2, 2);
                deviceSlice.DeviceTags[0] = DeviceTags[dataInfo.timeserie_name];
                deviceSlice.DeviceTags[1] = DeviceTags[timePoint];
                deviceSlice.CnlData[0] = new CnlData(dataInfo.value);
                deviceSlice.CnlData[1] = new CnlData(utcNow.ToUniversalTime().Ticks);
                deviceSlice.Descr = $"{dataInfo.timeserie_name}: {dataInfo.value}";
                DeviceData.EnqueueSlice(deviceSlice);
                Log.WriteLine($"[Deal]Message {message.MessageId}, timeserie_name: {dataInfo.timeserie_name}, value: {dataInfo.value}");

                LastRequestOK = true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, "Error handling the received message.");
                LastRequestOK = false;
            }
            finally
            {
                Monitor.Exit(sessionLock);
                this.FinishSession();
            }
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            if (useDataLifetime)
                InvalidateOutdatedData();

            if (googleCloudChannel == null)
                DeviceStatus = DeviceStatus.Error;

            SleepPollingDelay();
        }


        private string CalcTagCode(string tagCode, string suffix)
        {
            if (string.IsNullOrEmpty(tagCode)) return suffix;
            return tagCode + "." + suffix;
        }
    }
}
