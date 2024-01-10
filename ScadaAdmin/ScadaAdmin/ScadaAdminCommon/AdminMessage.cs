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
 * Module   : ScadaAdminCommon
 * Summary  : Specifies the messages sent and received by the Administrator application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

namespace Scada.Admin
{
    /// <summary>
    /// Specifies the messages sent and received by the Administrator application.
    /// <para>Задает сообщения, отправляемые и принимаемые приложением Администратор.</para>
    /// </summary>
    public static class AdminMessage
    {
        /// <summary>
        /// Notifies an editor form to refresh data.
        /// </summary>
        public const string RefreshData = "Admin.RefreshData";

        /// <summary>
        /// Notifies an editor form that the name of an open file changes.
        /// </summary>
        public const string UpdateFileName = "Admin.UpdateFileName";

        /// <summary>
        /// Causes a statistics form to get a new Agent client.
        /// </summary>
        public const string UpdateAgentClient = "Admin.UpdateAgentClient";

        /// <summary>
        /// Creates a new project.
        /// </summary>
        public const string NewProject = "Admin.NewProject";

        /// <summary>
        /// Opens the specified project.
        /// </summary>
        public const string OpenProject = "Admin.OpenProject";
    }
}
