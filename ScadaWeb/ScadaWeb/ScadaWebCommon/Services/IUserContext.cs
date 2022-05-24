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
 * Summary  : Defines functionality to access user data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Data.Entities;
using Scada.Web.Users;
using System;

namespace Scada.Web.Services
{
    /// <summary>
    /// Defines functionality to access user data.
    /// <para>Определяет функциональность для доступа данным пользователя.</para>
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Gets the user database entity.
        /// </summary>
        User UserEntity { get; }

        /// <summary>
        /// Gets the user access rights.
        /// </summary>
        UserRights Rights { get; }

        /// <summary>
        /// Gets the objects available to the user.
        /// </summary>
        UserObjects Objects { get; }

        /// <summary>
        /// Gets the main menu items available to the user.
        /// </summary>
        UserMenu Menu { get; }

        /// <summary>
        /// Gets the view explorer nodes available to the user.
        /// </summary>
        UserViews Views { get; }

        /// <summary>
        /// Gets the user's time zone.
        /// </summary>
        TimeZoneInfo TimeZone { get; }
    }
}
