using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    [TypeConverter(typeof(PipeSizeConverter))]
    [Serializable]
    public struct PipeSize
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly PipeSize Zero = new PipeSize(0, 0, 0);

        /// <summary>
        /// 默认
        /// </summary>
        public static readonly PipeSize Default = new PipeSize(20, 20, 5);


        /// <summary>
        /// 
        /// </summary>
        public PipeSize(int solidWidth, int flowWidth, int dashWidth)
            : this()
        {
            SolidWidth = solidWidth;
            FlowWidth = flowWidth;
            DashWidth = dashWidth;
        }


        /// <summary>
        /// 背景宽度
        /// </summary>
        [DisplayName("SolidWidth")]
        public int SolidWidth { get; set; }

        /// <summary>
        /// 流动宽度
        /// </summary>
        [DisplayName("FlowWidth")]
        public int FlowWidth { get; set; }

        /// <summary>
        /// 虚线宽度
        /// </summary>
        [DisplayName("DashWidth")]
        public int DashWidth { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public static PipeSize GetChildAsSize(XmlNode parentXmlNode, string childNodeName, PipeSize? defaultSize = null)
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            return node == null ?
                (defaultSize ?? Default) :
                new PipeSize(node.GetChildAsInt("SolidWidth"), node.GetChildAsInt("FlowWidth"), node.GetChildAsInt("DashWidth"));
        }

        /// <summary>
        /// 
        /// </summary>
        public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, PipeSize size)
        {
            XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
            xmlElem.AppendElem("SolidWidth", size.SolidWidth);
            xmlElem.AppendElem("FlowWidth", size.FlowWidth);
            xmlElem.AppendElem("DashWidth", size.DashWidth);
            return (XmlElement)parentXmlElem.AppendChild(xmlElem);
        }
    }
}