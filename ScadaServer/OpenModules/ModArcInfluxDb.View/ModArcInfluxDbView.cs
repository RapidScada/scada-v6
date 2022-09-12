// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Lang;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Modules.ModArcInfluxDb.View.Forms;

namespace Scada.Server.Modules.ModArcInfluxDb.View
{
    /// <summary>
    /// Implements the server module user interface.
    /// <para>Реализует пользовательский интерфейс серверного модуля.</para>
    /// </summary>
    public class ModArcInfluxDbView : ModuleView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModArcInfluxDbView()
        {
            //CanShowProperties = true;
        }


        /// <summary>
        /// Gets the module name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ?
                    "Архив InfluxDB" :
                    "InfluxDB Archive";
            }
        }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Модуль обеспечивает архивирование исторических данных в базу данных временных рядов InfluxDB." :
                    "The module provides archiving of historical data into InfluxDB time series database.";
            }
        }
        
        
        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, ModuleUtils.ModuleCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Shows a modal dialog box for editing module properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return new FrmConnManager(AppDirs.ConfigDir).ShowDialog() == DialogResult.OK;
            //return false;
        }

        /// <summary>
        /// Indicates whether the module can create an archive of the specified kind.
        /// </summary>
        public override bool CanCreateArchive(ArchiveKind kind)
        {
            return kind == ArchiveKind.Historical;
        }

        /// <summary>
        /// Creates a new archive user interface.
        /// </summary>
        public override ArchiveView CreateArchiveView(ArchiveConfig archiveConfig)
        {
            return new InfluxArchiveView(this, archiveConfig);
            //return null;
        }
    }
}
