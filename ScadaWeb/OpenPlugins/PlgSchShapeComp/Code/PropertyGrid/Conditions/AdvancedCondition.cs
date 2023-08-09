using System.Xml;
using System.ComponentModel;

using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid
{
	[Serializable]
	public class AdvancedCondition : Condition
	{
		public enum BlinkingSpeed
		{
			None,
			Slow,
			Fast
		}

		public AdvancedCondition()
			: base()
		{
			BackgroundColor = "";
			TextContent = "";
			IsVisible = true;
			Width = 0;
			Height = 0;
			Blinking = BlinkingSpeed.None;
		}

		[DisplayName("Background Color"), Category(Categories.Appearance)]
		public string BackgroundColor { get; set; }

		[DisplayName("Text Content"), Category(Categories.Appearance)]
		public string TextContent { get; set; }

		[DisplayName("Visible"), Category(Categories.Appearance)]
		public bool IsVisible { get; set; }

		[DisplayName("Rotation"), Category(Categories.Appearance)]
		[Description("The rotation angle of the shape in degrees.")]
		public int Rotation { get; set; }

		[DisplayName("Width"), Category(Categories.Appearance)]
		public int Width { get; set; }

		[DisplayName("Height"), Category(Categories.Appearance)]
		public int Height { get; set; }

		[DisplayName("Blinking Speed"), Category(Categories.Appearance)]
		public BlinkingSpeed Blinking { get; set; }

		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);
			BackgroundColor = xmlNode.GetChildAsString("BackgroundColor");
			TextContent = xmlNode.GetChildAsString("TextContent");
			IsVisible = xmlNode.GetChildAsBool("IsVisible");
			Rotation = xmlNode.GetChildAsInt("Rotation");
			Width = xmlNode.GetChildAsInt("Width");
			Height = xmlNode.GetChildAsInt("Height");
			Blinking = xmlNode.GetChildAsEnum<BlinkingSpeed>("Blinking");
		}

		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			xmlElem.AppendElem("BackgroundColor", BackgroundColor);
			xmlElem.AppendElem("Rotation", Rotation);
			xmlElem.AppendElem("TextContent", TextContent);
			xmlElem.AppendElem("IsVisible", IsVisible);
			xmlElem.AppendElem("Width", Width);
			xmlElem.AppendElem("Height", Height);
			xmlElem.AppendElem("Blinking", Blinking);
		}

		public override object Clone()
		{
			Condition clonedCondition = this.DeepClone();
			clonedCondition.SchemeView = SchemeView;
			return clonedCondition;
		}
	}
}