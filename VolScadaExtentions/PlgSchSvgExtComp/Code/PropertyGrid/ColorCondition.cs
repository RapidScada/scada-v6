using Scada.Web.Plugins.SchSvgExtComp;
using System;
using System.Drawing.Design;
using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchSvgExtComp
{
    /// <summary>
    /// Condition that defines a color.
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
            Condition clonedCondition = this.DeepClone(PlgUtils.SerializationBinder);
            clonedCondition.SchemeView = SchemeView;
            return clonedCondition;
        }
    }
}
