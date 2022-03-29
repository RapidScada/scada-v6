// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvDsMqtt.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvDsMqttView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvDsMqttView()
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
                return Locale.IsRussian ? "Источник данных MQTT" : "MQTT Data Source";
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
                    "Публикует данные всех устройств на MQTT-брокер." :
                    "Publishes data of all devices to MQTT broker.";
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
        /// Creates a new data source user interface.
        /// </summary>
        public override DataSourceView CreateDataSourceView(DataSourceConfig dataSourceConfig)
        {
            return new MqttDSV(this, dataSourceConfig);
        }
    }
}
