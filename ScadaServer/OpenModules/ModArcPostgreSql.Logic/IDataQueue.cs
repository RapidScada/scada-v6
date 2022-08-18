// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Represents a queue for writing to a database.
    /// <para>Представляет очередь для записи в базу данных.</para>
    /// </summary>
    internal interface IDataQueue
    {
        /// <summary>
        /// Gets the maximum queue size.
        /// </summary>
        int MaxQueueSize { get; }
        
        /// <summary>
        /// Gets the current queue size.
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Gets a value indicating whether the queue is in error state.
        /// </summary>
        bool HasError { get; }
    }
}
