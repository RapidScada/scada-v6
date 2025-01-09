using Scada.Web.Plugins.PlgScheme;
using Scada.Web.Plugins.PlgScheme.Model;
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
    public class TextScrollCondition : ISchemeViewAvailable, ICloneable
    {

        public TextScrollCondition()
        {
            SchemeView = null;
            Conditions = new List<TextScrollDetailCondition>();
        }

        [NonSerialized]
        protected SchemeView schemeView;

        [Browsable(false)]
        public SchemeView SchemeView
        {
            get
            {
                return schemeView;
            }
            set
            {
                schemeView = value;
            }
        }


        #region Attributes
        [DisplayName("Label"), Category(Categories.Appearance)]
        [DefaultValue("")]
        #endregion
        public string LabelName { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        #region Attributes
        [DisplayName("Conditions"), Category(Categories.Behavior)]
        [Description("The conditions for label output depending on the value of the input channel.")]
        [DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
        //[CM.Editor(typeof(TextScrollCollectionEditor), typeof(UITypeEditor))]
        #endregion
        public List<TextScrollDetailCondition> Conditions { get; protected set; }

        /// <summary>
        /// 通道号
        /// </summary>
        #region Attributes
        [DisplayName("Input channel"), Category(Categories.Data)]
        [Description("The input channel number associated with the component.")]
        [DefaultValue(0)]
        #endregion
        public int InCnlNum { get; set; }


        public virtual object Clone()
        {
            TextScrollCondition clonedCondition = ScadaUtils.DeepClone(this, PlgUtils.SerializationBinder);
            clonedCondition.SchemeView = SchemeView;
            return clonedCondition;
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public override string ToString()
        {
            return string.IsNullOrEmpty(LabelName) ? "名称" : LabelName;
        }


        /// <summary>
        /// 加载
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            LabelName = xmlNode.GetChildAsString("LabelName");
            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            XmlNode conditionsNode = xmlNode.SelectSingleNode("DetailConditions");
            if (conditionsNode != null)
            {
                Conditions = new List<TextScrollDetailCondition>();
                XmlNodeList conditionNodes = conditionsNode.SelectNodes("DetailCondition");
                foreach (XmlNode conditionNode in conditionNodes)
                {
                    TextScrollDetailCondition condition = new TextScrollDetailCondition();
                    condition.LoadFromXml(conditionNode);
                    Conditions.Add(condition); 
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            xmlElem.AppendElem("LabelName", LabelName);
            xmlElem.AppendElem("InCnlNum", InCnlNum);
            XmlElement conditionsElem = xmlElem.AppendElem("DetailConditions");
            foreach (Condition condition in Conditions)
            {
                XmlElement conditionElem = conditionsElem.AppendElem("DetailCondition");
                condition.SaveToXml(conditionElem);
            }
        }
    }
}