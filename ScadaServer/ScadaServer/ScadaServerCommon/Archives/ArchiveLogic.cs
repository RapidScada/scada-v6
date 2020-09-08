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
 * Summary  : Represents the base class for archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Server.Config;
using System;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents the base class for archive logic.
    /// <para>Представляет базовый класс логики архива.</para>
    /// </summary>
    public abstract class ArchiveLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected ArchiveLogic(ArchiveConfig archiveConfig, int[] cnlNums)
        {
            ArchiveConfig = archiveConfig ?? throw new ArgumentNullException("archiveConfig");
            CnlNums = cnlNums ?? throw new ArgumentNullException("archiveConfig");
            LastWriteTime = DateTime.MinValue;
            LastCleanupTime = DateTime.MinValue;
            CleanupPeriod = TimeSpan.FromDays(1);
        }


        /// <summary>
        /// Gets the archive configuration.
        /// </summary>
        protected ArchiveConfig ArchiveConfig { get; }

        /// <summary>
        /// Gets the input channel numbers processed by the archive.
        /// </summary>
        /// <remarks>Channel numbers are sorted and unique.</remarks>
        protected int[] CnlNums { get; }

        /// <summary>
        /// Gets the archive code.
        /// </summary>
        public string Code
        {
            get
            {
                return ArchiveConfig.Code;
            }
        }

        /// <summary>
        /// Gets the time (UTC) when when the archive was last written to.
        /// </summary>
        public DateTime LastWriteTime { get; protected set; }

        /// <summary>
        /// Gets or sets the time (UTC) when when the archive was last cleaned up.
        /// </summary>
        public DateTime LastCleanupTime { get; set; }

        /// <summary>
        /// Gets the required cleanup period for outdated data.
        /// </summary>
        public TimeSpan CleanupPeriod { get; protected set; }


        /// <summary>
        /// Deletes the outdated data from the archive.
        /// </summary>
        public virtual void DeleteOutdatedData()
        {
        }
    }
}
