// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Adapters;
using Scada.Data.Models;
using Scada.Data.Queues;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Server.Modules.ModArcBasic.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using static Scada.Server.Modules.ModArcBasic.Logic.ModuleConst;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// Implements the event archive logic.
    /// <para>Реализует логику архива событий.</para>
    /// </summary>
    internal class BasicEAL : EventArchiveLogic
    {
        private readonly ModuleConfig moduleConfig;        // the module configuration
        private readonly BasicEAO options;                 // the archive options
        private readonly ILog appLog;                      // the application log
        private readonly ILog arcLog;                      // the archive log
        private readonly DataQueue<Event> eventQueue;      // contains events for writing
        private readonly EventTableAdapter readingAdapter; // reads events
        private readonly EventTableAdapter writingAdapter; // writes events
        private readonly MemoryCache<DateTime, EventTable> tableCache; // the cache containing event tables
        private readonly object readingLock;               // synchronizes reading from the archive
        private readonly object writingLock;               // synchronizes writing to the archive

        private Thread thread;            // the thread for writing data
        private volatile bool terminated; // necessary to stop the thread
        private string archiveDir;        // the archive directory


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicEAL(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums, 
            ModuleConfig moduleConfig) : base(archiveContext, archiveConfig, cnlNums)
        {
            this.moduleConfig = moduleConfig ?? throw new ArgumentNullException(nameof(moduleConfig));
            options = new BasicEAO(archiveConfig.CustomOptions);
            appLog = archiveContext.Log;
            arcLog = options.LogEnabled ? CreateLog(ModuleUtils.ModuleCode) : null;
            eventQueue = new DataQueue<Event>(options.MaxQueueSize);
            readingAdapter = new EventTableAdapter();
            writingAdapter = new EventTableAdapter();
            tableCache = new MemoryCache<DateTime, EventTable>(CacheExpiration, CacheCapacity);
            readingLock = new object();
            writingLock = new object();

            thread = null;
            terminated = false;
            archiveDir = "";
        }


        /// <summary>
        /// Gets the archive options.
        /// </summary>
        protected override EventArchiveOptions ArchiveOptions => options;

        /// <summary>
        /// Gets the current archive status as text.
        /// </summary>
        public override string StatusText
        {
            get
            {
                return GetStatusText(eventQueue.Stats, eventQueue.Count);
            }
        }


        /// <summary>
        /// Gets the event table from the cache, creating a table if necessary.
        /// </summary>
        private EventTable GetEventTable(DateTime timestamp)
        {
            DateTime tableDate = timestamp.Date;

            EventTable eventTable = tableCache.GetOrCreate(tableDate, () =>
            {
                return new EventTable(tableDate);
            });

            RefreshEvents(eventTable);
            return eventTable;
        }

        /// <summary>
        /// Loads new events from a file if the file has changed.
        /// </summary>
        private void RefreshEvents(EventTable eventTable)
        {
            lock (eventTable)
            {
                if (string.IsNullOrEmpty(eventTable.FileName))
                {
                    eventTable.FileName = Path.Combine(archiveDir,
                        EventTableAdapter.GetTableFileName(Code, eventTable.TableDate));
                }

                DateTime lastWriteTime = File.Exists(eventTable.FileName) ?
                    File.GetLastWriteTimeUtc(eventTable.FileName) : DateTime.MinValue;

                if (lastWriteTime > DateTime.MinValue && lastWriteTime != eventTable.LastWriteTime)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    readingAdapter.FileName = eventTable.FileName;
                    readingAdapter.Fill(eventTable);
                    eventTable.LastWriteTime = lastWriteTime;

                    stopwatch.Stop();
                    arcLog?.WriteAction(Locale.IsRussian ?
                        "Чтение таблицы событий длины {0} успешно завершено за {1} мс" :
                        "Reading an event table of length {0} completed successfully in {1} ms",
                        eventTable.Events.Count, stopwatch.ElapsedMilliseconds);
                }
            }
        }

        /// <summary>
        /// Writes events from the queue.
        /// </summary>
        private void WriteEvents()
        {
            try
            {
                for (int i = 0; i < EventsPerIteration; i++)
                {
                    if (eventQueue.TryDequeueValue(out Event ev))
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        EventTable eventTable = GetEventTable(ev.Timestamp);
                        writingAdapter.FileName = eventTable.FileName;

                        lock (eventTable)
                        {
                            if (eventTable.AddEvent(ev))
                                writingAdapter.AppendEvent(ev); // write new event
                            else if (ev.Ack)
                                writingAdapter.WriteEventAck(ev); // update acknowledgement

                            eventTable.LastWriteTime = File.GetLastWriteTimeUtc(eventTable.FileName);
                            LastWriteTime = eventTable.LastWriteTime;
                        }

                        stopwatch.Stop();
                        arcLog?.WriteAction(ServerPhrases.WritingEventCompleted, stopwatch.ElapsedMilliseconds);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                eventQueue.Stats.HasError = true;
                appLog?.WriteError(ex, ServerPhrases.ArchiveMessage, Code, ServerPhrases.WriteFileError);
                arcLog?.WriteError(ex, ServerPhrases.WriteFileError);
                Thread.Sleep(ScadaUtils.ErrorDelay);
            }
        }

        /// <summary>
        /// Writing loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            while (!terminated)
            {
                WriteEvents();
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }


        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            archiveDir = Path.Combine(moduleConfig.SelectArcDir(options.UseCopyDir), Code);
            Directory.CreateDirectory(archiveDir);
            GetEventTable(DateTime.UtcNow); // preload the current table

            // start thread for writing data
            terminated = false;
            thread = new Thread(Execute);
            thread.Start();
        }

        /// <summary>
        /// Deletes the outdated data from the archive.
        /// </summary>
        public override void DeleteOutdatedData()
        {
            DirectoryInfo arcDirInfo = new DirectoryInfo(archiveDir);

            if (arcDirInfo.Exists)
            {
                lock (readingLock)
                {
                    DateTime minDT = DateTime.UtcNow.AddDays(-options.Retention);
                    string minFileName = EventTableAdapter.GetTableFileName(Code, minDT);
                    appLog.WriteAction(ServerPhrases.DeleteOutdatedData, Code, minDT.ToLocalizedDateString());

                    foreach (FileInfo fileInfo in
                        arcDirInfo.EnumerateFiles(Code + "*", SearchOption.TopDirectoryOnly))
                    {
                        if (string.CompareOrdinal(fileInfo.Name, minFileName) < 0)
                            fileInfo.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        public override Event GetEventByID(long eventID)
        {
            lock (readingLock)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                EventTable eventTable = GetEventTable(ScadaUtils.RetrieveTimeFromID(eventID));
                Event ev;

                lock (eventTable)
                {
                    ev = eventTable.GetEventByID(eventID);
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingEventCompleted, stopwatch.ElapsedMilliseconds);
                return ev;
            }
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        public override List<Event> GetEvents(TimeRange timeRange, DataFilter filter)
        {
            lock (readingLock)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                List<Event> events;
                List<DateTime> dates = new List<DateTime>(EnumerateDates(timeRange));

                // simple cases first
                if (dates.Count == 0)
                {
                    events = new List<Event>();
                }
                else if (dates.Count == 1)
                {
                    EventTable eventTable = GetEventTable(dates[0]);

                    lock (eventTable)
                    {
                        events = new List<Event>(eventTable.SelectEvents(timeRange, filter));
                    }
                }
                else
                {
                    // full case
                    events = new List<Event>();

                    if (filter == null)
                    {
                        foreach (DateTime date in dates)
                        {
                            EventTable eventTable = GetEventTable(date);

                            lock (eventTable)
                            {
                                events.AddRange(eventTable.SelectEvents(timeRange, null));
                            }
                        }
                    }
                    else
                    {
                        int limit = filter.Limit > 0 ? filter.Limit : int.MaxValue;
                        int selectedCount = 0;
                        int addedCount = 0;

                        DataFilter filterCopy = filter.ShallowCopy();
                        filterCopy.Limit = 0;
                        filterCopy.Offset = 0;

                        if (!filter.OriginBegin)
                            dates.Reverse();

                        foreach (DateTime date in dates)
                        {
                            EventTable eventTable = GetEventTable(date);

                            lock (eventTable)
                            {
                                foreach (Event ev in eventTable.SelectEvents(timeRange, filterCopy))
                                {
                                    if (++selectedCount > filter.Offset)
                                    {
                                        events.Add(ev);
                                        addedCount++;

                                        if (addedCount >= limit)
                                            break;
                                    }
                                }
                            }

                            if (addedCount >= limit)
                                break;
                        }

                        if (!filter.OriginBegin)
                            events.Reverse();
                    }
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingEventsCompleted, events.Count, stopwatch.ElapsedMilliseconds);
                return events;
            }
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public override void WriteEvent(Event ev)
        {
            if (TimeInsideRetention(ev.Timestamp, DateTime.UtcNow))
                eventQueue.Enqueue(ev.Timestamp, ev);
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public override void AckEvent(EventAck eventAck)
        {
            lock (writingLock)
            {
                EventTable eventTable = GetEventTable(ScadaUtils.RetrieveTimeFromID(eventAck.EventID));

                lock (eventTable)
                {
                    Event ev = eventTable.GetEventByID(eventAck.EventID);

                    if (ev != null)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        ev.Ack = true;
                        ev.AckTimestamp = eventAck.Timestamp;
                        ev.AckUserID = eventAck.UserID;

                        EventTableAdapter adapter = new EventTableAdapter { FileName = eventTable.FileName };
                        adapter.WriteEventAck(ev);
                        eventTable.LastWriteTime = File.GetLastWriteTimeUtc(eventTable.FileName);
                        LastWriteTime = eventTable.LastWriteTime;

                        stopwatch.Stop();
                        arcLog?.WriteAction(ServerPhrases.AckEventCompleted, stopwatch.ElapsedMilliseconds);
                    }
                }
            }
        }
    }
}
