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
 * Module   : ScadaCommon
 * Summary  : Represents an application storage environment
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Config;
using Scada.Log;

namespace Scada.Storages
{
    /// <summary>
    /// Represents an application storage environment.
    /// <para>Представляет окружение хранилища приложения.</para>
    /// </summary>
    /// <remarks>Use the required and init keywords in C# 11.</remarks>
    public class StorageContext
    {
        /// <summary>
        /// Gets or sets the instance configuration.
        /// </summary>
        public InstanceConfig InstanceConfig { get; set; }

        /// <summary>
        /// Gets or sets the current application.
        /// </summary>
        public ServiceApp App { get; set; }

        /// <summary>
        /// Gets or sets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; set; }

        /// <summary>
        /// Gets or sets the application log.
        /// </summary>
        public ILog Log { get; set; }
    }
}
