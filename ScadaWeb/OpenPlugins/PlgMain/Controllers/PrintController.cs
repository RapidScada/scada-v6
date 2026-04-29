// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Report;
using Scada.Web.Api;
using Scada.Web.Audit;
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
        private const string TableViewReportPrefix = "TableView";
        private const string EventReportPrefix = "Events";
        private const string HistDataReportPrefix = "HistData";

        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IAuditLog auditLog;
        private readonly IClientAccessor clientAccessor;
        private readonly IViewLoader viewLoader;
        private readonly PluginContext pluginContext;
        private readonly string templateDir;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PrintController(IWebContext webContext, IUserContext userContext, IAuditLog auditLog,
            IClientAccessor clientAccessor, IViewLoader viewLoader, PluginContext pluginContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.auditLog = auditLog;
            this.clientAccessor = clientAccessor;
            this.viewLoader = viewLoader;
            this.pluginContext = pluginContext;
            templateDir = Path.Combine(webContext.AppDirs.ExeDir, "wwwroot", "plugins", "Main", "templates");
        }


        /// <summary>
        /// Creates a new report context.
        /// </summary>
        private ReportContext CreateReportContext()
        {
            return new ReportContext
            {
                ConfigDatabase = webContext.ConfigDatabase,
                ScadaClient = clientAccessor.ScadaClient,
                Culture = Locale.Culture,
                TimeZone = userContext.TimeZone,
                Username = userContext.UserEntity.Name,
                TemplateDir = templateDir
            };
        }

        /// <summary>
        /// Generates an event report with the specified arguments.
        /// </summary>
        private MemoryStream GenerateEventReport(EventReportArgs args, out string fileName)
        {
            MemoryStream stream = new();
            bool success = false;

            try
            {
                EventReportBuilder builder = new(CreateReportContext());
                builder.Generate(args, stream);
                fileName = ReportUtils.BuildFileName(EventReportPrefix,
                    userContext.ConvertTimeFromUtc(builder.GenerateTime), OutputFormat.Xml2003);
                stream.Position = 0;
                success = true;
            }
            catch
            {
                stream.Dispose();
                throw;
            }
            finally
            {
                WriteToAuditLog(typeof(EventReportBuilder), success);
            }

            return stream;
        }

        /// <summary>
        /// Gets the numbers of available objects starting from the specified object.
        /// </summary>
        private List<int> GetObjNumHierarchy(int startObjNum)
        {
            List<int> objNums = [startObjNum];

            if (!webContext.ConfigDatabase.ObjTable.TryGetIndex("ParentObjNum", out ITableIndex parentObjIndex))
                throw new ScadaException(CommonPhrases.IndexNotFound);

            void AddChildObjects(int parentObjNum)
            {
                foreach (Obj childObj in parentObjIndex.SelectItems(parentObjNum))
                {
                    if (userContext.Rights.GetRightByObj(childObj.ObjNum).View)
                    {
                        objNums.Add(childObj.ObjNum);
                        AddChildObjects(childObj.ObjNum);
                    }
                }
            }

            AddChildObjects(startObjNum);
            return objNums;
        }

        /// <summary>
        /// Writes the entry to the audit log.
        /// </summary>
        private void WriteToAuditLog(Type reportType, bool success)
        {
            auditLog.Write(new AuditLogEntry(userContext.UserEntity)
            {
                ActionType = AuditActionType.GenerateReport,
                ActionArgs = AuditActionArgs.FromObject(new { ReportType = reportType.Name }),
                ActionResult = AuditActionResult.FromBool(success),
                Severity = success ? Severity.Info : Severity.Minor
            });
        }


        /// <summary>
        /// Prints the specified table view to an Excel workbook.
        /// </summary>
        public IActionResult PrintTableView(int viewID, DateTime startTime, DateTime endTime)
        {
            if (!viewLoader.GetView(viewID, out TableView tableView, out string errMsg))
                throw new ScadaException(errMsg);

            MemoryStream stream = new();
            string fileName;
            bool success = false;

            try
            {
                TableViewReportBuilder builder = new(CreateReportContext());
                builder.Generate(new TableViewReportArgs
                {
                    StartTime = userContext.ConvertTimeToUtc(startTime),
                    EndTime = userContext.ConvertTimeToUtc(endTime),
                    MaxPeriod = pluginContext.Options.MaxReportPeriod,
                    TableView = tableView,
                    TableOptions = pluginContext.GetTableOptions(tableView)
                }, stream);

                fileName = ReportUtils.BuildFileName(TableViewReportPrefix,
                    userContext.ConvertTimeFromUtc(builder.GenerateTime), OutputFormat.Xml2003);
                stream.Position = 0;
                success = true;
            }
            catch
            {
                stream.Dispose();
                throw;
            }
            finally
            {
                WriteToAuditLog(typeof(TableViewReportBuilder), success);
            }

            return File(stream, MediaTypeNames.Application.Octet, fileName);
        }

        /// <summary>
        /// Prints last events filtered by the specified view ID to an Excel workbook.
        /// </summary>
        public IActionResult PrintEventsByView(int viewID)
        {
            if (!viewLoader.GetView(viewID, out ViewBase view, out string errMsg))
                throw new ScadaException(errMsg);

            EventReportArgs args = new()
            {
                ArchiveCode = pluginContext.Options.EventArchiveCode,
                TailMode = true,
                EventCount = pluginContext.Options.EventCount,
                EventDepth = pluginContext.Options.EventDepth,
                View = view
            };

            return File(
                GenerateEventReport(args, out string fileName),
                MediaTypeNames.Application.Octet,
                fileName);
        }

        /// <summary>
        /// Prints all last events to an Excel workbook.
        /// </summary>
        public IActionResult PrintAllEvents()
        {
            EventReportArgs args = new()
            {
                ArchiveCode = pluginContext.Options.EventArchiveCode,
                TailMode = true,
                EventCount = pluginContext.Options.EventCount,
                EventDepth = pluginContext.Options.EventDepth,
                ObjNums = userContext.Rights.ViewAll ? null : userContext.Rights.GetAvailableObjs().ToArray()
            };

            return File(
                GenerateEventReport(args, out string fileName),
                MediaTypeNames.Application.Octet,
                fileName);
        }

        /// <summary>
        /// Generates an event report.
        /// </summary>
        public IActionResult PrintEventReport(DateTime startTime, DateTime endTime,
            string archive, int objNum, IntRange severities)
        {
            IList<int> objNums;

            if (objNum <= 0)
            {
                objNums = userContext.Rights.ViewAll
                    ? null // all objects
                    : userContext.Rights.GetAvailableObjs().ToArray();
            }
            else if (webContext.ConfigDatabase.ObjTable.PkExists(objNum))
            {
                objNums = GetObjNumHierarchy(objNum);
            }
            else
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Объект не найден в базе конфигурации." :
                    "Object not found in the configuration database.");
            }

            EventReportArgs args = new()
            {
                StartTime = userContext.ConvertTimeToUtc(startTime),
                EndTime = userContext.ConvertTimeToUtc(endTime),
                MaxPeriod = pluginContext.Options.MaxReportPeriod,
                ArchiveCode = archive,
                ObjNums = objNums,
                Severities = severities
            };

            return File(
                GenerateEventReport(args, out string fileName),
                MediaTypeNames.Application.Octet,
                fileName);
        }

        /// <summary>
        /// Generates a historical data report.
        /// </summary>
        public IActionResult PrintHistDataReport(DateTime startTime, DateTime endTime,
            string archive, IntRange cnlNums)
        {
            MemoryStream stream = new();
            string fileName;
            bool success = false;

            try
            {
                HistDataReportBuilder builder = new(CreateReportContext());
                builder.Generate(new HistDataReportArgs
                {
                    StartTime = userContext.ConvertTimeToUtc(startTime),
                    EndTime = userContext.ConvertTimeToUtc(endTime),
                    MaxPeriod = pluginContext.Options.MaxReportPeriod,
                    ArchiveCode = archive,
                    CnlNums = cnlNums
                }, stream);

                fileName = ReportUtils.BuildFileName(HistDataReportPrefix,
                    userContext.ConvertTimeFromUtc(builder.GenerateTime), OutputFormat.Xml2003);
                stream.Position = 0;
                success = true;
            }
            catch
            {
                stream.Dispose();
                throw;
            }
            finally
            {
                WriteToAuditLog(typeof(HistDataReportBuilder), success);
            }

            return File(stream, MediaTypeNames.Application.Octet, fileName);
        }
    }
}
