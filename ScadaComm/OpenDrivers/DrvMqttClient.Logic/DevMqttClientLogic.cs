// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Data.Models;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvMqttClient.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevMqttClientLogic : DeviceLogic
    {
        private MqttClientDeviceConfig config; // the device configuration
        private IMqttClient mqttClient;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevMqttClientLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = null;

            ConnectionRequired = false;
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            config = new MqttClientDeviceConfig();

            if (config.Load(Storage, MqttClientDeviceConfig.GetFileName(DeviceNum), out string errMsg))
            {
                //InitCmdMaps();
            }
            else
            {
                config = null;
                Log.WriteLine(errMsg);
                Log.WriteLine(Locale.IsRussian ?
                    "Взаимодействие с MQTT-брокером невозможно, т.к. конфигурация устройства не загружена" :
                    "Interaction with MQTT broker is impossible because device configuration is not loaded");
            }

            // ---
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
