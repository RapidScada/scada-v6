// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scada.Data.Entities;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a page for entering arguments for an event report.
    /// <para>Представляет страницу для ввода аргументов отчёта по событиям.</para>
    /// </summary>
    public class EventReportModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;

        public EventReportModel(IWebContext webContext, IUserContext userContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
        }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string ArchiveCode { get; private set; } = null;
        public List<SelectListItem> ArchiveList { get; private set; } = new();


        private void FillArchiveList()
        {
            foreach (Archive archive in webContext.ConfigDatabase.ArchiveTable)
            {
                ArchiveList.Add(new SelectListItem(archive.Name, archive.Code));

                //if (archive.Code == HistDataReportBuilder.DefaultArchiveCode)
                //    ArchiveCode = archive.Code;
            }
        }

        public void OnGet()
        {
            StartTime = userContext.ConvertTimeFromUtc(DateTime.UtcNow).Date;
            EndTime = StartTime.AddDays(1.0);
            FillArchiveList();
        }
    }
}
