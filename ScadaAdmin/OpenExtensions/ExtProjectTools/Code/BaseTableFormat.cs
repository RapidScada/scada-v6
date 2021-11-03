// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Admin.Extensions.ExtProjectTools.Code
{
    /// <summary>
    /// Specifies the file formats of the configuration database table.
    /// <para>Задает форматы файлов таблиц базы конфигурации.</para>
    /// </summary>
    public enum BaseTableFormat
    {
        /// <summary>
        /// Binary runtime tables.
        /// </summary>
        DAT,

        /// <summary>
        /// Project tables.
        /// </summary>
        XML,

        /// <summary>
        /// Comma-separated values.
        /// </summary>
        CSV
    }
}
