using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Web;
using System.Xml;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    [TypeConverter(typeof(IconConverter))]
    //[Editor(typeof(IconEditor), typeof(UITypeEditor))]
    [Serializable]
    public class Icon
    {
        /// <summary>
        /// 大类
        /// </summary>
        public string IconType { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public string Name { get; set; }

        public static Icon GetChildAsFont(XmlNode parentXmlNode, string childNodeName)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);

            if (node == null)
            {
                return null;
            }
            else
            {
                Icon font = new Icon();
                font.IconType = node.GetChildAsString("IconType", "");
                font.Name = node.GetChildAsString("Name", "");
                return font;
            }
        }

        public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, Icon icon)
        {
            if (icon == null)
            {
                return null;
            }
            else
            {
                XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
                xmlElem.AppendElem("IconType", icon.IconType);
                xmlElem.AppendElem("Name", icon.Name);
                return (XmlElement)parentXmlElem.AppendChild(xmlElem);
            }
        }

        /// <summary>
        /// Клонировать объект
        /// </summary>
        public Icon Clone()
        {
            return (Icon)ScadaUtils.DeepClone(this);
        }
    }
}