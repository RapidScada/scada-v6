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
 * Summary  : Represents the base class for server module logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2020
 */

using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Represents the base class for server module logic.
    /// <para>Представляет базовый класс логики серверного модуля.</para>
    /// </summary>
    public abstract class ModuleLogic
    {
        private IServerContext appContext; // the application context


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleLogic()
        {
            appContext = null;
            Log = null;
        }


        /// <summary>
        /// Gets or sets the module log.
        /// </summary>
        protected ILog Log { get; set; }

        /// <summary>
        /// Gets or sets the application context.
        /// </summary>
        public IServerContext AppContext
        {
            get
            {
                return appContext;
            }
            set
            {
                appContext = value ?? throw new ArgumentNullException();
                Log = appContext.Log;
            }
        }

        /// <summary>
        /// Gets the module code.
        /// </summary>
        public abstract string Code { get; }


        /// <summary>
        /// Creates a new archive of the specified kind.
        /// </summary>
        public virtual ArchiveLogic CreateArchive(ArchiveKind kind)
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
    }
}
