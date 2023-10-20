// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModArcPostgreSql.Config
{
    /// <summary>
    /// Specifies the partition sizes.
    /// <para>Задаёт размеры секций.</para>
    /// </summary>
    internal enum PartitionSize
    {
        OneDay,
        OneMonth,
        OneYear
    }
}
