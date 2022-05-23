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
 * Summary  : Defines functionality to access the archive environment
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Config;
using Scada.Log;
using Scada.Server.Config;
using System.Collections.Generic;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Defines functionality to access the archive environment.
    /// <para>Определяет функциональность для доступа к окружению архива.</para>
    /// </summary>
    public interface IArchiveContext
    {
        /// <summary>
        /// Gets the instance configuration.
        /// </summary>
        InstanceConfig InstanceConfig { get; }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        ServerConfig AppConfig { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        ServerDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        ILog Log { get; }

        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        ConfigDatabase ConfigDatabase { get; }

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        IDictionary<string, object> SharedData { get; }
    }
}
