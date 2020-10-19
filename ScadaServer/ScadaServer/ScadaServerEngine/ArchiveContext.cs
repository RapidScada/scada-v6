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
 * Summary  : Implements the server context interface for accessing the archive environment
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using System;
using System.Collections.Generic;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Implements the server context interface for accessing the archive environment.
    /// <para>Реализует интерфейс контекста сервера для доступа к окружению архива.</para>
    /// </summary>
    internal class ArchiveContext : IArchiveContext
    {
        private readonly CoreLogic coreLogic; // the server logic instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveContext(CoreLogic coreLogic, IDictionary<string, object> sharedData)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException(nameof(coreLogic));
            SharedData = sharedData ?? throw new ArgumentNullException(nameof(sharedData));
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public ServerConfig AppConfig => coreLogic.Config;

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public ServerDirs AppDirs => coreLogic.AppDirs;

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log => coreLogic.Log;

        /// <summary>
        /// Gets the configuration database cache.
        /// </summary>
        public BaseDataSet BaseDataSet => coreLogic.BaseDataSet;

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        public IDictionary<string, object> SharedData { get; }
    }
}
