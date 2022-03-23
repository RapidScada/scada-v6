// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.DataSources;

namespace Scada.Comm.Drivers.DrvDsMqtt.Logic
{
    /// <summary>
    /// Implements the data source logic.
    /// <para>Реализует логику источника данных.</para>
    /// </summary>
    internal class MqttDSL : DataSourceLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttDSL(ICommContext commContext, DataSourceConfig dataSourceConfig)
            : base(commContext, dataSourceConfig)
        {
            //options = new OpcUaServerDSO(dataSourceConfig.CustomOptions);
            //dsLog = CreateLog(DriverUtils.DriverCode);
        }
    }
}
