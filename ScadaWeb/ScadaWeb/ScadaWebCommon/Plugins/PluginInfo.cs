/*
 * Copyright 2023 Rapid Software LLC
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
 * Summary  : Provides information about a plugin.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Provides information about a plugin.
    /// </summary>
    public abstract class PluginInfo
    {
        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public abstract string Descr { get; }

        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        public virtual string Version
        {
            get
            {
                return GetType().Assembly.GetName().Version.ToString();
            }
        }
    }
}
