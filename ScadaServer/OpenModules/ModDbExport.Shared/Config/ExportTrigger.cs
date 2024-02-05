// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModDbExport.Config
{
    /// <summary>
    /// Specifies the triggers for data export.
    /// <para>Задаёт триггеры для экспорта данных.</para>
    /// </summary>
    internal enum ExportTrigger
    {
        /// <summary>
        /// Export data after receiving.
        /// </summary>
        OnReceive,

        /// <summary>
        /// Export data periodically.
        /// </summary>
        OnTimer
    }
}
