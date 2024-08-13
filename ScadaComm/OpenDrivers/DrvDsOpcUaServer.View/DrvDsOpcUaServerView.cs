// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvDsOpcUaServer.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvDsOpcUaServerView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvDsOpcUaServerView()
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
                return Locale.IsRussian ?
                    "Сервер OPC UA" :
                    "OPC UA Server";
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
                    "Предоставляет данные сторонним клиентам по стандарту OPC UA." :
                    "Provides data to third-party clients according to the OPC UA standard.";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, DriverUtils.DriverCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            DriverPhrases.Init();
        }

        /// <summary>
        /// Creates a new data source user interface.
        /// </summary>
        public override DataSourceView CreateDataSourceView(DataSourceConfig dataSourceConfig)
        {
            return new OpcUaServerDSV(this, dataSourceConfig);
        }
    }
}
