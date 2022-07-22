// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Report.Excel2003
{
    /// <summary>
    /// Represents a cell of Excel table row.
    /// <para>Представляет ячейку строки таблицы Excel.</para>
    /// </summary>
    internal class Cell
    {
        /// <summary>
        /// Specifies the cell data types.
        /// </summary>
        public static class DataTypes
        {
            public const string String = "String";
            public const string Number = "Number";
        }

        /// <summary>
        /// Ссылка на XML-узел, соответствующий ячейке строки таблицы Excel.
        /// </summary>
        protected XmlNode node;
        /// <summary>
        /// Ссылка на XML-узел, соответствующий данным ячейки.
        /// </summary>
        protected XmlNode dataNode;
        /// <summary>
        /// Родительская строка данной ячейки.
        /// </summary>
        protected Row parentRow;
        /// <summary>
        /// Индекс ячейки, 0 - неопределён.
        /// </summary>
        protected int index;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="xmlNode">Ссылка на XML-узел, соответствующий ячейке строки таблицы Excel.</param>
        public Cell(XmlNode xmlNode)
        {
            node = xmlNode;
            dataNode = null;
            parentRow = null;

            XmlAttribute attr = node.Attributes["ss:Index"];
            index = attr == null ? 0 : int.Parse(attr.Value);
        }


        /// <summary>
        /// Получить ссылку на XML-узел, соответствующий ячейке строки таблицы Excel.
        /// </summary>
        public XmlNode Node
        {
            get
            {
                return node;
            }
        }

        /// <summary>
        /// Получить или установить ссылку на XML-узел, соответствующий данным ячейки.
        /// </summary>
        public XmlNode DataNode
        {
            get
            {
                return dataNode;
            }
            set
            {
                dataNode = value;
            }
        }

        /// <summary>
        /// Получить или установить родительскую строку данной ячейки.
        /// </summary>
        public Row ParentRow
        {
            get
            {
                return parentRow;
            }
            set
            {
                parentRow = value;
            }
        }

        /// <summary>
        /// Получить или установить индекс ячейки (0 - неопределён), при установке модифицируется дерево XML-документа.
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
                ExcelUtils.SetAttribute(node, "Index", XmlNamespaces.ss, index <= 0 ? null : index.ToString(), true);
            }
        }

        /// <summary>
        /// Получить или установить тип данных (формат) ячейки.
        /// </summary>
        public string DataType
        {
            get
            {
                return ExcelUtils.GetAttribute(dataNode, "ss:Type");
            }
            set
            {
                ExcelUtils.SetAttribute(dataNode, "Type", XmlNamespaces.ss,
                    string.IsNullOrEmpty(value) ? DataTypes.String : value);
            }
        }

        /// <summary>
        /// Получить или установить текст.
        /// </summary>
        public string Text
        {
            get
            {
                return dataNode == null ? "" : dataNode.InnerText;
            }
            set
            {
                if (dataNode != null)
                    dataNode.InnerText = value;
            }
        }

        /// <summary>
        /// Получить или установить формулу.
        /// </summary>
        public string Formula
        {
            get
            {
                return ExcelUtils.GetAttribute(node, "ss:Formula");
            }
            set
            {
                ExcelUtils.SetAttribute(node, "Formula", XmlNamespaces.ss, value, true);
            }
        }

        /// <summary>
        /// Получить или установить ид. стиля.
        /// </summary>
        public string StyleID
        {
            get
            {
                return ExcelUtils.GetAttribute(node, "ss:StyleID");
            }
            set
            {
                ExcelUtils.SetAttribute(node, "StyleID", XmlNamespaces.ss, value, true);
            }
        }

        /// <summary>
        /// Получить или установить количество объединямых ячеек справа.
        /// </summary>
        public int MergeAcross
        {
            get
            {
                string valStr = ExcelUtils.GetAttribute(node, "ss:MergeAcross");
                return valStr == "" ? 0 : int.Parse(valStr);
            }
            set
            {
                ExcelUtils.SetAttribute(node, "MergeAcross", XmlNamespaces.ss, value < 1 ? "" : value.ToString(), true);
            }
        }

        /// <summary>
        /// Получить или установить количество объединямых ячеек вниз.
        /// </summary>
        public int MergeDown
        {
            get
            {
                string valStr = ExcelUtils.GetAttribute(node, "ss:MergeDown");
                return valStr == "" ? 0 : int.Parse(valStr);
            }
            set
            {
                ExcelUtils.SetAttribute(node, "MergeDown", XmlNamespaces.ss, value < 1 ? "" : value.ToString(), true);
            }
        }


        /// <summary>
        /// Рассчитать индекс в строке.
        /// </summary>
        public int CalcIndex()
        {
            if (index > 0)
            {
                return index;
            }
            else
            {
                int index = 0;
                foreach (Cell cell in parentRow.Cells)
                {
                    index = cell.Index > 0 ? cell.Index : index + 1;
                    if (cell == this)
                        return index;
                    index += cell.MergeAcross;
                }
                return 0;
            }
        }

        /// <summary>
        /// Клонировать ячейку.
        /// </summary>
        /// <returns>Копия ячейки.</returns>
        public Cell Clone()
        {
            XmlNode nodeClone = node.CloneNode(false);
            Cell cellClone = new(nodeClone)
            {
                parentRow = parentRow
            };

            if (dataNode != null)
            {
                cellClone.dataNode = dataNode.Clone();
                nodeClone.AppendChild(cellClone.dataNode);

                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Name != "Data")
                        nodeClone.AppendChild(childNode.Clone());
                }
            }

            return cellClone;
        }

        /// <summary>
        /// Установить числовой тип данных ячейки.
        /// </summary>
        public void SetNumberType()
        {
            DataType = DataTypes.Number;
        }
    }
}
