// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Modules.ModArcInfluxDb.Config;

namespace Scada.Server.Modules.ModArcInfluxDb.View.Forms
{
    /// <summary>
    /// Represents a connection manager form.
    /// <para>Представляет форму менеджера соединений.</para>
    /// </summary>
    public partial class FrmConnManager : Form
    {
        private readonly string configFileName;     // the module configuration file name
        private readonly ModuleConfig moduleConfig; // the module configuration
        private bool sortRequired;                  // indicated whether to sort the list

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConnManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConnManager(string configDir)
            : this()
        {
            configFileName = Path.Combine(configDir, ModuleConfig.ConfigFileName);
            moduleConfig = new ModuleConfig();
            sortRequired = false;
        }


        /// <summary>
        /// Gets the connection names.
        /// </summary>
        public string[] ConnectionNames => moduleConfig.Connections.Keys.ToArray();


        /// <summary>
        /// Loads the module configuration.
        /// </summary>
        private void LoadConfig()
        {
            if (File.Exists(configFileName) && !moduleConfig.Load(configFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Saves the module configuration.
        /// </summary>
        private bool SaveConfig()
        {
            if (moduleConfig.Save(configFileName, out string errMsg))
            {
                return true;
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
                return false;
            }
        }
    }
}
