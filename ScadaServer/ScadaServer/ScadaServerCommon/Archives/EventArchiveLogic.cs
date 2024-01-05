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
 * Module   : ScadaServerCommon
 * Summary  : Represents the base class for event archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Server.Config;
using System;
using System.Collections.Generic;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents the base class for event archive logic.
    /// <para>Представляет базовый класс логики архива событий.</para>
    /// </summary>
    /// <remarks>Descendants of this class must be thread-safe.</remarks>
    public abstract class EventArchiveLogic : ArchiveLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventArchiveLogic(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums)
            : base(archiveContext, archiveConfig, cnlNums)
        {
        }


        /// <summary>
        /// Gets the archive options.
        /// </summary>
        protected virtual EventArchiveOptions ArchiveOptions => null;


        /// <summary>
        /// Checks that the timestamp is inside the retention period.
        /// </summary>
        protected bool TimeInsideRetention(DateTime timestamp, DateTime now)
        {
            return ArchiveOptions != null && now.AddDays(-ArchiveOptions.Retention) <= timestamp;
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        public abstract Event GetEventByID(long eventID);

        /// <summary>
        /// Gets the events ordered by timestamp.
        /// </summary>
        public abstract List<Event> GetEvents(TimeRange timeRange, DataFilter filter);

        /// <summary>
        /// Writes the event.
        /// </summary>
        public abstract void WriteEvent(Event ev);

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public abstract void AckEvent(EventAck eventAck);
    }
}
