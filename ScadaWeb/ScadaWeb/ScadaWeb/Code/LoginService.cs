/*
 * Copyright 2025 Rapid Software LLC
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
 * Modified : 2025
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Web.Audit;
using Scada.Web.Config;
using Scada.Web.Lang;
using Scada.Web.Plugins;
using Scada.Web.Services;
using System.Security.Claims;

namespace Scada.Web.Code
{
    /// <summary>
    /// Performs user login and logout.
    /// <para>Выполняет вход и выход пользователя.</para>
    /// </summary>
    internal class LoginService(
        IWebContext webContext,
        IAuditLog auditLog,
        IClientAccessor clientAccessor,
        IHttpContextAccessor httpContextAccessor,
        IMemoryCache memoryCache) : ILoginService
    {
        private readonly HttpContext httpContext = httpContextAccessor?.HttpContext ??
            throw new ArgumentException("HTTP context must not be null.", nameof(httpContextAccessor));


        /// <summary>
        /// Logs in.
        /// </summary>
        private async Task DoLoginAsync(string username, int userID, int roleID,
            bool rememberMe, int rememberMeExpires)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userID.ToString(), ClaimValueTypes.Integer),
                new Claim(ClaimTypes.Role, roleID.ToString(), ClaimValueTypes.Integer)
            ];

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
            UserValidationResult result;
            string friendlyError;

            // check user by server
            try
            {
                result = clientAccessor.ScadaClient.ValidateUser(username, password);

                if (result.IsValid && result.RoleID == RoleID.Application)
                {
                    result.IsValid = false;
                    result.ErrorMessage = WebPhrases.IllegalRole;
                }

                friendlyError = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                result = UserValidationResult.Fail(ex.Message);
                friendlyError = WebPhrases.ClientError;
            }

            // check user by plugins
            UserLoginArgs userLoginArgs = new()
            {
                Username = username,
                UserID = result.UserID,
                RoleID = result.RoleID,
                SessionID = httpContext.Session.Id,
                RemoteIP = httpContext.Connection.RemoteIpAddress?.ToString(),
                UserIsValid = result.IsValid,
                ErrorMessage = result.ErrorMessage,
                FriendlyError = friendlyError
            };

            webContext.PluginHolder.OnUserLogin(userLoginArgs);

            // write to audit log
            auditLog.Write(new AuditLogEntry
            {
                ActionTime = DateTime.UtcNow,
                Username = username,
                ActionType = AuditActionType.Login,
                ActionResult = AuditActionResult.FromBool(userLoginArgs.UserIsValid),
                Severity = userLoginArgs.UserIsValid ? Severity.Info : Severity.Major,
                Message = userLoginArgs.FriendlyError
            });

            // show login result
            if (userLoginArgs.UserIsValid)
            {
                LoginOptions loginOptions = webContext.AppConfig.LoginOptions;
                await DoLoginAsync(username, result.UserID, result.RoleID,
                    loginOptions.AllowRememberMe && rememberMe, loginOptions.RememberMeExpires);

                webContext.Log.WriteAction(Locale.IsRussian ?
                    "Пользователь {0} вошёл в систему, роль {1}, IP {2}" :
                    "User {0} is logged in, role {1}, IP {2}",
                    username, userLoginArgs.RoleID, userLoginArgs.RemoteIP);
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
                // perform logout
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
                memoryCache.Remove(WebUtils.GetUserCacheKey(userLoginArgs.UserID));
                webContext.Log.WriteAction(Locale.IsRussian ?
                    "Пользователь {0} вышел из системы, IP {1}" :
                    "User {0} is logged out, IP {1}",
                    userLoginArgs.Username, userLoginArgs.RemoteIP);
                webContext.PluginHolder.OnUserLogout(userLoginArgs);

                // write to audit log
                auditLog.Write(new AuditLogEntry
                {
                    ActionTime = DateTime.UtcNow,
                    Username = userLoginArgs.Username,
                    ActionType = AuditActionType.Logout,
                    ActionResult = AuditActionResult.Success,
                    Severity = Severity.Info
                });
            }
        }
    }
}
