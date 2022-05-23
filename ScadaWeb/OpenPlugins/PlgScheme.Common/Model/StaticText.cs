// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.PlgScheme.Model
{
    /// <summary>
    /// Scheme component that represents static text
    /// <para>Компонент схемы, представляющий статическую надпись</para>
    /// </summary>
    [Serializable]
    public class StaticText : ComponentBase
    {
        /// <summary>
        /// Текст надписи по умолчанию
        /// </summary>
        public static readonly string DefaultText = Locale.IsRussian ? "Статическая надпись" : "Static text";


        /// <summary>
        /// Конструктор
        /// </summary>
        public StaticText()
        {
            ForeColor = "";
            Font = null;
            Text = DefaultText;
            HAlign = HorizontalAlignments.Left;
            VAlign = VerticalAlignments.Top;
            WordWrap = false;
            AutoSize = true;
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
        /// Получить или установить текст
        /// </summary>
        #region Attributes
        [DisplayName("Text"), Category(Categories.Appearance)]
        [Description("The text associated with the component.")]
        #endregion
        public string Text { get; set; }

        /// <summary>
        /// Получить или установить горизонтальное выравнивание
        /// </summary>
        #region Attributes
        [DisplayName("Horizontal alignment"), Category(Categories.Appearance)]
        [Description("Horizontal alignment of the text within the component.")]
        [DefaultValue(HorizontalAlignments.Left)]
        #endregion
        public HorizontalAlignments HAlign { get; set; }

        /// <summary>
        /// Получить или установить вертикальное выравнивание
        /// </summary>
        #region Attributes
        [DisplayName("Vertical alignment"), Category(Categories.Appearance)]
        [Description("Vertical alignment of the text within the component.")]
        [DefaultValue(VerticalAlignments.Top)]
        #endregion
        public VerticalAlignments VAlign { get; set; }

        /// <summary>
        /// Получить или установить признак переноса текста по словам
        /// </summary>
        #region Attributes
        [DisplayName("Word wrap"), Category(Categories.Appearance)]
        [Description("Text is automatically word-wrapped.")]
        [DefaultValue(false), TypeConverter(typeof(BooleanConverter))]
        #endregion
        public bool WordWrap { get; set; }

        /// <summary>
        /// Получить или установить признак авто размера
        /// </summary>
        #region Attributes
        [DisplayName("Auto size"), Category(Categories.Layout)]
        [Description("Automatic resizing based on content size.")]
        [DefaultValue(true), TypeConverter(typeof(BooleanConverter))]
        #endregion
        public bool AutoSize { get; set; }


        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
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
            AutoSize = xmlNode.GetChildAsBool("AutoSize");
        }

        /// <summary>
        /// Сохранить конфигурацию компонента в XML-узле
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
            xmlElem.AppendElem("AutoSize", AutoSize);
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
