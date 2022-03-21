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
using Scada.Data.Models;
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
        private readonly MqttPublisherDeviceConfig config; // the device configuration
        private readonly ScadaClient scadaClient;          // interacts with the Server service

        private IMqttClientChannel mqttClientChannel; // the communication channel reference
        private int[] cnlNums;                        // the numbers of the published channels
        private long cnlListID;                       // the cached channel list ID


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevMqttPublisherLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = new MqttPublisherDeviceConfig();
            scadaClient = new ScadaClient(commContext.AppConfig.ConnectionOptions);

            mqttClientChannel = null;
            cnlNums = null;
            cnlListID = 0;

            ConnectionRequired = false;
        }


        /// <summary>
        /// Initializes the numbers of the published channels.
        /// </summary>
        private void InitCnlNums()
        {

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

            if (LineContext.Channel is IMqttClientChannel channel)
            {
                mqttClientChannel = channel;
                InitCnlNums();
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

        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();

            /*if (scadaClient == null)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0}Устройство не привязано к базе конфигурации" :
                    "{0}The device is not bound to the configuration database", CommPhrases.ErrorPrefix);
            }
            else
            {
                //scadaClient.GetCurrentData()
            }*/

            if (PollingOptions.Delay > 0)
                Thread.Sleep(PollingOptions.Delay);

            FinishSession();
        }
    }
}
