/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Redirects to a specific view page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Plugins;
using Scada.Web.Services;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Redirects to a specific view page.
    /// <para>Перенаправляет на определенную страницу представления.</para>
    /// </summary>
    public class ViewFrameModel : PageModel
    {
        private readonly IUserContext userContext;
        private readonly IViewLoader viewLoader;

        public ViewFrameModel(IUserContext userContext, IViewLoader viewLoader)
        {
            this.userContext = userContext;
            this.viewLoader = viewLoader;
        }


        public bool ViewError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }


        public IActionResult OnGet(int? id)
        {
            int viewID = id ?? userContext.Views.GetFirstViewID() ?? 0;

            if (viewLoader.GetViewSpec(viewID, out ViewSpec viewSpec, out string errMsg))
            {
                ErrorMessage = "";
                string viewFrameUrl = Url.Content(viewSpec.GetFrameUrl(viewID));
                return LocalRedirect(viewFrameUrl);
            }
            else
            {
                ErrorMessage = errMsg;
                return Page();
            }
        }
    }
}
