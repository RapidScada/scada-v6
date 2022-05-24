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
 * Module   : Webstation Application
 * Summary  : Contains user data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Web.Lang;
using Scada.Web.Services;
using Scada.Web.Users;
using System;

namespace Scada.Web.Code
{
    /// <summary>
    /// Contains user data.
    /// <para>Содержит данные пользователя.</para>
    /// </summary>
    /// <remarks>A user context instance is shared by many clients with the same user ID.</remarks>
    internal class UserContext : IUserContext
    {
        /// <summary>
        /// The default disabled user entity.
        /// </summary>
        private static readonly User EmptyUser = new()
        {
            UserID = 0,
            Enabled = false,
            Name = WebPhrases.UnknownUsername,
            RoleID = RoleID.Disabled
        };

        /// <summary>
        /// The empty user context.
        /// </summary>
        public static readonly UserContext Empty = new();


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UserContext()
        {
            UserEntity = EmptyUser;
            Rights = new UserRights();
            Objects = new UserObjects();
            Menu = new UserMenu();
            Views = new UserViews();
            TimeZone = TimeZoneInfo.Local;
        }


        /// <summary>
        /// Gets the user database entity.
        /// </summary>
        public User UserEntity { get; set; }

        /// <summary>
        /// Gets the user access rights.
        /// </summary>
        public UserRights Rights { get; }

        /// <summary>
        /// Gets the objects available to the user.
        /// </summary>
        public UserObjects Objects { get; }

        /// <summary>
        /// Gets the main menu items available to the user.
        /// </summary>
        public UserMenu Menu { get; }

        /// <summary>
        /// Gets the view explorer nodes available to the user.
        /// </summary>
        public UserViews Views { get; }

        /// <summary>
        /// Gets the user's time zone.
        /// </summary>
        public TimeZoneInfo TimeZone { get; private set; }


        /// <summary>
        /// Sets the time zone.
        /// </summary>
        public void SetTimeZone(string timeZoneID)
        {
            TimeZone = string.IsNullOrEmpty(timeZoneID)
                ? TimeZoneInfo.Local
                : TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
        }
    }
}
