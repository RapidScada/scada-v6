// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModDbExport.Logic
{
    /// <summary>
    /// Specifies the commands supported by an exporter.
    /// <para>Задаёт команды, поддерживаемые экспортёром.</para>
    /// </summary>
    internal enum ExporterCommand
    {
        ExportArchive,
        ClearTaskQueue
    }
}
