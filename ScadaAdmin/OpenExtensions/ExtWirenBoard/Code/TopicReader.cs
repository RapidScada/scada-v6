// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Subscribing;
using Scada.Admin.Extensions.ExtWirenBoard.Code.Models;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Admin.Extensions.ExtWirenBoard.Code
{
    /// <summary>
    /// Represents a topic reader.
    /// <para>Представляет считыватель топиков.</para>
    /// </summary>
    internal class TopicReader
    {
        /// <summary>
        /// The topic for receiving information about Wiren Board devices and controls.
        /// </summary>
        private const string MainTopic = "/devices/#";
        /// <summary>
        /// Specifies how long to wait for new data after reading the latest data.
        /// </summary>
        private readonly TimeSpan ReadTimeout = TimeSpan.FromSeconds(3);

        private readonly MqttConnectionOptions connOptions; // the connection options
        private readonly RichTextBoxHelper logHelper;       // provides access to log
        private readonly object mqttLock;                   // synchronizes reading

        private CancellationTokenSource tokenSource;        // provides a cancellation mechanism
        private DateTime messageDT;                         // the timestamp of the last received message
        private bool stopReading;                           // indicates that a read operation should stop


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TopicReader(MqttConnectionOptions mqttConnectionOptions, RichTextBoxHelper logHelper)
        {
            connOptions = mqttConnectionOptions ?? throw new ArgumentNullException(nameof(mqttConnectionOptions));
            this.logHelper = logHelper ?? throw new ArgumentNullException(nameof(logHelper));
            mqttLock = new object();

            tokenSource = null;
            messageDT = DateTime.MinValue;
            stopReading = false;

            ReadResult = false;
            WirenBoardModel = new WirenBoardModel();
        }


        /// <summary>
        /// Gets the result of a read operation.
        /// </summary>
        public bool ReadResult { get; private set; }

        /// <summary>
        /// Gets the information about the Wiren Board.
        /// </summary>
        public WirenBoardModel WirenBoardModel { get; }


        /// <summary>
        /// Reads the topics from Wiren Board.
        /// </summary>
        private void ReadTopics()
        {
            IMqttClient mqttClient = null;

            try
            {
                MqttFactory mqttFactory = new();
                mqttClient = mqttFactory.CreateMqttClient();
                mqttClient.UseApplicationMessageReceivedHandler(MqttClient_ApplicationMessageReceived);

                logHelper.WriteMessage(string.Format(Locale.IsRussian ?
                    "Соединение с {0}:{1}" :
                    "Connect to {0}:{1}",
                    connOptions.Server, connOptions.Port));

                MqttClientConnectResultCode resultCode = 
                    mqttClient.ConnectAsync(connOptions.ToMqttClientOptions(), tokenSource.Token)
                    .Result.ResultCode;

                if (resultCode == MqttClientConnectResultCode.Success)
                {
                    logHelper.WriteMessage(Locale.IsRussian ?
                        "Соединение установлено успешно" :
                        "Connected successfully");

                    logHelper.WriteMessage(string.Format(Locale.IsRussian ?
                        "Подписка на {0}" :
                        "Subscribe to {0}", MainTopic));
                    MqttClientSubscribeOptions subscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                        .WithTopicFilter(f => f.WithTopic(MainTopic)).Build();
                    mqttClient.SubscribeAsync(subscribeOptions, tokenSource.Token).Wait();

                    while (!stopReading && !tokenSource.IsCancellationRequested)
                    {
                        Thread.Sleep(ScadaUtils.ThreadDelay);

                        lock (mqttLock)
                        {
                            if (DateTime.UtcNow - messageDT > ReadTimeout)
                            {
                                stopReading = true;
                                logHelper.WriteMessage(Locale.IsRussian ?
                                    "Таймаут чтения истёк" :
                                    "Read timeout expired");
                            }
                        }
                    }

                    if (WirenBoardModel.Devices.Count > 0)
                    {
                        logHelper.WriteSuccess(Locale.IsRussian ?
                            "Чтение топиков завершено успешно" :
                            "Reading topics completed successfully");
                        ReadResult = true;
                    }
                    else
                    {
                        logHelper.WriteError(Locale.IsRussian ?
                            "Подключенные устройства не найдены" :
                            "Connected devices not found");
                    }
                }
                else
                {
                    logHelper.WriteError(string.Format(Locale.IsRussian ?
                        "Не удалось установить соединение: {0}" :
                        "Unable to connect: {0}", resultCode));
                }
            }
            catch (Exception ex)
            {
                logHelper.WriteError(ex.Message);
            }
            finally
            {
                Disconnect(mqttClient);
                OnCompleted();
            }
        }

        /// <summary>
        /// Disconnects and disposes the MQTT client.
        /// </summary>
        private void Disconnect(IMqttClient mqttClient)
        {
            if (mqttClient == null || !mqttClient.IsConnected)
                return;

            try
            {
                logHelper.WriteMessage(string.Format(Locale.IsRussian ?
                    "Отключение от {0}:{1}" :
                    "Disconnect from {0}:{1}",
                    connOptions.Server, connOptions.Port));
                mqttClient.DisconnectAsync().Wait();
            }
            catch (Exception ex)
            {
                logHelper.WriteError(string.Format(Locale.IsRussian ?
                    "Ошибка при отключении: {0}" :
                    "Error disconnecting: {0}", ex.Message));
            }
            finally
            {
                mqttClient.Dispose();
            }
        }

        /// <summary>
        /// Raises a Completed event.
        /// </summary>
        private void OnCompleted()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles a message received event.
        /// </summary>
        private void MqttClient_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            if (stopReading)
                return;

            try
            {
                lock (mqttLock)
                {
                    messageDT = DateTime.UtcNow;
                    string topic = e.ApplicationMessage.Topic;
                    string payload = e.ApplicationMessage.ConvertPayloadToString();
                    logHelper.WriteMessage(string.Format("{0} {1}", topic, payload));

                    if (e.ApplicationMessage.Retain)
                    {
                        if (!WirenBoardModel.AddTopic(topic, payload, out string errMsg))
                            logHelper.WriteError(errMsg);
                    }
                    else
                    {
                        stopReading = true;
                        logHelper.WriteMessage(Locale.IsRussian ?
                            "Получен маркер завершения чтения" :
                            "Read completion marker received");
                    }
                }
            }
            catch (Exception ex)
            {
                logHelper.WriteError(ex.Message);
            }
        }


        /// <summary>
        /// Starts reading topics.
        /// </summary>
        public void Start()
        {
            if (tokenSource != null)
                throw new ScadaException("Topic reader already started.");

            tokenSource = new CancellationTokenSource();
            Task.Run(ReadTopics);
        }

        /// <summary>
        /// Breaks the current read operation.
        /// </summary>
        public void Break()
        {
            tokenSource?.Cancel();
        }


        /// <summary>
        /// Occurs when a read operation is completed.
        /// </summary>
        public event EventHandler Completed;
    }
}
