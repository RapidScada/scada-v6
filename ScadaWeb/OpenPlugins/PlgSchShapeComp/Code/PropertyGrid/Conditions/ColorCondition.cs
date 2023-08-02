
using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid
{
	[Serializable]
	public class ColorCondition : Condition
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public ColorCondition()
			: base()
		{
			Color = "";
		}


		/// <summary>
		/// Get or set the color displayed when the condition is met.
		/// </summary>
		#region Attributes
		[DisplayName("Color"), Category(Categories.Appearance)]
	
		#endregion
		public string Color { get; set; }


		/// <summary>
		/// Load the condition from an XML node.
		/// </summary>
		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);
			Color = xmlNode.GetChildAsString("Color");
		}

		/// <summary>
		/// Save the condition to an XML node.
		/// </summary>
		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			xmlElem.AppendElem("Color", Color);
		}

		/// <summary>
		/// Clone the object.
		/// </summary>
		public override object Clone()
		{
			Condition clonedCondition = this.DeepClone();
			clonedCondition.SchemeView = SchemeView;
			return clonedCondition;
		}
	}
}