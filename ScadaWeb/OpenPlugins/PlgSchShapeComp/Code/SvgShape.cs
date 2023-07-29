using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code
{
	[Serializable]
	public class SvgShape : ComponentBase
	{
		public SvgShape()
		{
			ShapeType = "Circle";
			BackColor = "black";
		}

		[DisplayName("Shape Type"), Category(Categories.Appearance)]
		[Description("The type of SVG shape.")]
		public string ShapeType { get; set; }

		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);
			ShapeType = xmlNode.GetChildAsString("ShapeType");
		}

		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			xmlElem.AppendElem("ShapeType", ShapeType);
		}
	}
}