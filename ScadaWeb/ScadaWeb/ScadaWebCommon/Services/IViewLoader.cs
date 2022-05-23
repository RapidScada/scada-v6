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
 * Module   : ScadaWebCommon
 * Summary  : Defines functionality to load views available to the current user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Models;
using Scada.Web.Plugins;

namespace Scada.Web.Services
{
    /// <summary>
    /// Defines functionality to load views available to the current user.
    /// <para>Определяет функциональность для загрузки представлений, доступных текущему пользователю.</para>
    /// </summary>
    public interface IViewLoader
    {
        /// <summary>
        /// Gets the view specification.
        /// </summary>
        bool GetViewSpec(int viewID, out ViewSpec viewSpec, out string errMsg);

        /// <summary>
        /// Gets a view from the server or cache.
        /// </summary>
        bool GetView<T>(int viewID, out T view, out string errMsg) where T : ViewBase;

        /// <summary>
        /// Gets a view from the cache.
        /// </summary>
        bool GetViewFromCache(int viewID, out ViewBase view, out string errMsg);
    }
}
