// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Plugins.PlgScheme.Code;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgScheme.Areas.Scheme.Pages
{
    /// <summary>
    /// Represents a scheme view page.
    /// <para>Представляет страницу представления схемы.</para>
    /// </summary>
    public class SchemeViewModel : PageModel
    {
        private readonly IUserContext userContext;
        private readonly IViewLoader viewLoader;

        public SchemeViewModel(IUserContext userContext, IViewLoader viewLoader)
        {
            this.userContext = userContext;
            this.viewLoader = viewLoader;
        }

        public bool ViewError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public int ViewID { get; set; }

        public void OnGet(int? id)
        {
            // TODO: avoid loading scheme here
            ViewID = id ?? userContext.Views.GetFirstViewID() ?? 0;
            viewLoader.GetView(ViewID, out SchemeView schemeView, out string errMsg);
            ErrorMessage = errMsg;
            ViewData["Title"] = schemeView == null
                ? string.Format(PluginPhrases.SchemeViewTitle, ViewID)
                : schemeView.Title;
        }
    }
}
