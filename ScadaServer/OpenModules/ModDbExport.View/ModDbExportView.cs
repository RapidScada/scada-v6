// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Server.Modules.ModDbExport.View
{
    /// <summary>
    /// Implements the server module user interface.
    /// <para>Реализует пользовательский интерфейс серверного модуля.</para>
    /// </summary>
    public class ModDbExportView : ModuleView
    {
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
    }
}
