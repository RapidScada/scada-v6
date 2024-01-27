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
 * Module   : ScadaServerEngine
 * Summary  : Holds active modules and helps to call their methods
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Log;
using Scada.Server.Lang;
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
        private readonly ILog log;                       // the application log
        private readonly List<ModuleLogic> modules;      // all the modules
        private readonly List<ModuleLogic> logicModules; // the modules that have a logical purpose
        private readonly List<ModuleLogic> authModules;  // the modules that have an authorization purpose
        private readonly Dictionary<string, ModuleLogic> moduleMap; // the modules accessed by code
        private readonly object moduleLock;              // synchronizes access to the modules
        private volatile bool serviceStopped;            // ignore method calls after the service is stopped


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            modules = new List<ModuleLogic>();
            logicModules = new List<ModuleLogic>();
            authModules = new List<ModuleLogic>();
            moduleMap = new Dictionary<string, ModuleLogic>();
            moduleLock = new object();
            serviceStopped = false;
        }


        /// <summary>
        /// Adds the specified module to the lists.
        /// </summary>
        public void AddModule(ModuleLogic moduleLogic)
        {
            if (moduleLogic == null)
                throw new ArgumentNullException(nameof(moduleLogic));

            if (moduleMap.ContainsKey(moduleLogic.Code))
                throw new ScadaException("Module already exists.");

            modules.Add(moduleLogic);
            moduleMap.Add(moduleLogic.Code, moduleLogic);

            if (moduleLogic.ModulePurposes.HasFlag(ModulePurposes.Logic))
                logicModules.Add(moduleLogic);

            if (moduleLogic.ModulePurposes.HasFlag(ModulePurposes.Auth))
                authModules.Add(moduleLogic);
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
                        log.WriteError(ex, ServerPhrases.ErrorInModule, nameof(OnServiceStart), moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnServiceStop method of the modules.
        /// </summary>
        public void OnServiceStop()
        {
            serviceStopped = true;

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
                        log.WriteError(ex, ServerPhrases.ErrorInModule, nameof(OnServiceStop), moduleLogic.Code);
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
                        log.WriteError(ex, ServerPhrases.ErrorInModule, nameof(OnIteration), moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnCurrentDataProcessing method of the modules.
        /// </summary>
        public void OnCurrentDataProcessing(Slice slice)
        {
            if (serviceStopped) 
                return;

            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnCurrentDataProcessing(slice);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInModule, 
                            nameof(OnCurrentDataProcessing), moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnCurrentDataProcessed method of the modules.
        /// </summary>
        public void OnCurrentDataProcessed(Slice slice)
        {
            if (serviceStopped)
                return;

            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnCurrentDataProcessed(slice);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInModule, 
                            nameof(OnCurrentDataProcessed), moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnHistoricalDataProcessing method of the modules.
        /// </summary>
        public void OnHistoricalDataProcessing(Slice slice)
        {
            if (serviceStopped)
                return;

            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnHistoricalDataProcessing(slice);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInModule, 
                            nameof(OnHistoricalDataProcessing), moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnHistoricalDataProcessed method of the modules.
        /// </summary>
        public void OnHistoricalDataProcessed(Slice slice)
        {
            if (serviceStopped)
                return;

            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnHistoricalDataProcessed(slice);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInModule, 
                            nameof(OnHistoricalDataProcessed), moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnEvent method of the modules.
        /// </summary>
        public void OnEvent(Event ev)
        {
            if (serviceStopped)
                return;

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
                        log.WriteError(ex, ServerPhrases.ErrorInModule, nameof(OnEvent), moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnEventAck method of the modules.
        /// </summary>
        public void OnEventAck(EventAck eventAck)
        {
            if (serviceStopped)
                return;

            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in logicModules)
                {
                    try
                    {
                        moduleLogic.OnEventAck(eventAck);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInModule, nameof(OnEventAck), moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the OnCommand method of the modules.
        /// </summary>
        public void OnCommand(TeleCommand command, CommandResult commandResult)
        {
            if (serviceStopped)
                return;

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
                        log.WriteError(ex, ServerPhrases.ErrorInModule, nameof(OnCommand), moduleLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the ValidateUser method of the modules until a user is handled.
        /// </summary>
        public UserValidationResult ValidateUser(string username, string password)
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in authModules)
                {
                    try
                    {
                        UserValidationResult result = moduleLogic.ValidateUser(username, password);

                        if (result.Handled)
                            return result;
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInModule, nameof(ValidateUser), moduleLogic.Code);
                    }
                }
            }

            return UserValidationResult.Empty;
        }

        /// <summary>
        /// Calls the FindUser method of the modules until a user is found.
        /// </summary>
        public User FindUser(int userID)
        {
            lock (moduleLock)
            {
                foreach (ModuleLogic moduleLogic in authModules)
                {
                    try
                    {
                        User user = moduleLogic.FindUser(userID);

                        if (user != null)
                            return user;
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInModule, nameof(FindUser), moduleLogic.Code);
                    }
                }
            }

            return null;
        }
    }
}
