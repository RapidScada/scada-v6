// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Entities;
using Scada.Data.Models;
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
        /// Represents metadata about a column.
        /// </summary>
        private class ColumnMeta
        {
            public DateTime UtcTime { get; set; }
            public string ShortDate { get; set; }
            public string ShortTime { get; set; }
            public int PointIndex { get; set; }
        }

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
        private Row headerRow;
        private Row itemRowTemplate;
        private Cell dataColCellTemplate;
        private Cell dataCellTemplate;
        private TableItem currentTableItem;


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
            headerRow = null;
            itemRowTemplate = null;
            dataColCellTemplate = null;
            dataCellTemplate = null;
            currentTableItem = null;

            GenerateTime = DateTime.MinValue;
        }


        /// <summary>
        /// Gets the time when a workbook was last built, UTC.
        /// </summary>
        public DateTime GenerateTime { get; private set; }


        /// <summary>
        /// Gets the workbook title.
        /// </summary>
        private string GetTitle()
        {
            DateTime userStartTime = TimeZoneInfo.ConvertTimeFromUtc(workbookArgs.TimeRange.StartTime, workbookArgs.TimeZone);
            DateTime userEndTime = TimeZoneInfo.ConvertTimeFromUtc(workbookArgs.TimeRange.EndTime, workbookArgs.TimeZone);
            return string.Format(dict.TitleFormat,
                workbookArgs.TableView.Title,
                userStartTime.ToLocalizedString(),
                userEndTime.ToLocalizedString());
        }

        /// <summary>
        /// Creates a map of channel indexes accessed by channel number.
        /// </summary>
        private static Dictionary<int, int> MapCnlNums(TrendBundle trendBundle)
        {
            Dictionary<int, int> map = new();
            int idx = 0;

            foreach (int cnlNum in trendBundle.CnlNums)
            {
                map[cnlNum] = idx;
                idx++;
            }

            return map;

        }

        /// <summary>
        /// Gets metadata about the columns.
        /// </summary>
        private List<ColumnMeta> GetColumnMetas(TrendBundle trendBundle)
        {
            List<ColumnMeta> columnMetas = new();
            DateTime curDT = workbookArgs.TimeRange.StartTime;
            int tablePeriod = workbookArgs.TableOptions.Period <= 0
                ? TableOptions.DefaultPeriod
                : workbookArgs.TableOptions.Period;
            int pointIndex = 0;

            while (curDT <= workbookArgs.TimeRange.EndTime)
            {
                DateTime userCurDT = TimeZoneInfo.ConvertTimeFromUtc(curDT, workbookArgs.TimeZone);
                columnMetas.Add(new ColumnMeta
                {
                    UtcTime = curDT,
                    ShortDate = userCurDT.ToString("m", Locale.Culture),
                    ShortTime = userCurDT.ToString("t", Locale.Culture),
                    PointIndex = FindPointIndex(trendBundle, curDT, ref pointIndex)
                });
                curDT = curDT.AddMinutes(tablePeriod);
            }

            return columnMetas;
        }

        /// <summary>
        /// Finds a point index in the trend bundle. Returns -1 if the index is not found.
        /// </summary>
        private static int FindPointIndex(TrendBundle trendBundle, DateTime timestamp, ref int currentIndex)
        {
            while (currentIndex < trendBundle.Timestamps.Count)
            {
                DateTime pointTimestamp = trendBundle.Timestamps[currentIndex];

                if (timestamp == pointTimestamp)
                    return currentIndex++;
                else if (timestamp < pointTimestamp)
                    return -1;

                currentIndex++;
            }

            return -1;
        }


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
            if (headerRow != null && 
                itemRowTemplate != null &&
                dataColCellTemplate != null &&
                dataCellTemplate != null)
            {
                // prepare data
                TrendBundle trendBundle = scadaClient.GetTrends(
                    archiveEntity.Bit, workbookArgs.TimeRange, workbookArgs.TableView.CnlNumList.ToArray());
                Dictionary<int, int> cnlNumMap = MapCnlNums(trendBundle);
                List<ColumnMeta> columnMetas = GetColumnMetas(trendBundle);
                CnlDataFormatter formatter = new(configDatabase, configDatabase.Enums, workbookArgs.TimeZone);

                // modify workbook
                Table table = headerRow.ParentTable;
                table.RemoveUnwantedAttrs();
                table.ParentWorksheet.Name = dict.WorksheetName;
                table.Columns.Last().Span = columnMetas.Count - 1;

                // header row
                int cellIndex = headerRow.Cells.IndexOf(dataColCellTemplate);
                headerRow.RemoveCell(cellIndex);
                bool showDate = columnMetas.First().ShortDate != columnMetas.Last().ShortDate;

                foreach (ColumnMeta columnMeta in columnMetas)
                {
                    Cell dataColCell = dataColCellTemplate.Clone();
                    dataColCell.Text = showDate 
                        ? columnMeta.ShortDate + " " + columnMeta.ShortTime 
                        : columnMeta.ShortTime;
                    headerRow.AppendCell(dataColCell);
                }

                // table view items
                cellIndex = itemRowTemplate.Cells.IndexOf(dataCellTemplate);
                itemRowTemplate.RemoveCell(cellIndex);

                int rowIndex = table.Rows.IndexOf(itemRowTemplate);
                table.RemoveRow(rowIndex);

                foreach (TableItem tableItem in workbookArgs.TableView.VisibleItems)
                {
                    currentTableItem = tableItem;
                    Row itemRow = itemRowTemplate.Clone();
                    renderer.ProcessRow(itemRow);

                    TrendBundle.CnlDataList trend = cnlNumMap.TryGetValue(tableItem.CnlNum, out int cnlIdx)
                        ? trendBundle.Trends[cnlIdx] 
                        : null;

                    foreach (ColumnMeta columnMeta in columnMetas)
                    {
                        Cell dataCell = dataCellTemplate.Clone();

                        if (trend == null || columnMeta.PointIndex < 0)
                        {
                            dataCell.Text = "";
                        }
                        else
                        {
                            CnlData cnlData = trend[columnMeta.PointIndex];
                            CnlDataFormatted cnlDataF = formatter.FormatCnlData(cnlData, tableItem.Cnl, false);
                            dataCell.Text = cnlDataF.DispVal;
                        }

                        itemRow.AppendCell(dataCell);
                    }

                    table.AppendRow(itemRow);
                }
            }
        }

        private void Renderer_DirectiveFound(object sender, ExcelDirectiveEventArgs e)
        {
            string cellText = null;

            if (e.Stage == ProcessingStage.Processing)
            {
                if (e.DirectiveName == ReportDirectives.RepRow)
                {
                    if (e.DirectiveValue == "Header")
                        headerRow = e.Cell.ParentRow;
                    else if (e.DirectiveValue == "Item")
                        itemRowTemplate = e.Cell.ParentRow;
                }
                else if (e.DirectiveValue == "Title")
                    cellText = GetTitle();
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
                else if (e.DirectiveValue == "DataCol")
                    dataColCellTemplate = e.Cell;
                else if (e.DirectiveValue == "Data")
                    dataCellTemplate = e.Cell;
            }
            else if (e.Stage == ProcessingStage.Postprocessing)
            {
                if (e.DirectiveValue == "Name")
                    cellText = currentTableItem.Text;
            }

            if (cellText != null)
                e.Cell.Text = cellText;
        }
    }
}
