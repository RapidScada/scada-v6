// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Report.Excel2003
{
    /// <summary>
    /// Represents a table of Excel worksheet.
    /// <para>Представляет таблицу листа Excel.</para>
    /// </summary>
    internal class Table
    {
        /// <summary>
        /// Ссылка на XML-узел, соответствующий таблице листа Excel.
        /// </summary>
        protected XmlNode node;
        /// <summary>
        /// Список столбцов таблицы.
        /// </summary>
        protected List<Column> columns;
        /// <summary>
        /// Список строк таблицы.
        /// </summary>
        protected List<Row> rows;
        /// <summary>
        /// Родительский лист данной таблицы.
        /// </summary>
        protected Worksheet parentWorksheet;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="xmlNode">Ссылка на XML-узел, соответствующий таблице листа Excel.</param>
        public Table(XmlNode xmlNode)
        {
            node = xmlNode;
            columns = new List<Column>();
            rows = new List<Row>();
            parentWorksheet = null;
        }


        /// <summary>
        /// Получить ссылку на XML-узел, соответствующий таблице листа Excel.
        /// </summary>
        public XmlNode Node
        {
            get
            {
                return node;
            }
        }

        /// <summary>
        /// Получить список столбцов таблицы.
        /// </summary>
        public List<Column> Columns
        {
            get
            {
                return columns;
            }
        }

        /// <summary>
        /// Получить список строк таблицы.
        /// </summary>
        public List<Row> Rows
        {
            get
            {
                return rows;
            }
        }

        /// <summary>
        /// Получить или установить родительский лист данной таблицы.
        /// </summary>
        public Worksheet ParentWorksheet
        {
            get
            {
                return parentWorksheet;
            }
            set
            {
                parentWorksheet = value;
            }
        }


        /// <summary>
        /// Удалить атрибуты XML-узла таблицы, необходимые для корректного отображения книги Excel.
        /// </summary>
        public void RemoveTableNodeAttrs()
        {
            node.Attributes.RemoveNamedItem("ss:ExpandedColumnCount");
            node.Attributes.RemoveNamedItem("ss:ExpandedRowCount");
        }

        /// <summary>
        /// Найти столбец в таблице по индексу.
        /// </summary>
        /// <param name="columnIndex">Индекс искомого столбца, начиная с 1.</param>
        /// <returns>Столбец, удовлетворяющий критерию поиска, или null, если столбец не найден.</returns>
        public Column FindColumn(int columnIndex)
        {
            int index = 0;
            foreach (Column column in columns)
            {
                index = column.Index > 0 ? column.Index : index + 1;
                int endIndex = index + column.Span;

                if (index <= columnIndex && columnIndex <= endIndex)
                    return column;

                index = endIndex;
            }

            return null;
        }

        /// <summary>
        /// Добавить столбец в конец списка столбцов таблицы и модифицировать дерево XML-документа.
        /// </summary>
        public void AppendColumn(Column column)
        {
            if (columns.Count > 0)
                node.InsertAfter(column.Node, columns[columns.Count - 1].Node);
            else
                node.PrependChild(column.Node);

            column.ParentTable = this;
            columns.Add(column);
        }

        /// <summary>
        /// Вставить столбец в список столбцов таблицы и модифицировать дерево XML-документа.
        /// </summary>
        public void InsertColumn(int listIndex, Column column)
        {
            if (columns.Count == 0 || listIndex == 0)
                node.PrependChild(column.Node);
            else
                node.InsertAfter(column.Node, columns[listIndex - 1].Node);

            column.ParentTable = this;
            columns.Insert(listIndex, column);
        }

        /// <summary>
        /// Удалить столбец из списка столбцов таблицы и модифицировать дерево XML-документа.
        /// </summary>
        public void RemoveColumn(int listIndex)
        {
            Column column = columns[listIndex];
            column.ParentTable = null;
            node.RemoveChild(column.Node);
            columns.RemoveAt(listIndex);
        }

        /// <summary>
        /// Удалить все столбцы из списка столбцов таблицы и модифицировать дерево XML-документа.
        /// </summary>
        public void RemoveAllColumns()
        {
            while (columns.Count > 0)
            {
                RemoveColumn(0);
            }
        }

        /// <summary>
        /// Добавить строку в конец списка строк таблицы и модифицировать дерево XML-документа.
        /// </summary>
        public void AppendRow(Row row)
        {
            node.AppendChild(row.Node);
            row.ParentTable = this;
            rows.Add(row);
        }

        /// <summary>
        /// Вставить строку в список строк таблицы и модифицировать дерево XML-документа.
        /// </summary>
        public void InsertRow(int listIndex, Row row)
        {
            if (rows.Count == 0)
                node.AppendChild(row.Node);
            else if (listIndex == 0)
                node.InsertBefore(row.Node, rows[0].Node);
            else
                node.InsertAfter(row.Node, rows[listIndex - 1].Node);

            row.ParentTable = this;
            rows.Insert(listIndex, row);
        }

        /// <summary>
        /// Удалить строку из списка строк таблицы и модифицировать дерево XML-документа.
        /// </summary>
        public void RemoveRow(int listIndex)
        {
            Row row = rows[listIndex];
            row.ParentTable = null;
            node.RemoveChild(row.Node);
            rows.RemoveAt(listIndex);
        }

        /// <summary>
        /// Удалить все строки из списка строк таблицы и модифицировать дерево XML-документа.
        /// </summary>
        public void RemoveAllRows()
        {
            while (rows.Count > 0)
            {
                RemoveRow(0);
            }
        }

        /// <summary>
        /// Клонировать таблицу.
        /// </summary>
        /// <returns>Копия таблицы.</returns>
        public Table Clone()
        {
            XmlNode nodeClone = node.CloneNode(false);
            Table tableClone = new(nodeClone)
            {
                parentWorksheet = parentWorksheet
            };

            foreach (Column column in columns)
            {
                Column columnClone = column.Clone();
                tableClone.columns.Add(columnClone);
                nodeClone.AppendChild(columnClone.Node);
            }

            foreach (Row row in rows)
            {
                Row rowClone = row.Clone();
                tableClone.rows.Add(rowClone);
                nodeClone.AppendChild(rowClone.Node);
            }

            return tableClone;
        }
    }
}
