/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaCommEngine
 * Summary  : Represents a data queue that transfers data to a server
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Client;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Represents a data queue that transfers data to a server.
    /// <para>Представляет очередь данных, которая передаёт данные на сервер.</para>
    /// </summary>
    internal class DataQueue
    {
        /// <summary>
        /// Represents a queue item.
        /// </summary>
        private class QueueItem<T> where T : class
        {
            public QueueItem(DateTime creationTime, T value)
            {
                CreationTime = creationTime;
                Value = value ?? throw new ArgumentNullException("value");
            }
            public DateTime CreationTime { get; }
            public T Value { get; }
        }


        /// <summary>
        /// The minimum queue size.
        /// </summary>
        private const int MinQueueSize = 100;
        /// <summary>
        /// The number of queue items to send when performing a data transfer operation.
        /// </summary>
        private const int BundleSize = 10;

        private readonly TimeSpan maxCurDataAge;  // determines sending current data as historical
        private readonly TimeSpan dataLifetime;   // the data lifetime in the queue
        private readonly ScadaClient scadaClient; // communicates with the server
        private readonly ILog log;                // the application log

        private readonly int maxQueueSize;                            // the maximum queue size
        private readonly Queue<QueueItem<DeviceSlice>> curDataQueue;  // the current data queue
        private readonly Queue<QueueItem<DeviceSlice>> histDataQueue; // the historical data queue
        private readonly Queue<QueueItem<DeviceEvent>> eventQueue;    // the event queue

        private int curDataSkipped;  // the number of skipped slices of current data 
        private int histDataSkipped; // the number of skipped slices of historical data
        private int eventSkipped;    // the number of skipped events


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataQueue(GeneralOptions generalOptions, ScadaClient scadaClient, ILog log)
        {
            if (generalOptions == null) 
                throw new ArgumentNullException(nameof(generalOptions));

            maxCurDataAge = TimeSpan.FromSeconds(generalOptions.MaxCurDataAge);
            dataLifetime = TimeSpan.FromSeconds(generalOptions.DataLifetime);
            this.scadaClient = scadaClient ?? throw new ArgumentNullException(nameof(scadaClient));
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            maxQueueSize = Math.Max(generalOptions.MaxQueueSize, MinQueueSize);
            curDataQueue = new Queue<QueueItem<DeviceSlice>>(maxQueueSize);
            histDataQueue = new Queue<QueueItem<DeviceSlice>>(maxQueueSize);
            eventQueue = new Queue<QueueItem<DeviceEvent>>(maxQueueSize);

            curDataSkipped = 0;
            histDataSkipped = 0;
            eventSkipped = 0;
        }


        /// <summary>
        /// Transfers current data to the server.
        /// </summary>
        private void TransferCurrentData()
        {

        }

        /// <summary>
        /// Transfers historical data to the server.
        /// </summary>
        private void TransferHistoricalData()
        {

        }

        /// <summary>
        /// Transfers events to the server.
        /// </summary>
        private void TransferEvents()
        {
            try
            {
                for (int i = 0; i < BundleSize; i++)
                {
                    /*
                    // retrieve an event from the queue
                    QueueItem<EventTableLight.Event> queueItem;
                    EventTableLight.Event ev;

                    lock (eventQueue)
                    {
                        if (eventQueue.Count > 0)
                        {
                            queueItem = eventQueue.Dequeue();
                            ev = queueItem.Value;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // export the event
                    if (DateTime.UtcNow - queueItem.Timestamp > maxWaitingTime)
                    {
                        eventStats.SkippedItems++;
                        log.WriteError(string.Format(Localization.UseRussian ?
                            "Устаревшее событие за {0} не экспортировано" :
                            "The outdated event for {0} is not exported",
                            ev.DateTime.ToLocalizedString()));
                    }
                    else if (serverComm.SendEvent(ev, out bool sendingResult) && sendingResult)
                    {
                        eventStats.ExportedItems++;
                        eventStats.ErrorState = false;
                    }
                    else
                    {
                        // return the unsent event to the queue
                        lock (eventQueue)
                        {
                            eventQueue.Enqueue(queueItem);
                        }

                        eventStats.ErrorState = true;
                        Thread.Sleep(ErrorDelay);
                    }*/
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при передаче событий" :
                    "Error transferring events");
            }
        }

        /// <summary>
        /// Calls the FailedToSendCallback method of the device slice.
        /// </summary>
        private void CallFailedToSend(DeviceSlice deviceSlice)
        {
            try
            {
                deviceSlice.FailedToSendCallback?.Invoke(deviceSlice);
            }
            catch (Exception ex)
            {
                log.WriteException(ex);
            }
        }

        /// <summary>
        /// Calls the FailedToSendCallback method of the device event.
        /// </summary>
        private void CallFailedToSend(DeviceEvent deviceEvent)
        {
            try
            {
                deviceEvent.FailedToSendCallback?.Invoke(deviceEvent);
            }
            catch (Exception ex)
            {
                log.WriteException(ex);
            }
        }


        /// <summary>
        /// Adds the slice of the current data to the queue for transfer to the server.
        /// </summary>
        public void EnqueueCurrentData(DeviceSlice deviceSlice)
        {
            if (deviceSlice == null)
                throw new ArgumentNullException(nameof(deviceSlice));

            lock (curDataQueue)
            {
                // remove current data from the beginning of the queue
                while (curDataQueue.Count >= maxQueueSize)
                {
                    curDataQueue.Dequeue();
                    curDataSkipped++;
                }

                // append current data
                curDataQueue.Enqueue(new QueueItem<DeviceSlice>(deviceSlice.Timestamp, deviceSlice));
            }
        }

        /// <summary>
        /// Adds the slice of historical data to the queue for transfer to the server.
        /// </summary>
        public void EnqueueHistoricalData(DeviceSlice deviceSlice)
        {
            if (deviceSlice == null)
                throw new ArgumentNullException(nameof(deviceSlice));

            lock (histDataQueue)
            {
                if (histDataQueue.Count < maxQueueSize)
                {
                    histDataQueue.Enqueue(new QueueItem<DeviceSlice>(DateTime.UtcNow, deviceSlice));
                }
                else
                {
                    log.WriteError(string.Format(Locale.IsRussian ?
                        "Невозможно добавить архивные данные в очередь. Максимальный размер очереди {0} превышен" :
                        "Unable to enqueue historical data. The maximum size of the queue {0} is exceeded",
                        maxQueueSize));

                    histDataSkipped++;
                    CallFailedToSend(deviceSlice);
                }
            }
        }

        /// <summary>
        /// Adds the event to the queue for transfer to the server.
        /// </summary>
        public void EnqueueEvent(DeviceEvent deviceEvent)
        {
            if (deviceEvent == null)
                throw new ArgumentNullException(nameof(deviceEvent));

            lock (eventQueue)
            {
                if (eventQueue.Count < maxQueueSize)
                {
                    eventQueue.Enqueue(new QueueItem<DeviceEvent>(DateTime.UtcNow, deviceEvent));
                }
                else
                {
                    log.WriteError(string.Format(Locale.IsRussian ?
                        "Невозможно добавить событие в очередь. Максимальный размер очереди {0} превышен" :
                        "Unable to enqueue an event. The maximum size of the queue {0} is exceeded",
                        maxQueueSize));

                    eventSkipped++;
                    CallFailedToSend(deviceEvent);
                }
            }
        }

        /// <summary>
        /// Transfers data to the server.
        /// </summary>
        public void TransferData()
        {
            TransferCurrentData();
            TransferHistoricalData();
            TransferEvents();
        }

        /// <summary>
        /// Appends information about the queue to the string builder.
        /// </summary>
        public void AppendInfo(StringBuilder sb)
        {
            if (sb == null)
                throw new ArgumentNullException(nameof(sb));

            if (Locale.IsRussian)
            {
                sb.Append("Очередь текущих данных  : ")
                    .Append(curDataQueue.Count).Append(" из ").Append(maxQueueSize)
                    .Append(", пропущено ").Append(curDataSkipped);

                sb.Append("Очередь архивных данных : ")
                    .Append(histDataQueue.Count).Append(" из ").Append(maxQueueSize)
                    .Append(", пропущено ").Append(histDataSkipped);

                sb.Append("Очередь событий         : ")
                    .Append(eventQueue.Count).Append(" из ").Append(maxQueueSize)
                    .Append(", пропущено ").Append(eventSkipped);
            }
            else
            {
                sb.Append("Current data queue    : ")
                    .Append(curDataQueue.Count).Append(" of ").Append(maxQueueSize)
                    .Append(", skipped ").Append(curDataSkipped);

                sb.Append("Historical data queue : ")
                    .Append(histDataQueue.Count).Append(" of ").Append(maxQueueSize)
                    .Append(", skipped ").Append(histDataSkipped);

                sb.Append("Event queue             : ")
                    .Append(eventQueue.Count).Append(" of ").Append(maxQueueSize)
                    .Append(", skipped ").Append(eventSkipped);
            }
        }
    }
}
