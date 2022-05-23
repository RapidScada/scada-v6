// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Entities;
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
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;

        public SchemeViewModel(IWebContext webContext, IUserContext userContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
        }

        public int ViewID { get; set; }
        public bool ControlRight { get; set; }

        public void OnGet(int? id)
        {
            ViewID = id ?? userContext.Views.GetFirstViewID() ?? 0;
            View viewEntity = webContext.ConfigDatabase.ViewTable.GetItem(ViewID);
            ControlRight = webContext.AppConfig.GeneralOptions.EnableCommands &&
                userContext.Rights.GetRightByView(viewEntity).Control;
            ViewData["Title"] = string.Format(PluginPhrases.SchemeViewTitle, ViewID);
        }
    }
}
