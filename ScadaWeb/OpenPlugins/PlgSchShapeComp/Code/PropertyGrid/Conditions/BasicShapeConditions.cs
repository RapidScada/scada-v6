using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid
{
	[Serializable]
	public class BasicShapeConditions : AdvancedConditions
	{
		public BasicShapeConditions()
					: base()
		{
			IsVisible = true;
			Blinking = BlinkingSpeed.None;
			BackgroundColor = "";
			Height = "";
			Width = "";
		}

		[DisplayName("Height"), Category(Categories.Appearance)]
		public string Height { get; set; }

		[DisplayName("Width"), Category(Categories.Appearance)]
		public string Width { get; set; }

		

		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);
			Height = xmlNode.GetChildAsString("Height");
			Width = xmlNode.GetChildAsString("Width");
		}

		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			xmlElem.AppendElem("Height", Height);
			xmlElem.AppendElem("Width", Width);
		}

	}
}