using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using Scada.Web.Plugins.SchSvgExtComp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Xml;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// Scheme component that represents static picture
    /// <para>Компонент схемы, представляющий статический рисунок</para>
    /// </summary>
    [Serializable]
    public class Windmill1 : ComponentBase, IDynamicComponent
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Windmill1()
        {
            serBinder = PlgUtils.SerializationBinder;

            BorderColor = "Gray";
            BorderWidth = 1;
            IconObj = new Icon();
            Conditions = new List<IconCondition>();
            InCnlNum = 0;
        }


        /// <summary>
        /// Получить или установить наименование изображения
        /// </summary>
        #region Attributes
        [DisplayName("Icon"), Category(Categories.Appearance)]
        [Description("The icon from the collection of scheme icons.")]
        [DefaultValue("")]
        #endregion
        public Icon IconObj { get; set; }


        /// <summary>
        /// 
        /// </summary>
        #region Attributes
        [DisplayName("Conditions"), Category(Categories.Behavior)]
        [Description("The conditions for icon output depending on the value of the input channel.")]
        [DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
        //[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        #endregion
        public List<IconCondition> Conditions { get; protected set; }

        /// <summary>
        /// Получить или установить номер входного канала
        /// </summary>
        #region Attributes
        [DisplayName("Input channel"), Category(Categories.Data)]
        [Description("The input channel number associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int InCnlNum { get; set; }
        public Actions Action { get; set; } = Actions.None;
        public int CtrlCnlNum { get; set; } = 0;

        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");
            if (conditionsNode != null)
            {
                Conditions = new List<IconCondition>();
                XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
                foreach (XmlNode conditionNode in conditionNodes)
                {
                    var condition = new IconCondition { SchemeView = SchemeView };
                    condition.LoadFromXml(conditionNode);
                    Conditions.Add(condition);
                }
            }
            IconObj = Icon.GetChildAsFont(xmlNode,"Icon");
            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
            foreach (Condition condition in Conditions)
            {
                XmlElement conditionElem = conditionsElem.AppendElem("Condition");
                condition.SaveToXml(conditionElem);
            }
            Icon.AppendElem(xmlElem, "Icon", IconObj);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return BuildDisplayName(IconObj.Name);
        }

        /// <summary>
        /// 实体克隆
        /// </summary>
        public override ComponentBase Clone()
        {
            Windmill1 clonedComponent = (Windmill1)base.Clone();
            foreach (Condition condition in clonedComponent.Conditions)
            {
                condition.SchemeView = SchemeView;
            }

            return clonedComponent;
        }
    }
}
