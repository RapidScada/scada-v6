/*
 * Copyright 2025 Rapid Software LLC
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
 * Modified : 2025
 */

using Scada.Data.Models;
using Scada.Data.Queues;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents the base class for archive logic.
    /// <para>Представляет базовый класс логики архива.</para>
    /// </summary>
    /// <remarks>Descendants of this class must be thread-safe.</remarks>
    public abstract class ArchiveLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected ArchiveLogic(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums)
        {
            ArchiveContext = archiveContext ?? throw new ArgumentNullException(nameof(archiveContext));
            ArchiveConfig = archiveConfig ?? throw new ArgumentNullException(nameof(archiveConfig));
            CnlNums = cnlNums ?? throw new ArgumentNullException(nameof(cnlNums));
            LastWriteTime = DateTime.UtcNow;
            Code = archiveConfig.Code;
            Title = ServerUtils.GetArchiveTitle(Code, archiveConfig.Name);
            IsReady = false;
            LastCleanupTime = DateTime.MinValue;
            CleanupPeriod = TimeSpan.FromDays(1);
        }


        /// <summary>
        /// Gets the archive context.
        /// </summary>
        protected IArchiveContext ArchiveContext { get; }

        /// <summary>
        /// Gets the archive configuration.
        /// </summary>
        protected ArchiveConfig ArchiveConfig { get; }

        /// <summary>
        /// Gets the channel numbers processed by the archive.
        /// </summary>
        /// <remarks>Channel numbers are sorted and unique.</remarks>
        protected int[] CnlNums { get; }

        /// <summary>
        /// Gets or sets the time (UTC) when the archive was last written to.
        /// </summary>
        protected DateTime LastWriteTime { get; set; }


        /// <summary>
        /// Gets the archive code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the archive title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the archive is ready for reading and writing.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets or sets the time (UTC) when the archive was last cleaned up.
        /// </summary>
        public DateTime LastCleanupTime { get; set; }

        /// <summary>
        /// Gets the required cleanup period for outdated data.
        /// </summary>
        public TimeSpan CleanupPeriod { get; protected set; }

        /// <summary>
        /// Gets the current archive status as text.
        /// </summary>
        public virtual string StatusText => ArchiveUtils.GetStatusText(IsReady, null, null);


        /// <summary>
        /// Creates a log file of the archive.
        /// </summary>
        protected ILog CreateLog(string moduleCode)
        {
            return new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(ArchiveContext.AppDirs.LogDir, moduleCode + "_" + Code + ".log"),
                CapacityMB = ArchiveContext.AppConfig.GeneralOptions.MaxLogSize
            };
        }


        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public virtual void MakeReady()
        {
        }

        /// <summary>
        /// Closes the archive.
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// Deletes the outdated data from the archive.
        /// </summary>
        public virtual void DeleteOutdatedData()
        {
        }

        /// <summary>
        /// Gets the time (UTC) when the archive was last written to.
        /// </summary>
        public virtual DateTime GetLastWriteTime()
        {
            return LastWriteTime;
        }
    }
}
