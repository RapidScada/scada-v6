
using System.ComponentModel;
using System.Xml;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using static Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid.AdvancedConditions;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid
{
	public class PictureConditions : ImageCondition
	{
		public PictureConditions()
			: base()
		{
			IsVisible = true;
			Blinking = BlinkingSpeed.None;
		}

		[DisplayName("Rotation"), Category(Categories.Appearance)]
		[Description("The rotation angle of the shape in degrees.")]
		public string Rotation { get; set; }



		[DisplayName("Blinking Speed"), Category(Categories.Appearance)]
		public BlinkingSpeed Blinking { get; set; }

		[DisplayName("Visible"), Category(Categories.Appearance)]
		public bool IsVisible { get; set; }


		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);
			IsVisible = xmlNode.GetChildAsBool("IsVisible");
			Rotation = xmlNode.GetChildAsString("Rotation");
			Blinking = xmlNode.GetChildAsEnum<BlinkingSpeed>("Blinking");
		}

		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
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
