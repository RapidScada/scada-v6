/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Represents the base class for server module logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2022
 */

using Scada.Data.Models;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Storages;
using System;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Represents the base class for server module logic.
    /// <para>Представляет базовый класс логики серверного модуля.</para>
    /// </summary>
    public abstract class ModuleLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleLogic(IServerContext serverContext)
        {
            ServerContext = serverContext ?? throw new ArgumentNullException(nameof(serverContext));
            AppDirs = serverContext.AppDirs;
            Storage = serverContext.Storage;
            Log = serverContext.Log;
        }


        /// <summary>
        /// Gets the server context.
        /// </summary>
        protected IServerContext ServerContext { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        protected ServerDirs AppDirs { get; }

        /// <summary>
        /// Gets the application storage.
        /// </summary>
        public IStorage Storage { get; }

        /// <summary>
        /// Gets or sets the module log.
        /// </summary>
        protected ILog Log { get; set; }

        /// <summary>
        /// Gets the module code.
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// Gets the module version.
        /// </summary>
        public virtual string Version
        {
            get
            {
                return GetType().Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets the module purposes.
        /// </summary>
        public virtual ModulePurposes ModulePurposes
        {
            get
            {
                return ModulePurposes.Logic;
            }
        }


        /// <summary>
        /// Creates a new archive logic.
        /// </summary>
        public virtual ArchiveLogic CreateArchive(IArchiveContext archiveContext, ArchiveConfig archiveConfig, 
            int[] cnlNums)
        {
            return null;
        }

        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public virtual void OnServiceStart()
        {
        }

        /// <summary>
        /// Performs actions when the service stops.
        /// </summary>
        public virtual void OnServiceStop()
        {
        }

        /// <summary>
        /// Performs actions on a new iteration of the main operating cycle.
        /// </summary>
        public virtual void OnIteration()
        {
        }

        /// <summary>
        /// Performs actions after receiving and before processing new current data.
        /// </summary>
        /// <remarks>In general, channel numbers are not sorted.</remarks>
        public virtual void OnCurrentDataProcessing(int[] cnlNums, CnlData[] cnlData, int deviceNum)
        {
        }

        /// <summary>
        /// Performs actions after receiving and processing new current data.
        /// </summary>
        /// <remarks>In general, channel numbers are not sorted.</remarks>
        public virtual void OnCurrentDataProcessed(int[] cnlNums, CnlData[] cnlData, int deviceNum)
        {
        }

        /// <summary>
        /// Performs actions after receiving and before processing new historical data.
        /// </summary>
        public virtual void OnHistoricalDataProcessing(Slice slice, int deviceNum)
        {
        }

        /// <summary>
        /// Performs actions after receiving and processing new historical data.
        /// </summary>
        public virtual void OnHistoricalDataProcessed(Slice slice, int deviceNum)
        {
        }

        /// <summary>
        /// Performs actions after creating and before writing an event.
        /// </summary>
        public virtual void OnEvent(Event ev)
        {
        }

        /// <summary>
        /// Performs actions when acknowledging an event.
        /// </summary>
        public virtual void OnEventAck(long eventID, DateTime timestamp, int userID)
        {
        }

        /// <summary>
        /// Performs actions after receiving and before enqueuing a telecontrol command.
        /// </summary>
        public virtual void OnCommand(TeleCommand command, CommandResult commandResult)
        {
        }

        /// <summary>
        /// Validates the username and password.
        /// </summary>
        public virtual bool ValidateUser(string username, string password,
            out int userID, out int roleID, out string errMsg, out bool handled)
        {
            userID = 0;
            roleID = 0;
            errMsg = "";
            handled = false;
            return false;
        }
    }
}
