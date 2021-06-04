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
 * Summary  : Contains main menu items available to a user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
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
        /// Initializes the user menu.
        /// </summary>
        public void Init(ILog log, BaseDataSet baseDataSet, UserRights userRights)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));
            if (baseDataSet == null)
                throw new ArgumentNullException(nameof(baseDataSet));
            if (userRights == null)
                throw new ArgumentNullException(nameof(userRights));

            try
            {
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при инициализации меню пользователя" :
                    "Error initializing user menu");
            }
        }
    }
}
