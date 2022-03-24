// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Lang;
using Scada.Log;

namespace Scada.Comm.Drivers.DrvDsMqtt.Logic
{
    /// <summary>
    /// Implements the data source logic.
    /// <para>Реализует логику источника данных.</para>
    /// </summary>
    internal class MqttDSL : DataSourceLogic
    {
        private readonly MqttDSO dsOptions;                 // the data source options
        private readonly ILog dsLog;                        // the data source log
        private readonly MqttClientHelper mqttClientHelper; // keeps MQTT connection

        private Thread thread;            // the thread for communication with the server
        private volatile bool terminated; // necessary to stop the thread


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttDSL(ICommContext commContext, DataSourceConfig dataSourceConfig)
            : base(commContext, dataSourceConfig)
        {
            dsOptions = new MqttDSO(dataSourceConfig.CustomOptions);
            dsLog = CreateLog(DriverUtils.DriverCode);
            mqttClientHelper = new MqttClientHelper(dsOptions.ConnectionOptions, dsLog);

            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Handles a message received event.
        /// </summary>
        private void MqttClient_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            dsLog.WriteLine();
            dsLog.WriteAction("MqttClient_ApplicationMessageReceived");
        }

        /// <summary>
        /// Reconnects the MQTT client to the MQTT broker if it is disconnected.
        /// </summary>
        private void ReconnectIfRequired()
        {
            if (!mqttClientHelper.Client.IsConnected && mqttClientHelper.Connect())
            {
                //Subscribe();
            }
        }

        /// <summary>
        /// Executes an MQTT communication loop.
        /// </summary>
        private void Execute()
        {
            while (!terminated)
            {
                ReconnectIfRequired();
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }


        /// <summary>
        /// Makes the data source ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            dsLog.WriteBreak();

            // setup MQTT client
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
        }
    }
}
