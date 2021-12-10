// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;
using System.ComponentModel;

namespace Scada.Web.Plugins.PlgSchBasicComp.Code
{
    /// <summary>
    /// Size of a popup
    /// <para>Размер всплывающего окна</para>
    /// </summary>
    [TypeConverter(typeof(PopupSizeConverter))]
    [Serializable]
    public struct PopupSize
    {
        /// <summary>
        /// Размер по умолчанию
        /// </summary>
        public static readonly PopupSize Default = new(PopupWidth.Normal, 300);


        /// <summary>
        /// Конструктор
        /// </summary>
        public PopupSize(PopupWidth width, int height)
            : this()
        {
            Width = width;
            Height = height;
        }


        /// <summary>
        /// Получить или установить ширину
        /// </summary>
        [DisplayName("Width")]
        public PopupWidth Width { get; set; }

        /// <summary>
        /// Получить или установить высоту
        /// </summary>
        [DisplayName("Height")]
        public int Height { get; set; }


        /// <summary>
        /// Получить значение дочернего XML-узла в виде размера
        /// </summary>
        public static PopupSize GetChildAsSize(XmlNode parentXmlNode, string childNodeName)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            return node == null ?
                Default :
                new PopupSize(node.GetChildAsEnum<PopupWidth>("Width"), node.GetChildAsInt("Height"));
        }

        /// <summary>
        /// Создать и добавить XML-элемент размера
        /// </summary>
        public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, PopupSize popupSize)
        {
            XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
            xmlElem.AppendElem("Width", popupSize.Width);
            xmlElem.AppendElem("Height", popupSize.Height);
            return (XmlElement)parentXmlElem.AppendChild(xmlElem);
        }
    }
}
