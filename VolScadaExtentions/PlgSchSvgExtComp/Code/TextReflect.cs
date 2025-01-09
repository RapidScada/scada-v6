using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// 文字映射
    /// </summary>
    [Serializable]
    public class TextReflect : ComponentBase, IDynamicComponent
    {
        /// <summary>
        /// 构造
        /// </summary>
        public TextReflect()
        {
            serBinder = PlgUtils.SerializationBinder;

            BorderColor = "Gray";
            BorderWidth = 0;
            InCnlNum = 0;
            Size = new Size(70, 70);
            Conditions = new List<TextReflectCondition>();
            Font = null;
            Marquee = YesNoEnum.否;
            MarqueeDirection = LeftRightEnum.右;
        }

        /// <summary>
        /// 颜色
        /// </summary>
        [DisplayName("Foreground Color"), Category(Categories.Appearance)]
        [Description("The foreground color of the component, which is used to display text.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string ForeColor { get; set; }

        /// <summary>
        /// 字体
        /// </summary>
        #region Attributes
        [DisplayName("Font"), Category(Categories.Appearance)]
        [Description("The font used to display text in the component.")]
        #endregion
        public Font Font { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        #region Attributes
        [DisplayName("Conditions"), Category(Categories.Behavior)]
        [Description("The conditions for show on the value of the input channel.")]
        [DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
        //[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        #endregion
        public List<TextReflectCondition> Conditions { get; protected set; }

        /// <summary>
        /// 跑马灯
        /// </summary>
        #region Attributes
        [DisplayName("Marquee(跑马灯)"), Category(Categories.Behavior)]
        [Description("The marquee associated with the component.")]
        [DefaultValue("")]
        #endregion
        public YesNoEnum Marquee { get; set; }

        /// <summary>
        /// 跑马灯方向
        /// </summary>
        #region Attributes
        [DisplayName("Marquee direction"), Category(Categories.Behavior)]
        [Description("The marquee direction associated with the component.")]
        [DefaultValue("")]
        #endregion
        public LeftRightEnum MarqueeDirection { get; set; }

        /// <summary>
        /// 跑马灯速度
        /// </summary>
        #region Attributes
        [DisplayName("Marquee peed"), Category(Categories.Behavior)]
        [Description("The marquee speed associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int MarqueeSpeed { get; set; }

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

            ForeColor = xmlNode.GetChildAsString("ForeColor");
            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            Marquee = xmlNode.GetChildAsEnum<YesNoEnum>("Marquee");
            MarqueeDirection = xmlNode.GetChildAsEnum<LeftRightEnum>("MarqueeDirection");
            MarqueeSpeed = xmlNode.GetChildAsInt("MarqueeSpeed");
            Font = Font.GetChildAsFont(xmlNode, "Font");
            XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");
            if (conditionsNode != null)
            {
                Conditions = new List<TextReflectCondition>();
                XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
                foreach (XmlNode conditionNode in conditionNodes)
                {
                    TextReflectCondition condition = new TextReflectCondition { SchemeView = SchemeView };
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
            xmlElem.AppendElem("ForeColor", ForeColor);
            xmlElem.AppendElem("Marquee", Marquee);
            xmlElem.AppendElem("MarqueeSpeed", MarqueeSpeed);
            xmlElem.AppendElem("MarqueeDirection", MarqueeDirection);
            Font.AppendElem(xmlElem, "Font", Font);
            XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
            foreach (Condition condition in Conditions)
            {
                XmlElement conditionElem = conditionsElem.AppendElem("Condition");
                condition.SaveToXml(conditionElem);
            }
        }
    }
}
