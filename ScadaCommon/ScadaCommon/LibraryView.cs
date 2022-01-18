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
 * Module   : ScadaCommon
 * Summary  : Represents the base class for library user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Agent;
using Scada.Client;
using Scada.Data.Models;
using System;

namespace Scada
{
    /// <summary>
    /// Represents the base class for library user interface.
    /// <para>Представляет базовый класс пользовательского интерфейса библиотеки.</para>
    /// </summary>
    public abstract class LibraryView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LibraryView()
        {
            BaseDataSet = null;
            AppDirs = null;
            AgentClient = null;
            CanShowProperties = false;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LibraryView(LibraryView parentView)
        {
            if (parentView == null)
                throw new ArgumentNullException(nameof(parentView));

            BaseDataSet = parentView.BaseDataSet;
            AppDirs = parentView.AppDirs;
            AgentClient = parentView.AgentClient;
        }


        /// <summary>
        /// Gets or sets the configuration database cache.
        /// </summary>
        public BaseDataSet BaseDataSet { get; set; }

        /// <summary>
        /// Gets or sets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; set; }

        /// <summary>
        /// Gets or sets the client to interact with the agent.
        /// </summary>
        /// <remarks>Can be null.</remarks>
        public IAgentClient AgentClient { get; set; }

        /// <summary>
        /// Gets a value indicating whether the module can show a properties dialog box.
        /// </summary>
        public bool CanShowProperties { get; protected set; }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public virtual void LoadDictionaries()
        {
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
