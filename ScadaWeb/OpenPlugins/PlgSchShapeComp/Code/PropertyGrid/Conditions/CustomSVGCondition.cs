
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid
{
	[Serializable]
	public class CustomSVGCondition : AdvancedCondition
	{
		public CustomSVGCondition()
			: base()
		{
			TextContent = "";
			IsVisible = true;
			Blinking = BlinkingSpeed.None;
		}

		[DisplayName("Text Content"), Category(Categories.Appearance)]
		public string TextContent { get; set; }

		[DisplayName("Is Visible"), Category(Categories.Appearance)]
		public bool IsVisible { get; set; }

		[DisplayName("Blinking"), Category(Categories.Appearance)]
		public BlinkingSpeed Blinking { get; set; }

	}
}
