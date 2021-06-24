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
 * Module   : ScadaWebCommon
 * Summary  : Defines functionality to load views by a current user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Web.Plugins;

namespace Scada.Web.Services
{
    /// <summary>
    /// Defines functionality to load views by a current user.
    /// <para>Определяет функциональность для загрузки представлений текущим пользователем.</para>
    /// </summary>
    interface IViewLoader
    {
        /// <summary>
        /// Gets the view specification.
        /// </summary>
        bool GetViewSpec(int viewID, out ViewSpec viewSpec, out string errMsg);

        /// <summary>
        /// Gets a view from the server or cache.
        /// </summary>
        bool GetView<T>(int viewID, out T view, out string errMsg) where T : BaseView;

        /// <summary>
        /// Gets a view from the cache.
        /// </summary>
        bool GetViewFromCache(int viewID, out BaseView view, out string errMsg);
    }
}
