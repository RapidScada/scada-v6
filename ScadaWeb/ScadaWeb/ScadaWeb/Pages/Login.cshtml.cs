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
 * Summary  : Represents a login page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Lang;
using Scada.Web.Code;
using Scada.Web.Config;
using Scada.Web.Lang;
using Scada.Web.Plugins;
using Scada.Web.Services;
using Scada.Web.Users;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Represents a login page.
    /// <para>Представляет страницу входа в систему.</para>
    /// </summary>
    [BindProperties]
    public class LoginModel : PageModel
    {
        private const string CaptchaSessionKey = "ScadaWeb.CaptchaCode";

        private readonly IWebContext webContext;
        private readonly IClientAccessor clientAccessor;
        private readonly dynamic dict;


        public LoginModel(IWebContext webContext, IClientAccessor clientAccessor)
        {
            this.webContext = webContext;
            this.clientAccessor = clientAccessor;
            dict = Locale.GetDictionary("Scada.Web.Pages.Login");
        }


        public string Username { get; set; }

        public string Password { get; set; }

        public string CaptchaCode { get; set; }

        public bool RememberMe { get; set; }

        [TempData]
        public bool JustLogout { get; set; }


        private bool CheckReady()
        {
            if (webContext.IsReadyToLogin)
            {
                return true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, dict.NotReady);
                return false;
            }
        }

        private void ValidateCaptcha()
        {
            if (webContext.AppConfig.LoginOptions.RequireCaptcha)
            {
                string requiredCode = HttpContext.Session.GetString(CaptchaSessionKey);

                if (string.IsNullOrEmpty(requiredCode) || string.IsNullOrEmpty(CaptchaCode) ||
                    !string.Equals(requiredCode, CaptchaCode.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    webContext.Log.WriteError(Locale.IsRussian ?
                        "Указан неверный защитный код, IP {0}" :
                        "Invalid captcha specified, IP {0}", HttpContext.Connection.RemoteIpAddress);
                    ModelState.AddModelError(string.Empty, dict.InvalidCaptcha);
                    ModelState.Remove(nameof(CaptchaCode));
                    CaptchaCode = "";
                }
            }
        }

        private async Task<IActionResult> ValidateUserAsync(string returnUrl)
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
                    .ValidateUser(Username, Password, out userID, out roleID, out errMsg);
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
                Username = Username,
                UserID = userID,
                RoleID = roleID,
                SessionID = HttpContext.Session.Id,
                RemoteIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserIsValid = userIsValid,
                ErrorMessage = errMsg,
                FriendlyError = friendlyError
            };

            webContext.PluginHolder.OnUserLogin(userLoginArgs);

            // show login result
            if (userLoginArgs.UserIsValid)
            {
                await LoginAsync(Username, userID, roleID);
                webContext.Log.WriteAction(Locale.IsRussian ?
                    "Пользователь {0} вошёл в систему {0}, IP {1}" :
                    "User {0} is logged in, IP {1}",
                    Username, userLoginArgs.RemoteIP);
                return RedirectToStartPage(returnUrl, userID);
            }
            else
            {
                webContext.Log.WriteError(Locale.IsRussian ?
                    "Неудачная попытка входа в систему пользователя {0}, IP {1}: {2}" :
                    "Unsuccessful login attempt for user {0}, IP {1}: {2}",
                    Username, userLoginArgs.RemoteIP, userLoginArgs.ErrorMessage);
                ModelState.AddModelError(string.Empty, userLoginArgs.FriendlyError);
                return Page();
            }
        }

        private async Task LoginAsync(string username, int userID, int roleID)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userID.ToString(), ClaimValueTypes.Integer),
                new Claim(ClaimTypes.Role, roleID.ToString(), ClaimValueTypes.Integer)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();
            LoginOptions loginOptions = webContext.AppConfig.LoginOptions;

            if (loginOptions.AllowRememberMe && RememberMe)
            {
                authProperties.IsPersistent = true;
                authProperties.ExpiresUtc = DateTime.UtcNow.AddDays(loginOptions.RememberMeExpires);
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private IActionResult RedirectToStartPage(string returnUrl, int userID)
        {
            UserConfig userConfig = webContext.PluginHolder.GetUserConfig(userID);
            string url = Url.Content(ScadaUtils.FirstNonEmpty(
                returnUrl,
                userConfig?.StartPage,
                webContext.AppConfig.GeneralOptions.DefaultStartPage,
                WebPath.DefaultStartPage.PrependTilda()));
            return LocalRedirect(url);
        }

        public HtmlString GetCaptchaImage()
        {
            Captcha captcha = new();
            string captchaCode = captcha.GenerateCode();
            HttpContext.Session.SetString(CaptchaSessionKey, captchaCode);
            return new HtmlString(captcha.GenerateImageSrc(captchaCode));
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            bool isReady = CheckReady();
            LoginOptions loginOptions = webContext.AppConfig.LoginOptions;

            // already logged in
            if (isReady && User.IsAuthenticated())
                return RedirectToStartPage(returnUrl, User.GetUserID());

            // auto login
            if (!string.IsNullOrEmpty(loginOptions.AutoLoginUsername))
            {
                Username = loginOptions.AutoLoginUsername;
                Password = loginOptions.AutoLoginPassword;
                RememberMe = loginOptions.AllowRememberMe;

                if (isReady && !JustLogout)
                    return await ValidateUserAsync(returnUrl);
            }

            // normal login
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            CheckReady();
            ValidateCaptcha();

            return ModelState.IsValid
                ? await ValidateUserAsync(returnUrl)
                : Page();
        }
    }
}
