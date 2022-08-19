// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Log;

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Represents the base class for data queues.
    /// <para>Представляет базовый класс для очередей данных.</para>
    /// </summary>
    internal abstract class QueueBase
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QueueBase(int maxQueueSize)
        {
            MaxQueueSize = maxQueueSize;
            HasError = false;
            LastCommitTime = DateTime.MinValue;
            ArchiveCode = "";
            AppLog = null;
            ArcLog = null;
            Connection = null;
        }


        /// <summary>
        /// Gets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; }

        /// <summary>
        /// Gets the current queue size.
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Gets a value indicating whether the queue is in error state.
        /// </summary>
        public bool HasError { get; protected set; }

        /// <summary>
        /// Gets the time (UTC) of the last successful commit.
        /// </summary>
        public DateTime LastCommitTime { get; protected set; }

        /// <summary>
        /// Gets or sets the archive code.
        /// </summary>
        public string ArchiveCode { get; init; }

        /// <summary>
        /// Gets or sets the application log.
        /// </summary>
        public ILog AppLog { get; init; }

        /// <summary>
        /// Gets or sets the archive log.
        /// </summary>
        public ILog ArcLog { get; init; }

        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        public NpgsqlConnection Connection { get; set; }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the queue.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }
    }
}
