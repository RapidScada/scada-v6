using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System.ComponentModel;
using System.Xml;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid
{
	[Serializable]
	public class BarGraphCondition : AdvancedCondition
	{
		public enum BarLevel
		{
			None,
			Low,
			Min,
			High,
			Medium,
			Max
		}

		public BarGraphCondition() : base()
		{
			FillColor = "";
			Level = BarLevel.None;
		}

		[DisplayName("Fill Color"), Category(Categories.Appearance)]
		public string FillColor { get; set; }

		[DisplayName("Bar Level"), Category(Categories.Appearance)]
		public BarLevel Level { get; set; }

		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);
			FillColor = xmlNode.GetChildAsString("FillColor");
			Level = xmlNode.GetChildAsEnum<BarLevel>("Level");
		}

		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			xmlElem.AppendElem("FillColor", FillColor);
			xmlElem.AppendElem("Level", Level);
		}

		public override object Clone()
		{
			Condition clonedCondition = this.DeepClone();
			clonedCondition.SchemeView = SchemeView;
			return clonedCondition;
		}
	}
}
