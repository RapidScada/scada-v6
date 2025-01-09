using Scada.Data.Entities;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using Scada.Web.Plugins.PlgSchSvgExtComp;
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
    /// 传送带
    /// </summary>
    [Serializable]
    public class ConveyerBelt : ComponentBase, IDynamicComponent
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ConveyerBelt()
        {
            serBinder = PlgUtils.SerializationBinder;

            BorderColor = "Gray";
            BorderWidth = 0;
            InCnlNum = 0;
            Size = new Size(70, 70);
            ConveyerName = "";
            Conditions = new List<ConveyerBeltCondition>();
            Action = Actions.None;
            PopupSize = PopupSize.Default;
            Target = LinkTarget.Self;
            Url = "";
            ViewID = 0;
        }

        /// <summary>
        /// 传送带图片
        /// </summary>
        #region Attributes
        [DisplayName("ConveyerBelt"), Category(Categories.Appearance)]
        [Description("The conveyer belt from the collection of scheme images.")]
        [DefaultValue("")]
        #endregion
        public string ConveyerName { get; set; }

        /// <summary>
        /// 根据条件变化颜色
        /// </summary>
        #region Attributes
        [DisplayName("Conditions"), Category(Categories.Behavior)]
        [Description("The conditions for image output depending on the value of the input channel.")]
        [DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
        #endregion
        public List<ConveyerBeltCondition> Conditions { get; protected set; }

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
        /// 动作值
        /// </summary>
        #region Attributes
        [DisplayName("Action value"), Category(Categories.Behavior)]
        [Description("The action value.")]
        [DefaultValue(Actions.None)]
        #endregion
        public double ActionValue { get; set; }

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
        /// SVG内容
        /// </summary>
        [Browsable(false)]
        public string ConveyerContent
        {
            get
            {
                if (!string.IsNullOrEmpty(ConveyerName))
                {
                    var appDir = SvgExtApp.WebContext.AppDirs.PluginDir;
                    var imageDir = Path.Combine(appDir, "SchSvgExtComp", "ConveyerImage", ConveyerName);
                    try
                    {
                        return File.ReadAllText(imageDir, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        return $"文件{ConveyerName}异常，请确认是SVG格式，{ex.Message}";
                    }
                }

                return "";
            }
        }

        /// <summary>
        /// 输入通道
        /// </summary>
        #region Attributes
        [DisplayName("Input channel"), Category(Categories.Data)]
        [Description("The input channel number associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int InCnlNum { get; set; }

        /// <summary>
        /// 控制通道
        /// </summary>
        #region Attributes
        [DisplayName("Output channel"), Category(Categories.Data)]
        [Description("The output channel number associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int CtrlCnlNum { get; set; }

        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);

            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
            ActionValue = xmlNode.GetChildAsDouble("ActionValue");
            ConveyerName = xmlNode.GetChildAsString("ConveyerName");
            XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");
            if (conditionsNode != null)
            {
                Conditions = new List<ConveyerBeltCondition>();
                XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
                foreach (XmlNode conditionNode in conditionNodes)
                {
                    ConveyerBeltCondition condition = new ConveyerBeltCondition { SchemeView = SchemeView };
                    condition.LoadFromXml(conditionNode);
                    Conditions.Add(condition);
                }
            }
            Action = xmlNode.GetChildAsEnum<Actions>("Action");
            PopupSize = PopupSize.GetChildAsSize(xmlNode, "PopupSize");
            Target = xmlNode.GetChildAsEnum<LinkTarget>("Target");
            Url = xmlNode.GetChildAsString("Url");
            ViewID = xmlNode.GetChildAsInt("ViewID");
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
            xmlElem.AppendElem("CtrlCnlNum", CtrlCnlNum);
            xmlElem.AppendElem("ActionValue", ActionValue);
            xmlElem.AppendElem("ConveyerName", ConveyerName);
            XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
            foreach (Condition condition in Conditions)
            {
                XmlElement conditionElem = conditionsElem.AppendElem("Condition");
                condition.SaveToXml(conditionElem);
            }
            xmlElem.AppendElem("Action", Action);
            PopupSize.AppendElem(xmlElem, "PopupSize", PopupSize);
            xmlElem.AppendElem("Target", Target);
            xmlElem.AppendElem("Url", Url);
            xmlElem.AppendElem("ViewID", ViewID);
        }
    }
}
