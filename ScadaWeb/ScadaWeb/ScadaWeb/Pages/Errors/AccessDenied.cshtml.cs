// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Scada.Web.Pages.Errors
{
    /// <summary>
    /// Represents a page containing an insufficient rights message.
    /// <para>Представляет страницу, которая содержит сообщение об отсутствии прав доступа.</para>
    /// </summary>
    public class AccessDeniedModel : PageModel
    {
        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public void OnGet()
        {
        }
    }
}
