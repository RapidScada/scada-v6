// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Web.Plugins.PlgMain.Report;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a page for entering arguments for a historical data report.
    /// <para>Представляет страницу для ввода аргументов отчёта по историческим данным.</para>
    /// </summary>
    public class HistDataReportModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;

        public HistDataReportModel(IWebContext webContext, IUserContext userContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
        }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public List<SelectListItem> ArchiveList { get; private set; } = new();


        private void FillArchiveList()
        {
            foreach (Archive archive in webContext.ConfigDatabase.ArchiveTable
                .Where(a => a.ArchiveKindID == ArchiveKindID.Historical)
                .OrderBy(a => a.Name))
            {
                ArchiveList.Add(new SelectListItem(archive.Name, archive.Code,
                    archive.Code == HistDataReportBuilder.DefaultArchiveCode));
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
