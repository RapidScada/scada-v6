// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Publishing;
using MQTTnet.Protocol;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Comm.Lang;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Scada.Comm.Drivers.DrvDsMqtt.Logic
{
    /// <summary>
    /// Implements the data source logic.
    /// <para>Реализует логику источника данных.</para>
    /// </summary>
    internal class MqttDSL : DataSourceLogic
    {
        /// <summary>
        /// Represents a dictionary that stores device topics accessed by tag code.
        /// </summary>
        private class DeviceTopics : Dictionary<string, string>
        {
        }

        /// <summary>
        /// Represents a command received from an MQTT broker.
        /// </summary>
        private class ReceivedCommand
        {
            public int DeviceNum { get; set; }
            public string CmdCode { get; set; }
            public double CmdVal { get; set; }
            public string CmdData { get; set; }
        }

        /// <summary>
        /// The minimum queue size.
        /// </summary>
        private const int MinQueueSize = 100;
        /// <summary>
        /// The number of queue items to publish in one iteration.
        /// </summary>
        private const int BundleSize = 10;
        /// <summary>
        /// The topic to receive commands.
        /// </summary>
        private const string CommandTopic = "command";

        private readonly MqttDSO dsOptions;                           // the data source options
        private readonly ILog dsLog;                                  // the data source log
        private readonly MqttClientHelper mqttClientHelper;           // encapsulates an MQTT client
        private readonly string commandTopic;                         // the full command topic name
        private readonly TimeSpan dataLifetime;                       // the data lifetime in the queue
        private readonly HashSet<int> deviceFilter;                   // the device IDs to filter data
        private readonly int maxQueueSize;                            // the maximum queue size
        private readonly Queue<QueueItem<DeviceSlice>> curDataQueue;  // the current data queue
        private readonly Dictionary<int, DeviceTopics> topicByDevice; // the topics accessed by device number

        private Thread thread;            // the thread for communication with the server
        private volatile bool terminated; // necessary to stop the thread
        private int curDataSkipped;       // the number of skipped slices of current data 


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttDSL(ICommContext commContext, DataSourceConfig dataSourceConfig)
            : base(commContext, dataSourceConfig)
        {
            dsOptions = new MqttDSO(dataSourceConfig.CustomOptions);
            dsLog = CreateLog(DriverUtils.DriverCode);
            mqttClientHelper = new MqttClientHelper(dsOptions.ConnectionOptions, dsLog);
            commandTopic = dsOptions.PublishOptions.RootTopic + CommandTopic;
            dataLifetime = TimeSpan.FromSeconds(dsOptions.PublishOptions.DataLifetime);
            deviceFilter = dsOptions.PublishOptions.DeviceFilter.Count > 0 ? 
                new HashSet<int>(dsOptions.PublishOptions.DeviceFilter) : null;
            maxQueueSize = Math.Max(dsOptions.PublishOptions.MaxQueueSize, MinQueueSize);
            curDataQueue = new Queue<QueueItem<DeviceSlice>>(maxQueueSize);
            topicByDevice = new Dictionary<int, DeviceTopics>();

            thread = null;
            terminated = false;
            curDataSkipped = 0;
        }


        /// <summary>
        /// Fills the device topic dictionary.
        /// </summary>
        private void FillDeviceTopics()
        {
            // topic example:
            // Communicator/line001/device001/Sin
            foreach (ILineContext lineContext in CommContext.GetCommLines())
            {
                string lineTopic = dsOptions.PublishOptions.RootTopic + 
                    CommUtils.GetLineLogFileName(lineContext.CommLineNum, "") + "/";

                IEnumerable<DeviceLogic> devices = deviceFilter == null
                    ? lineContext.SelectDevices()
                    : lineContext.SelectDevices(d => deviceFilter.Contains(d.DeviceNum));

                foreach (DeviceLogic deviceLogic in devices)
                {
                    string deviceTopic = lineTopic + CommUtils.GetDeviceLogFileName(deviceLogic.DeviceNum, "") + "/";
                    DeviceTopics deviceTopics = new();

                    foreach (DeviceTag deviceTag in deviceLogic.DeviceTags)
                    {
                        if (!string.IsNullOrEmpty(deviceTag.Code))
                            deviceTopics[deviceTag.Code] = deviceTopic + deviceTag.Code;
                    }

                    if (deviceTopics.Count > 0)
                        topicByDevice[deviceLogic.DeviceNum] = deviceTopics;
                }
            }
        }

        /// <summary>
        /// Reconnects the MQTT client to the MQTT broker if it is disconnected.
        /// </summary>
        private bool ReconnectIfRequired()
        {
            if (mqttClientHelper.IsConnected)
            {
                return true;
            }
            else if (mqttClientHelper.Connect())
            {
                SubscribeToCommands();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Subscribes to the command topic.
        /// </summary>
        private void SubscribeToCommands()
        {
            try
            {
                dsLog.WriteLine();
                dsLog.WriteAction(Locale.IsRussian ?
                    "Подписка на топик команд" :
                    "Subscribe to command topic");

                mqttClientHelper.Subscribe(new MqttTopicFilter
                {
                    Topic = commandTopic,
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce
                });
            }
            catch (Exception ex)
            {
                dsLog.WriteError(Locale.IsRussian ?
                    "Ошибка при подписке на топик команд: {0}" :
                    "Error subscribing to command topic: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Publishes current data to an MQTT broker.
        /// </summary>
        private void PublishCurrentData()
        {
            for (int i = 0; i < BundleSize; i++)
            {
                // get a slice from the queue without removing it
                QueueItem<DeviceSlice> queueItem;
                DeviceSlice deviceSlice;

                lock (curDataQueue)
                {
                    if (curDataQueue.TryPeek(out queueItem))
                        deviceSlice = queueItem.Value;
                    else
                        break;
                }

                // publish the slice
                if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                {
                    dsLog.WriteError(Locale.IsRussian ?
                        "Устаревшие текущие данные удалены из очереди" :
                        "Outdated current data removed from the queue");
                    curDataSkipped++;
                }
                else if (!PublishDeviceSlice(deviceSlice))
                {
                    break; // the slice is not removed from the queue
                }

                // remove the slice from the queue
                lock (curDataQueue)
                {
                    if (curDataQueue.Count > 0 && curDataQueue.Peek() == queueItem)
                        curDataQueue.Dequeue();
                }
            }
        }

        /// <summary>
        /// Publishes the device slice.
        /// </summary>
        private bool PublishDeviceSlice(DeviceSlice deviceSlice)
        {
            try
            {
                if (topicByDevice.TryGetValue(deviceSlice.DeviceNum, out DeviceTopics deviceTopics))
                {
                    int dataIndex = 0;

                    foreach (DeviceTag deviceTag in deviceSlice.DeviceTags)
                    {
                        if (!string.IsNullOrEmpty(deviceTag.Code) && 
                            deviceTopics.TryGetValue(deviceTag.Code, out string topic))
                        {
                            string payloadStr = BuildPayload(deviceTag, deviceSlice.CnlData, dataIndex);
                            MqttApplicationMessage message = new()
                            {
                                Topic = topic,
                                Payload = Encoding.UTF8.GetBytes(payloadStr),
                                QualityOfServiceLevel = (MqttQualityOfServiceLevel)dsOptions.PublishOptions.QosLevel,
                                Retain = dsOptions.PublishOptions.Retain
                            };

                            if (dsOptions.PublishOptions.DetailedLog)
                                dsLog.WriteAction("{0} {1} = {2}", CommPhrases.SendNotation, topic, payloadStr);

                            MqttClientPublishResult result = mqttClientHelper.Publish(message);

                            if (result.ReasonCode != MqttClientPublishReasonCode.Success)
                            {
                                dsLog.WriteError(CommPhrases.ErrorPrefix + result.ReasonCode);
                                return false;
                            }
                        }

                        dataIndex += deviceTag.DataLength;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                dsLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при публикации данных" :
                    "Error publishing data");
                return false;
            }
        }

        /// <summary>
        /// Builds a payload based on the device tag data to publish.
        /// </summary>
        public string BuildPayload(DeviceTag deviceTag, CnlData[] cnlData, int dataIndex)
        {
            int dataLength = deviceTag.DataLength;
            int stat = CnlDataConverter.GetStatus(cnlData, dataIndex, dataLength);
            bool formatIsEmpty = string.IsNullOrEmpty(dsOptions.PublishOptions.PublishFormat);

            if (formatIsEmpty && stat <= CnlStatusID.Undefined)
                return dsOptions.PublishOptions.UndefinedValue;

            string valStr = "";

            switch (deviceTag.DataType)
            {
                case TagDataType.Double:
                    valStr = deviceTag.IsNumericArray
                        ? ArrayToString(CnlDataConverter.GetDoubleArray(cnlData, dataIndex, dataLength))
                        : CnlDataConverter.GetDouble(cnlData, dataIndex).ToString(NumberFormatInfo.InvariantInfo);
                    break;

                case TagDataType.Int64:
                    valStr = deviceTag.IsNumericArray
                        ? ArrayToString(CnlDataConverter.GetInt64Array(cnlData, dataIndex, dataLength))
                        : CnlDataConverter.GetInt64(cnlData, dataIndex).ToString(NumberFormatInfo.InvariantInfo);
                    break;

                case TagDataType.ASCII:
                    valStr = CnlDataConverter.GetAscii(cnlData, dataIndex, dataLength);
                    break;

                case TagDataType.Unicode:
                    valStr = CnlDataConverter.GetUnicode(cnlData, dataIndex, dataLength);
                    break;
            }

            return formatIsEmpty
                ? valStr
                : MqttUtils.FormatPayload(dsOptions.PublishOptions.PublishFormat, valStr, stat);
        }

        /// <summary>
        /// Converts the array of doubles to a string representation.
        /// </summary>
        private static string ArrayToString(double[] arr)
        {
            return "[" + string.Join(", ", arr.Select(x => x.ToString(NumberFormatInfo.InvariantInfo))) + "]";
        }

        /// <summary>
        /// Converts the array of integers to a string representation.
        /// </summary>
        private static string ArrayToString(long[] arr)
        {
            return "[" + string.Join(", ", arr) + "]";
        }

        /// <summary>
        /// Checks if the device filter allows the specified device number.
        /// </summary>
        private bool CheckDeviceFilter(int deviceNum)
        {
            return deviceFilter == null || deviceFilter.Contains(deviceNum);
        }

        /// <summary>
        /// Executes an MQTT communication loop.
        /// </summary>
        private void Execute()
        {
            while (!terminated)
            {
                if (ReconnectIfRequired())
                    PublishCurrentData();

                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }

        /// <summary>
        /// Handles a message received event.
        /// </summary>
        private void MqttClient_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                string topic = e.ApplicationMessage.Topic;
                string payload = e.ApplicationMessage.ConvertPayloadToString();

                dsLog.WriteAction("{0} {1} = {2}", CommPhrases.ReceiveNotation,
                    topic, payload.GetPreview(MqttUtils.MessagePreviewLength));

                // parse and pass command
                if (topic == commandTopic)
                {
                    ReceivedCommand command = JsonSerializer.Deserialize<ReceivedCommand>(payload);
                    CommContext.SendCommand(new TeleCommand
                    {
                        CommandID = ScadaUtils.GenerateUniqueID(),
                        CreationTime = DateTime.UtcNow,
                        DeviceNum = command.DeviceNum,
                        CmdCode = command.CmdCode,
                        CmdVal = command.CmdVal,
                        CmdData = ScadaUtils.HexToBytes(command.CmdData, false, true)
                    }, DriverUtils.DriverCode);
                }
            }
            catch (JsonException)
            {
                dsLog.WriteError(Locale.IsRussian ?
                    "Ошибка при получении команды из JSON." :
                    "Error parsing command from JSON.");
            }
            catch (Exception ex)
            {
                dsLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при обработке полученного сообщения. Топик: {0}" :
                    "Error handling the received message. Topic: {0}", e?.ApplicationMessage?.Topic);
            }
        }


        /// <summary>
        /// Makes the data source ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            dsLog.WriteBreak();
            mqttClientHelper.Client.UseApplicationMessageReceivedHandler(MqttClient_ApplicationMessageReceived);
        }

        /// <summary>
        /// Starts the data source.
        /// </summary>
        public override void Start()
        {
            dsLog.WriteAction(Locale.IsRussian ?
                "Источник данных MQTT запущен" :
                "MQTT data source started");

            if (!dsOptions.PublishOptions.DetailedLog)
            {
                dsLog.WriteInfo(Locale.IsRussian ?
                    "Детальный журнал отключен" :
                    "Detailed log is disabled");
            }

            // get information about devices
            FillDeviceTopics();

            // start thread
            terminated = false;
            thread = new Thread(Execute);
            thread.Start();
        }

        /// <summary>
        /// Closes the data source.
        /// </summary>
        public override void Close()
        {
            // stop thread
            if (thread != null)
            {
                terminated = true;
                thread.Join();
                thread = null;
            }

            // disconnect
            mqttClientHelper.Close();

            dsLog.WriteLine();
            dsLog.WriteAction(Locale.IsRussian ?
                "Источник данных MQTT остановлен" :
                "MQTT data source stopped");
            dsLog.WriteBreak();
        }

        /// <summary>
        /// Writes the slice of the current data.
        /// </summary>
        public override void WriteCurrentData(DeviceSlice deviceSlice)
        {
            if (CheckDeviceFilter(deviceSlice.DeviceNum))
            {
                lock (curDataQueue)
                {
                    // remove data if capacity is not enough
                    while (curDataQueue.Count >= maxQueueSize)
                    {
                        curDataQueue.Dequeue();
                        curDataSkipped++;
                    }

                    // append current data
                    curDataQueue.Enqueue(new QueueItem<DeviceSlice>(deviceSlice.Timestamp, deviceSlice));
                }
            }
        }
        
        /// <summary>
        /// Appends information about the data source to the string builder.
        /// </summary>
        public override void AppendInfo(StringBuilder sb)
        {
            sb
                .AppendLine()
                .AppendLine(DataSourceConfig.Name)
                .Append('-', DataSourceConfig.Name.Length).AppendLine();

            if (Locale.IsRussian)
            {
                sb
                    .Append("Соединение             : ").AppendLine(mqttClientHelper.GetConnectionState(true))
                    .Append("Очередь текущих данных : ")
                    .Append(curDataQueue.Count).Append(" из ").Append(maxQueueSize)
                    .Append(", пропущено ").Append(curDataSkipped).AppendLine();
            }
            else
            {
                sb
                    .Append("Connection         : ").AppendLine(mqttClientHelper.GetConnectionState(false))
                    .Append("Current data queue : ")
                    .Append(curDataQueue.Count).Append(" of ").Append(maxQueueSize)
                    .Append(", skipped ").Append(curDataSkipped).AppendLine();
            }
        }
    }
}
