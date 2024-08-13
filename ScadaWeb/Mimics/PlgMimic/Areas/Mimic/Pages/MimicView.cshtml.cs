// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Lang;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMimic.Areas.Mimic.Pages
{
    /// <summary>
    /// Represents a mimic view page.
    /// <para>Представляет страницу представления мнемосхемы.</para>
    /// </summary>
    public class MimicViewModel(IUserContext userContext) : PageModel
    {
        public int ViewID { get; private set; } = 0;

        public void OnGet(int? id)
        {
            ViewID = id ?? userContext.Views.GetFirstViewID() ?? 0;
            ViewData["Title"] = string.Format(WebPhrases.ViewTitle, ViewID);
        }
    }
}
