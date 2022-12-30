// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Forms.Forms
{
    /// <summary>
    /// Represents a form for editing options.
    /// <para>Представляет форму для редактирования параметров.</para>
    /// </summary>
    public partial class FrmOptions : Form
    {
        private object options;    // the options to edit
        private ConfigBase config; // the module configuration

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmOptions()
        {
            InitializeComponent();

            options = null;
            config = null;
            ConfigFileName = "";
        }


        /// <summary>
        /// Gets or sets the options to edit.
        /// </summary>
        public object Options
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
                config = value as ConfigBase;
            }
        }

        /// <summary>
        /// Gets or sets the module configuration.
        /// </summary>
        public ConfigBase Config
        {
            get
            {
                return config;
            }
            set
            {
                options = value;
                config = value;
            }
        }

        /// <summary>
        /// Gets or sets the configuration file name.
        /// </summary>
        public string ConfigFileName { get; set; }


        private void FrmOptions_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);

            if (!string.IsNullOrEmpty(ConfigFileName) && File.Exists(ConfigFileName) &&
                Config != null && !Config.Load(ConfigFileName, out string errMsg))
            {
                ScadaUiUtils.ShowError(errMsg);
            }

            propertyGrid.SelectedObject = Options;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigFileName) ||
                Config == null || Config.Save(ConfigFileName, out string errMsg))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
            }
        }
    }
}
