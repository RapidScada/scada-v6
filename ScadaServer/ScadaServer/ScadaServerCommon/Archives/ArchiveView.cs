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
 * Summary  : Represents the base class for archive user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Server.Config;
using Scada.Server.Modules;
using System;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents the base class for archive user interface.
    /// <para>Представляет базовый класс пользовательского интерфейса архива.</para>
    /// </summary>
    public abstract class ArchiveView : LibraryView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveView(ModuleView parentView, ArchiveConfig archiveConfig)
            : base(parentView)
        {
            AppConfig = parentView.AppConfig;
            ArchiveConfig = archiveConfig ?? throw new ArgumentNullException(nameof(archiveConfig));
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        /// <remarks>Do not modify the configuration.</remarks>
        public ServerConfig AppConfig { get; }

        /// <summary>
        /// Gets the archive configuration.
        /// </summary>
        public ArchiveConfig ArchiveConfig { get; }
    }
}
