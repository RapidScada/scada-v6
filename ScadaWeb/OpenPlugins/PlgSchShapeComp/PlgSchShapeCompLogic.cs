using Scada.Web.Plugins.PlgScheme;
using Scada.Web.Plugins.PlgSchShapeComp.Code;
using Scada.Web.Services;



namespace Scada.Web.Plugins.PlgSchShapeComp
{
	public class PlgSchShapeCompLogic : PluginLogic, ISchemeComp
	{
		public PlgSchShapeCompLogic(IWebContext webContext) : base(webContext)
		{
		}

		public override string Code => "PlgSchShapeComp";
		CompLibSpec ISchemeComp.CompLibSpec
		{
			get
			{
				return new ShapeCompLibSpec();
			}
		 }

	}
}
