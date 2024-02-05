// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Drawing;
using System.Xml;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Point in a two-dimensional plane
    /// <para>Точка в двумерной плоскости</para>
    /// </summary>
    [TypeConverter(typeof(PointConverter))]
    [Serializable]
    public struct Point
    {
        /// <summary>
        /// Точка по умолчанию
        /// </summary>
        public static readonly Point Default = new(0, 0);


        /// <summary>
        /// Конструктор
        /// </summary>
        public Point(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }


        /// <summary>
        /// Получить или установить координату X
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Получить или установить координату Y
        /// </summary>
        public int Y { get; set; }


        /// <summary>
        /// Получить значение дочернего XML-узла в виде точки
        /// </summary>
        public static Point GetChildAsPoint(XmlNode parentXmlNode, string childNodeName)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            return node == null ?
                Default :
                new Point(node.GetChildAsInt("X"), node.GetChildAsInt("Y"));
        }

        /// <summary>
        /// Создать и добавить XML-элемент точки
        /// </summary>
        public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, Point point)
        {
            XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
            xmlElem.AppendElem("X", point.X);
            xmlElem.AppendElem("Y", point.Y);
            return (XmlElement)parentXmlElem.AppendChild(xmlElem);
        }
    }
}
