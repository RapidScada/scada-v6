// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Data.Models;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvDsMqtt.View.Forms
{
    /// <summary>
    /// Represents a form for editing data source options.
    /// <para>Представляет форму для редактирования параметров источника данных.</para>
    /// </summary>
    public partial class FrmMqttDSO : Form
    {
        private readonly BaseDataSet baseDataSet;           // the configuration database
        private readonly DataSourceConfig dataSourceConfig; // the data source configuration
        private readonly MqttDSO options;                   // the data source options


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmMqttDSO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmMqttDSO(BaseDataSet baseDataSet, DataSourceConfig dataSourceConfig)
            : this()
        {
            this.baseDataSet = baseDataSet ?? throw new ArgumentNullException(nameof(baseDataSet));
            this.dataSourceConfig = dataSourceConfig ?? throw new ArgumentNullException(nameof(dataSourceConfig));
            options = new MqttDSO(dataSourceConfig.CustomOptions);
        }


        private void FrmMqttDSO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            //OptionsToControls();
        }
    }
}
