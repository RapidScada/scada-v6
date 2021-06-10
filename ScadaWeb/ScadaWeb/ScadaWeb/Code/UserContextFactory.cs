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
 * Module   : Webstation Application
 * Summary  : Creates user context instances
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using Scada.Web.Plugins;
using Scada.Web.Services;

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
            User userEntity = webContext.BaseDataSet.UserTable.GetItem(userID) ?? 
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
                userContext.Rights.Init(webContext.BaseDataSet, webContext.RightMatrix, userEntity.RoleID);
                userContext.Menu.Init(webContext, userEntity, userContext.Rights);
                userContext.Views.Init(webContext, userContext.Rights);
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
                    httpContext?.User?.Identity != null &&
                    httpContext.User.Identity.IsAuthenticated &&
                    httpContext.User.GetUserID(out int userID)))
                {
                    log.WriteWarning(Locale.IsRussian ?
                        "Невозможно создать контекст пользователя" :
                        "Unable to create user context");
                    return UserContext.Empty;
                }

                IMemoryCache memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
                return memoryCache.GetOrCreate(WebUtils.GetUserKey(userID), entry =>
                {
                    entry.SetSlidingExpiration(WebUtils.CacheExpiration);
                    entry.AddExpirationToken(new CancellationChangeToken(webContext.CacheExpirationTokenSource.Token));
                    return CreateUserContext(userID, webContext);
                });
            }
            catch (Exception ex)
            {
                if (log == null)
                    throw;

                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при создании контекста пользователя" :
                    "Error creating user context");
                return UserContext.Empty;
            }
        }
    }
}
