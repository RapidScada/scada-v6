// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.PlgSchBasicComp.Code
{
    /// <summary>
    /// Condition that defines a color.
    /// <para>Условие, которое определяет цвет.</para>
    /// </summary>
    [Serializable]
    public class ColorCondition : Condition
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public ColorCondition()
            : base()
        {
            Color = "";
        }


        /// <summary>
        /// Получить или установить цвет, отображаемый при выполнении условия.
        /// </summary>
        #region Attributes
        [DisplayName("Color"), Category(Categories.Appearance)]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string Color { get; set; }


        /// <summary>
        /// Загрузить условие из XML-узла.
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            Color = xmlNode.GetChildAsString("Color");
        }

        /// <summary>
        /// Сохранить условие в XML-узле.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("Color", Color);
        }

        /// <summary>
        /// Клонировать объект.
        /// </summary>
        public override object Clone()
        {
            Condition clonedCondition = this.DeepClone(/*PlgUtils.SerializationBinder*/);
            clonedCondition.SchemeView = SchemeView;
            return clonedCondition;
        }
    }
}
