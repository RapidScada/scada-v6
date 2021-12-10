// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Lang;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchBasicComp.Code
{
    /// <summary>
    /// Scheme component that represents a link
    /// <para>Компонент схемы, представляющий ссылку</para>
    /// </summary>
    [Serializable]
    public class Link : StaticText
    {
        /// <summary>
        /// Текст ссылки по умолчанию
        /// </summary>
        new public static readonly string DefaultText = Locale.IsRussian ? "Ссылка" : "Link";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Link()
            : base()
        {
            //serBinder = PlgUtils.SerializationBinder;

            Text = DefaultText;
            BackColorOnHover = "";
            BorderColorOnHover = "";
            ForeColorOnHover = "";
            UnderlineOnHover = true;
            CnlNums = new List<int>();
            PopupSize = PopupSize.Default;
            Target = LinkTarget.Self;
            Url = "";
            ViewID = 0;
        }


        /// <summary>
        /// Получить или установить цвет фона при наведении указателя мыши
        /// </summary>
        #region Attributes
        [DisplayName("Back color on hover"), Category(Categories.Behavior)]
        [Description("The background color of the component when user rests the pointer on it.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string BackColorOnHover { get; set; }

        /// <summary>
        /// Получить или установить цвет рамки при наведении указателя мыши
        /// </summary>
        #region Attributes
        [DisplayName("Border color on hover"), Category(Categories.Behavior)]
        [Description("The border color of the component when user rests the pointer on it.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string BorderColorOnHover { get; set; }

        /// <summary>
        /// Получить или установить цвет текста при наведени указателя мыши
        /// </summary>
        #region Attributes
        [DisplayName("Fore color on hover"), Category(Categories.Behavior)]
        [Description("The foreground color of the component, which is used to display text, when user rests the pointer on it.")]
        //[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        #endregion
        public string ForeColorOnHover { get; set; }

        /// <summary>
        /// Получить или установить признак подчёркивания при наведении указателя мыши
        /// </summary>
        #region Attributes
        [DisplayName("Underline on hover"), Category(Categories.Behavior)]
        [Description("Underline text when user rests the pointer on the component.")]
        [DefaultValue(false), TypeConverter(typeof(BooleanConverter))]
        #endregion
        public bool UnderlineOnHover { get; set; }

        /// <summary>
        /// Gets the input channels displayed on the chart.
        /// </summary>
        #region Attributes
        [DisplayName("Input channels"), Category(Categories.Data)]
        [Description("The input channels that are inserted as URL parameters.")]
        //[TypeConverter(typeof(RangeConverter)), Editor(typeof(RangeEditor), typeof(UITypeEditor))]
        #endregion
        public List<int> CnlNums { get; protected set; }

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
        /// Gets the input channel numbers associated with the component.
        /// </summary>
        public override List<int> GetInCnlNums()
        {
            return CnlNums;
        }

        /// <summary>
        /// Загрузить конфигурацию компонента из XML-узла
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            BackColorOnHover = xmlNode.GetChildAsString("BackColorOnHover");
            BorderColorOnHover = xmlNode.GetChildAsString("BorderColorOnHover");
            ForeColorOnHover = xmlNode.GetChildAsString("ForeColorOnHover");
            UnderlineOnHover = xmlNode.GetChildAsBool("UnderlineOnHover");
            CnlNums.Clear();
            CnlNums.AddRange(ScadaUtils.ParseIntArray(xmlNode.GetChildAsString("CnlNums")));
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

            xmlElem.AppendElem("BackColorOnHover", BackColorOnHover);
            xmlElem.AppendElem("BorderColorOnHover", BorderColorOnHover);
            xmlElem.AppendElem("ForeColorOnHover", ForeColorOnHover);
            xmlElem.AppendElem("UnderlineOnHover", UnderlineOnHover);
            xmlElem.AppendElem("CnlNums", CnlNums.ToLongString());
            PopupSize.AppendElem(xmlElem, "PopupSize", PopupSize);
            xmlElem.AppendElem("Target", Target);
            xmlElem.AppendElem("Url", Url);
            xmlElem.AppendElem("ViewID", ViewID);
        }
    }
}
