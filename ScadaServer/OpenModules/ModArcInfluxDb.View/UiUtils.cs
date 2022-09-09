// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Modules.ModArcInfluxDb.Config;
using Scada.Server.Modules.ModArcInfluxDb.View.Forms;

namespace Scada.Server.Modules.ModArcInfluxDb.View
{
    /// <summary>
    /// The class provides helper methods for the archive option forms.
    /// <para>Класс, предоставляющий вспомогательные методы для форм параметров архива.</para>
    /// </summary>
    internal class UiUtils
    {
        /// <summary>
        /// Fills the connection combo box from a configuration file.
        /// </summary>
        public static void FillConnections(ComboBox comboBox, string configDir)
        {
            string configFileName = Path.Combine(configDir, ModuleConfig.ConfigFileName);

            if (File.Exists(configFileName))
            {
                ModuleConfig moduleConfig = new();

                if (moduleConfig.Load(configFileName, out string errMsg))
                {
                    comboBox.Items.Clear();
                    comboBox.Items.AddRange(moduleConfig.Connections.Keys.ToArray());
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }
        }
        
        /// <summary>
        /// Shows a connection manager and updates the connection combo box.
        /// </summary>
        public static void EditConnections(ComboBox comboBox, string configDir)
        {
            FrmConnManager frmConnManager = new(configDir);

            if (frmConnManager.ShowDialog() == DialogResult.OK)
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(frmConnManager.ConnectionNames);
            }
        }
    }
}
