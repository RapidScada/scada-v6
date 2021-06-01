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
 * Summary  : Represents a logout page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Lang;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Represents a logout page.
    /// <para>Представляет страницу выхода из системы.</para>
    /// </summary>
    public class LogoutModel : PageModel
    {
        private readonly IWebContext webContext;

        public LogoutModel(IWebContext webContext)
        {
            this.webContext = webContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string username = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.Name) : null;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            if (!string.IsNullOrEmpty(username))
            {
                webContext.Log.WriteAction(Locale.IsRussian ?
                    "Пользователь {0} вышел из системы, IP {1}" :
                    "User {0} is logged out, IP {1}",
                    User.FindFirstValue(ClaimTypes.Name), HttpContext.Connection.RemoteIpAddress);
            }

            return RedirectToPage(WebUrl.LoginPage);
        }
    }
}
