// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Data.Queues;
using Scada.Log;
using System.Diagnostics;

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Represents the base class for data queues.
    /// <para>Представляет базовый класс для очередей данных.</para>
    /// </summary>
    internal abstract class QueueBase<T>
    {
        /// <summary>
        /// The minimum queue size.
        /// </summary>
        private const int MinQueueSize = 100;
        /// <summary>
        /// The minimum batch size.
        /// </summary>
        private const int MinBatchSize = 100;

        protected readonly Queue<T> queue; // contains queue data


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QueueBase(int maxQueueSize, int batchSize)
        {
            MaxQueueSize = Math.Max(maxQueueSize, MinQueueSize);
            BatchSize = Math.Min(batchSize, MinBatchSize);
            LastCommitTime = DateTime.MinValue;
            ArchiveCode = "";
            AppLog = null;
            ArcLog = null;
            Connection = null;
            Stats = new QueueStats
            {
                Enabled = true,
                MaxQueueSize = MaxQueueSize
            };

            queue = new Queue<T>(MaxQueueSize);
        }


        /// <summary>
        /// Gets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; }

        /// <summary>
        /// Gets the number of items transferred in one transaction.
        /// </summary>
        public int BatchSize { get; }

        /// <summary>
        /// Gets the current queue size.
        /// </summary>
        public int Count => queue.Count;

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
        /// Gets the queue statistics.
        /// </summary>
        public QueueStats Stats { get; }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the queue.
        /// </summary>
        public object SyncRoot => this;


        /// <summary>
        /// Removes excess items from the beginning of the queue.
        /// </summary>
        protected bool RemoveExcessItems(out int lostCount)
        {
            lostCount = 0;

            lock (SyncRoot)
            {
                while (queue.Count > MaxQueueSize)
                {
                    queue.Dequeue();
                    lostCount++;
                }
            }

            return lostCount > 0;
        }

        /// <summary>
        /// Enqueues the item to the queue.
        /// </summary>
        public void Enqueue(T item)
        {
            lock (SyncRoot)
            {
                queue.Enqueue(item);
            }
        }
        
        /// <summary>
        /// Removes and returns the item at the beginning of the queue.
        /// </summary>
        public bool TryDequeue(out T item)
        {
            lock (queue)
            {
                return queue.TryDequeue(out item);
            }
        }

        /// <summary>
        /// Retrieves items from the queue and inserts or updates them in the database.
        /// </summary>
        public abstract bool ProcessItems();

        /// <summary>
        /// Processes all data from the queue within the specified time.
        /// </summary>
        public void FlushItems(int duration)
        {
            TimeSpan durationSpan = TimeSpan.FromSeconds(duration);
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed <= durationSpan && ProcessItems())
            {
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }
    }
}
