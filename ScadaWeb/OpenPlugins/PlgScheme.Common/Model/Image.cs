// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml;

namespace Scada.Web.Plugins.PlgScheme.Model
{
    /// <summary>
    /// Image of a scheme
    /// <para>Изображение схемы</para>
    /// </summary>
    //[TypeConverter(typeof(ImageConverter))]
    //[Editor(typeof(ImageEditor), typeof(UITypeEditor))]
    public class Image
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Image()
        {
            Name = "";
            Data = null;
        }


        /// <summary>
        /// Получить или установить наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получить или установить данные
        /// </summary>
        public byte[] Data { get; set; }


        /// <summary>
        /// Загрузить изображение из XML-узла
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            Name = xmlNode.GetChildAsString("Name");
            Data = Convert.FromBase64String(xmlNode.GetChildAsString("Data"));
        }

        /// <summary>
        /// Сохранить изображение в XML-узле
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("Name", Name);
            xmlElem.AppendElem("Data", 
                Data != null && Data.Length > 0 ? 
                Convert.ToBase64String(Data, Base64FormattingOptions.None) : "");
        }

        /// <summary>
        /// Копировать объект
        /// </summary>
        public Image Copy()
        {
            return new Image()
            {
                Name = Name,
                Data = Data
            };
        }
    }
}
