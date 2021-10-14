// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Lang;
using Scada.Storages;

namespace Scada.Comm.Drivers.DrvDsScadaServer.Logic
{
    /// <summary>
    /// Implements the driver logic.
    /// <para>Реализует логику драйвера.</para>
    /// </summary>
    public class DrvDsScadaServerLogic : DriverLogic
    {
        private readonly DriverConfig driverConfig; // the driver configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvDsScadaServerLogic(ICommContext commContext)
            : base(commContext)
        {
            driverConfig = new DriverConfig();
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
            return new ScadaServerDSL(commContext, dataSourceConfig, driverConfig);
        }

        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public override void OnServiceStart()
        {
            if (CommContext.Storage.GetFileInfo(DataCategory.Config, DriverConfig.DefaultFileName).Exists &&
                !driverConfig.Load(CommContext.Storage, DriverConfig.DefaultFileName, out string errMsg))
            {
                Log.WriteError(CommPhrases.DriverMessage, Code, errMsg);
            }
        }
    }
}
