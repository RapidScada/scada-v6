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
using Scada.Web.Plugins;
using Scada.Web.Services;
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

        [TempData]
        public bool JustLogout { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsAuthenticated())
            {
                UserLoginArgs userLoginArgs = new()
                {
                    Username = User.GetUsername(),
                    UserID = User.GetUserID(),
                    RoleID = User.GetRoleID(),
                    SessionID = HttpContext.Session.Id,
                    RemoteIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    UserIsValid = true,
                    ErrorMessage = "",
                    FriendlyError = ""
                };

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                webContext.Log.WriteAction(Locale.IsRussian ?
                    "Пользователь {0} вышел из системы, IP {1}" :
                    "User {0} is logged out, IP {1}",
                    userLoginArgs.Username, userLoginArgs.RemoteIP);
                webContext.PluginHolder.OnUserLogout(userLoginArgs);
            }

            JustLogout = true;
            return RedirectToPage(WebPath.LoginPage);
        }
    }
}
