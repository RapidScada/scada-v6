/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommon
 * Summary  : Represents a thread-safe data queue
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2023
 */

using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data.Queues
{
    /// <summary>
    /// Represents a thread-safe data queue.
    /// <para>Представляет потокобезопасную очередь данных.</para>
    /// </summary>
    public class DataQueue<T> : IDataQueue<T> where T : class
    {
        /// <summary>
        /// The minimum queue size.
        /// </summary>
        private const int MinSize = 100;

        private readonly Queue<QueueItem<T>> queue; // contains queue data


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataQueue(int maxSize)
            : this(true, maxSize, "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataQueue(bool enabled, int maxSize, string title)
        {
            if (maxSize < MinSize)
                maxSize = MinSize;

            queue = enabled ? new Queue<QueueItem<T>>(maxSize) : null;

            Enabled = enabled;
            MaxSize = maxSize;
            RemoveExceeded = false;
            Stats = new QueueStats
            {
                Enabled = enabled,
                Title = title,
                MaxQueueSize = maxSize
            };
        }


        /// <summary>
        /// Gets a value indicating whether the queue is in use.
        /// </summary>
        public bool Enabled { get; }

        /// <summary>
        /// Gets the maximum queue size.
        /// </summary>
        public int MaxSize { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to remove items from the beginning of the queue 
        /// if the size is exceeded.
        /// </summary>
        public bool RemoveExceeded { get; set; }

        /// <summary>
        /// Gets the current queue size.
        /// </summary>
        public int Count => queue == null ? 0 : queue.Count;

        /// <summary>
        /// Gets a value indicating whether the queue is empty.
        /// </summary>
        public bool IsEmpty => queue == null || queue.Count == 0;

        /// <summary>
        /// Gets the queue statistics.
        /// </summary>
        public QueueStats Stats { get; }


        /// <summary>
        /// Enqueues the specified value.
        /// </summary>
        public bool Enqueue(DateTime creationTime, T value)
        {
            return Enqueue(creationTime, value, out _);
        }

        /// <summary>
        /// Enqueues the specified value.
        /// </summary>
        public bool Enqueue(DateTime creationTime, T value, out string errMsg)
        {
            if (queue == null)
            {
                errMsg = Locale.IsRussian ?
                    "Очередь отключена" :
                    "Queue is disabled";
                return false;
            }

            lock (queue)
            {
                if (RemoveExceeded)
                {
                    // remove items from the beginning of the queue
                    while (queue.Count >= MaxSize)
                    {
                        queue.Dequeue();
                        Stats.SkippedItems++;
                    }
                }
                else if (queue.Count >= MaxSize)
                {
                    errMsg = string.Format(Locale.IsRussian ?
                        "Невозможно добавить данные в очередь \"{0}\". Максимальный размер очереди {1} превышен" :
                        "Unable to add data to the queue \"{0}\". The maximum size of the queue {1} is exceeded",
                        Stats.Title, MaxSize);

                    Stats.SkippedItems++;
                    return false;
                }

                // append item
                queue.Enqueue(new QueueItem<T>(creationTime, value));
                errMsg = "";
                return true;
            }
        }
        
        /// <summary>
        /// Enqueues the specified values.
        /// </summary>
        public bool Enqueue(DateTime creationTime, IEnumerable<T> values, out string errMsg)
        {
            if (queue == null)
            {
                errMsg = Locale.IsRussian ?
                    "Очередь отключена" :
                    "Queue is disabled";
                return false;
            }

            lock (queue)
            {
                foreach (T value in values)
                {
                    if (!Enqueue(creationTime, value, out errMsg))
                        return false;
                }
            }

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Removes and returns the item at the beginning of the queue.
        /// </summary>
        public bool TryDequeue(out QueueItem<T> item)
        {
            if (queue != null)
            {
                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        item = queue.Dequeue();
                        return true;
                    }
                }
            }

            item = default;
            return false;
        }

        /// <summary>
        /// Removes and returns the item value at the beginning of the queue.
        /// </summary>
        public bool TryDequeueValue(out T value)
        {
            if (TryDequeue(out QueueItem<T> item))
            {
                value = item.Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        /// Returns the item at the beginning of the queue without removing it.
        /// </summary>
        public bool TryPeek(out QueueItem<T> item)
        {
            if (queue != null)
            {
                lock (queue)
                {
                    if (queue.Count > 0)
                    {
                        item = queue.Peek();
                        return true;
                    }
                }
            }

            item = default;
            return false;
        }

        /// <summary>
        /// Returns the item to the end of the queue.
        /// </summary>
        public void ReturnItem(QueueItem<T> item)
        {
            if (queue != null)
            {
                lock (queue)
                {
                    queue.Enqueue(item);
                }
            }
        }

        /// <summary>
        /// Removes the specified item at the beginning of the queue.
        /// </summary>
        public void RemoveItem(QueueItem<T> item)
        {
            if (queue != null)
            {
                lock (queue)
                {
                    if (queue.Count > 0 && queue.Peek() == item)
                        queue.Dequeue();
                }
            }
        }

        /// <summary>
        /// Checks that the queue is empty and raises an event if it is.
        /// </summary>
        public void CheckEmpty()
        {
            if (queue != null && queue.Count == 0)
                Empty?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Appends information to the string builder.
        /// </summary>
        public void AppendInfo(StringBuilder sbInfo)
        {
            Stats.AppendInfo(sbInfo, queue?.Count);
        }

        /// <summary>
        /// Appends information to the string builder as a single line.
        /// </summary>
        public void AppendShortInfo(StringBuilder sbInfo, int titleWidth)
        {
            Stats.AppendShortInfo(sbInfo, queue?.Count, titleWidth);
        }


        /// <summary>
        /// Occurs when the queue is empty.
        /// </summary>
        public event EventHandler Empty;
    }
}
