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
 * Summary  : Performs user login and logout
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Scada.Lang;
using Scada.Web.Config;
using Scada.Web.Lang;
using Scada.Web.Plugins;
using Scada.Web.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scada.Web.Code
{
    /// <summary>
    /// Performs user login and logout.
    /// <para>Выполняет вход и выход пользователя.</para>
    /// </summary>
    internal class LoginService : ILoginService
    {
        private readonly IWebContext webContext;
        private readonly IClientAccessor clientAccessor;
        private readonly HttpContext httpContext;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LoginService(IWebContext webContext, IClientAccessor clientAccessor, 
            IHttpContextAccessor httpContextAccessor)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
            this.clientAccessor = clientAccessor;
            httpContext = httpContextAccessor?.HttpContext ?? 
                throw new ArgumentException("HTTP context must not be null.", nameof(httpContextAccessor));
        }


        /// <summary>
        /// Logs in.
        /// </summary>
        private async Task DoLoginAsync(string username, int userID, int roleID, 
            bool rememberMe, int rememberMeExpires)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userID.ToString(), ClaimValueTypes.Integer),
                new Claim(ClaimTypes.Role, roleID.ToString(), ClaimValueTypes.Integer)
            };

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties authProperties = new();

            if (rememberMe)
            {
                authProperties.IsPersistent = true;
                authProperties.ExpiresUtc = DateTime.UtcNow.AddDays(rememberMeExpires);
            }

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        /// <summary>
        /// Validates the username and password, and logs in.
        /// </summary>
        public async Task<SimpleResult> LoginAsync(string username, string password, bool rememberMe)
        {
            bool userIsValid = false;
            int userID = 0;
            int roleID = 0;
            string errMsg;
            string friendlyError;

            // check user by server
            try
            {
                userIsValid = clientAccessor.ScadaClient
                    .ValidateUser(username, password, out userID, out roleID, out errMsg);
                friendlyError = errMsg;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                friendlyError = WebPhrases.ClientError;
            }

            // check user by plugins
            UserLoginArgs userLoginArgs = new()
            {
                Username = username,
                UserID = userID,
                RoleID = roleID,
                SessionID = httpContext.Session.Id,
                RemoteIP = httpContext.Connection.RemoteIpAddress?.ToString(),
                UserIsValid = userIsValid,
                ErrorMessage = errMsg,
                FriendlyError = friendlyError
            };

            webContext.PluginHolder.OnUserLogin(userLoginArgs);

            // show login result
            if (userLoginArgs.UserIsValid)
            {
                LoginOptions loginOptions = webContext.AppConfig.LoginOptions;
                await DoLoginAsync(username, userID, roleID, 
                    loginOptions.AllowRememberMe && rememberMe, loginOptions.RememberMeExpires);

                webContext.Log.WriteAction(Locale.IsRussian ?
                    "Пользователь {0} вошёл в систему {0}, IP {1}" :
                    "User {0} is logged in, IP {1}",
                    username, userLoginArgs.RemoteIP);
                return SimpleResult.Success();
            }
            else
            {
                webContext.Log.WriteError(Locale.IsRussian ?
                    "Неудачная попытка входа в систему пользователя {0}, IP {1}: {2}" :
                    "Unsuccessful login attempt for user {0}, IP {1}: {2}",
                    username, userLoginArgs.RemoteIP, userLoginArgs.ErrorMessage);
                return SimpleResult.Fail(userLoginArgs.FriendlyError);
            }
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        public async Task LogoutAsync()
        {
            if (httpContext.User.IsAuthenticated())
            {
                UserLoginArgs userLoginArgs = new()
                {
                    Username = httpContext.User.GetUsername(),
                    UserID = httpContext.User.GetUserID(),
                    RoleID = httpContext.User.GetRoleID(),
                    SessionID = httpContext.Session.Id,
                    RemoteIP = httpContext.Connection.RemoteIpAddress?.ToString(),
                    UserIsValid = true,
                    ErrorMessage = "",
                    FriendlyError = ""
                };

                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                webContext.Log.WriteAction(Locale.IsRussian ?
                    "Пользователь {0} вышел из системы, IP {1}" :
                    "User {0} is logged out, IP {1}",
                    userLoginArgs.Username, userLoginArgs.RemoteIP);
                webContext.PluginHolder.OnUserLogout(userLoginArgs);
            }
        }
    }
}
