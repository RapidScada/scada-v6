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
 * Summary  : Contains main menu items available to a user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2022
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using Scada.Web.Plugins;
using Scada.Web.Services;
using Scada.Web.TreeView;
using System;
using System.Collections.Generic;

namespace Scada.Web.Users
{
    /// <summary>
    /// Contains main menu items available to a user.
    /// <para>Содержит пункты главного меню, доступные пользователю.</para>
    /// </summary>
    public class UserMenu
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UserMenu()
        {
            MenuItems = new List<MenuItem>();
        }


        /// <summary>
        /// Gets the menu items.
        /// </summary>
        public List<MenuItem> MenuItems { get; }


        /// <summary>
        /// Merges the menu items recursively.
        /// </summary>
        private static void MergeMenuItems(List<MenuItem> existingItems, List<MenuItem> addedItems, int level)
        {
            if (addedItems == null)
                return;

            addedItems.Sort();

            foreach (MenuItem addedItem in addedItems)
            {
                addedItem.Level = level;
                int ind = existingItems.BinarySearch(addedItem);

                if (ind >= 0)
                {
                    // merge
                    MenuItem existingItem = existingItems[ind];

                    if (existingItem.Subitems.Count > 0 && addedItem.Subitems.Count > 0)
                    {
                        // add subitems recursively
                        MergeMenuItems(existingItem.Subitems, addedItem.Subitems, level + 1);
                    }
                    else
                    {
                        // simply add subitems
                        addedItem.Subitems.Sort();
                        existingItem.Subitems.AddRange(addedItem.Subitems);
                        SetMenuItemLevels(addedItem.Subitems, level + 1);
                    }
                }
                else
                {
                    // insert the menu item and its subitems
                    addedItem.Subitems.Sort();
                    existingItems.Insert(~ind, addedItem);
                    SetMenuItemLevels(addedItem.Subitems, level + 1);
                }
            }
        }

        /// <summary>
        /// Sets the nesting levels of the menu items recursively.
        /// </summary>
        private static void SetMenuItemLevels(List<MenuItem> items, int level)
        {
            if (items != null)
            {
                foreach (MenuItem item in items)
                {
                    item.Level = level;
                    SetMenuItemLevels(item.Subitems, level + 1);
                }
            }
        }


        /// <summary>
        /// Initializes the user menu.
        /// </summary>
        public void Init(IWebContext webContext, User user, UserRights userRights)
        {
            if (webContext == null)
                throw new ArgumentNullException(nameof(webContext));
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (userRights == null)
                throw new ArgumentNullException(nameof(userRights));

            try
            {
                // add menu items from plugins
                foreach (PluginLogic pluginLogic in webContext.PluginHolder.EnumeratePlugins())
                {
                    MergeMenuItems(MenuItems, 
                        webContext.PluginHolder.GetUserMenuItems(pluginLogic, user, userRights), 0);
                }

                // add default menu items
                MergeMenuItems(MenuItems, new List<MenuItem>() { MenuItem.FromKnownMenuItem(KnownMenuItem.About) }, 0);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при инициализации меню пользователя" :
                    "Error initializing user menu");
            }
        }
    }
}
