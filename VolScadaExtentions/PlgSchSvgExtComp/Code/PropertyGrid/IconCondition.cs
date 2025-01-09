using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    [Serializable]
    public class IconCondition : Condition
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public IconCondition()
            : base()
        {
            IconObj = new Icon();
        }


        /// <summary>
        /// Получить или установить наименование изображения, отображаемого при выполнении условия
        /// </summary>
        #region Attributes
        //[DisplayName("Icon"), Category(Categories.Appearance)]
        //[TypeConverter(typeof(IconConverter)), Editor(typeof(IconEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        #endregion
        public Icon IconObj { get; set; }

        /// <summary>
        /// Загрузить условие из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            IconObj = Icon.GetChildAsFont(xmlNode, "Icon");
        }

        /// <summary>
        /// Сохранить условие в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            Icon.AppendElem(xmlElem, "Icon", IconObj);
        }


        /// <summary>
        /// Клонировать объект.
        /// </summary>
        public override object Clone()
        {
            Condition clonedCondition = ScadaUtils.DeepClone(this, PlgUtils.SerializationBinder);
            clonedCondition.SchemeView = SchemeView;
            return clonedCondition;
        }
    }
}