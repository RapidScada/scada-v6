// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Lang;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Modules.ModArcPostgreSql.View.Forms;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModArcPostgreSql.View
{
    /// <summary>
    /// Implements the server module user interface.
    /// <para>Реализует пользовательский интерфейс серверного модуля.</para>
    /// </summary>
    public class ModArcPostgreSqlView : ModuleView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModArcPostgreSqlView()
        {
            CanShowProperties = true;
        }


        /// <summary>
        /// Gets the module name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Архив PostgreSQL" : "PostgreSQL Archive";
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
                    "Модуль предоставляет архивирование данных в базу данных PostgreSQL." :
                    "The module provides data archiving into PostgreSQL database.";
            }
        }


        /// <summary>
        /// Shows a modal dialog box for editing module properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return new FrmConnManager(AppDirs.ConfigDir).ShowDialog() == DialogResult.OK;
        }

        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, ModuleUtils.ModuleCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            ModulePhrases.Init();
        }

        /// <summary>
        /// Indicates whether the module can create an archive of the specified kind.
        /// </summary>
        public override bool CanCreateArchive(ArchiveKind kind)
        {
            return kind == ArchiveKind.Current || kind == ArchiveKind.Historical || kind == ArchiveKind.Events;
        }

        /// <summary>
        /// Creates a new archive user interface.
        /// </summary>
        public override ArchiveView CreateArchiveView(ArchiveConfig archiveConfig)
        {
            return archiveConfig.Kind switch
            {
                ArchiveKind.Current => new PostgreCAV(this, archiveConfig),
                ArchiveKind.Historical => new PostgreHAV(this, archiveConfig),
                ArchiveKind.Events => new PostgreEAV(this, archiveConfig),
                _ => null
            };
        }
    }
}
