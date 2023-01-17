// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModDbExport.Logic
{
    /// <summary>
    /// Specifies the DB connection statuses.
    /// <para>Задаёт статусы соединения с БД.</para>
    /// </summary>
    internal enum ConnectionStatus
    {
        Undefined,
        Normal,
        Error,
    }
}
