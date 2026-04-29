/*
 * Copyright 2025 Rapid Software LLC
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
 * Summary  : Represents renderer options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2025
 * Modified : 2025
 */

namespace Scada.Web.TreeView
{
    /// <summary>
    /// Represents renderer options.
    /// <para>Представляет параметры рендерера.</para>
    /// </summary>
    public class TreeViewRendererOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether expanders are located to the left.
        /// </summary>
        public bool ExpanderLeft { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to show node icons.
        /// </summary>
        public bool ShowIcons { get; set; } = false;

        /// <summary>
        /// Gets or sets the folder icon URL.
        /// </summary>
        public string FolderIconUrl { get; set; } = "";

        /// <summary>
        /// Gets or sets the default node icon URL.
        /// </summary>
        public string NodeIconUrl { get; set; } = "";
    }
}
