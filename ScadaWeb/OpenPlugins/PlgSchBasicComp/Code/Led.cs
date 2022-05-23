// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchBasicComp.Code
{
    /// <summary>
    /// Scheme component that represents a led.
    /// <para>Компонент схемы, представляющий светодиод.</para>
    /// </summary>
    [Serializable]
    public class Led : ComponentBase, IDynamicComponent
    {
        /// <summary>
        /// Размер по умолчанию.
        /// </summary>
        public static readonly Size DefaultSize = new(30, 30);


        /// <summary>
        /// Конструктор.
        /// </summary>
        public Led()
            : base()
        {
            //serBinder = PlgUtils.SerializationBinder;

            BackColor = "Silver";
            BorderColor = "Black";
            BorderWidth = 3;
            BorderOpacity = 30;
            Action = Actions.None;
            Conditions = new List<ColorCondition>();
            AddDefaultConditions();
            InCnlNum = 0;
            CtrlCnlNum = 0;
            Size = DefaultSize;
        }


        /// <summary>
        /// Получить или установить непрозрачность границы.
        /// </summary>
        #region Attributes
        [DisplayName("Border opacity"), Category(Categories.Appearance)]
        [Description("The border opacity percentage of the component.")]
        #endregion
        public int BorderOpacity { get; set; }

        /// <summary>
        /// Получить или установить действие.
        /// </summary>
        #region Attributes
        [DisplayName("Action"), Category(Categories.Behavior)]
        [Description("The action executed by clicking the left mouse button on the component.")]
        [DefaultValue(Actions.None)]
        #endregion
        public Actions Action { get; set; }

        /// <summary>
        /// Получить условия, определяющие цвет заливки.
        /// </summary>
        #region Attributes
        [DisplayName("Conditions"), Category(Categories.Behavior)]
        [Description("The conditions determining the fill color depending on the value of the input channel.")]
        [DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
        //[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        #endregion
        public List<ColorCondition> Conditions { get; protected set; }

        /// <summary>
        /// Получить или установить номер входного канала.
        /// </summary>
        #region Attributes
        [DisplayName("Input channel"), Category(Categories.Data)]
        [Description("The input channel number associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int InCnlNum { get; set; }

        /// <summary>
        /// Получить или установить номер канала управления.
        /// </summary>
        #region Attributes
        [DisplayName("Output channel"), Category(Categories.Data)]
        [Description("The output channel number associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int CtrlCnlNum { get; set; }
        
        
        /// <summary>
        /// Добавить условия по умолчанию.
        /// </summary>
        protected void AddDefaultConditions()
        {
            Conditions.Add(new ColorCondition
            {
                CompareOperator1 = CompareOperators.LessThanEqual,
                Color = "Red"
            });

            Conditions.Add(new ColorCondition
            {
                CompareOperator1 = CompareOperators.GreaterThan,
                Color = "Green"
            });
        }

        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла.
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            BorderOpacity = xmlNode.GetChildAsInt("BorderOpacity");
            Action = xmlNode.GetChildAsEnum<Actions>("Action");

            XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");
            if (conditionsNode != null)
            {
                Conditions = new List<ColorCondition>();
                XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
                foreach (XmlNode conditionNode in conditionNodes)
                {
                    ColorCondition condition = new ColorCondition { SchemeView = SchemeView };
                    condition.LoadFromXml(conditionNode);
                    Conditions.Add(condition);
                }
            }

            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле.
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);

            xmlElem.AppendElem("BorderOpacity", BorderOpacity);
            xmlElem.AppendElem("Action", Action);

            XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
            foreach (Condition condition in Conditions)
            {
                XmlElement conditionElem = conditionsElem.AppendElem("Condition");
                condition.SaveToXml(conditionElem);
            }

            xmlElem.AppendElem("InCnlNum", InCnlNum);
            xmlElem.AppendElem("CtrlCnlNum", CtrlCnlNum);
        }

        /// <summary>
        /// Клонировать объект.
        /// </summary>
        public override ComponentBase Clone()
        {
            Led clonedComponent = (Led)base.Clone();

            foreach (Condition condition in clonedComponent.Conditions)
            {
                condition.SchemeView = SchemeView;
            }

            return clonedComponent;
        }
    }
}