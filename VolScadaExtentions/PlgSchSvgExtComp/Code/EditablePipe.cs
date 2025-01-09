using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using Scada.Web.Plugins.PlgSchSvgExtComp;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// Scheme component that represents static picture
    /// </summary>
    [Serializable]
    public class EditablePipe : ComponentBase,IDynamicComponent
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public EditablePipe()
        {
            serBinder = PlgUtils.SerializationBinder;

            BorderColor = "Gray";
            BorderWidth = 0;
            InCnlNum = 0;
            MinValue = 0;
            MaxValue = 100;
            PipeSize = PipeSize.Default;
            PipePadding = 0.0D;
            PipeBackColor = "#0a7ae2";
            PipeFlowColor = "#119bfa";
            PipeLineType = PipeLineTypes.线路;
            PipeDirection = EditablePipeDirections.正向;
            Size = new Size(170, 50);
            Conditions = new List<ColorCondition>();
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
        /// 管道类型
        /// </summary>
        [DisplayName("Pipe type"), Category(Categories.Appearance)]
        [Description("The pipe line type associated with the component.")]
        public PipeLineTypes PipeLineType { get; set; }

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
        [Obsolete("可编辑不需要该属性"), Browsable(false)]
        public double PipePadding { get; set; }

        /// <summary>
        /// 管道颜色
        /// </summary>
        [DisplayName("Pipe color"), Category(Categories.Appearance)]
        [Description("The pipe color associated with the component.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string PipeFlowColor { get; set; }

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
        [DefaultValue(EditablePipeDirections.正向)]
        public EditablePipeDirections PipeDirection { get; set; }

        /// <summary>
        /// 显示颜色条件
        /// </summary>
        #region Attributes
        [DisplayName("Conditions"), Category(Categories.Behavior)]
        [Description("The conditions for button color depending on the value of the input channel.")]
        [DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
        #endregion
        public List<ColorCondition> Conditions { get; protected set; }


        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            MaxValue = xmlNode.GetChildAsInt("MaxValue");
            MinValue = xmlNode.GetChildAsInt("MinValue");
            PipeSize = PipeSize.GetChildAsSize(xmlNode, "PipeSize");
            PipePadding = xmlNode.GetChildAsDouble("PipePadding");
            PipeFlowColor = xmlNode.GetChildAsString("PipeColor");
            PipeBackColor = xmlNode.GetChildAsString("PipeBackColor");
            PipeDirection = xmlNode.GetChildAsEnum<EditablePipeDirections>("PipeDirection");
            PipeLineType = xmlNode.GetChildAsEnum<PipeLineTypes>("PipeLineType");

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
            xmlElem.AppendElem("PipeColor", PipeFlowColor);
            xmlElem.AppendElem("PipeBackColor", PipeBackColor);
            xmlElem.AppendElem("PipeDirection", PipeDirection);
            xmlElem.AppendElem("PipeLineType", PipeLineType);

            XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
            foreach (Condition condition in Conditions)
            {
                XmlElement conditionElem = conditionsElem.AppendElem("Condition");
                condition.SaveToXml(conditionElem);
            }
        }
    }
    /// <summary>
    /// 可编辑管流向
    /// </summary>
    public enum EditablePipeDirections
    {
        正向 = 0,
        反向 = 1,
    }
    /// <summary>
    /// 类型
    /// </summary>
    public enum PipeLineTypes
    {
        线路 = 0,
        管路 = 1,
        轨迹 = 2,
    }
}
