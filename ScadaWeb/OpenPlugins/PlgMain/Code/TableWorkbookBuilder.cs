// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Report.Xml2003;
using Scada.Report.Xml2003.Excel;
using System.Xml;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Builds an Excel workbook that contains a table view.
    /// <para>Создает книгу Excel, которая содержит табличное представление.</para>
    /// </summary>
    internal class TableWorkbookBuilder
    {
        /// <summary>
        /// The workbook template file name.
        /// </summary>
        private const string TemplateFileName = "TableViewTemplate.xml";

        private readonly ConfigDatabase configDatabase;
        private readonly ScadaClient scadaClient;
        private readonly string templateFilePath;
        private readonly WorkbookRenderer renderer;
        private readonly dynamic dict;

        private TableWorkbookArgs workbookArgs;
        private Archive archiveEntity;
        private Row itemRowTemplate;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableWorkbookBuilder(ConfigDatabase configDatabase, ScadaClient scadaClient, string templateDir)
        {
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
            this.scadaClient = scadaClient ?? throw new ArgumentNullException(nameof(scadaClient));
            templateFilePath = Path.Combine(templateDir, TemplateFileName);
            renderer = new WorkbookRenderer();
            renderer.BeforeProcessing += Renderer_BeforeProcessing;
            renderer.AfterProcessing += Renderer_AfterProcessing;
            renderer.DirectiveFound += Renderer_DirectiveFound;
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Code.TableWorkbookBuilder");

            workbookArgs = null;
            archiveEntity = null;
            itemRowTemplate = null;

            GenerateTime = DateTime.MinValue;
        }


        /// <summary>
        /// Gets the time when a workbook was last built, UTC.
        /// </summary>
        public DateTime GenerateTime { get; private set; }


        /// <summary>
        /// Generates a workbook to the output stream.
        /// </summary>
        public void Generate(TableWorkbookArgs args, Stream outStream)
        {
            workbookArgs = args ?? throw new ArgumentNullException(nameof(args));
            workbookArgs.Validate();
            ArgumentNullException.ThrowIfNull(outStream, nameof(outStream));

            archiveEntity = configDatabase.ArchiveTable.SelectFirst(
                new TableFilter("Code", args.TableOptions.ArchiveCode));

            if (archiveEntity == null)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Архив не найдён в базе конфигурации." :
                    "Archive not found in the configuration database.");
            }

            renderer.Render(templateFilePath, outStream);
        }

        private void Renderer_BeforeProcessing(object sender, XmlDocument e)
        {
            itemRowTemplate = null;
            GenerateTime = DateTime.UtcNow;
        }

        private void Renderer_AfterProcessing(object sender, XmlDocument e)
        {
            /*if (eventRowTemplate != null)
            {
                // select events
                TimeRange timeRange = new(GenerateTime.AddDays(-workbookArgs.EventDepth), GenerateTime, true);
                EventFilter filter = workbookArgs.View == null
                    ? new EventFilter(workbookArgs.EventCount)
                    : new EventFilter(workbookArgs.EventCount, workbookArgs.View);
                List<Event> events = scadaClient.GetEvents(archiveEntity.Bit, timeRange, filter, false, out _);
                CnlDataFormatter formatter = new(configDatabase, configDatabase.Enums, workbookArgs.TimeZone);

                // modify workbook
                Table eventTable = eventRowTemplate.ParentTable;
                eventTable.RemoveUnwantedAttrs();
                eventTable.ParentWorksheet.Name = dict.WorksheetName;

                int rowIndex = eventTable.Rows.IndexOf(eventRowTemplate);
                eventTable.RemoveRow(rowIndex);

                foreach (Event ev in events)
                {
                    currentEventF = formatter.FormatEvent(ev);
                    Row eventRow = eventRowTemplate.Clone();
                    renderer.ProcessRow(eventRow);
                    eventTable.AppendRow(eventRow);
                }
            }*/
        }

        private void Renderer_DirectiveFound(object sender, ExcelDirectiveEventArgs e)
        {
            string cellText = null;

            if (e.Stage == ProcessingStage.Processing)
            {
                if (e.DirectiveName == ReportDirectives.RepRow && e.DirectiveValue == "Item")
                    itemRowTemplate = e.Cell.ParentRow;
                else if (e.DirectiveValue == "Title")
                    cellText = workbookArgs.TableView.Title;
                else if (e.DirectiveValue == "GenCaption")
                    cellText = dict.GenCaption;
                else if (e.DirectiveValue == "Gen")
                    cellText = TimeZoneInfo.ConvertTimeFromUtc(GenerateTime, workbookArgs.TimeZone).ToLocalizedString();
                else if (e.DirectiveValue == "ArcCaption")
                    cellText = dict.ArcCaption;
                else if (e.DirectiveValue == "Arc")
                    cellText = archiveEntity.Name;
                else if (e.DirectiveValue == "TzCaption")
                    cellText = dict.TzCaption;
                else if (e.DirectiveValue == "Tz")
                    cellText = workbookArgs.TimeZone.DisplayName;
                else if (e.DirectiveValue == "ItemCol")
                    cellText = dict.ItemCol;
            }
            else if (e.Stage == ProcessingStage.Postprocessing)
            {
                /*if (e.DirectiveValue == "Time")
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
                    cellText = currentEventF.Ack;*/
            }

            if (cellText != null)
                e.Cell.Text = cellText;
        }
    }
}
