// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.DataSources;

namespace Scada.Comm.Drivers.DrvDsMqtt.Logic
{
    /// <summary>
    /// Implements the driver logic.
    /// <para>Реализует логику драйвера.</para>
    /// </summary>
    public class DrvDsMqttLogic : DriverLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvDsMqttLogic(ICommContext commContext)
            : base(commContext)
        {
        }

        /// <summary>
        /// Gets the driver code.
        /// </summary>
        public override string Code
        {
            get
            {
                return DriverUtils.DriverCode;
            }
        }

        /// <summary>
        /// Creates a new data source.
        /// </summary>
        public override DataSourceLogic CreateDataSource(ICommContext commContext, DataSourceConfig dataSourceConfig)
        {
            return new MqttDSL(commContext, dataSourceConfig);
        }
    }
}
