﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Report;
using Scada.Web.Api;
using Scada.Web.Authorization;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Plugins.PlgMain.Report;
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
        private const string HistDataReportPrefix = "HistData";
        private const string EventReportPrefix = "Events";

        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IClientAccessor clientAccessor;
        private readonly IViewLoader viewLoader;
        private readonly PluginContext pluginContext;
        private readonly string templateDir;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PrintController(IWebContext webContext, IUserContext userContext, IClientAccessor clientAccessor,
            IViewLoader viewLoader, PluginContext pluginContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.clientAccessor = clientAccessor;
            this.viewLoader = viewLoader;
            this.pluginContext = pluginContext;
            templateDir = Path.Combine(webContext.AppDirs.ExeDir, "wwwroot", "plugins", "Main", "templates");
        }


        /// <summary>
        /// Creates a new report context.
        /// </summary>
        private IReportContext CreateReportContext()
        {
            return new ReportContext
            {
                ConfigDatabase = webContext.ConfigDatabase,
                ScadaClient = clientAccessor.ScadaClient,
                TemplateDir = templateDir
            };
        }

        /// <summary>
        /// Generates an event report with the specified arguments.
        /// </summary>
        private Stream GenerateEventReport(EventReportArgs args, out DateTime generateTime)
        {
            generateTime = DateTime.UtcNow;
            return null;
        }

        /// <summary>
        /// Prints events filtered by view to an Excel workbook.
        /// </summary>
        private Stream PrintEvents(ViewBase view, out DateTime generateTime)
        {
            MemoryStream stream = new();

            try
            {
                EventWorkbookBuilder builder = new(webContext.ConfigDatabase, clientAccessor.ScadaClient, templateDir);
                builder.Generate(new EventWorkbookArgs
                {
                    ArchiveCode = pluginContext.Options.EventArchiveCode,
                    EventCount = pluginContext.Options.EventCount,
                    EventDepth = pluginContext.Options.EventDepth,
                    View = view,
                    TimeZone = userContext.TimeZone
                }, stream);

                generateTime = builder.GenerateTime;
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
            if (!viewLoader.GetView(viewID, out TableView tableView, out string errMsg))
                throw new ScadaException(errMsg);

            MemoryStream stream = new();
            DateTime generateTime;

            try
            {
                TableWorkbookBuilder builder = new(webContext.ConfigDatabase, clientAccessor.ScadaClient, templateDir);
                builder.Generate(new TableWorkbookArgs
                {
                    TableView = tableView,
                    TableOptions = pluginContext.GetTableOptions(tableView),
                    TimeRange = userContext.CreateTimeRangeUtc(startTime, endTime, true),
                    TimeZone = userContext.TimeZone
                }, stream);

                generateTime = builder.GenerateTime;
                stream.Position = 0;
            }
            catch
            {
                stream.Dispose();
                throw;
            }

            return File(
                stream,
                MediaTypeNames.Application.Octet,
                ReportUtils.BuildFileName("TableView", generateTime, OutputFormat.Xml2003));
        }

        /// <summary>
        /// Prints last events filtered by the specified view ID to an Excel workbook.
        /// </summary>
        public IActionResult PrintEventsByView(int viewID)
        {
            if (!viewLoader.GetView(viewID, out ViewBase view, out string errMsg))
                throw new ScadaException(errMsg);

            return File(
                PrintEvents(view, out DateTime generateTime),
                MediaTypeNames.Application.Octet,
                ReportUtils.BuildFileName(EventReportPrefix, generateTime, OutputFormat.Xml2003));
        }

        /// <summary>
        /// Prints all last events to an Excel workbook.
        /// </summary>
        public IActionResult PrintAllEvents()
        {
            // TODO: available events
            return File(
                PrintEvents(null, out DateTime generateTime),
                MediaTypeNames.Application.Octet,
                ReportUtils.BuildFileName(EventReportPrefix, generateTime, OutputFormat.Xml2003));
        }

        /// <summary>
        /// Generates a historical data report.
        /// </summary>
        public IActionResult PrintHistDataReport(DateTime startTime, DateTime endTime, 
            string archive, IntRange cnlNums)
        {
            MemoryStream stream = new();
            DateTime generateTime;

            try
            {
                HistDataReportBuilder builder = new(CreateReportContext());
                builder.Generate(new HistDataReportArgs
                {
                    StartTime = userContext.ConvertTimeToUtc(startTime),
                    EndTime = userContext.ConvertTimeToUtc(endTime),
                    TimeZone = userContext.TimeZone,
                    ArchiveCode = archive,
                    CnlNums = cnlNums,
                    MaxPeriod = pluginContext.Options.MaxReportPeriod
                }, stream);

                generateTime = builder.GenerateTime;
                stream.Position = 0;
            }
            catch
            {
                stream.Dispose();
                throw;
            }

            return File(
                stream,
                MediaTypeNames.Application.Octet,
                ReportUtils.BuildFileName(HistDataReportPrefix, generateTime, OutputFormat.Xml2003));
        }

        /// <summary>
        /// Generates an event report.
        /// </summary>
        public IActionResult PrintEventReport(DateTime startTime, DateTime endTime, 
            string archive, int objNum, IntRange severities)
        {
            if (!webContext.ConfigDatabase.ObjTable.PkExists(objNum))
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Объект не найден в базе конфигурации." :
                    "Object not found in the configuration database.");
            }

            EventReportArgs args = new()
            {
                StartTime = userContext.ConvertTimeToUtc(startTime),
                EndTime = userContext.ConvertTimeToUtc(endTime),
                TimeZone = userContext.TimeZone,
                ArchiveCode = archive,
                ObjNum = objNum,
                Severities = severities,
                MaxPeriod = pluginContext.Options.MaxReportPeriod
            };

            return File(
                GenerateEventReport(args, out DateTime generateTime),
                MediaTypeNames.Application.Octet,
                ReportUtils.BuildFileName(EventReportPrefix, generateTime, OutputFormat.Xml2003));
        }
    }
}
