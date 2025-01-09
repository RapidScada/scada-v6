using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using Scada.Web.Plugins.SchSvgExtComp;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins
{
    [Serializable]
    public class ConveyerBeltCondition : Condition
    {

        public ConveyerBeltCondition()
            : base()
        {
            ConveyerColor = "Gray";
        }

        /// <summary>
        /// 静态颜色
        /// </summary>
        [DisplayName("Conveyer color"), Category(Categories.Appearance)]
        [Description("The conveyer color associated with the component.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string ConveyerColor { get; set; }

        public override object Clone()
        {
            ConveyerBeltCondition clonedCondition = ScadaUtils.DeepClone(this, PlgUtils.SerializationBinder);
            clonedCondition.SchemeView = SchemeView;
            return clonedCondition;
        }

        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            ConveyerColor = xmlNode.GetChildAsString("ConveyerColor");
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("ConveyerColor", ConveyerColor);
        }
    }
}