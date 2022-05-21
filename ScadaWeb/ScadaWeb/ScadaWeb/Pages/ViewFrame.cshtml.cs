// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
