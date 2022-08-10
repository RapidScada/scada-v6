// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using System.Xml;

namespace Scada.Report.Xml2003.Excel
{
    /// <summary>
    /// Represents a row of Excel table.
    /// <para>Представляет строку таблицы Excel.</para>
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Ссылка на XML-узел, соответствующий строке таблицы Excel.
        /// </summary>
        protected XmlNode node;
        /// <summary>
        /// Список ячеек строки
        /// </summary>
        protected List<Cell> cells;
        /// <summary>
        /// Родительская таблица данной строки.
        /// </summary>
        protected Table parentTable;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="xmlNode">Ссылка на XML-узел, соответствующий строке таблицы Excel.</param>
        public Row(XmlNode xmlNode)
        {
            node = xmlNode;
            cells = new List<Cell>();
            parentTable = null;
        }


        /// <summary>
        /// Получить ссылку на XML-узел, соответствующий строке таблицы Excel.
        /// </summary>
        public XmlNode Node
        {
            get
            {
                return node;
            }
        }

        /// <summary>
        /// Получить список ячеек строки.
        /// </summary>
        public List<Cell> Cells
        {
            get
            {
                return cells;
            }
        }

        /// <summary>
        /// Получить или установить родительскую таблицу данной строки.
        /// </summary>
        public Table ParentTable
        {
            get
            {
                return parentTable;
            }
            set
            {
                parentTable = value;
            }
        }

        /// <summary>
        /// Получить или установить высоту.
        /// </summary>
        public double Height
        {
            get
            {
                string heightStr = ExcelUtils.GetAttribute(node, "ss:Height");
                return double.TryParse(heightStr, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double height) ?
                    height : 0;
            }
            set
            {
                SetRowHeight(node, value);
            }
        }


        /// <summary>
        /// Клонировать строку.
        /// </summary>
        /// <returns>Копия строки.</returns>
        public Row Clone()
        {
            XmlNode nodeClone = node.CloneNode(false);
            Row rowClone = new(nodeClone)
            {
                parentTable = parentTable
            };

            foreach (Cell cell in cells)
            {
                Cell cellClone = cell.Clone();
                rowClone.cells.Add(cellClone);
                nodeClone.AppendChild(cellClone.Node);
            }

            return rowClone;
        }

        /// <summary>
        /// Найти ячейку в строке по индексу.
        /// </summary>
        /// <param name="cellIndex">Индекс искомой ячейки, начиная с 1.</param>
        /// <returns>Ячейка, удовлетворяющая критерию поиска, или null, если ячейка не найдена.</returns>
        public Cell FindCell(int cellIndex)
        {
            int index = 0;
            foreach (Cell cell in cells)
            {
                index = cell.Index > 0 ? cell.Index : index + 1;
                int endIndex = index + cell.MergeAcross;

                if (index <= cellIndex && cellIndex <= endIndex)
                    return cell;

                index = endIndex;
            }

            return null;
        }

        /// <summary>
        /// Добавить ячейку в конец списка ячеек строки и модифицировать дерево XML-документа.
        /// </summary>
        public void AppendCell(Cell cell)
        {
            cells.Add(cell);
            cell.ParentRow = this;
            node.AppendChild(cell.Node);
        }

        /// <summary>
        /// Вставить ячейку в список ячеек строки и модифицировать дерево XML-документа.
        /// </summary>
        public void InsertCell(int listIndex, Cell cell)
        {
            cells.Insert(listIndex, cell);
            cell.ParentRow = this;

            if (cells.Count == 1)
                node.AppendChild(cell.Node);
            else if (listIndex == 0)
                node.PrependChild(cell.Node);
            else
                node.InsertAfter(cell.Node, cells[listIndex - 1].Node);
        }

        /// <summary>
        /// Удалить ячейку из списка ячеек строки и модифицировать дерево XML-документа.
        /// </summary>
        public void RemoveCell(int listIndex)
        {
            Cell cell = cells[listIndex];
            cell.ParentRow = null;
            node.RemoveChild(cell.Node);
            cells.RemoveAt(listIndex);
        }

        /// <summary>
        /// Установить высоту строки.
        /// </summary>
        public static void SetRowHeight(XmlNode rowNode, double height)
        {
            ExcelUtils.SetAttribute(rowNode, "AutoFitHeight", XmlNamespaces.Ss, "0");
            ExcelUtils.SetAttribute(rowNode, "Height", XmlNamespaces.Ss,
                height > 0 ? height.ToString(NumberFormatInfo.InvariantInfo) : "", true);
        }
    }
}
