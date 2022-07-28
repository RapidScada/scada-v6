// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Data.Models;
using Scada.Report;
using Scada.Web.Authorization;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;
using System.Net.Mime;

namespace Scada.Web.Plugins.PlgMain.Controllers
{
    /// <summary>
    /// Represents a controller for printing table views and events.
    /// <para>Представляет контроллер для печати табличных представлений и событий.</para>
    /// </summary>
    [Authorize(Policy = PolicyName.Restricted)]
    [Route("Main/Print/[action]")]
    public class PrintController : Controller
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IClientAccessor clientAccessor;
        private readonly IViewLoader viewLoader;
        private readonly string templateDir;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PrintController(IWebContext webContext, IUserContext userContext, IClientAccessor clientAccessor, 
            IViewLoader viewLoader)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.clientAccessor = clientAccessor;
            this.viewLoader = viewLoader;
            templateDir = Path.Combine(webContext.AppDirs.ExeDir, "wwwroot", "plugins", "Main", "templates");
        }


        /// <summary>
        /// Prints events matching the specified filter to an Excel workbook.
        /// </summary>
        private Stream PrintEvents(EventFilter filter)
        {
            MemoryStream stream = new();

            try
            {
                EventWorkbookBuilder builder = new(webContext.ConfigDatabase, clientAccessor.ScadaClient, templateDir);
                builder.Build(filter, stream);
                stream.Position = 0;
            }
            catch
            {
                stream.Dispose();
                throw;
            }

            return stream;
        }

        /// <summary>
        /// Prints the specified table view to an Excel workbook.
        /// </summary>
        public IActionResult PrintTableView(int viewID, DateTime startTime, DateTime endTime)
        {
            string filePath = @"C:\SCADA\ScadaWeb\wwwroot\images\gear.png";
            return PhysicalFile(filePath, "application/octet-stream", Path.GetFileName(filePath));
        }

        /// <summary>
        /// Prints last events filtered by the specified view ID to an Excel workbook.
        /// </summary>
        public IActionResult PrintEventsByView(int viewID)
        {
            if (!viewLoader.GetView(viewID, out ViewBase view, out string errMsg))
                throw new ScadaException(errMsg);

            return File(
                PrintEvents(new EventFilter(100, view)), // TODO: report arguments: event count, archive bit
                MediaTypeNames.Application.Octet,
                ReportUtils.BuildFileName("Events", OutputFormat.Xml2003));
        }

        /// <summary>
        /// Prints all last events to an Excel workbook.
        /// </summary>
        public IActionResult PrintAllEvents()
        {
            return File(
                PrintEvents(new EventFilter(100)),
                MediaTypeNames.Application.Octet,
                ReportUtils.BuildFileName("Events", OutputFormat.Xml2003));
        }
    }
}
