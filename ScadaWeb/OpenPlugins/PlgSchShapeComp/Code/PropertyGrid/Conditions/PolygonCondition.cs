
using System.Xml;
using System.ComponentModel;

using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid
{
	/// <summary>
	/// additional properties specific to polygons.
	/// </summary>
	[Serializable]
	public class PolygonCondition : AdvancedCondition
	{


		public PolygonCondition() : base()
		{
			Sides = 3;
			Color = "";
		}

		/// <summary>
		/// Property to get or set the number of sides in the polygon. 
		/// This value is user-defined.
		/// </summary>
		[DisplayName("Sides"), Category(Categories.Appearance)]
		[Description("The number of sides in the polygon.")]
		//[CM.Editor(typeof(NumberSelectEditor), typeof(UITypeEditor))]
		[DefaultValue(3)]
		public int Sides { get; set; }

		/// <summary>
		/// Property to get or set the color of the polygon. 
		/// </summary>
		[DisplayName("Color"), Category(Categories.Appearance)]
		[Description("The color of the polygon.")]
		//[CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
		public string Color { get; set; }

		/// <summary>
		/// Loads the PolygonCondition from an XML node
		/// </summary>
		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);
			Sides = xmlNode.GetChildAsInt("Sides");
			Color = xmlNode.GetChildAsString("Color");
		}

		/// <summary>
		/// Overriding SaveToXml
		/// </summary>
		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			xmlElem.AppendElem("Sides", Sides);
			xmlElem.AppendElem("Color", Color);

		}

		public override object Clone()
		{
			Condition clonedCondition = this.DeepClone();
			clonedCondition.SchemeView = SchemeView;
			return clonedCondition;
		}
	}
}