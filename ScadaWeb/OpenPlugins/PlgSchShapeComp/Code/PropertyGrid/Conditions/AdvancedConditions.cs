using System.Xml;
using System.ComponentModel;

using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid
{
	[Serializable]
	public class AdvancedConditions : Condition
	{
		public enum BlinkingSpeed
		{
			None,
			Slow,
			Fast
		}

		public AdvancedConditions()
			: base()
		{
			BackgroundColor = "";
			IsVisible = true;
			Blinking = BlinkingSpeed.None;
		}

		[DisplayName("Background Color"), Category(Categories.Appearance)]
		public string BackgroundColor { get; set; }

		[DisplayName("Visible"), Category(Categories.Appearance)]
		public bool IsVisible { get; set; }

		[DisplayName("Rotation"), Category(Categories.Appearance)]
		public string Rotation { get; set; }

		[DisplayName("Blinking Speed"), Category(Categories.Appearance)]
		public BlinkingSpeed Blinking { get; set; }

		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);
			BackgroundColor = xmlNode.GetChildAsString("BackgroundColor");
			IsVisible = xmlNode.GetChildAsBool("IsVisible");
			Rotation = xmlNode.GetChildAsString("Rotation");
			Blinking = xmlNode.GetChildAsEnum<BlinkingSpeed>("Blinking");
		}

		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			xmlElem.AppendElem("BackgroundColor", BackgroundColor);
			xmlElem.AppendElem("Rotation", Rotation);
			xmlElem.AppendElem("IsVisible", IsVisible);
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