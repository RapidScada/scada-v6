// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Report.Xml2003.Excel
{
    /// <summary>
    /// Represents a worksheet of Excel workbook.
    /// <para>Представляет лист книги Excel.</para>
    /// </summary>
    public class Worksheet
    {
        /// <summary>
        /// Ссылка на XML-узел, соответствующий листу книги Excel.
        /// </summary>
        protected XmlNode node;
        /// <summary>
        /// Имя листа.
        /// </summary>
        protected string name;
        /// <summary>
        /// Таблица с содержимым листа.
        /// </summary>
        protected Table table;
        /// <summary>
        /// Родительская книга данного листа.
        /// </summary>
        protected Workbook parentWorkbook;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="xmlNode">Ссылка на XML-узел, соответствующий листу книги Excel.</param>
        public Worksheet(XmlNode xmlNode)
        {
            node = xmlNode;
            name = xmlNode.Attributes["ss:Name"].Value;
            table = null;
        }


        /// <summary>
        /// Получить ссылку на XML-узел, соответствующий листу книги Excel.
        /// </summary>
        public XmlNode Node
        {
            get
            {
                return node;
            }
        }

        /// <summary>
        /// Получить или установить имя листа, при установке модифицируется дерево XML-документа.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                node.Attributes["ss:Name"].Value = name;
            }
        }

        /// <summary>
        /// Получить или установить таблицу с содержимым листа.
        /// </summary>
        public Table Table
        {
            get
            {
                return table;
            }
            set
            {
                table = value;
                table.ParentWorksheet = this;
            }
        }

        /// <summary>
        /// Получить или установить родительскую книгу данного листа.
        /// </summary>
        public Workbook ParentWorkbook
        {
            get
            {
                return parentWorkbook;
            }
            set
            {
                parentWorkbook = value;
            }
        }


        /// <summary>
        /// Split the worksheet into panes horizontally.
        /// </summary>
        public void SplitHorizontal(int rowIndex)
        {
            XmlDocument xmlDoc = node.OwnerDocument;
            XmlNamespaceManager nsmgr = new(new NameTable());
            nsmgr.AddNamespace("report", XmlNamespaces.X);

            if (node.SelectSingleNode("report:WorksheetOptions", nsmgr) is XmlNode optionsNode)
            {
                string rowIndexStr = rowIndex.ToString();

                XmlNode splitNode = optionsNode.SelectSingleNode("report:SplitHorizontal", nsmgr);
                if (splitNode == null)
                {
                    splitNode = xmlDoc.CreateElement("SplitHorizontal");
                    optionsNode.AppendChild(splitNode);
                }
                splitNode.InnerText = rowIndexStr;

                XmlNode paneNode = optionsNode.SelectSingleNode("report:TopRowBottomPane", nsmgr);
                if (paneNode == null)
                {
                    paneNode = xmlDoc.CreateElement("TopRowBottomPane");
                    optionsNode.AppendChild(paneNode);
                }
                paneNode.InnerText = rowIndexStr;
            }
        }

        /// <summary>
        /// Clones the worksheet.
        /// </summary>
        public Worksheet Clone()
        {
            XmlNode nodeClone = node.CloneNode(false);
            Worksheet worksheetClone = new(nodeClone);

            Table tableClone = table.Clone();
            worksheetClone.table = tableClone;
            nodeClone.AppendChild(tableClone.Node);

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name != "Table")
                    nodeClone.AppendChild(childNode.Clone());
            }

            return worksheetClone;
        }
    }
}
