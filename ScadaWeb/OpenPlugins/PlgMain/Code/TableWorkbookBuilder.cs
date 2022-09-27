// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Report.Xml2003;
using Scada.Report.Xml2003.Excel;
using System.Globalization;
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
        /// <summary>
        /// Indicates a column for further data writing.
        /// </summary>
        private const string NextTimeSymbol = "*";

        private readonly ConfigDatabase configDatabase;
        private readonly ScadaClient scadaClient;
        private readonly string templateFilePath;
        private readonly WorkbookRenderer renderer;
        private readonly dynamic dict;

        private TableWorkbookArgs workbookArgs;
        private Archive archiveEntity;
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
            GenerateTime = DateTime.UtcNow;
        }

        private void Renderer_AfterProcessing(object sender, XmlDocument e)
        {
            if (dataColCellTemplate != null &&
                dataCellTemplate != null)
            {
                // prepare data
                TrendBundle trendBundle = scadaClient.GetTrends(
                    archiveEntity.Bit, workbookArgs.TimeRange, workbookArgs.TableView.CnlNumList.ToArray());
                Dictionary<int, int> cnlNumMap = MapCnlNums(trendBundle);
                List<ColumnMeta> columnMetas = GetColumnMetas(trendBundle);
                CnlDataFormatter formatter = new(configDatabase, workbookArgs.TimeZone)
                {
                    Culture = CultureInfo.InvariantCulture
                };

                // modify workbook
                Table table = dataColCellTemplate.ParentRow.ParentTable;
                table.RemoveUnwantedAttrs();
                table.ParentWorksheet.Name = dict.WorksheetName;
                table.Columns.Last().Span = columnMetas.Count - 1;

                // header row
                Row headerRow = dataColCellTemplate.ParentRow;
                headerRow.RemoveCell(dataColCellTemplate);
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
                Row itemRowTemplate = dataCellTemplate.ParentRow;
                itemRowTemplate.RemoveCell(dataCellTemplate);
                table.RemoveRow(itemRowTemplate);

                foreach (TableItem tableItem in workbookArgs.TableView.VisibleItems)
                {
                    currentTableItem = tableItem;
                    Row itemRow = itemRowTemplate.Clone();
                    renderer.ProcessRow(itemRow);

                    TrendBundle.CnlDataList trend = cnlNumMap.TryGetValue(tableItem.CnlNum, out int cnlIdx)
                        ? trendBundle.Trends[cnlIdx] 
                        : null;
                    Cnl itemCnl = tableItem.Cnl;
                    bool showVal = trend != null && itemCnl != null && itemCnl.IsArchivable();
                    ColumnMeta prevColumnMeta = null;

                    foreach (ColumnMeta columnMeta in columnMetas)
                    {
                        Cell dataCell = dataCellTemplate.Clone();

                        if (showVal)
                        {
                            if (columnMeta.PointIndex >= 0)
                            {
                                CnlData cnlData = trend[columnMeta.PointIndex];
                                CnlDataFormatted cnlDataF = formatter.FormatCnlData(cnlData, itemCnl, false);
                                dataCell.Text = cnlDataF.DispVal;

                                if (formatter.LastResultInfo.IsFloat)
                                    dataCell.SetNumberType();

                                if (cnlDataF.Colors.Length > 0)
                                    renderer.Workbook.SetColor(dataCell.Node, null, cnlDataF.Colors[0]);
                            }
                            else
                            {
                                dataCell.Text = prevColumnMeta != null && 
                                    prevColumnMeta.UtcTime < GenerateTime && GenerateTime <= columnMeta.UtcTime
                                    ? NextTimeSymbol 
                                    : "";
                            }
                        }
                        else
                        {
                            dataCell.Text = "";
                        }

                        itemRow.AppendCell(dataCell);
                        prevColumnMeta = columnMeta;
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
                if (e.DirectiveValue == "Title")
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
