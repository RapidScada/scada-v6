using System;
using System.Drawing.Design;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgSchSvgExtComp;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// Scheme component that represents a button
    /// </summary>
    [Serializable]
    public class ColoredButton : ComponentBase, IDynamicComponent
    {
        /// <summary>
        /// 默认尺寸
        /// </summary>
        public static readonly Size DefaultSize = new Size(100, 30);
        /// <summary>
        /// 默认图片尺寸
        /// </summary>
        public static readonly Size DefaultImageSize = new Size(16, 16);
        /// <summary>
        /// 默认文本
        /// </summary>
        public static readonly string DefaultText = "颜色按钮";

        /// <summary>
        /// 构造
        /// </summary>
        public ColoredButton()
            : base()
        {
            serBinder = PlgUtils.SerializationBinder;

            ForeColor = "";
            Font = null;
            ImageName = "";
            ImageSize = DefaultImageSize;
            Text = DefaultText;
            Action = Actions.None;
            BoundProperty = BoundProperties.None;
            InCnlNum = 0;
            CtrlCnlNum = 0;
            Size = DefaultSize;
            Conditions = new List<ColorCondition>();
            ActionValue = 0;
        }


        /// <summary>
        /// Получить или установить цвет текста
        /// </summary>
        #region Attributes
        [DisplayName("Foreground color"), Category(Categories.Appearance)]
        [Description("The foreground color of the component, which is used to display text.")]
        #endregion
        public string ForeColor { get; set; }

        /// <summary>
        /// Получить или установить шрифт
        /// </summary>
        #region Attributes
        [DisplayName("Font"), Category(Categories.Appearance)]
        [Description("The font used to display text in the component.")]
        #endregion
        public Font Font { get; set; }

        /// <summary>
        /// Получить или установить наименование изображения
        /// </summary>
        #region Attributes
        [DisplayName("Image"), Category(Categories.Appearance)]
        [Description("The image from the collection of scheme images.")]
        [DefaultValue("")]
        #endregion
        public string ImageName { get; set; }

        /// <summary>
        /// Получить или установить размер изображения
        /// </summary>
        #region Attributes
        [DisplayName("Image size"), Category(Categories.Appearance)]
        [Description("The size of the button image in pixels.")]
        #endregion
        public Size ImageSize { get; set; }

        /// <summary>
        /// Получить или установить текст
        /// </summary>
        #region Attributes
        [DisplayName("Text"), Category(Categories.Appearance)]
        [Description("The text associated with the component.")]
        #endregion
        public string Text { get; set; }

        /// <summary>
        /// 动作
        /// </summary>
        #region Attributes
        [DisplayName("Action"), Category(Categories.Behavior)]
        [Description("The action executed by clicking the left mouse button on the component.")]
        [DefaultValue(Actions.None)]
        #endregion
        public Actions Action { get; set; }

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
        /// 动作值
        /// </summary>
        #region Attributes
        [DisplayName("Action value"), Category(Categories.Behavior)]
        [Description("The action value for send command now.")]
        [DefaultValue(Actions.None)]
        #endregion
        public double ActionValue { get; set; }

        /// <summary>
        /// Получить или установить свойство, привязанное ко входному каналу
        /// </summary>
        #region Attributes
        [DisplayName("Bound Property"), Category(Categories.Behavior)]
        [Description("The button property that is bound to the input channel associated with the component.")]
        [DefaultValue(BoundProperties.None)]
        #endregion
        public BoundProperties BoundProperty { get; set; }

        /// <summary>
        /// Получить или установить номер входного канала
        /// </summary>
        #region Attributes
        [DisplayName("Input channel"), Category(Categories.Data)]
        [Description("The input channel number associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int InCnlNum { get; set; }

        /// <summary>
        /// Получить или установить номер канала управления
        /// </summary>
        #region Attributes
        [DisplayName("Output channel"), Category(Categories.Data)]
        [Description("The output channel number associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int CtrlCnlNum { get; set; }

        /// <summary>
        /// Получить или установить размер всплывающего окна
        /// </summary>
        #region Attributes
        [DisplayName("Popup size"), Category("Navigation")]
        [Description("The size of a popup for the appropriate target.")]
        #endregion
        public PopupSize PopupSize { get; set; }

        /// <summary>
        /// Получить или установить целевое окно для перехода
        /// </summary>
        #region Attributes
        [DisplayName("Target"), Category("Navigation")]
        [Description("The target frame for the link.")]
        [DefaultValue(LinkTarget.Self)]
        #endregion
        public LinkTarget Target { get; set; }

        /// <summary>
        /// Получить или установить адрес для перехода
        /// </summary>
        #region Attributes
        [DisplayName("URL"), Category("Navigation")]
        [Description("The address to navigate.")]
        #endregion
        public string Url { get; set; }

        /// <summary>
        /// Получить или установить ид. представления для перехода
        /// </summary>
        #region Attributes
        [DisplayName("View ID"), Category("Navigation")]
        [Description("The identifier of the view to navigate.")]
        [DefaultValue(0)]
        #endregion
        public int ViewID { get; set; }

        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            ForeColor = xmlNode.GetChildAsString("ForeColor");
            Font = Font.GetChildAsFont(xmlNode, "Font");
            ImageName = xmlNode.GetChildAsString("ImageName");
            ImageSize = Size.GetChildAsSize(xmlNode, "ImageSize");
            Text = xmlNode.GetChildAsString("Text");
            Action = xmlNode.GetChildAsEnum<Actions>("Action");
            BoundProperty = xmlNode.GetChildAsEnum<BoundProperties>("BoundProperty");
            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
            ActionValue = xmlNode.GetChildAsDouble("ActionValue");
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

            PopupSize = PopupSize.GetChildAsSize(xmlNode, "PopupSize");
            Target = xmlNode.GetChildAsEnum<LinkTarget>("Target");
            Url = xmlNode.GetChildAsString("Url");
            ViewID = xmlNode.GetChildAsInt("ViewID");

        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);

            xmlElem.AppendElem("ForeColor", ForeColor);
            Font.AppendElem(xmlElem, "Font", Font);
            xmlElem.AppendElem("ImageName", ImageName);
            Size.AppendElem(xmlElem, "ImageSize", ImageSize);
            xmlElem.AppendElem("Text", Text);
            xmlElem.AppendElem("Action", Action);
            xmlElem.AppendElem("BoundProperty", BoundProperty);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
            xmlElem.AppendElem("CtrlCnlNum", CtrlCnlNum);
            xmlElem.AppendElem("ActionValue", ActionValue);
            XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
            foreach (Condition condition in Conditions)
            {
                XmlElement conditionElem = conditionsElem.AppendElem("Condition");
                condition.SaveToXml(conditionElem);
            }
            PopupSize.AppendElem(xmlElem, "PopupSize", PopupSize);
            xmlElem.AppendElem("Target", Target);
            xmlElem.AppendElem("Url", Url);
            xmlElem.AppendElem("ViewID", ViewID);
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return BuildDisplayName(Text);
        }

        /// <summary>
        /// 复制元素
        /// </summary>
        public override ComponentBase Clone()
        {
            ColoredButton clonedComponent = (ColoredButton)base.Clone();

            foreach (Condition condition in clonedComponent.Conditions)
            {
                condition.SchemeView = SchemeView;
            }

            return clonedComponent;
        }
    }
}