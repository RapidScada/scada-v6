// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Defines a font
    /// <para>Определяет шрифт</para>
    /// </summary>
    //[TypeConverter(typeof(FontConverter))]
    //[Editor(typeof(FontEditor), typeof(UITypeEditor))]
    [Serializable]
    public class Font
    {
        /// <summary>
        /// Наименование по умолчанию
        /// </summary>
        public const string DefaultName = "Arial";
        /// <summary>
        /// Размер по умолчанию
        /// </summary>
        public const int DefaultSize = 12;
        /// <summary>
        /// Шрифт по умолчанию
        /// </summary>
        public static readonly Font Default = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Font()
        {
            Name = DefaultName;
            Size = DefaultSize;
            Bold = false;
            Italic = false;
            Underline = false;
        }


        /// <summary>
        /// Получить или установить наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получить или установить размер
        /// </summary>
        public int Size { get; set; }
        
        /// <summary>
        /// Получить или установить признак, что шрифт жирный
        /// </summary>
        public bool Bold { get; set; }
        
        /// <summary>
        /// Получить или установить признак, что шрифт курсив
        /// </summary>
        public bool Italic { get; set; }
        
        /// <summary>
        /// Получить или установить признак, что шрифт подчёркнутый
        /// </summary>
        public bool Underline { get; set; }


        /// <summary>
        /// Получить значение дочернего XML-узла в виде шрифта
        /// </summary>
        public static Font GetChildAsFont(XmlNode parentXmlNode, string childNodeName)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);

            if (node == null)
            {
                return null;
            }
            else
            {
                Font font = new();
                font.Name = node.GetChildAsString("Name", Default.Name);
                font.Size = node.GetChildAsInt("Size", Default.Size);
                font.Bold = node.GetChildAsBool("Bold", Default.Bold);
                font.Italic = node.GetChildAsBool("Italic", Default.Italic);
                font.Underline = node.GetChildAsBool("Underline", Default.Underline);
                return font;
            }
        }

        /// <summary>
        /// Создать и добавить XML-элемент шрифта
        /// </summary>
        public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, Font font)
        {
            if (font == null)
            {
                return null;
            }
            else
            {
                XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
                xmlElem.AppendElem("Name", font.Name);
                xmlElem.AppendElem("Size", font.Size);
                xmlElem.AppendElem("Bold", font.Bold);
                xmlElem.AppendElem("Italic", font.Italic);
                xmlElem.AppendElem("Underline", font.Underline);
                return (XmlElement)parentXmlElem.AppendChild(xmlElem);
            }
        }

        /// <summary>
        /// Клонировать объект
        /// </summary>
        public Font Clone()
        {
            return ScadaUtils.DeepClone(this);
        }
    }
}
