// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Report;
using Scada.Report.Xml2003;
using Scada.Report.Xml2003.Excel;
using System.Xml;

namespace Scada.Web.Plugins.PlgMain.Report
{
    /// <summary>
    /// Builds an Excel workbook that contains events.
    /// <para>Создает книгу Excel, которая содержит события.</para>
    /// </summary>
    public class EventReportBuilder : ReportBuilder
    {
        /// <summary>
        /// The workbook template file name.
        /// </summary>
        private const string TemplateFileName = "EventTemplate.xml";
        /// <summary>
        /// The default code of the event archive.
        /// </summary>
        public const string DefaultArchiveCode = "Events";

        private readonly string templateFilePath;
        private readonly WorkbookRenderer renderer;
        private readonly dynamic reportDict;
        private readonly dynamic dict;

        private EventReportArgs reportArgs;
        private Archive archiveEntity;
        private Row eventRowTemplate;
        private EventFormatted currentEventF;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventReportBuilder(IReportContext reportContext)
            : base(reportContext)
        {
            templateFilePath = Path.Combine(ReportContext.TemplateDir, TemplateFileName);
            renderer = new WorkbookRenderer();
            renderer.BeforeProcessing += Renderer_BeforeProcessing;
            renderer.AfterProcessing += Renderer_AfterProcessing;
            renderer.DirectiveFound += Renderer_DirectiveFound;
            reportDict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Report");
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Report.EventReportBuilder");

            reportArgs = null;
            archiveEntity = null;
            eventRowTemplate = null;
            currentEventF = null;
        }


        /// <summary>
        /// Gets the time range according to the report arguments.
        /// </summary>
        private TimeRange GetTimeRange()
        {
            return reportArgs.TailMode
                ? new TimeRange(GenerateTime.AddDays(-reportArgs.EventDepth), GenerateTime, true)
                : new TimeRange(reportArgs.StartTime, reportArgs.EndTime, true);
        }

        /// <summary>
        /// Gets the event filter according to the report arguments.
        /// </summary>
        private DataFilter GetEventFilter()
        {
            DataFilter dataFilter = new(typeof(Event));

            if (reportArgs.TailMode)
            {
                dataFilter.Limit = reportArgs.EventCount;
                dataFilter.OriginBegin = false;
            }

            if (reportArgs.FilterByView)
                dataFilter.AddCondition("CnlNum", FilterOperator.In, reportArgs.View.CnlNumList);

            if (reportArgs.FilterByObj)
                dataFilter.AddCondition("ObjNum", FilterOperator.In, reportArgs.ObjNums);

            if (reportArgs.FilterBySeverity)
                dataFilter.AddCondition("Severity", FilterOperator.In, reportArgs.Severities);

            return dataFilter;
        }

        /// <summary>
        /// Generates a report to the output stream.
        /// </summary>
        public override void Generate(ReportArgs args, Stream outStream)
        {
            Generate(new EventReportArgs(args), outStream);
        }

        /// <summary>
        /// Generates a report to the output stream.
        /// </summary>
        public void Generate(EventReportArgs args, Stream outStream)
        {
            reportArgs = args ?? throw new ArgumentNullException(nameof(args));
            reportArgs.Validate();
            ArgumentNullException.ThrowIfNull(outStream, nameof(outStream));

            // find archive
            string archiveCode = ScadaUtils.FirstNonEmpty(args.ArchiveCode, DefaultArchiveCode);
            archiveEntity = ReportContext.ConfigDatabase.ArchiveTable
                .SelectFirst(new TableFilter("Code", archiveCode)) ??
                throw new ScadaException(reportDict.ArchiveNotFound);

            // render report
            renderer.Render(templateFilePath, outStream);
        }


        private void Renderer_BeforeProcessing(object sender, XmlDocument e)
        {
            GenerateTime = DateTime.UtcNow;
        }

