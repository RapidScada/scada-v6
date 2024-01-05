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
 * Module   : ScadaWebCommon
 * Summary  : Represents plugins that implement specific features
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Represents plugins that implement specific features.
    /// <para>Представляет плагины, реализующие определенные функции.</para>
    /// </summary>
    public class FeaturedPlugins
    {
        /// <summary>
        /// Gets or sets the chart plugin.
        /// </summary>
        public PluginLogic ChartPlugin { get; set; }

        /// <summary>
        /// Gets or sets the command plugin.
        /// </summary>
        public PluginLogic CommandPlugin { get; set; }

        /// <summary>
        /// Gets or sets the event acknowledgement plugin.
        /// </summary>
        public PluginLogic EventAckPlugin { get; set; }

        /// <summary>
        /// Gets or sets the user management plugin.
        /// </summary>
        public PluginLogic UserManagementPlugin { get; set; }

        /// <summary>
        /// Gets or sets the notification plugin.
        /// </summary>
        public PluginLogic NotificationPlugin { get; set; }
    }
}
