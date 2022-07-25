// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Report.Xml2003.Excel
{
    /// <summary>
    /// Renders an Excel workbook in SpreadsheetML format from a template.
    /// <para>Генерирует книгу Excel в формате SpreadsheetML из шаблона.</para>
    /// </summary>
    public class WorkbookRenderer : Xml2003Renderer
    {
        /// <summary>
        /// The name of an XML element that can contain a report directive.
        /// </summary>
        protected const string DirectiveElem = "Data";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WorkbookRenderer()
            : base()
        {
        }


        /// <summary>
        /// Gets or sets the processed worksheet.
        /// </summary>
        protected Worksheet ProcessedWorksheet { get; set; }

        /// <summary>
        /// Gets or sets the processed table of the worksheet.
        /// </summary>
        protected Table ProcessedTable { get; set; }

        /// <summary>
        /// Gets or sets the processed row of the table.
        /// </summary>
        protected Row ProcessedRow { get; set; }

        /// <summary>
        /// Gets or sets the processed cell of the row.
        /// </summary>
        protected Cell ProcessedCell { get; set; }

        /// <summary>
        /// Gets the rendered Excel workbook.
        /// </summary>
        public Workbook Workbook { get; protected set; }


        /// <summary>
        /// Resets the fields before processing.
        /// </summary>
        protected override void ResetFields()
        {
            base.ResetFields();
            ProcessedWorksheet = null;
            ProcessedTable = null;
            ProcessedRow = null;
            ProcessedCell = null;
            Workbook = null;
        }


        /// <summary>
        /// Processes the XML document recursively.
        /// </summary>
        protected override void ProcessXmlDoc(XmlNode xmlNode)
        {
            if (xmlNode.Name == DirectiveElem)
            {
                ProcessedCell.DataNode = xmlNode;
                FindDirectives(ProcessedCell); // find and process directives in the cell
                OnNodeProcessed(xmlNode);
            }
            else
            {
                // retrieve workbook elements
                if (xmlNode.Name == "Workbook")
                {
                    Workbook = new Workbook(xmlNode);
                }
                else if (xmlNode.Name == "Styles")
                {
                    Workbook.StylesNode = xmlNode;
                }
                else if (xmlNode.Name == "Style")
                {
                    Style style = new(xmlNode);
                    Workbook.Styles.Add(style.ID, style);
                }
                else if (xmlNode.Name == "Worksheet")
                {
                    ProcessedWorksheet = new Worksheet(xmlNode) { ParentWorkbook = Workbook };
                    Workbook.Worksheets.Add(ProcessedWorksheet);
                }
                else if (xmlNode.Name == "Table")
                {
                    ProcessedTable = new Table(xmlNode) { ParentWorksheet = ProcessedWorksheet };
                    ProcessedWorksheet.Table = ProcessedTable;
                }
                else if (xmlNode.Name == "Column")
                {
                    ProcessedTable.Columns.Add(new Column(xmlNode) { ParentTable = ProcessedTable });
                }
                else if (xmlNode.Name == "Row")
                {
                    ProcessedRow = new Row(xmlNode) { ParentTable = ProcessedTable };
                    ProcessedTable.Rows.Add(ProcessedRow);
                }
                else if (xmlNode.Name == "Cell")
                {
                    ProcessedCell = new Cell(xmlNode) { ParentRow = ProcessedRow };
                    ProcessedRow.Cells.Add(ProcessedCell);
                }

                OnNodeProcessed(xmlNode);

                // process child nodes
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    ProcessXmlDoc(node);
                }
            }
        }

        /// <summary>
        /// Finds and processes directives of the specified cell.
        /// </summary>
        protected void FindDirectives(Cell cell)
        {
            void InvokeDirectiveFound(string dirName, string dirVal)
            {
                OnDirectiveFound(new ExcelDirectiveEventArgs
                {
                    Stage = Stage,
                    DirectiveName = dirName,
                    DirectiveValue = dirVal,
                    Node = cell.DataNode,
                    Cell = cell
                });
            }

            if (cell.DataNode is XmlNode dataNode)
            {
                if (FindDirective(dataNode.InnerText, ReportDirectives.RepRow, out string val, out string rest))
                {
                    dataNode.InnerText = rest;
                    InvokeDirectiveFound(ReportDirectives.RepRow, val);
                }
                else if (FindDirective(dataNode.InnerText, ReportDirectives.RepVal, out val, out _))
                {
                    InvokeDirectiveFound(ReportDirectives.RepVal, val);
                }
            }
        }


        /// <summary>
        /// Process directives in the specified Excel table.
        /// </summary>
        public void ProcessTable(Table table)
        {
            foreach (Row row in table.Rows)
            {
                ProcessRow(row);
            }
        }

        /// <summary>
        /// Process directives in the specified Excel row.
        /// </summary>
        public void ProcessRow(Row row)
        {
            foreach (Cell cell in row.Cells)
            {
                FindDirectives(cell);
            }
        }
    }
}
