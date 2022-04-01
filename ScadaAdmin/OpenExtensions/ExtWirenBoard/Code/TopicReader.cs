// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
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
        private readonly MqttConnectionOptions connOptions; // the connection options
        private readonly RichTextBoxHelper logHelper;       // provides access to log
        private CancellationTokenSource tokenSource;        // provides a cancellation mechanism


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TopicReader(MqttConnectionOptions mqttConnectionOptions, RichTextBoxHelper logHelper)
        {
            connOptions = mqttConnectionOptions ?? throw new ArgumentNullException(nameof(mqttConnectionOptions));
            this.logHelper = logHelper ?? throw new ArgumentNullException(nameof(logHelper));
            tokenSource = null;
            ReadResult = false;
        }


        /// <summary>
        /// Gets the result of a read operation.
        /// </summary>
        public bool ReadResult { get; private set; }


        /// <summary>
        /// Reads the topics from Wiren Board.
        /// </summary>
        private void ReadTopics()
        {
            IMqttClient mqttClient = null;

            try
            {
                mqttClient = new MqttFactory().CreateMqttClient();
                logHelper.WriteMessage(string.Format(Locale.IsRussian ?
                    "Соединение с {0}:{1}" :
                    "Connect to {0}:{1}",
                    connOptions.Server, connOptions.Port));

                MqttClientConnectResultCode resultCode = 
                    mqttClient.ConnectAsync(connOptions.ToMqttClientOptions(), tokenSource.Token)
                    .Result.ResultCode;

                if (resultCode == MqttClientConnectResultCode.Success)
                {
                    logHelper.WriteMessage(string.Format(Locale.IsRussian ?
                        "Соединение установлено успешно" :
                        "Connected successfully"));
                    ReadResult = true;
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
        public void Disconnect(IMqttClient mqttClient)
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
