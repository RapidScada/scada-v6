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
 * Summary  : Represents the base class for server module user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Agent;
using Scada.Client;
using Scada.Data.Models;
using Scada.Server.Archives;
using Scada.Server.Config;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Represents the base class for server module user interface.
    /// <para>Представляет базовый класс пользовательского интерфейса серверного модуля.</para>
    /// </summary>
    public abstract class ModuleView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleView()
        {
            BaseDataSet = null;
            AppConfig = null;
            AppDirs = null;
            ScadaClient = null;
            AgentClient = null;
            CanShowProperties = false;
        }


        /// <summary>
        /// Gets the configuration database cache.
        /// </summary>
        public BaseDataSet BaseDataSet { get; }

        /// <summary>
        /// Gets or sets the application configuration.
        /// </summary>
        /// <remarks>Do not modify the configuration.</remarks>
        public ServerConfig AppConfig { get; set; }

        /// <summary>
        /// Gets or sets the application directories.
        /// </summary>
        public ServerDirs AppDirs { get; set; }

        /// <summary>
        /// Gets or sets the client to interact with the server.
        /// </summary>
        /// <remarks>Null allowed.</remarks>
        public ScadaClient ScadaClient { get; set; }

        /// <summary>
        /// Gets or sets the client to interact with the agent.
        /// </summary>
        /// <remarks>Null allowed.</remarks>
        public IAgentClient AgentClient { get; set; }

        /// <summary>
        /// Gets a value indicating whether the module can show a properties dialog box.
        /// </summary>
        public bool CanShowProperties { get; protected set; }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        public abstract string Descr { get; }


        /// <summary>
        /// Indicates whether the module can create an archive of the specified kind.
        /// </summary>
        public virtual bool CanCreateArchive(ArchiveKind kind)
        {
            return false;
        }

        /// <summary>
        /// Creates a new archive user interface of the specified kind.
        /// </summary>
        public virtual ArchiveView CreateArchiveView(ArchiveKind kind)
        {
            return null;
        }

        /// <summary>
        /// Shows a modal dialog box for editing module properties.
        /// </summary>
        public virtual bool ShowProperties()
        {
            return false;
        }
    }
}
