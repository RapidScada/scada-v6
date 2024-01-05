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
 * Summary  : Represents a table that contains events
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Models;
using System;
using System.Collections.Generic;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents a table that contains events.
    /// <para>Представляет таблицу, которая содержит события.</para>
    /// </summary>
    public class EventTable
    {
        /// <summary>
        /// Defines a method that compares two events by timestamp.
        /// </summary>
        protected class EventTimeComparer : IComparer<Event>
        {
            /// <summary>
            /// Compares two events.
            /// </summary>
            public int Compare(Event x, Event y)
            {
                DateTime t1 = x == null ? DateTime.MinValue : x.Timestamp;
                DateTime t2 = y == null ? DateTime.MinValue : y.Timestamp;
                return DateTime.Compare(t1, t2);
            }
        }


        /// <summary>
        /// Compares events by timestamp.
        /// </summary>
        protected static readonly EventTimeComparer EventComparer = new EventTimeComparer();
        /// <summary>
        /// The event object to use for searching.
        /// </summary>
        protected static readonly Event EventToFind = new Event();

        /// <summary>
        /// The events accessed by ID.
        /// </summary>
        protected Dictionary<long, Event> eventsByID;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventTable()
        {
            eventsByID = new Dictionary<long, Event>();
            TableDate = DateTime.MinValue;
            FileName = "";
            LastWriteTime = DateTime.MinValue;
            Events = new List<Event>();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventTable(DateTime tableDate)
            : this()
        {
            TableDate = tableDate;
        }


        /// <summary>
        /// Gets the date of the data stored in the table.
        /// </summary>
        public DateTime TableDate { get; }

        /// <summary>
        /// Gets or sets the full file name of the table.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the time (UTC) when the table file was last written to.
        /// </summary>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// Gets the events sorted by timestamp.
        /// </summary>
        public List<Event> Events { get; }


        /// <summary>
        /// Finds the index of the event with the specified timestamp, 
        /// or finds the insertion index if no such event exists.
        /// </summary>
        protected int FindEventIndex(DateTime timestamp)
        {
            EventToFind.Timestamp = timestamp;
            int index = Events.BinarySearch(EventToFind, EventComparer);
            return index >= 0 ? index : ~index;
        }

        /// <summary>
        /// Clears events before loading new data.
        /// </summary>
        public void BeginLoadData()
        {
            eventsByID = null;
            Events.Clear();
        }

        /// <summary>
        /// Sorts the events by timestamp and populates the event dictionary to access events by ID.
        /// </summary>
        public void EndLoadData()
        {
            Events.Sort(EventComparer);
            eventsByID = new Dictionary<long, Event>(Events.Count);
            Events.ForEach(ev => eventsByID.Add(ev.EventID, ev));
        }

        /// <summary>
        /// Gets the event by ID, or null if the specified ID is not found.
        /// </summary>
        public Event GetEventByID(long eventID)
        {
            return eventsByID != null && eventsByID.TryGetValue(eventID, out Event ev) ? ev : null;
        }

        /// <summary>
        /// Selects the events that match the specified filter.
        /// </summary>
        public IEnumerable<Event> SelectEvents(TimeRange timeRange, DataFilter filter)
        {
            if (Events.Count == 0)
                yield break;

            DateTime startTime = timeRange.StartTime;
            DateTime endTime = timeRange.EndTime;
            bool endInclusive = timeRange.EndInclusive;
            DateTime minEventTime = Events[0].Timestamp;
            DateTime maxEventTime = Events[Events.Count - 1].Timestamp;

            if (filter == null)
            {
                if (startTime <= minEventTime && (maxEventTime < endTime || maxEventTime == endTime && endInclusive))
                {
                    // select all events
                    foreach (Event ev in Events)
                    {
                        yield return ev;
                    }
                }
                else
                {
                    // select all events for the specified period
                    int startIndex = startTime <= minEventTime ? 0 : FindEventIndex(startTime);

                    for (int i = startIndex, cnt = Events.Count; i < cnt; i++)
                    {
                        Event ev = Events[i];
                        DateTime timestamp = ev.Timestamp;

                        if (startTime <= timestamp)
                        {
                            if (timestamp < endTime || timestamp == endTime && endInclusive)
                                yield return ev;
                            else
                                break;
                        }
                    }
                }
            }
            else
            {
                int limit = filter.Limit > 0 ? filter.Limit : int.MaxValue;
                int satisfiedCount = 0;
                int addedCount = 0;

                if (filter.OriginBegin)
                {
                    // select filtered events for the specified period
                    int startIndex = startTime <= minEventTime ? 0 : FindEventIndex(startTime);

                    for (int i = startIndex, cnt = Events.Count; i < cnt; i++)
                    {
                        Event ev = Events[i];
                        DateTime timestamp = ev.Timestamp;

                        if (startTime <= timestamp)
                        {
                            if (timestamp < endTime || timestamp == endTime && endInclusive)
                            {
                                if (filter.IsSatisfied(ev) && ++satisfiedCount > filter.Offset)
                                {
                                    yield return ev;
                                    addedCount++;

                                    if (addedCount >= limit)
                                        break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // select filtered events for the specified period from the end of the table in reverse order
                    int endIndex = maxEventTime <= endTime ? Events.Count - 1 : FindEventIndex(endTime);

                    for (int i = endIndex; i >= 0; i--)
                    {
                        Event ev = Events[i];
                        DateTime timestamp = ev.Timestamp;

                        if (timestamp < endTime || timestamp == endTime && endInclusive)
                        {
                            if (startTime <= timestamp)
                            {
                                if (filter.IsSatisfied(ev) && ++satisfiedCount > filter.Offset)
                                {
                                    yield return ev;
                                    addedCount++;

                                    if (addedCount >= limit)
                                        break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified event to the table, keeping the events sorted.
        /// </summary>
        public bool AddEvent(Event ev)
        {
            if (ev == null)
                throw new ArgumentNullException(nameof(ev));

            if (eventsByID == null)
            {
                return false;
            }
            else if (eventsByID.TryGetValue(ev.EventID, out Event existingEvent))
            {
                ev.Position = existingEvent.Position;
                return false;
            }
            else
            {
                int index = Events.BinarySearch(ev, EventComparer);
                if (index < 0)
                    index = ~index;

                Events.Insert(index, ev);
                eventsByID.Add(ev.EventID, ev);
                return true;
            }
        }
    }
}
