// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Drawing;
using System.Xml;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Size in two-dimensional space
    /// <para>Размер в двумерном пространстве</para>
    /// </summary>
    [TypeConverter(typeof(SizeConverter))]
    [Serializable]
    public struct Size
    {
        /// <summary>
        /// Нулевой размер
        /// </summary>
        public static readonly Size Zero = new(0, 0);
        /// <summary>
        /// Размер по умолчанию
        /// </summary>
        public static readonly Size Default = new(100, 100);


        /// <summary>
        /// Конструктор
        /// </summary>
        public Size(int width, int height)
            : this()
        {
            Width = width;
            Height = height;
        }


        /// <summary>
        /// Получить или установить ширину
        /// </summary>
        [DisplayName("Width")]
        public int Width { get; set; }

        /// <summary>
        /// Получить или установить высоту
        /// </summary>
        [DisplayName("Height")]
        public int Height { get; set; }


        /// <summary>
        /// Получить значение дочернего XML-узла в виде размера
        /// </summary>
        public static Size GetChildAsSize(XmlNode parentXmlNode, string childNodeName, Size? defaultSize = null)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            return node == null ?
                (defaultSize ?? Default) :
                new Size(node.GetChildAsInt("Width"), node.GetChildAsInt("Height"));
        }

        /// <summary>
        /// Создать и добавить XML-элемент размера
        /// </summary>
        public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, Size size)
        {
            XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
            xmlElem.AppendElem("Width", size.Width);
            xmlElem.AppendElem("Height", size.Height);
            return (XmlElement)parentXmlElem.AppendChild(xmlElem);
        }
    }
}
