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
 * Summary  : Represents a page of a view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

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

        public bool ViewError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public int ViewID { get; set; }
        public string ViewFrameUrl { get; set; }

        public ViewModel(IWebContext webContext, IUserContext userContext, IViewLoader viewLoader)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.viewLoader = viewLoader;
        }

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
