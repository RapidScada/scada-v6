// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using Scada.Comm.Config;
using Scada.Comm.Devices;

namespace Scada.Comm.Drivers.DrvMqtt.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevMqttLogic : DeviceLogic
    {
        private IMqttClient mqttClient;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevMqttLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            ConnectionRequired = false;
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            MqttFactory mqttFactory = new();
            mqttClient = mqttFactory.CreateMqttClient();

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Log.WriteLine("!!! Application message = " + e.ApplicationMessage.Topic + ", " + 
                    e.ApplicationMessage.ConvertPayloadToString());
            });

            IMqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.mqtt-dashboard.com")
                .WithClientId("rapidscada")
                .Build();

            Task<MqttClientConnectResult> connectTask = mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            connectTask.Wait();
            Log.WriteLine("!!! Connect result = " + connectTask.Result);

            MqttClientSubscribeOptions mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic("/myparam1"); })
                .Build();

            mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None).Wait();
            Log.WriteLine("!!! MQTT client subscribed to topic.");
        }

        /// <summary>
        /// Performs actions when terminating a communication line.
        /// </summary>
        public override void OnCommLineTerminate()
        {
            if (mqttClient != null)
            {
                mqttClient.DisconnectAsync().Wait();
                mqttClient.Dispose();
            }
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            //base.Session();
            //FinishSession();
        }
    }
}
