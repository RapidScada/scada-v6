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
    /// 液位组件
    /// </summary>
    [Serializable]
    public class WaterLevel : ComponentBase, IDynamicComponent
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public WaterLevel()
        {
            serBinder = PlgUtils.SerializationBinder;

            BorderColor = "Gray";
            ForeColor = "Gray";
            BorderWidth = 0;
            InCnlNum = 0;
            Size = new Size(50, 170);
            MinValue = 0;
            MaxValue = 100;
            Flow = YesNoEnum.是;
        }

        /// <summary>
        /// 颜色
        /// </summary>
        [DisplayName("Color"), Category(Categories.Appearance)]
        [Description("The color associated with the component.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string ForeColor { get; set; }


        /// <summary>
        /// 是否流动
        /// </summary>
        #region Attributes
        [DisplayName("Flow(流动)"), Category(Categories.Behavior)]
        [Description("The flow associated with the component.")]
        [DefaultValue("")]
        #endregion
        public YesNoEnum Flow { get; set; }

        /// <summary>
        /// 通道号
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
        /// 最大值
        /// </summary>
        #region Attributes
        [DisplayName("Max value"), Category(Categories.Data)]
        [Description("The max value associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        #region Attributes
        [DisplayName("Min value"), Category(Categories.Data)]
        [Description("The min value associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int MinValue { get; set; }

        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            ForeColor = xmlNode.GetChildAsString("ForeColor");
            Flow = xmlNode.GetChildAsEnum<YesNoEnum>("Flow");
            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            MaxValue = xmlNode.GetChildAsInt("MaxValue");
            MinValue = xmlNode.GetChildAsInt("MinValue");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);

            xmlElem.AppendElem("ForeColor", ForeColor);
            xmlElem.AppendElem("Flow", Flow);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
            xmlElem.AppendElem("MaxValue", MaxValue);
            xmlElem.AppendElem("MinValue", MinValue);
        }
    }
}
