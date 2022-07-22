// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using System.Xml;

namespace Scada.Report.Xml2003.Excel
{
    /// <summary>
    /// Represents a column of Excel table.
    /// <para>Представляет столбец таблицы Excel.</para>
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Ссылка на XML-узел, соответствующий столбцу таблицы Excel.
        /// </summary>
        protected XmlNode node;
        /// <summary>
        /// Родительская таблица данного столбца.
        /// </summary>
        protected Table parentTable;
        /// <summary>
        /// Индекс столбца, 0 - неопределён.
        /// </summary>
        protected int index;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="xmlNode">Ссылка на XML-узел, соответствующий столбцу таблицы Excel.</param>
        public Column(XmlNode xmlNode)
        {
            node = xmlNode;
            parentTable = null;

            XmlAttribute attr = node.Attributes["ss:Index"];
            index = attr == null ? 0 : int.Parse(attr.Value);
        }


        /// <summary>
        /// Получить ссылку на XML-узел, соответствующий столбцу таблицы Excel.
        /// </summary>
        public XmlNode Node
        {
            get
            {
                return node;
            }
        }

        /// <summary>
        /// Получить или установить родительскую таблицу данного столбца.
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
        /// Получить или установить индекс столбца (0 - неопределён), при установке модифицируется дерево XML-документа.
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
        /// Получить или установить ширину.
        /// </summary>
        public double Width
        {
            get
            {
                string widthStr = ExcelUtils.GetAttribute(node, "ss:Width");
                return double.TryParse(widthStr, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double width) ?
                    width : 0;
            }
            set
            {
                SetColumnWidth(node, value);
            }
        }

        /// <summary>
        /// Получить или установить количество объединямых колонок справа.
        /// </summary>
        public int Span
        {
            get
            {
                string valStr = ExcelUtils.GetAttribute(node, "ss:Span");
                return valStr == "" ? 0 : int.Parse(valStr);
            }
            set
            {
                ExcelUtils.SetAttribute(node, "Span", XmlNamespaces.ss, value < 1 ? "" : value.ToString(), true);
            }
        }


        /// <summary>
        /// Клонировать столбец.
        /// </summary>
        /// <returns>Копия столбца.</returns>
        public Column Clone()
        {
            return new Column(node.Clone())
            {
                parentTable = parentTable
            };
        }

        /// <summary>
        /// Установить ширину столбца.
        /// </summary>
        public static void SetColumnWidth(XmlNode columnNode, double width)
        {
            ExcelUtils.SetAttribute(columnNode, "AutoFitWidth", XmlNamespaces.ss, "0");
            ExcelUtils.SetAttribute(columnNode, "Width", XmlNamespaces.ss,
                width > 0 ? width.ToString(NumberFormatInfo.InvariantInfo) : "", true);
        }
    }
}
