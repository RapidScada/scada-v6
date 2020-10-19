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

        private readonly ILog log;                       // the application log
        private readonly List<ModuleLogic> modules;      // all the modules
        private readonly List<ModuleLogic> logicModules; // the modules that have only a logical purpose
        private readonly Dictionary<string, ModuleLogic> moduleMap; // the modules accessed by code
        private readonly object moduleLock;              // synchronizes access to the modules


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            modules = new List<ModuleLogic>();
            logicModules = new List<ModuleLogic>();
            moduleMap = new Dictionary<string, ModuleLogic>();
            moduleLock = new object();
        }


        /// <summary>
        /// Adds the specified module to the lists.
        /// </summary>
        public void AddModule(ModuleLogic moduleLogic)
        {
            if (moduleLogic == null)
                throw new ArgumentNullException(nameof(moduleLogic));

            modules.Add(moduleLogic);
            moduleMap[moduleLogic.Code] = moduleLogic;

            if (moduleLogic.ModulePurposes.HasFlag(ModulePurposes.Logic))
                logicModules.Add(moduleLogic);
        }

        /// <summary>
        /// Gets the module by code.
        /// </summary>
        public bool GetModule(string moduleCode, out ModuleLogic moduleLogic)
        {
            return moduleMap.TryGetValue(moduleCode, out moduleLogic);
        }

        /// <summary>
        /// Calls the OnServiceStart method of the modules.
        /// </summary>
        public void OnServiceStart()
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in modules)
                {
                    try
                    {
                        moduleLogic.OnServiceStart();
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, nameof(OnServiceStart), moduleLogic.Code);
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
                foreach (ModuleLogic moduleLogic in modules)
                {
                    try
                    {
                        moduleLogic.OnServiceStop();
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, nameof(OnServiceStop), moduleLogic.Code);
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
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnIteration();
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, nameof(OnIteration), moduleLogic.Code);
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
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnCurrentDataProcessing(deviceNum, cnlNums, cnlData);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, nameof(OnCurrentDataProcessing), moduleLogic.Code);
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
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnCurrentDataProcessed(deviceNum, cnlNums, cnlData);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, nameof(OnCurrentDataProcessed), moduleLogic.Code);
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
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnHistoricalDataProcessing(deviceNum, slice);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, nameof(OnHistoricalDataProcessing), moduleLogic.Code);
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
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnHistoricalDataProcessed(deviceNum, slice);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, nameof(OnHistoricalDataProcessed), moduleLogic.Code);
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
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnEvent(ev);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, nameof(OnEvent), moduleLogic.Code);
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
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnEventAck(eventID, timestamp, userID);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, nameof(OnEventAck), moduleLogic.Code);
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
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnCommand(command, commandResult);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInModule, nameof(OnCommand), moduleLogic.Code);
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
                foreach (ModuleLogic moduleLogic in logicModules)
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
                        log.WriteException(ex, ErrorInModule, nameof(ValidateUser), moduleLogic.Code);
                    }
                }
            }

            return false;
        }
    }
}
