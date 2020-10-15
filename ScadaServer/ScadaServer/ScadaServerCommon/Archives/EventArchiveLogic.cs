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
 * Module   : ScadaServerCommon
 * Summary  : Represents the base class for event archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
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
        /// Gets the event by ID.
        /// </summary>
        public abstract Event GetEventByID(long eventID);

        /// <summary>
        /// Gets the events.
        /// </summary>
        public abstract List<Event> GetEvents(TimeRange timeRange, DataFilter filter);

        /// <summary>
        /// Writes the event.
        /// </summary>
        public abstract void WriteEvent(Event ev);

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public abstract void AckEvent(long eventID, DateTime timestamp, int userID);
    }
}
