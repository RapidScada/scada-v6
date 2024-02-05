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
 * Summary  : Contains reports available to a user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2023
 */

using Scada.Data.Entities;
using Scada.Lang;
using Scada.Web.Plugins;
using Scada.Web.Services;
using Scada.Web.TreeView;

namespace Scada.Web.Users
{
    /// <summary>
    /// Contains reports available to a user.
    /// <para>Содержит отчёты, доступные пользователю.</para>
    /// </summary>
    public class UserReports : UserMenu
    {
        /// <summary>
        /// Initializes the user reports.
        /// </summary>
        public override void Init(IWebContext webContext, User user, UserRights userRights)
        {
            ArgumentNullException.ThrowIfNull(webContext, nameof(webContext));
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(userRights, nameof(userRights));

            try
            {
                foreach (PluginLogic pluginLogic in webContext.PluginHolder.EnumeratePlugins())
                {
                    MergeMenuItems(MenuItems,
                        webContext.PluginHolder.GetUserReports(pluginLogic, user, userRights), 0);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при инициализации отчётов пользователя" :
                    "Error initializing user reports");
            }
        }

        /// <summary>
        /// Flattens the report hierarchy.
        /// </summary>
        public List<MenuItem> ToFlatList()
        {
            List<MenuItem> reportItems = new();

            void AddItems(List<MenuItem> items)
            {
                foreach (MenuItem menuItem in items)
                {
                    reportItems.Add(menuItem);
                    AddItems(menuItem.Subitems);
                }
            }

            AddItems(MenuItems);
            return reportItems;
        }
    }
}
