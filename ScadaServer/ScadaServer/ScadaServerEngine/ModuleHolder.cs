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
 * Summary  : Holds active modules and helps to call their methods
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Log;
using Scada.Server.Modules;
using System;
using System.Collections.Generic;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Holds active modules and helps to call their methods.
    /// <para>Содержит активные модули и помогает вызывать их методы.</para>
    /// </summary>
    internal class ModuleHolder
    {
        private static readonly string ErrorInModule = Locale.IsRussian ?
            "Error calling the {0} method of the {1} module" :
            "Ошибка при вызове метода {0} модуля {1}";

        private readonly ILog log;          // the application log
        private readonly object moduleLock; // synchronizes access to the modules


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException("log");
            moduleLock = new object();

            Modules = new List<ModuleLogic>();
            LogicModules = new List<ModuleLogic>();
        }


        /// <summary>
        /// Gets all the modules.
        /// </summary>
        public List<ModuleLogic> Modules { get; }

        /// <summary>
        /// Gets the modules that have only a logical purpose.
        /// </summary>
        public List<ModuleLogic> LogicModules { get; }


        /// <summary>
        /// Calls the OnServiceStart method of the modules.
        /// </summary>
        public void OnServiceStart()
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in Modules)
                {
                    try
                    {
                        moduleLogic.OnServiceStart();
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnServiceStart", moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnServiceStop method of the modules.
        /// </summary>
        public void OnServiceStop()
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in Modules)
                {
                    try
                    {
                        moduleLogic.OnServiceStop();
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnServiceStop", moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnIteration method of the modules.
        /// </summary>
        public void OnIteration()
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in LogicModules)
                {
                    try
                    {
                        moduleLogic.OnIteration();
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnIteration", moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnCurrentDataProcessing method of the modules.
        /// </summary>
        public void OnCurrentDataProcessing(int deviceNum, int[] cnlNums, CnlData[] cnlData)
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in LogicModules)
                {
                    try
                    {
                        moduleLogic.OnCurrentDataProcessing(deviceNum, cnlNums, cnlData);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnCurrentDataProcessing", moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnCurrentDataProcessed method of the modules.
        /// </summary>
        public void OnCurrentDataProcessed(int deviceNum, int[] cnlNums, CnlData[] cnlData)
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in LogicModules)
                {
                    try
                    {
                        moduleLogic.OnCurrentDataProcessed(deviceNum, cnlNums, cnlData);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnCurrentDataProcessed", moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnHistoricalDataProcessing method of the modules.
        /// </summary>
        public void OnHistoricalDataProcessing(int deviceNum, Slice slice)
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in LogicModules)
                {
                    try
                    {
                        moduleLogic.OnHistoricalDataProcessing(deviceNum, slice);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnHistoricalDataProcessing", moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnHistoricalDataProcessed method of the modules.
        /// </summary>
        public void OnHistoricalDataProcessed(int deviceNum, Slice slice)
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in LogicModules)
                {
                    try
                    {
                        moduleLogic.OnHistoricalDataProcessed(deviceNum, slice);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnHistoricalDataProcessed", moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnEvent method of the modules.
        /// </summary>
        public void OnEvent(Event ev)
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in LogicModules)
                {
                    try
                    {
                        moduleLogic.OnEvent(ev);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnEvent", moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnEventAck method of the modules.
        /// </summary>
        public void OnEventAck(long eventID, DateTime timestamp, int userID)
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in LogicModules)
                {
                    try
                    {
                        moduleLogic.OnEventAck(eventID, timestamp, userID);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnEventAck", moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnCommand method of the modules.
        /// </summary>
        public void OnCommand(TeleCommand command, CommandResult commandResult)
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in LogicModules)
                {
                    try
                    {
                        moduleLogic.OnCommand(command, commandResult);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnCommand", moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the ValidateUser method of the modules until a user is handled.
        /// </summary>
        public bool ValidateUser(string username, string password, 
            out int userID, out int roleID, out string errMsg, out bool handled)
        {
            userID = 0;
            roleID = 0;
            errMsg = "";
            handled = false;

            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in LogicModules)
                {
                    try
                    {
                        bool userIsValid = moduleLogic.ValidateUser(username, password, 
                            out userID, out roleID, out errMsg, out handled);

                        if (handled)
                            return userIsValid;
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, "OnCommand", moduleLogic.Code);
                    }
                }
            }

            return false;
        }
    }
}
