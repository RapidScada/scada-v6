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
 * Summary  : Overrides subscribing to events raised during cookie authentication
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Scada.Web.Authorization;
using System;
using System.Threading.Tasks;

namespace Scada.Web.Code
{
    /// <summary>
    /// Overrides subscribing to events raised during cookie authentication.
    /// </summary>
    /// <remarks>
    /// See
    /// https://github.com/dotnet/aspnetcore/issues/9039
    /// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/Cookies/src/CookieAuthenticationEvents.cs
    /// </remarks>
    public class CookieAuthEvents : CookieAuthenticationEvents
    {
        private const string AuthCookieKey = ".AspNetCore.Cookies";

        IMemoryCache memoryCache;
        public CookieAuthEvents(IServiceProvider serviceProvider)
        {
            memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
        }

        public override Task SignedIn(CookieSignedInContext context)
        {
            if (context.Scheme.Name == CookieAuthenticationDefaults.AuthenticationScheme)
            {
                var setCookieArr = context.Response.Headers.SetCookie;
                if(setCookieArr.Count == 1 && setCookieArr[0] != null)
                {
                    var authCookie = setCookieArr[0].GetMidStr("Cookies=", ";");
                    memoryCache.Set(ScadaUtils.ComputeHash(authCookie), authCookie);
                }
            }
            return base.SignedIn(context);
        }

        public override Task SigningOut(CookieSigningOutContext context)
        {
            if (context.Scheme.Name == CookieAuthenticationDefaults.AuthenticationScheme &&
                context.Request.Cookies.TryGetValue(AuthCookieKey, out string authCookie))
            {
                memoryCache.Remove(ScadaUtils.ComputeHash(authCookie));
            }

            return base.SigningOut(context);
        }

        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.Path.StartsWithSegments("/api") &&
                context.Response.StatusCode == StatusCodes.Status200OK)
            {
                // do not redirect to the login page
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
            else
            {
                return base.RedirectToLogin(context);
            }
        }

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.Path.StartsWithSegments("/api") &&
                context.Response.StatusCode == StatusCodes.Status200OK)
            {
                // do not redirect to the access denied page
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
            else
            {
                return base.RedirectToAccessDenied(context);
            }
        }

        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            //首次登录强制修改密码
            if (context.Request.Path.Value.ToLower().IndexOf("userprofile") > -1 ||
                context.Request.Path.Value.ToLower().IndexOf("login") > -1 ||
                context.Request.Path.Value.ToLower().IndexOf("loginphone") > -1 ||
                context.Request.Path.Value.ToLower().IndexOf("modifypassword") > -1
                ) return base.ValidatePrincipal(context);

            var principal = context.Principal;
            var firstWebLogin = principal.FindFirst(ClaimExtTypes.FirstWebLogin)?.Value;
            if (!string.IsNullOrEmpty(firstWebLogin) && bool.Parse(firstWebLogin))
                context.Response.Redirect("/UserProfile");
            else if (principal.Identity.IsAuthenticated && context.Request.Cookies.TryGetValue(AuthCookieKey, out string authCookie))
            {
                //校验是否登出
                if (!memoryCache.TryGetValue(ScadaUtils.ComputeHash(authCookie),out _))
                    context.Response.Redirect(WebPath.UserLoginPage);
                else
                    return base.ValidatePrincipal(context);
            }
            else
                return base.ValidatePrincipal(context);
            return Task.CompletedTask;
        }

    }
}
