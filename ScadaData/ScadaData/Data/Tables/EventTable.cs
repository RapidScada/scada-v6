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
 * Module   : ScadaData
 * Summary  : Represents a table that contains events
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents a table that contains events.
    /// <para>Представляет таблицу, которая содержит события.</para>
    /// </summary>
    public class EventTable
    {
        /// <summary>
        /// The events accessed by ID.
        /// </summary>
        protected Dictionary<long, Event> eventsByID;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventTable()
        {
            eventsByID = null;
            TableDate = DateTime.MinValue;
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
        /// Gets the events sorted by timestamp.
        /// </summary>
        public List<Event> Events { get; }


        /// <summary>
        /// Clears events before loading new data.
        /// </summary>
        public void BeginLoadData(int capacity)
        {
            eventsByID = null;
            Events.Clear();
            Events.Capacity = capacity;
        }

        /// <summary>
        /// Sorts the events by timestamp and populates the event dictionary to access events by ID.
        /// </summary>
        public void EndLoadData()
        {

        }

        /// <summary>
        /// Gets the event by ID, or null if the specified ID is not found.
        /// </summary>
        public Event GetEventByID(long eventID)
        {
            return eventsByID != null && eventsByID.TryGetValue(eventID, out Event ev) ? ev : null;
        }
    }
}
