// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a page that contains a list of events.
    /// <para>Представляет страницу, содержащую список событий.</para>
    /// </summary>
    public class EventsModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly PluginContext pluginContext;


        public EventsModel(IWebContext webContext, PluginContext pluginContext)
        {
            this.webContext = webContext;
            this.pluginContext = pluginContext;
        }

        public int ArchiveBit { get; set; }

        public void OnGet()
        {
            ArchiveBit = webContext.ConfigDatabase.FindArchiveBit(
                pluginContext.Options.EventArchiveCode, Data.Const.ArchiveBit.Event);
        }
    }
}
