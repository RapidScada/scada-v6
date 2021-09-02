/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : ScadaAdminCommon
 * Summary  : Defines functionality to access the Administrator environment
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Admin.Project;
using Scada.Agent;

namespace Scada.Admin.Extensions
{
    /// <summary>
    /// Defines functionality to access the Administrator environment.
    /// <para>Определяет функциональность для доступа к окружению Администратора.</para>
    /// </summary>
    public interface IAdminContext
    {
        /// <summary>
        /// Get the project currently open.
        /// </summary>
        public ScadaProject CurrentProject { get; }

        /// <summary>
        /// Gets the current Agent client.
        /// </summary>
        public IAgentClient CurrentAgent { get; }
    }
}
