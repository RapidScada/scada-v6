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
 * Module   : ScadaServerCommon
 * Summary  : Represents the base class for server module user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Server.Archives;
using Scada.Server.Config;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Represents the base class for server module user interface.
    /// <para>Представляет базовый класс пользовательского интерфейса серверного модуля.</para>
    /// </summary>
    public abstract class ModuleView : LibraryView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleView()
            : base()
        {
            AppConfig = null;
        }


        /// <summary>
        /// Gets or sets the application configuration.
        /// </summary>
        /// <remarks>Do not modify the configuration.</remarks>
        public ServerConfig AppConfig { get; set; }


        /// <summary>
        /// Indicates whether the module can create an archive of the specified kind.
        /// </summary>
        public virtual bool CanCreateArchive(ArchiveKind kind)
        {
            return false;
        }

        /// <summary>
        /// Creates a new archive user interface.
        /// </summary>
        public virtual ArchiveView CreateArchiveView(ArchiveConfig archiveConfig)
        {
            return null;
        }
    }
}
