// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Model;
using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Lang;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchBasicComp.Code
{
    /// <summary>
    /// Scheme component that represents a button
    /// <para>Компонент схемы, представляющий кнопку</para>
    /// </summary>
    [Serializable]
    public class Button : ComponentBase, IDynamicComponent
    {
        /// <summary>
        /// Размер кнопки по умолчанию
        /// </summary>
        public static readonly Size DefaultSize = new(100, 30);
        /// <summary>
        /// Размер по умолчанию
        /// </summary>
        public static readonly Size DefaultImageSize = new(16, 16);
        /// <summary>
        /// Текст кнопки по умолчанию
        /// </summary>
        public static readonly string DefaultText = Locale.IsRussian ? "Кнопка" : "Button";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Button()
            : base()
        {
            //serBinder = PlgUtils.SerializationBinder;

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
        }


        /// <summary>
        /// Получить или установить цвет текста
        /// </summary>
        #region Attributes
        [DisplayName("Foreground color"), Category(Categories.Appearance)]
        [Description("The foreground color of the component, which is used to display text.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
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
        //[TypeConverter(typeof(ImageConverter)), Editor(typeof(ImageEditor), typeof(UITypeEditor))]
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
        /// Получить или установить действие
        /// </summary>
        #region Attributes
        [DisplayName("Action"), Category(Categories.Behavior)]
        [Description("The action executed by clicking the left mouse button on the component.")]
        [DefaultValue(Actions.None)]
        #endregion
        public Actions Action { get; set; }

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
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return BuildDisplayName(Text);
        }
    }
}