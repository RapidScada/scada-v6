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
using Microsoft.Extensions.Caching.Memory;
using Scada.Lang;
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
        private readonly IMemoryCache memoryCache;

        public bool ViewError { get; set; }
        public string ErrorMessage { get; set; }
        public string FrameUrl { get; set; }

        public ViewModel(IWebContext webContext, IUserContext userContext, IMemoryCache memoryCache)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.memoryCache = memoryCache;

            ViewError = false;
            ErrorMessage = "";
            FrameUrl = "";
        }

        public void OnGet(int? id)
        {
            int viewID = id ?? userContext.Views.GetFirstViewID() ?? 0;
            dynamic dict = Locale.GetDictionary("Scada.Web.Pages.View");

            if (viewID <= 0)
            {
                ViewError = true;
                ErrorMessage = dict.ViewNotSpecified;
                return;
            }

            // find view
            Data.Entities.View viewEntity = webContext.BaseDataSet.ViewTable.GetItem(viewID);
            
            if (viewEntity == null)
            {
                ViewError = true;
                ErrorMessage = dict.ViewNotExists;
                return;
            }

            // check access rights
            if (!userContext.Rights.GetRightByObj(viewEntity.ObjNum ?? 0).View)
            {
                ViewError = true;
                ErrorMessage = dict.InsufficientRights;
                return;
            }

            // get view specification
            ViewSpec viewSpec = memoryCache.GetOrCreate(WebUtils.GetViewSpecKey(viewID), entry =>
            {
                entry.SetDefaultOptions(webContext);
                return webContext.GetViewSpec(viewEntity);
            });

            if (viewSpec == null)
            {
                ViewError = true;
                ErrorMessage = dict.UnableResolveSpec;
                return;
            }

            ViewData["SelectedViewID"] = viewID; // used by _MainLayout
            FrameUrl = Url.Content(viewSpec.GetFrameUrl(viewID));
        }

        public HtmlString RenderBottomTabs()
        {
            StringBuilder sbHtml = new();

            foreach (DataWindowSpec spec in webContext.PluginHolder.AllDataWindowSpecs())
            {
                sbHtml.AppendFormat("<div class='tab' data-url='{0}'>{1}</div>",
                    Url.Content(spec.Url), HttpUtility.HtmlEncode(spec.Title));
            }

            return new HtmlString(sbHtml.ToString());
        }
    }
}
