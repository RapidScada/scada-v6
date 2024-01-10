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
 * Summary  : Defines functionality of a deployment form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Admin.Deployment;
using Scada.Client;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Defines functionality of a deployment form.
    /// <para>Определяет функциональность формы развёртывания.</para>
    /// </summary>
    public interface IDeploymentForm
    {
        /// <summary>
        /// Gets a value indicating whether the selected profile changed.
        /// </summary>
        bool ProfileChanged { get; }

        /// <summary>
        /// Gets a value indicating whether the Agent connection options were modified.
        /// </summary>
        bool ConnectionModified { get; }


        /// <summary>
        /// Determines whether two specified profiles have the different names.
        /// </summary>
        static bool ProfilesDifferent(DeploymentProfile profile1, DeploymentProfile profile2)
        {
            if (profile1 == profile2)
                return false;
            else if (profile1 == null || profile2 == null)
                return true;
            else
                return profile1.Name != profile2.Name;
        }

        /// <summary>
        /// Determines whether two specified profiles have the different Agent connections.
        /// </summary>
        static bool ConnectionsDifferent(DeploymentProfile profile1, DeploymentProfile profile2)
        {
            if (profile1 == profile2)
            {
                return false;
            }
            else if (profile1 == null || profile2 == null)
            {
                return true;
            }
            else
            {
                return !ConnectionOptions.Equals(
                    profile1.AgentEnabled ? profile1.AgentConnectionOptions : null,
                    profile2.AgentEnabled ? profile2.AgentConnectionOptions : null);
            }
        }
    }
}
