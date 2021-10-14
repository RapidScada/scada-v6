// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvDsScadaServer.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvDsScadaServerView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvDsScadaServerView()
            : base()
        {
            CanCreateDataSource = true;
        }


        /// <summary>
        /// Gets the driver name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Источник данных Сервер" : "Server Data Source";
            }
        }

        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Обеспечивает связь между службами Коммуникатора и Сервера." :
                    "Provides communication between the Communicator and Server services.";
            }
        }


        /// <summary>
        /// Creates a new data source user interface.
        /// </summary>
        public override DataSourceView CreateDataSourceView(DataSourceConfig dataSourceConfig)
        {
            return null;
        }
    }
}
