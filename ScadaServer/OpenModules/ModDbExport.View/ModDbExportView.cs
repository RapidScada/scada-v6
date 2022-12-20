// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Lang;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Modules.ModDbExport.View.Forms;

namespace Scada.Server.Modules.ModDbExport.View
{
    /// <summary>
    /// Implements the server module user interface.
    /// <para>Реализует пользовательский интерфейс серверного модуля.</para>
    /// </summary>
    public class ModDbExportView : ModuleView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModDbExportView()
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
                return Locale.IsRussian ?
                    "Экпорт в БД" :
                    "Database Export";
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
                    "Модуль экспортирует данные в сторонние базы данных. " + 
                    "Поддерживает Microsoft SQL Server, Oracle, PostgreSQL и MySQL." :
                    "The module exports data to external databases. " + 
                    "Supports Microsoft SQL Server, Oracle, PostgreSQL and MySQL.";
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
            return new FrmModuleConfig(ConfigDataset, AppDirs).ShowDialog() == DialogResult.OK;
        }
    }
}
 