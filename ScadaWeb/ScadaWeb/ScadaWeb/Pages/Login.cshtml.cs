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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Client;
using Scada.Lang;
using Scada.Web.Config;
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
        private readonly IWebContext webContext;
        private readonly IClientAccessor clientAccessor;


        public LoginModel(IWebContext webContext, IClientAccessor clientAccessor)
        {
            this.webContext = webContext;
            this.clientAccessor = clientAccessor;
        }


        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }


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

        public IActionResult OnGet()
        {
            if (!webContext.IsReadyToLogin)
            {
                dynamic dict = Locale.GetDictionary("Scada.Web.Pages.Login"); 
                ModelState.AddModelError(string.Empty, dict.NotReady);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ScadaClient scadaClient = clientAccessor.ScadaClient;

                if (scadaClient.ValidateUser(Username, Password, out int userID, out int roleID, out string errMsg))
                {
                    await LoginAsync(Username, userID, roleID);
                    webContext.Log.WriteAction(Locale.IsRussian ?
                        "Пользователь {0} вошёл в систему {0}, IP {1}" :
                        "User {0} is logged in, IP {1}",
                        Username, HttpContext.Connection.RemoteIpAddress);

                    string url = !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? 
                        returnUrl : WebUrl.DefaultStartPage;
                    return RedirectToPage(url);
                }
                else
                {
                    webContext.Log.WriteError(Locale.IsRussian ?
                        "Неудачная попытка входа в систему пользователя {0}, IP {1}: {2}" :
                        "Unsuccessful login attempt for user {0}, IP {1}: {2}",
                        Username, HttpContext.Connection.RemoteIpAddress, errMsg);
                    ModelState.AddModelError(string.Empty, errMsg);
                }
            }

            return Page();
        }
    }
}
