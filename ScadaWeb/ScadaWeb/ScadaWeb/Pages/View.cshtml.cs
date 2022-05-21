// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Plugins;
using Scada.Web.Services;
using System.Text;
using System.Web;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Represents a page of a view.
    /// <para>Представляет страницу представления.</para>
    /// </summary>
    public class ViewModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IViewLoader viewLoader;


        public ViewModel(IWebContext webContext, IUserContext userContext, IViewLoader viewLoader)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.viewLoader = viewLoader;
        }


        public bool ViewError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public int ViewID { get; set; }
        public string ViewFrameUrl { get; set; }


        public void OnGet(int? id)
        {
            ViewID = id ?? userContext.Views.GetFirstViewID() ?? 0;
            ViewData["SelectedViewID"] = ViewID; // used by _MainLayout

            if (viewLoader.GetViewSpec(ViewID, out ViewSpec viewSpec, out string errMsg))
            {
                ErrorMessage = "";
                ViewFrameUrl = Url.Content(viewSpec.GetFrameUrl(ViewID));
            }
            else
            {
                ErrorMessage = errMsg;
                ViewFrameUrl = "";
            }
        }

        public HtmlString RenderBottomTabs()
        {
            StringBuilder sbHtml = new();

            foreach (DataWindowSpec spec in webContext.PluginHolder.AllDataWindowSpecs())
            {
                sbHtml.AppendFormat("<div class='bottom-pnl-tab' data-url='{0}'>{1}</div>",
                    Url.Content(spec.Url), HttpUtility.HtmlEncode(spec.Title));
            }

            return new HtmlString(sbHtml.ToString());
        }
    }
}
