using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// Scheme component that represents static picture
    /// </summary>
    [Serializable]
    public class BasePipe : ComponentBase,IDynamicComponent
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public BasePipe()
        {
            serBinder = PlgUtils.SerializationBinder;

            BorderColor = "Gray";
            BorderWidth = 0;
            InCnlNum = 0;
            MinValue = 0;
            MaxValue = 100;
            PipeSize = PipeSize.Default;
            PipePadding = 0.0D;
            PipeColor = "#ec912a";
            PipeBackColor = "Blue";

            PipeDirection = PipeDirections.左;
            Size = new Size(170, 50);
        }

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
        /// 管道尺寸
        /// </summary>
        [DisplayName("Pipe size"), Category(Categories.Appearance)]
        [Description("The pipe size associated with the component.")]
        public PipeSize PipeSize { get; set; }

        /// <summary>
        /// 管道边距
        /// </summary>
        [DisplayName("Pipe padding"), Category(Categories.Appearance)]
        [Description("The pipe padding associated with the component.")]
        public double PipePadding { get; set; }

        /// <summary>
        /// 管道颜色
        /// </summary>
        [DisplayName("Pipe color"), Category(Categories.Appearance)]
        [Description("The pipe color associated with the component.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string PipeColor { get; set; }

        /// <summary>
        /// 管道背色
        /// </summary>
        [DisplayName("Pipe back color"), Category(Categories.Appearance)]
        [Description("The pipe back color associated with the component.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string PipeBackColor { get; set; }


        /// <summary>
        /// 管道流方向
        /// </summary>
        [DisplayName("Pipe direction"), Category(Categories.Appearance)]
        [Description("The pipe direction associated with the component.")]
        [DefaultValue(PipeDirections.左)]
        public PipeDirections PipeDirection { get; set; }

        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");

            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            MaxValue = xmlNode.GetChildAsInt("MaxValue");
            MinValue = xmlNode.GetChildAsInt("MinValue");
            PipeSize = PipeSize.GetChildAsSize(xmlNode, "PipeSize");
            PipePadding = xmlNode.GetChildAsDouble("PipePadding");
            PipeColor = xmlNode.GetChildAsString("PipeColor");
            PipeBackColor = xmlNode.GetChildAsString("PipeBackColor");
            PipeDirection = xmlNode.GetChildAsEnum<PipeDirections>("PipeDirection");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
            xmlElem.AppendElem("MaxValue", MaxValue);
            xmlElem.AppendElem("MinValue", MinValue);
            PipeSize.AppendElem(xmlElem, "PipeSize", PipeSize);
            xmlElem.AppendElem("PipePadding", PipePadding);
            xmlElem.AppendElem("PipeColor", PipeColor);
            xmlElem.AppendElem("PipeBackColor", PipeBackColor);
            xmlElem.AppendElem("PipeDirection", PipeDirection);
        }
    }

    public enum PipeDirections
    {
        左 = 0,
        右 = 1,
        上 = 2,
        下 = 3,
    }
}
