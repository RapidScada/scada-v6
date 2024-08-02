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
 * Modified : 2024
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
        /// Notifies a child form that the configuration database has been reloaded.
        /// </summary>
        public const string BaseReload = "Admin.BaseReload";

        /// <summary>
        /// Notifies a child form that data needs to be refreshed.
        /// </summary>
        public const string RefreshData = "Admin.RefreshData";

        /// <summary>
        /// Notifies an editor form that the name of an open file has changed.
        /// </summary>
        public const string UpdateFileName = "Admin.UpdateFileName";

        /// <summary>
        /// Tells a statistics form to get a new Agent client.
        /// </summary>
        public const string UpdateAgentClient = "Admin.UpdateAgentClient";

        /// <summary>
        /// Tells the application to create a new project.
        /// </summary>
        public const string NewProject = "Admin.NewProject";

        /// <summary>
        /// Tells the application to open the specified project.
        /// Message arguments: string Path
        /// </summary>
        public const string OpenProject = "Admin.OpenProject";
    }
}
