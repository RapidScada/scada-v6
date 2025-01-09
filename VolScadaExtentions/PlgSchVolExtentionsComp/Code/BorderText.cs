using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.SchVolExtentionsComp
{
    /// <summary>
    /// 带边框的文本
    /// </summary>
    [Serializable]
    public class BorderText : ComponentBase
    {
        /// <summary>
        /// 默认文字
        /// </summary>
        public static readonly string DefaultText = "";


        /// <summary>
        /// 构造
        /// </summary>
        public BorderText()
        {
            BorderWidth = 2;
            BorderColor = "DarkGray";
            ForeColor = "";
            Font = null;
            Text = DefaultText;
            HAlign = HorizontalAlignments.Left;
            VAlign = VerticalAlignments.Center;
            WordWrap = true;
            Size = new Size(100, 40);
        }


        /// <summary>
        /// 背景色
        /// </summary>
        #region Attributes
        [DisplayName("Foreground color"), Category(Categories.Appearance)]
        [Description("The foreground color of the component, which is used to display text.")]
        #endregion
        public string ForeColor { get; set; }

        /// <summary>
        /// 文字
        /// </summary>
        #region Attributes
        [DisplayName("Font"), Category(Categories.Appearance)]
        [Description("The font used to display text in the component.")]
        #endregion
        public Font Font { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        #region Attributes
        [DisplayName("Text"), Category(Categories.Appearance)]
        [Description("The text associated with the component.")]
        #endregion
        public string Text { get; set; }

        /// <summary>
        /// 水平对齐
        /// </summary>
        #region Attributes
        [DisplayName("Horizontal alignment"), Category(Categories.Appearance)]
        [Description("Horizontal alignment of the text within the component.")]
        [DefaultValue(HorizontalAlignments.Left)]
        #endregion
        public HorizontalAlignments HAlign { get; set; }

        /// <summary>
        /// 垂直对齐
        /// </summary>
        #region Attributes
        [DisplayName("Vertical alignment"), Category(Categories.Appearance)]
        [Description("Vertical alignment of the text within the component.")]
        [DefaultValue(VerticalAlignments.Top)]
        #endregion
        public VerticalAlignments VAlign { get; set; }

        /// <summary>
        /// 自动换行
        /// </summary>
        #region Attributes
        [DisplayName("Word wrap"), Category(Categories.Appearance)]
        [Description("Text is automatically word-wrapped.")]
        [DefaultValue(false), TypeConverter(typeof(BooleanConverter))]
        #endregion
        public bool WordWrap { get; set; }

        /// <summary>
        /// 从xml中加载
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            ForeColor = xmlNode.GetChildAsString("ForeColor");
            Font = Font.GetChildAsFont(xmlNode, "Font");
            Text = xmlNode.GetChildAsString("Text");
            HAlign = xmlNode.GetChildAsEnum<HorizontalAlignments>("HAlign");
            VAlign = xmlNode.GetChildAsEnum<VerticalAlignments>("VAlign");
            WordWrap = xmlNode.GetChildAsBool("WordWrap");
        }

        /// <summary>
        /// 保存到xml中
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);

            xmlElem.AppendElem("ForeColor", ForeColor);
            Font.AppendElem(xmlElem, "Font", Font);
            xmlElem.AppendElem("Text", Text);
            xmlElem.AppendElem("HAlign", HAlign);
            xmlElem.AppendElem("VAlign", VAlign);
            xmlElem.AppendElem("WordWrap", WordWrap);
        }

        /// <summary>
        /// 重写string
        /// </summary>
        public override string ToString()
        {
            return BuildDisplayName(Text);
        }
    }
}