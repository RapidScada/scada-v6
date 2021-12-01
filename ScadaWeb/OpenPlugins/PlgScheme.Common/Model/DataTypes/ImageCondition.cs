// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgScheme.Model.DataTypes
{
    /// <summary>
    /// Condition that defines an image
    /// <para>Условие, которое определяет изображение</para>
    /// </summary>
    [Serializable]
    public class ImageCondition : Condition
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ImageCondition()
            : base()
        {
            ImageName = "";
        }


        /// <summary>
        /// Получить или установить наименование изображения, отображаемого при выполнении условия
        /// </summary>
        #region Attributes
        [DisplayName("Image"), Category(Categories.Appearance)]
        //[TypeConverter(typeof(ImageConverter)), Editor(typeof(ImageEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        #endregion
        public string ImageName { get; set; }
        
        
        /// <summary>
        /// Загрузить условие из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            ImageName = xmlNode.GetChildAsString("ImageName");
        }

        /// <summary>
        /// Сохранить условие в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("ImageName", ImageName);
        }
    }
}
