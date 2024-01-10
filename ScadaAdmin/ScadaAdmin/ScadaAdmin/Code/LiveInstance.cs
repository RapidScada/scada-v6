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
 * Module   : Administrator
 * Summary  : Provides editing and data exchange with an instance
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.Project;
using Scada.Agent;
using System;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Provides editing and data exchange with an instance.
    /// <para>Обеспечивает редактирование и обмен данными с экземпляром.</para>
    /// </summary>
    internal class LiveInstance
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LiveInstance(ProjectInstance projectInstance)
        {
            ProjectInstance = projectInstance ?? throw new ArgumentNullException(nameof(projectInstance));
            AgentClient = null;
            IsReady = false;
        }


        /// <summary>
        /// Gets the project instance.
        /// </summary>
        public ProjectInstance ProjectInstance { get; private set; }

        /// <summary>
        /// Gets or sets the client of the Agent service.
        /// </summary>
        public IAgentClient AgentClient { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the instance configuration is loaded and 
        /// the agent client is created.
        /// </summary>
        public bool IsReady { get; set; }
    }
}
