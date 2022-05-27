// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Lang;
using Scada.Web.Plugins.PlgWebPage.Code;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgWebPage.Areas.WebPage.Pages
{
    /// <summary>
    /// Represents a landing page for a further redirect.
    /// <para>Представляет посадочную страницу для дальнейшего перенаправления.</para>
    /// </summary>
    public class LandingModel : PageModel
    {
        private readonly IUserContext userContext;
        private readonly IViewLoader viewLoader;
        private readonly dynamic dict;

        public LandingModel(IUserContext userContext, IViewLoader viewLoader)
        {
            this.userContext = userContext;
            this.viewLoader = viewLoader;
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgWebPage.Areas.Main.Pages.Landing");
        }

        public string ErrorMessage { get; private set; } = "";
        public HtmlString RedirectUrl { get; private set; } = HtmlString.Empty;

        public void OnGet(int? id)
        {
            int viewID = id ?? userContext.Views.GetFirstViewID() ?? 0;

            if (viewLoader.GetView(viewID, out WebPageView webPageView, out string errMsg))
            {
                ViewData["Title"] = webPageView.Title;

                if (string.IsNullOrEmpty(webPageView.ViewEntity.Args))
                    ErrorMessage = dict.UrlIsEmpty;
                else
                    RedirectUrl = new HtmlString(webPageView.ViewEntity.Args);
            }
            else
            {
                ViewData["Title"] = string.Format(dict.PageTitle, viewID);
                ErrorMessage = errMsg;
            }
        }
    }
}
