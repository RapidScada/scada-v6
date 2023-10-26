/*
 * Copyright 2023 Rapid Software LLC
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
 * Modified : 2023
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Data.TwoFactorAuth;
using Scada.Lang;
using Scada.Web.Audit;
using Scada.Web.Authorization;
using Scada.Web.Config;
using Scada.Web.Lang;
using Scada.Web.Plugins;
using Scada.Web.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
        private readonly IAuditLog auditLog;
        private readonly IClientAccessor clientAccessor;
        private readonly HttpContext httpContext;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LoginService(IWebContext webContext, IAuditLog auditLog,
            IClientAccessor clientAccessor, IHttpContextAccessor httpContextAccessor)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
            this.auditLog = auditLog ?? throw new ArgumentNullException(nameof(auditLog));
            this.clientAccessor = clientAccessor ?? throw new ArgumentNullException(nameof(clientAccessor));
            httpContext = httpContextAccessor?.HttpContext ?? 
                throw new ArgumentException("HTTP context must not be null.", nameof(httpContextAccessor));
        }


        /// <summary>
        /// Logs in.
        /// </summary>
        private async Task DoLoginAsync(string username, WebUserValidationResult validResult, bool rememberMe, int rememberMeExpires, string browserIdentity)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, validResult.UserID.ToString(), ClaimValueTypes.Integer),
                new Claim(ClaimTypes.Role, validResult.RoleID.ToString(), ClaimValueTypes.Integer),
                new Claim(ClaimExtTypes.BrowserIdentity, browserIdentity.ToString(), ClaimValueTypes.String),
                new Claim(ClaimExtTypes.FirstWebLogin, validResult.NeedModifyPwd.ToString(), ClaimValueTypes.Boolean),
            };

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties authProperties = new();

            if (rememberMe)
            {
                authProperties.IsPersistent = true;
                authProperties.ExpiresUtc = DateTime.UtcNow.AddDays(rememberMeExpires);
            }
            if (validResult.NeedFactorAuth)
            {
                await httpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }
            else
            {
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            if (httpContext.Request.Cookies.TryGetValue("loginErr", out string errMsg))
            {
                
            }
        }

        /// <summary>
        /// Validates the username and password, and logs in.
        /// </summary>
        public async Task<SimpleResult> LoginAsync(string username, string password, string loginType, string browserIdentity, bool rememberMe)
        {
            WebUserValidationResult result;
            string friendlyError;

            // check user by server
            try
            {
                //获取客户端IP地址
                var clientIpAddr = httpContext.Connection.RemoteIpAddress.MapToIPv4()?.ToString();
                result = clientAccessor.ScadaClient.ValidateWebUser(username, password, loginType, browserIdentity, clientIpAddr);

                friendlyError = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                result = WebUserValidationResult.Fail(ex.Message);
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
                await DoLoginAsync(username, result,
                    loginOptions.AllowRememberMe && rememberMe, loginOptions.RememberMeExpires, browserIdentity);

                webContext.Log.WriteAction("User {0} is logged in, IP {1}", username, userLoginArgs.RemoteIP);
                return SimpleResult.Success(result);
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
                await httpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
                webContext.Log.WriteAction("User {0} is logged out, IP {1}", userLoginArgs.Username, userLoginArgs.RemoteIP);
                webContext.PluginHolder.OnUserLogout(userLoginArgs);
            }
        }

        /// <summary>
        /// 获取2FA密钥
        /// </summary>
        public async Task<TwoFactorAuthInfoResult> GetTwoFactorAuthenticatorKeyAsync(int userId)
        {
            var result = clientAccessor.ScadaClient.GetTwoFAKey(userId);
            return result;
        }


        /// <summary>
        /// 验证2FA密钥
        /// </summary>
        public async Task<TwoFactorAuthValidateResult> VerifyTwoFactorAuthenticatorKeyAsync(int userId, int code, bool trustDevice, string browserId)
        {
            var result = clientAccessor.ScadaClient.VerifyTwoFAKey(userId, code, trustDevice, browserId);
            return result;
        }

        /// <summary>
        /// 重置2FA密钥
        /// </summary>
        public async Task<SimpleResult> ResetTwoFactorAuthenticatorKeyAsync(int userId)
        {
            //return Task.FromResult(clientAccessor.ScadaClient.ResetTwoFactorAuthenticatorKey(username));
            return SimpleResult.Success();
        }

        /// <summary>
        /// 重置用户
        /// </summary>
        public async Task<SimpleResult> ResetUserAsync(int userId)
        {
            // return Task.FromResult(clientAccessor.ScadaClient.ResetUser(username));
            return SimpleResult.Success();
        }
    }
}
