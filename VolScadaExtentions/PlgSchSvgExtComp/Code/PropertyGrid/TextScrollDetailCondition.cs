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
    public class TextScrollDetailCondition : Condition
    {

        public TextScrollDetailCondition()
            : base()
        {
            ShowName = "";
        }

        #region Attributes
        [DisplayName("Name"), Category(Categories.Appearance)]
        [Description("The name associated with the component.")]
        [DefaultValue("")]
        #endregion
        public string ShowName { get; set; }

        public override object Clone()
        {
            TextScrollDetailCondition clonedCondition = ScadaUtils.DeepClone(this, PlgUtils.SerializationBinder);
            clonedCondition.SchemeView = SchemeView;
            return clonedCondition;
        }


        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
            ShowName = xmlNode.GetChildAsString("ShowName");
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveToXml(XmlElement xmlElem)
        {
            base.SaveToXml(xmlElem);
            xmlElem.AppendElem("ShowName", ShowName);
        }
    }
}