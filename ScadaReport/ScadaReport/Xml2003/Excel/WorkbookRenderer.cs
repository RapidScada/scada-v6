// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Report.Xml2003.Excel
{
    /// <summary>
    /// Renders an Excel workbook in SpreadsheetML format from a template.
    /// <para>Генерирует книгу Excel в формате SpreadsheetML из шаблона.</para>
    /// </summary>
    public class WorkbookRenderer
    {
        /// <summary>
        /// The name of an XML element that can contain a report directive.
        /// </summary>
        protected const string DirectiveElem = "Data";


        /// <summary>
        /// The current processing stage.
        /// </summary>
        protected ProcessingStage stage;
        /// <summary>
        /// The processed worksheet.
        /// </summary>
        protected Worksheet processedWorksheet;
        /// <summary>
        /// The processed table of the worksheet.
        /// </summary>
        protected Table processedTable;
        /// <summary>
        /// The processed row of the table.
        /// </summary>
        protected Row processedRow;
        /// <summary>
        /// The processed cell of the row.
        /// </summary>
        protected Cell processedCell;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WorkbookRenderer()
            : base()
        {
            ResetFields();
        }


        /// <summary>
        /// Gets the rendered XML document.
        /// </summary>
        public XmlDocument XmlDoc { get; protected set; }

        /// <summary>
        /// Gets the rendered Excel workbook.
        /// </summary>
        public Workbook Workbook { get; protected set; }


        /// <summary>
        /// Finds the specified directive in the string, provides the directive value and the rest of the string.
        /// </summary>
        protected static bool FindDirective(string s, string name, out string val, out string rest)
        {
            // "name=val", instead of '=' can be any character
            int valStartInd = name.Length + 1;

            if (valStartInd <= s.Length && s.StartsWith(name, StringComparison.Ordinal))
            {
                int valEndInd = s.IndexOf(" ", valStartInd);

                if (valEndInd < 0)
                {
                    val = s[valStartInd..];
                    rest = "";
                }
                else
                {
                    val = s[valStartInd..valEndInd];
                    rest = s[(valEndInd + 1)..];
                }

                return true;
            }
            else
            {
                val = "";
                rest = s;
                return false;
            }
        }

        /// <summary>
        /// Resets the fields before processing.
        /// </summary>
        protected void ResetFields()
        {
            stage = ProcessingStage.NotStarted;
            processedWorksheet = null;
            processedTable = null;
            processedRow = null;
            processedCell = null;

            XmlDoc = null;
            Workbook = null;
        }


        /// <summary>
        /// Processes the XML document recursively.
        /// </summary>
        protected void ProcessXmlDoc(XmlNode xmlNode)
        {
            if (xmlNode.Name == DirectiveElem)
            {
                processedCell.DataNode = xmlNode;
                FindDirectives(processedCell); // find and process directives in the cell
                NodeProcessed?.Invoke(this, xmlNode);
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
                    processedWorksheet = new Worksheet(xmlNode) { ParentWorkbook = Workbook };
                    Workbook.Worksheets.Add(processedWorksheet);
                }
                else if (xmlNode.Name == "Table")
                {
                    processedTable = new Table(xmlNode) { ParentWorksheet = processedWorksheet };
                    processedWorksheet.Table = processedTable;
                }
                else if (xmlNode.Name == "Column")
                {
                    processedTable.Columns.Add(new Column(xmlNode) { ParentTable = processedTable });
                }
                else if (xmlNode.Name == "Row")
                {
                    processedRow = new Row(xmlNode) { ParentTable = processedTable };
                    processedTable.Rows.Add(processedRow);
                }
                else if (xmlNode.Name == "Cell")
                {
                    processedCell = new Cell(xmlNode) { ParentRow = processedRow };
                    processedRow.Cells.Add(processedCell);
                }

                NodeProcessed?.Invoke(this, xmlNode);

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
            if (cell.DataNode is XmlNode dataNode)
            {
                if (FindDirective(dataNode.InnerText, ReportDirectives.RepRow, out string val, out string rest))
                {
                    dataNode.InnerText = rest;
                    DirectiveFound?.Invoke(this, new DirectiveEventArgs
                    {
                        Stage = stage,
                        Cell = cell,
                        DirectiveName = ReportDirectives.RepRow,
                        DirectiveValue = val
                    });
                }
                else if (FindDirective(dataNode.InnerText, ReportDirectives.RepVal, out val, out _))
                {
                    DirectiveFound?.Invoke(this, new DirectiveEventArgs
                    {
                        Stage = stage,
                        Cell = cell,
                        DirectiveName = ReportDirectives.RepVal,
                        DirectiveValue = val
                    });
                }
            }
        }


        /// <summary>
        /// Renders a workbook to the output stream.
        /// </summary>
        public void Render(string templateFileName, Stream outStream)
        {
            ArgumentNullException.ThrowIfNull(templateFileName, nameof(templateFileName));
            ArgumentNullException.ThrowIfNull(outStream, nameof(outStream));
            ResetFields();

            // load and parse XML template
            XmlDoc = new XmlDocument();
            XmlDoc.Load(templateFileName);

            // render report by modifying XML document
            stage = ProcessingStage.Preprocessing;
            BeforeProcessing?.Invoke(this, XmlDoc);

            stage = ProcessingStage.Processing;
            ProcessXmlDoc(XmlDoc.DocumentElement);

            stage = ProcessingStage.Postprocessing;
            AfterProcessing?.Invoke(this, XmlDoc);
            stage = ProcessingStage.Completed;

            // write report to output stream
            XmlDoc.Save(outStream);
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


        /// <summary>
        /// Occurs before processing an XML document.
        /// </summary>
        public event EventHandler<XmlDocument> BeforeProcessing;

        /// <summary>
        /// Occurs after processing an XML document.
        /// </summary>
        public event EventHandler<XmlDocument> AfterProcessing;

        /// <summary>
        /// Occurs when an XML node is processed.
        /// </summary>
        public event EventHandler<XmlNode> NodeProcessed;

        /// <summary>
        /// Occurs when a report directive is found.
        /// </summary>
        public event EventHandler<DirectiveEventArgs> DirectiveFound;
    }
}
