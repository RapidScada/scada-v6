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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Code;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Represents a login page.
    /// <para>Представляет страницу входа.</para>
    /// </summary>
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly AppData appData;

        public LoginModel(AppData appData)
        {
            this.appData = appData;
        }


        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }


        private async Task DoLoginAsync()
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin@gmail.com"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Role, "Administrator"),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private async Task DoLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public void OnGet()
        {
            DoLogoutAsync().Wait();
        }

        public IActionResult OnPost(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (Username == "admin" && Password == "scada")
                {
                    DoLoginAsync().Wait();
                    appData.Log.WriteAction("Login OK!");

                    string url = !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? returnUrl : "/View";
                    return RedirectToPage(url);// LocalRedirect(url);
                }
                else
                {
                    appData.Log.WriteAction("Login error!");
                    ModelState.AddModelError(string.Empty, "Incorrect username or password.");
                }
            }

            return Page();
        }
    }
}
