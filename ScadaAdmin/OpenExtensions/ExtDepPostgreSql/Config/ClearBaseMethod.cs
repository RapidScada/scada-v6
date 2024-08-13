// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using NCM = System.ComponentModel;

namespace Scada.Admin.Extensions.ExtDepPostgreSql.Config
{
    /// <summary>
    /// Specifies the methods for clearing the configuration database.
    /// <para>Задаёт методы очистки базы конфигурации.</para>
    /// </summary>
    [NCM.TypeConverter(typeof(EnumConverter))]
    internal enum ClearBaseMethod
    {
        [Description("Drop tables")]
        DropTables,

        [Description("Truncate tables")]
        TruncateTables
    }
}
