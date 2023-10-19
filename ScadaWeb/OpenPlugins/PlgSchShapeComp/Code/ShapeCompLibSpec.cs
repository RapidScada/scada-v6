
using Scada.Web.Plugins.PlgScheme;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code
{
	public class ShapeCompLibSpec : CompLibSpec
	{
		public override string XmlPrefix => "shape";
		public override string XmlNs => "urn:rapidscada:scheme:shape";
		public override string GroupHeader => "Shape";

		public override List<string> Styles
		{
			get
			{
				return new List<string>()
				{
					"SchShapeComp/css/shapecomp.min.css"
				};
			}
		}
		public override List<string> Scripts
		{
			get
			{
				return new List<string>()
				{
					"SchShapeComp/js/shapecomp-render.js"
				};
			}
		}

		protected override List<CompItem> CreateCompItems()
		{
			return new List<CompItem>()
			{
				new CompItem(null,typeof(BasicShape)),
				new CompItem(null,	typeof(CustomSVG)),
				new CompItem(null, typeof(BarGraph)),
			};
		}
		protected override CompFactory CreateCompFactory()
		{
			return new ShapeCompFactory();
		}
	}

}