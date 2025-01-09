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
    /// Scheme component that represents turbine
    /// </summary>
    [Serializable]
    public class Turbine : ComponentBase, IDynamicComponent
    {
        /// <summary>
        /// 构造
        /// </summary>
        public Turbine()
        {
            serBinder = PlgUtils.SerializationBinder;

            ForeColor = "Gray";
            BorderColor = "Gray";
            BorderWidth = 0;
            InCnlNum = 0;
            Size = new Size(70, 70);

        }

        /// <summary>
        /// Получить или установить наименование изображения
        /// </summary>
        #region Attributes
        [DisplayName("Turbine"), Category(Categories.Appearance)]
        [Description("The turbine from the collection of scheme images.")]
        //[TypeConverter(typeof(ImageConverter)), Editor(typeof(SvgImageEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        #endregion
        public string TurbineName { get; set; }

        /// <summary>
        /// 风扇转向
        /// </summary>
        #region Attributes
        [DisplayName("TurbineDirection"), Category(Categories.Appearance)]
        [Description("The icon from the collection of scheme TurbineDirection.")]
        [DefaultValue(TurbineDirections.顺时针)]
        #endregion
        public TurbineDirections TurbineDirection { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [DisplayName("Color"), Category(Categories.Appearance)]
        [Description("The color associated with the component.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string ForeColor { get; set; }

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
            ForeColor = xmlNode.GetChildAsString("ForeColor");
            TurbineName = xmlNode.GetChildAsString("TurbineName");
            TurbineDirection = xmlNode.GetChildAsEnum<TurbineDirections>("TurbineDirection");
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
            xmlElem.AppendElem("ForeColor", ForeColor);
            xmlElem.AppendElem("TurbineName", TurbineName);
            xmlElem.AppendElem("TurbineDirection", TurbineDirection);
        }
    }

    public enum TurbineDirections
    {
        顺时针 = 0,
        逆时针 = 1,
    }
}
