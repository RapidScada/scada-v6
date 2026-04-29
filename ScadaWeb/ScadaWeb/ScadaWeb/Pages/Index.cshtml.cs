// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Redirects to the login page.
    /// <para>Перенаправляет на страницу входа.</para>
    /// </summary>
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage(WebPath.LoginPage);
        }
    }
}
