// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModDbExport.Config
{
    /// <summary>
    /// Specifies the data kinds of the queries.
    /// <para>Задаёт виды данных запросов.</para>
    /// </summary>
    internal enum DataKind
    {
        Current,
        Historical,
        Event,
        EventAck,
        Command
    }
}
