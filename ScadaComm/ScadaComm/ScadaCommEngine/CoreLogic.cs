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
 * Summary  : Implements of the core Communicator logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Config;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Implements of the core Communicator logic.
    /// <para>Реализует основную логику Коммуникатора.</para>
    /// </summary>
    internal class CoreLogic
    {
        /// <summary>
        /// The period of writing application info.
        /// </summary>
        private static readonly TimeSpan WriteInfoPeriod = TimeSpan.FromSeconds(1);

        private readonly string infoFileName;    // the full file name to write application information

        private Thread thread;                   // the working thread of the logic
        private volatile bool terminated;        // necessary to stop the thread
        private DateTime utcStartDT;             // the UTC start time
        private DateTime startDT;                // the local start time
        private ServiceStatus serviceStatus;     // the current service status

        private List<CommLine> commLines; // the active communication lines


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CoreLogic(CommConfig config, CommDirs appDirs, ILog log)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            AppDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            Log = log ?? throw new ArgumentNullException(nameof(log));
            SharedData = null;

            infoFileName = Path.Combine(appDirs.LogDir, CommUtils.InfoFileName);

            thread = null;
            terminated = false;
            utcStartDT = DateTime.MinValue;
            startDT = DateTime.MinValue;
            serviceStatus = ServiceStatus.Undefined;

            commLines = null;
        }


        /// <summary>
        /// Gets the Communicator configuration.
        /// </summary>
        public CommConfig Config { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public CommDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log { get; }

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        public SortedDictionary<string, object> SharedData { get; private set; }


        /// <summary>
        /// Operating cycle running in a separate thread.
        /// </summary>
        private void Execute()
        {

        }


        /// <summary>
        /// Starts processing logic.
        /// </summary>
        public bool StartProcessing()
        {
            return false;
        }

        /// <summary>
        /// Stops processing logic.
        /// </summary>
        public void StopProcessing()
        {

        }
    }
}
