// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.PlgScheme.Model
{
    /// <summary>
    /// Scheme component that represents static picture
    /// <para>Компонент схемы, представляющий статический рисунок</para>
    /// </summary>
    [Serializable]
    public class StaticPicture : ComponentBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public StaticPicture()
        {
            BorderColor = "Gray";
            BorderWidth = 1;
            ImageName = "";
            ImageStretch = ImageStretches.None;
        }


        /// <summary>
        /// Получить или установить наименование изображения
        /// </summary>
        #region Attributes
        [DisplayName("Image"), Category(Categories.Appearance)]
        [Description("The image from the collection of scheme images.")]
        //[TypeConverter(typeof(ImageConverter)), Editor(typeof(ImageEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        #endregion
        public string ImageName { get; set; }

        /// <summary>
        /// Получить или установить растяжение изображения
        /// </summary>
        #region Attributes
        [DisplayName("Image stretch"), Category(Categories.Appearance)]
        [Description("Stretch the image.")]
        [DefaultValue(ImageStretches.None)]
        #endregion
        public ImageStretches ImageStretch { get; set; }

        
        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            ImageName = xmlNode.GetChildAsString("ImageName");
            ImageStretch = xmlNode.GetChildAsEnum<ImageStretches>("ImageStretch");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);

            xmlElem.AppendElem("ImageName", ImageName);
            xmlElem.AppendElem("ImageStretch", ImageStretch);
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return BuildDisplayName(ImageName);
        }
    }
}
