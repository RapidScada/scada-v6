

using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using Scada.Web.Plugins.SchSvgExtComp;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins
{
    [Serializable]
    public class SignalLightCondition : Condition
    {

        public SignalLightCondition()
            : base()
        {
            ForeColor = "Green";
            FlashingColor = "Red";
            FlashingType = FlashingTypes.否;
        }

        /// <summary>
        /// 静态颜色
        /// </summary>
        //[DisplayName("Fore color"), Category(Categories.Appearance)]
        [Description("The fore color associated with the component.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string ForeColor { get; set; }


        /// <summary>
        /// 是否闪烁
        /// </summary>
        [DisplayName("Flashing"), Category(Categories.Appearance)]
        [Description("The flashing associated with the component.")]
        [DefaultValue(FlashingTypes.否)]
        public FlashingTypes FlashingType { get; set; }

        /// <summary>
        /// 闪烁颜色
        /// </summary>
        [DisplayName("Flashing color"), Category(Categories.Appearance)]
        [Description("The flashing color associated with the component.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [DefaultValue("Red")]
        public string FlashingColor { get; set; }

        public override object Clone()
        {
            SignalLightCondition clonedCondition = ScadaUtils.DeepClone(this, PlgUtils.SerializationBinder);
            clonedCondition.SchemeView = SchemeView;
            return clonedCondition;
        }

        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            ForeColor = xmlNode.GetChildAsString("ForeColor");
            FlashingColor = xmlNode.GetChildAsString("FlashingColor");
            FlashingType = xmlNode.GetChildAsEnum<FlashingTypes>("FlashingType");
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("ForeColor", ForeColor);
            xmlElem.AppendElem("FlashingColor", FlashingColor);
            xmlElem.AppendElem("FlashingType", FlashingType);
        }
    }

    public enum FlashingTypes
    {
        否 = 0,
        是 = 1,
    }
}