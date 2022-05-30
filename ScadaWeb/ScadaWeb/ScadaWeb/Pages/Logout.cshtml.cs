// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private readonly ILoginService loginService;

        public LogoutModel(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [TempData]
        public bool JustLogout { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await loginService.LogoutAsync();
            JustLogout = true;
            return RedirectToPage(WebPath.LoginPage);
        }
    }
}
