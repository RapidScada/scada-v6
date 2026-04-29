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
 * Summary  : Specifies the common keys to pass data from a page model to its view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2025
 * Modified : 2025
 */

namespace Scada.Web.Pages
{
    /// <summary>
    /// Specifies the common keys to pass data from a page model to its view.
    /// <para>Задаёт общие ключи для передачи данных из модели страницы в её представление.</para>
    /// </summary>
    public static class ViewDataKey
    {
        /// <summary>
        /// Page title.
        /// </summary>
        public const string Title = nameof(Title);

        /// <summary>
        /// CSS class of a body HTML element.
        /// </summary>
        public const string BodyCssClass = nameof(BodyCssClass);

        /// <summary>
        /// View ID to select a menu item in the view explorer.
        /// </summary>
        public const string SelectedViewID = nameof(SelectedViewID);

        /// <summary>
        /// URL to select a menu item in the main menu.
        /// </summary>
        public const string SelectedURL = nameof(SelectedURL);
    }
}
