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
    /// Scheme component that represents TextScroll
    /// </summary>
    [Serializable]
    public class TextScroll : ComponentBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public TextScroll()
        {
            serBinder = PlgUtils.SerializationBinder;

            ForeColor = "Black";
            BorderColor = "Transparent";
            BorderWidth = 0;
            Size = new Size(70, 70);
            Font = null;
            Conditions = new List<TextScrollCondition>();
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
        /// 
        /// </summary>
        #region Attributes
        [DisplayName("Toggle channel"), Category(Categories.Data)]
        [Description("控制控件显示隐藏，0不控制")]
        [DefaultValue(0)]
        #endregion
        public int ToggleCnlNum { get; set; }

        /// <summary>
        /// Gets or sets the offset of input channel numbers.
        /// </summary>
        public int InCnlOffset
        {
            get
            {
                return base.schemeView.Args.GetValueAsInt("inCnlOffset");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        #region Attributes
        [DisplayName("Conditions"), Category(Categories.Behavior)]
        [Description("The conditions for image output depending on the value of the input channel.")]
        [DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
        //[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        #endregion
        public List<TextScrollCondition> Conditions { get; protected set; }

        public override List<int> GetInCnlNums()
        {
            var conds = Conditions.Select(x => x.InCnlNum).ToList();
            if(this.ToggleCnlNum>0)conds.Add(this.ToggleCnlNum);
            return conds.Distinct().ToList();
        }

        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            ToggleCnlNum = xmlNode.GetChildAsInt("ToggleCnlNum");
            ForeColor = xmlNode.GetChildAsString("ForeColor");
            Font = Font.GetChildAsFont(xmlNode, "Font");
            XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");
            if (conditionsNode != null)
            {
                Conditions = new List<TextScrollCondition>();
                XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
                foreach (XmlNode conditionNode in conditionNodes)
                {
                    TextScrollCondition condition = new TextScrollCondition { SchemeView = SchemeView };
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
            xmlElem.AppendElem("ToggleCnlNum", ToggleCnlNum);
            xmlElem.AppendElem("ForeColor", ForeColor);
            Font.AppendElem(xmlElem, "Font", Font);
            XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
            foreach (TextScrollCondition condition in Conditions)
            {
                XmlElement conditionElem = conditionsElem.AppendElem("Condition");
                condition.SaveToXml(conditionElem);
            }
        }
    }

    public enum TextScrollDirections
    {
        左 = 0,
        右 = 1,
    }
}
