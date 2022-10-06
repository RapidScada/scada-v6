// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scada.Data.Entities;
using Scada.Lang;
using Scada.Web.Plugins.PlgMain.Report;
using Scada.Web.Services;
using Scada.Web.Users;

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
        private readonly dynamic dict;

        public EventReportModel(IWebContext webContext, IUserContext userContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Areas.Main.Pages.EventReport");
        }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public List<SelectListItem> ArchiveList { get; private set; } = new();
        public List<SelectListItem> ObjList { get; private set; } = new();


        private void FillArchiveList()
        {
            foreach (Archive archive in webContext.ConfigDatabase.ArchiveTable)
            {
                ArchiveList.Add(new SelectListItem(archive.Name, archive.Code, 
                    archive.Code == EventReportBuilder.DefaultArchiveCode));
            }
        }

        private void FillObjList()
        {
            string itemText = userContext.Rights.ViewAll ? dict.AllObjItem : dict.AllAvailObjItem;
            ObjList.Add(new SelectListItem(itemText, "0"));

            foreach (ObjectItem objectItem in userContext.Objects)
            {
                ObjList.Add(new SelectListItem(
                    objectItem.Text,
                    objectItem.ObjNum.ToString()));
            }
        }

        public void OnGet()
        {
            StartTime = userContext.ConvertTimeFromUtc(DateTime.UtcNow).Date;
            EndTime = StartTime.AddDays(1.0);
            FillArchiveList();
            FillObjList();
        }
    }
}
