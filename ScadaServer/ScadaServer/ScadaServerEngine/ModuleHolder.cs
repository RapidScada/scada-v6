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

        private readonly ILog log; // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException("log");
            Modules = new List<ModuleLogic>();
        }


        /// <summary>
        /// Gets the modules.
        /// </summary>
        public List<ModuleLogic> Modules { get; private set; }


        /// <summary>
        /// Calls the OnServiceStart method of the modules.
        /// </summary>
        public void CallOnServiceStart()
        {
            lock (Modules)
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
        public void CallOnServiceStop()
        {
            lock (Modules)
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
    }
}
