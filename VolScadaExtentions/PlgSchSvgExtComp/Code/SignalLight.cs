using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// 信号灯
    /// </summary>
    [Serializable]
    public class SignalLight : ComponentBase, IDynamicComponent
    {
        /// <summary>
        /// 构造
        /// </summary>
        public SignalLight()
        {
            serBinder = PlgUtils.SerializationBinder;

            BorderColor = "Gray";
            BorderWidth = 0;
            InCnlNum = 0;
            Size = new Size(70, 70);
            Conditions = new List<SignalLightCondition>();
        }

        /// <summary>
        /// Получить условия вывода изображений
        /// </summary>
        #region Attributes
        [DisplayName("Conditions"), Category(Categories.Behavior)]
        [Description("The conditions for color output depending on the value of the input channel.")]
        [DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
        //[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        #endregion
        public List<SignalLightCondition> Conditions { get; protected set; }

        /// <summary>
        /// 输入通道
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
        /// 加载
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");
            if (conditionsNode != null)
            {
                Conditions = new List<SignalLightCondition>();
                XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
                foreach (XmlNode conditionNode in conditionNodes)
                {
                    SignalLightCondition condition = new SignalLightCondition { SchemeView = SchemeView };
                    condition.LoadFromXml(conditionNode);
                    Conditions.Add(condition);
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
            XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
            foreach (Condition condition in Conditions)
            {
                XmlElement conditionElem = conditionsElem.AppendElem("Condition");
                condition.SaveToXml(conditionElem);
            }
        }
    }
}
