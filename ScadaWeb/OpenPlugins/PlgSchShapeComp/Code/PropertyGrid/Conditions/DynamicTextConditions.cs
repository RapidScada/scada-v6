using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid
{
	[Serializable]
	public class DynamicTextConditions : AdvancedConditions
	{
		public DynamicTextConditions()
			: base()
		{
			IsVisible = true;
			Blinking = BlinkingSpeed.None;
			TextContent = "";
			FontSize = "";
		}

		[DisplayName("FontSize"), Category(Categories.Appearance)]
		public string FontSize { get; set; }

		[DisplayName("Text Content"), Category(Categories.Appearance)]
		public string TextContent { get; set; }

		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);
			TextContent = xmlNode.GetChildAsString("TextContent");
			FontSize = xmlNode.GetChildAsString("FontSize");
		}

		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			xmlElem.AppendElem("TextContent", TextContent);
			xmlElem.AppendElem("FontSize", FontSize);
		}

	}
}