        private void Renderer_AfterProcessing(object sender, XmlDocument e)
        {
            if (eventRowTemplate != null)
            {
                // select events
                List<Event> events = ReportContext.ScadaClient.GetEvents(
                    archiveEntity.Bit, GetTimeRange(), GetEventFilter(), false, out _);
                CnlDataFormatter formatter = new(ReportContext.ConfigDatabase, ReportContext.TimeZone)
                {
                    Culture = ReportContext.Culture
                };

                // modify workbook
                Table eventTable = eventRowTemplate.ParentTable;
                eventTable.RemoveRow(eventRowTemplate);
                eventTable.RemoveUnwantedAttrs();
                eventTable.ParentWorksheet.Name = dict.WorksheetName;

                foreach (Event ev in events)
                {
                    currentEventF = formatter.FormatEvent(ev);
                    Row eventRow = eventRowTemplate.Clone();
                    renderer.ProcessRow(eventRow);
                    eventTable.AppendRow(eventRow);
                }
            }
        }

        private void Renderer_DirectiveFound(object sender, ExcelDirectiveEventArgs e)
        {
            string cellText = null;

            if (e.Stage == ProcessingStage.Processing)
            {
                if (e.DirectiveName == ReportDirectives.RepRow && e.DirectiveValue == "Event")
                    eventRowTemplate = e.Cell.ParentRow;
                else if (e.DirectiveValue == "Title")
                    cellText = string.Format(dict.TitleFormat, reportArgs.EventCount, reportArgs.EventDepth * 24);
                else if (e.DirectiveValue == "GenCaption")
                    cellText = dict.GenCaption;
                else if (e.DirectiveValue == "Gen")
                    cellText = ReportContext.ConvertTimeFromUtc(GenerateTime).ToLocalizedString(ReportContext.Culture);
                else if (e.DirectiveValue == "ArcCaption")
                    cellText = dict.ArcCaption;
                else if (e.DirectiveValue == "Arc")
                    cellText = archiveEntity.Name;
                else if (e.DirectiveValue == "FilterCaption")
                    cellText = dict.FilterCaption;
                else if (e.DirectiveValue == "Filter")
                    cellText = reportArgs.View == null ? dict.AllEventsFilter : reportArgs.View.Title;
                else if (e.DirectiveValue == "TzCaption")
                    cellText = dict.TzCaption;
                else if (e.DirectiveValue == "Tz")
                    cellText = ReportContext.TimeZone.DisplayName;
                else if (e.DirectiveValue == "TimeCol")
                    cellText = dict.TimeCol;
                else if (e.DirectiveValue == "ObjCol")
                    cellText = dict.ObjCol;
                else if (e.DirectiveValue == "DevCol")
                    cellText = dict.DevCol;
                else if (e.DirectiveValue == "CnlCol")
                    cellText = dict.CnlCol;
                else if (e.DirectiveValue == "DescrCol")
                    cellText = dict.DescrCol;
                else if (e.DirectiveValue == "SevCol")
                    cellText = dict.SevCol;
                else if (e.DirectiveValue == "AckCol")
                    cellText = dict.AckCol;
            }
            else if (e.Stage == ProcessingStage.Postprocessing)
            {
                if (e.DirectiveValue == "Time")
                    cellText = currentEventF.Time;
                else if (e.DirectiveValue == "Obj")
                    cellText = currentEventF.Obj;
                else if (e.DirectiveValue == "Dev")
                    cellText = currentEventF.Dev;
                else if (e.DirectiveValue == "Cnl")
                    cellText = currentEventF.Cnl;
                else if (e.DirectiveValue == "Descr")
                    cellText = currentEventF.Descr;
                else if (e.DirectiveValue == "Sev")
                    cellText = currentEventF.Sev;
                else if (e.DirectiveValue == "Ack")
                    cellText = currentEventF.Ack;

                if (!string.IsNullOrEmpty(currentEventF.Color))
                    renderer.Workbook.SetColor(e.Cell.Node, null, currentEventF.Color);
            }

            if (cellText != null)
                e.Cell.Text = cellText;
        }
    }
}
