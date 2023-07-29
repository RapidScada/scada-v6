using System;
using System.Xml;
using  System.ComponentModel;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code
{
	[TypeConverter(typeof(PopupSizeConverter))]
	[Serializable]
	public struct PopupSize
	{
		/// <summary>
		/// Default size
		/// </summary>
		public static readonly PopupSize Default = new PopupSize(PopupWidth.Normal, 300);


		/// <summary>
		/// Constructor
		/// </summary>
		public PopupSize(PopupWidth width, int height)
			: this()
		{
			Width = width;
			Height = height;
		}


		/// <summary>
		/// Get or set the width
		/// </summary>
		[DisplayName("Width")]
		public PopupWidth Width { get; set; }

		/// <summary>
		/// Get or set the height
		/// </summary>
		[DisplayName("Height")]
		public int Height { get; set; }


		/// <summary>
		/// Get the value of the child XML node as a size
		/// </summary>
		public static PopupSize GetChildAsSize(XmlNode parentXmlNode, string childNodeName)
		{
			XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
			return node == null ?
				Default :
				new PopupSize(node.GetChildAsEnum<PopupWidth>("Width"), node.GetChildAsInt("Height"));
		}

		/// <summary>
		/// Create and add an XML element of size
		/// </summary>
		public static XmlElement AppendElem(XmlElement parentXmlElem, string elemName, PopupSize popupSize)
		{
			XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
			xmlElem.AppendElem("Width", popupSize.Width);
			xmlElem.AppendElem("Height", popupSize.Height);
			return (XmlElement)parentXmlElem.AppendChild(xmlElem);
		}
	}
}