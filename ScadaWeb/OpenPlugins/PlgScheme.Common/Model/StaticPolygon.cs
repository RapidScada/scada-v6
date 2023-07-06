using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;




namespace Scada.Web.Plugins.PlgScheme.Model
{
	/// <summary>
	/// Scheme component represents static polygon
	/// </summary>
	[Serializable]
	public class StaticPolygon : ComponentBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public StaticPolygon()
		{
			BackColor = "black";
			RoundedCorners = false;
			NumberOfSides = 4;
			CornerRadius = 0;
		}


		/// <summary>
		/// Get or set the Sides that define the Sides of polygon
		/// </summary>
		[DisplayName("Sides"), Category(Categories.Appearance)]
		[Description("The Sides that define the polygon.")]
		[DefaultValue(4)]
		public int NumberOfSides { get; set; }

		/// <summary>
		/// Get or set the corners of the polygon
		/// </summary>
		[DisplayName("Rounded corners"), Category(Categories.Appearance)]
		[Description("If true, the corners of the polygon will be rounded.")]
		[DefaultValue(false)]
		public bool RoundedCorners { get; set; }



		/// <summary>
		/// Get or set the radius of the rounded corners of the polygon
		/// </summary>
		[DisplayName("Corner radius"), Category(Categories.Appearance)]
		[Description("The radius of the rounded corners of the polygon.")]
		[DefaultValue(0)]
		public int CornerRadius { get; set; }

		/// <summary>
		/// Load component configuration from XML node
		/// </summary>
		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);
			//  PolyName = xmlNode.GetChildAsString("PolyName");
			NumberOfSides = xmlNode.GetChildAsInt("NumberOfSides");
			//BackgroundColor = xmlNode.GetChildAsString("BackgroundColor");
			RoundedCorners = xmlNode.GetChildAsBool("RoundedCorners");
			CornerRadius = xmlNode.GetChildAsInt("CornerRadius");
		}

		/// <summary>
		/// Save the configuration of a component in an XML node
		/// </summary>
		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			xmlElem.AppendElem("NumberOfSides", NumberOfSides);
			xmlElem.AppendElem("RoundedCorners", RoundedCorners);
			xmlElem.AppendElem("CornerRadius", CornerRadius);
		}

	}
}
