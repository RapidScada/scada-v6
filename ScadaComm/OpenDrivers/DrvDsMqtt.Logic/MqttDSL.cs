// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MQTTnet;
using MQTTnet.Client.Options;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqtt;
using Scada.Log;

namespace Scada.Comm.Drivers.DrvDsMqtt.Logic
{
    /// <summary>
    /// Implements the data source logic.
    /// <para>Реализует логику источника данных.</para>
    /// </summary>
    internal class MqttDSL : DataSourceLogic
    {
        private readonly MqttFactory mqttFactory;                 // creates MQTT objects
        private readonly MqttConnectionOptions connectionOptions; // the connection options
        private readonly IMqttClientOptions clientOptions;        // the client options
        private readonly ILog dsLog;                              // the data source log

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttDSL(ICommContext commContext, DataSourceConfig dataSourceConfig)
            : base(commContext, dataSourceConfig)
        {
            mqttFactory = new MqttFactory();
            connectionOptions = new MqttConnectionOptions(dataSourceConfig.CustomOptions);
            clientOptions = connectionOptions.ToMqttClientOptions();
            dsLog = CreateLog(DrvDsMqttLogic.DriverCode);
        }


        /// <summary>
        /// Makes the data source ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            dsLog.WriteBreak();
        }

        /// <summary>
        /// Starts the data source.
        /// </summary>
        public override void Start()
        {
        }

        /// <summary>
        /// Closes the data source.
        /// </summary>
        public override void Close()
        {
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
