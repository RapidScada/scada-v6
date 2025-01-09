using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using Scada.Web.Plugins.SchSvgExtComp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Xml;

namespace Scada.Web.Plugins
{
    [Serializable]
    public class TextReflectCondition : Condition
    {

        public TextReflectCondition()
            : base()
        {
            ForeColor = "Green";
            ShowName = "";
        }


        #region Attributes
        [DisplayName("Name"), Category(Categories.Appearance)]
        [DefaultValue("")]
        #endregion
        public string ShowName { get; set; }

        /// <summary>
        /// 静态颜色
        /// </summary>
        [DisplayName("Fore color"), Category(Categories.Appearance)]
        [Description("The fore color associated with the component.")]
        //[CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [DefaultValue("")]
        public string ForeColor { get; set; }


        public override object Clone()
        {
            TextReflectCondition clonedCondition = ScadaUtils.DeepClone(this, PlgUtils.SerializationBinder);
            clonedCondition.SchemeView = SchemeView;
            return clonedCondition;
        }

        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            ForeColor = xmlNode.GetChildAsString("ForeColor");
            ShowName = xmlNode.GetChildAsString("ShowName");
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("ForeColor", ForeColor);
            xmlElem.AppendElem("ShowName", ShowName);
        }
    }
}