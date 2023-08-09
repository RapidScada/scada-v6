using Scada.Web.Plugins.PlgScheme;
using Scada.Web.Plugins.PlgScheme.Model;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code
{
	public class ShapeCompFactory : CompFactory
	{
		public override ComponentBase CreateComponent(string typeName, bool nameIsShort)
		{
			if (NameEquals("SvgShape", typeof(SvgShape).FullName, typeName, nameIsShort))
				return new SvgShape();
			else if (NameEquals("Polygon", typeof(Polygon).FullName,typeName,nameIsShort))
				return new Polygon();
			else if(NameEquals("CustomSVG", typeof(CustomSVG).FullName,typeName,nameIsShort)) 
				return new CustomSVG();
			else if (NameEquals("BarGraph", typeof(BarGraph).FullName, typeName, nameIsShort))
				return new BarGraph();
			else
				return null;
        }
	}
}