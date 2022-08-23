// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Drivers.DrvDsScadaServer.View.Forms;
using Scada.Forms;
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
            CanShowProperties = true;
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
                    "Источник данных Сервер" : 
                    "Server Data Source";
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
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, DriverUtils.DriverCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Shows a modal dialog box for editing driver properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return new FrmConnManager(AppDirs.ConfigDir).ShowDialog() == DialogResult.OK;
        }

        /// <summary>
        /// Creates a new data source user interface.
        /// </summary>
        public override DataSourceView CreateDataSourceView(DataSourceConfig dataSourceConfig)
        {
            return new ScadaServerDSV(this, dataSourceConfig);
        }
    }
}
