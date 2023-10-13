using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Log;


using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code
{
	[Serializable]
	public class CustomSVG : ComponentBase, IDynamicComponent
	{
		public CustomSVG() : base() 
		{

			Action = Actions.None;
			Conditions = new List<CustomSVGCondition>();
			InCnlNum = 0;
			CtrlCnlNum = 0;
			InCnlNumCustom = "NA (0)";
			CtrlCnlNumCustom = "NA (0)";
			SvgCode = "";
		}

		private string _svgCode;

		[DisplayName("SVG File"), Category(Categories.Appearance)]
		[Description("SVG file .")]
		[DefaultValue("")]
		public string SvgCode
		{
			get => _svgCode;
			set
			{
				_svgCode = value;
			}
		}


		[DisplayName("Conditions"), Category(Categories.Behavior)]
		[Description("The conditions for polygon output depending on the value of the input channel.")]
		[DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
		public List<CustomSVGCondition> Conditions { get; protected set; }


		/// <summary>
		/// Get or set the input channel number
		/// </summary>
		[Browsable(false)]
		[DisplayName("Input channel"), Category(Categories.Data)]
		[Description("The input channel number associated with the component.")]
		[DefaultValue(0)]
		public int InCnlNum { get; set; }

		[DisplayName("Rotation"), Category(Categories.Appearance)]
		[Description("The rotation angle of the SVG shape in degrees.")]
		[DefaultValue(0)]
		public int Rotation { get; set; }

		/// <summary>
		/// Get or set the input channel number 
		/// </summary>
		[DisplayName("Input channel"), Category(Categories.Data)]
		[Description("The input channel number associated with the component.")]
		public string InCnlNumCustom { get; set; }

		/// <summary>
		/// Get or set the control channel number
		/// </summary>
		[DisplayName("Output channel"), Category(Categories.Data)]
		[Description("The output channel number associated with the component.")]
		[DefaultValue(0)]
		public int CtrlCnlNum { get; set; }



		/// <summary>
		/// Get or set the control channel number custom
		/// </summary>
		[DisplayName("Output channel"), Category(Categories.Data)]
		[Description("The output channel number associated with the component.")]
		public string CtrlCnlNumCustom { get; set; }


		/// <summary>
		/// Get or set the action
		/// </summary>
		[DisplayName("Action"), Category(Categories.Behavior)]
		[Description("The action executed by clicking the left mouse button on the component.")]
		[DefaultValue(Actions.None)]
		public Actions Action { get; set; }

		
		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);

			Action = xmlNode.GetChildAsEnum<Actions>("Action");

			XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");

			if (conditionsNode != null)
			{
				Conditions = new List<CustomSVGCondition>();
				XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
				foreach (XmlNode conditionNode in conditionNodes)
				{
					CustomSVGCondition condition = new CustomSVGCondition { SchemeView = SchemeView };
					condition.LoadFromXml(conditionNode);
					Conditions.Add(condition);
				}
			}
			InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
			CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
			InCnlNumCustom = xmlNode.GetChildAsString("InCnlNumCustom");
			CtrlCnlNumCustom = xmlNode.GetChildAsString("CtrlCnlNumCustom");
			SvgCode = xmlNode.GetChildAsString("SVGCode");
			Rotation = xmlNode.GetChildAsInt("Rotation");

		}


		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);


			XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
			foreach (CustomSVGCondition condition in Conditions)
			{
				XmlElement conditionElem = conditionsElem.AppendElem("Condition");
				condition.SaveToXml(conditionElem);
			}

			xmlElem.AppendElem("SVGCode", SvgCode);
			xmlElem.AppendElem("Rotation", Rotation);
			xmlElem.AppendElem("InCnlNum", InCnlNum);
			xmlElem.AppendElem("CtrlCnlNum", CtrlCnlNum);
			xmlElem.AppendElem("Action", Action.ToString());
			xmlElem.AppendElem("InCnlNumCustom", InCnlNumCustom);
			xmlElem.AppendElem("CtrlCnlNumCustom", CtrlCnlNumCustom);

		}
		/// <summary>
		/// Clone  object
		/// </summary>
		public override ComponentBase Clone()
		{
			CustomSVG cloneComponent = (CustomSVG)base.Clone();

			foreach (AdvancedCondition condition in cloneComponent.Conditions)
			{
				condition.SchemeView = schemeView;
			}

			return cloneComponent;
		}

	}

}