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
 * Summary  : Creates user context instances
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Scada.Data.Entities;
using Scada.Lang;
using Scada.Log;
using Scada.Web.Services;
using Scada.Web.Users;
using System;

namespace Scada.Web.Code
{
    /// <summary>
    /// Creates user context instances.
    /// <para>Создает экземпляры контекста пользователя.</para>
    /// </summary>
    internal static class UserContextFactory
    {
        /// <summary>
        /// Creates a new user context.
        /// </summary>
        private static UserContext CreateUserContext(int userID, IWebContext webContext)
        {
            User userEntity = webContext.ConfigDatabase.UserTable.GetItem(userID) ?? 
                webContext.PluginHolder.FindUser(userID);

            if (userEntity == null)
            {
                webContext.Log.WriteError(Locale.IsRussian ?
                    "Пользователь с ид. {0} не найден при создании контекста пользователя" :
                    "User with ID {0} not found when creating user context", userID);
                return UserContext.Empty;
            }
            else
            {
                UserContext userContext = new() { UserEntity = userEntity };
                userContext.Rights.Init(webContext.ConfigDatabase.RightMatrix, userEntity.RoleID);
                userContext.Objects.Init(webContext.ConfigDatabase.ObjTable, userContext.Rights);
                userContext.Menu.Init(webContext, userEntity, userContext.Rights);
                userContext.Views.Init(webContext, userContext.Rights);

                UserConfig userConfig = webContext.PluginHolder.GetUserConfig(userID);
                userContext.SetTimeZone(
                    userConfig?.TimeZone ?? 
                    webContext.AppConfig.GeneralOptions.DefaultTimeZone);

                return userContext;
            }
        }

        /// <summary>
        /// Gets from cache or creates a user context.
        /// </summary>
        public static IUserContext GetUserContext(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            ILog log = null;

            try
            {
                IWebContext webContext = serviceProvider.GetRequiredService<IWebContext>();
                log = webContext.Log;

                IHttpContextAccessor httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                HttpContext httpContext = httpContextAccessor.HttpContext;

                if (!(webContext.IsReady &&
                    httpContext?.User != null &&
                    httpContext.User.IsAuthenticated() &&
                    httpContext.User.GetUserID(out int userID)))
                {
                    return UserContext.Empty;
                }

                IMemoryCache memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
                return memoryCache.GetOrCreate(WebUtils.GetUserCacheKey(userID), entry =>
                {
                    entry.SetDefaultOptions(webContext);
                    return CreateUserContext(userID, webContext);
                });
            }
            catch (Exception ex)
            {
                if (log == null)
                    throw;

                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при создании контекста пользователя" :
                    "Error creating user context");
                return UserContext.Empty;
            }
        }
    }
}
