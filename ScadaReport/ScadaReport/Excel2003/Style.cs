// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Report.Excel2003
{
    /// <summary>
    /// Represents a style of Excel workbook.
    /// <para>Представляет стиль книги Excel.</para>
    /// </summary>
    internal class Style
    {
        /// <summary>
        /// Ссылка на XML-узел, соответствующий стилю книги Excel.
        /// </summary>
        protected XmlNode node;
        /// <summary>
        /// Идентификатор стиля.
        /// </summary>
        protected string id;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="xmlNode">Ссылка на XML-узел, соответствующий стилю книги Excel.</param>
        public Style(XmlNode xmlNode)
        {
            node = xmlNode;
            id = xmlNode.Attributes["ss:ID"].Value;
        }


        /// <summary>
        /// Получить ссылку на XML-узел, соответствующий стилю книги Excel.
        /// </summary>
        public XmlNode Node
        {
            get
            {
                return node;
            }
        }

        /// <summary>
        /// Получить или установить идентификатор стиля, при установке модифицируется дерево XML-документа.
        /// </summary>
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                node.Attributes["ss:ID"].Value = id;
            }
        }


        /// <summary>
        /// Клонировать стиль.
        /// </summary>
        /// <returns>Копия стиля.</returns>
        public Style Clone()
        {
            return new Style(node.Clone());
        }
    }
}
