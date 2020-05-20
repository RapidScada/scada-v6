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
 * Module   : ScadaServerEngine
 * Summary  : Defines functionality of an event archive
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Defines functionality of an event archive.
    /// <para>Определяет функциональность архива событий.</para>
    /// </summary>
    public interface IEventArchive
    {
        /// <summary>
        /// Gets the required cleanup period for outdated data.
        /// </summary>
        TimeSpan CleanupPeriod { get; }


        /// <summary>
        /// Deletes the outdates data from the archive.
        /// </summary>
        void DeleteOutdatedData();

        /// <summary>
        /// Writes the event.
        /// </summary>
        void WriteEvent(object ev);

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        void AckEvent(long eventID, DateTime timestamp, int userID);
    }
}
