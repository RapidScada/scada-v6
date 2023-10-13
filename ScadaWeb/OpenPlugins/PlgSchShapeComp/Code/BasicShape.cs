using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code
{
	[Serializable]
	public class BasicShape : ComponentBase, IDynamicComponent
	{
		public BasicShape()
		{
			ShapeType = "Circle";
			BackColor = "black";
			Action = Actions.None;
			Conditions = new List<AdvancedCondition>();
			InCnlNum = 0;
			CtrlCnlNum = 0;
			InCnlNumCustom = "NA (0)";
			CtrlCnlNumCustom = "NA (0)";
			Width = 100;
			Height = 130;
		}

		[DisplayName("Conditions"), Category(Categories.Behavior)]
		[Description("The conditions for SVG Shape output depending on the value of the input channel.")]
		public List<AdvancedCondition> Conditions { get; protected set; }


		[DisplayName("Shape Type"), Category(Categories.Appearance)]
		[Description("The type of SVG shape.")]
		public string ShapeType { get; set; }

		[DisplayName("Width"), Category(Categories.Appearance)]
		[Description("The Width of SVG shape.")]
		public int Width { get; set; }

		[DisplayName("Rotation"), Category(Categories.Appearance)]
		[Description("The rotation angle of the SVG shape in degrees.")]
		[DefaultValue(0)]
		public int Rotation { get; set; }

		[DisplayName("Height"), Category(Categories.Appearance)]
		[Description("The Height of SVG shape.")]
		[DefaultValue(130)]
		public int Height { get; set; }

		/// <summary>
		/// Get or set the input channel number
		/// </summary>
		[Browsable(false)]
		[DisplayName("Input channel"), Category(Categories.Data)]
		[Description("The input channel number associated with the component.")]
		[DefaultValue(0)]
		public int InCnlNum { get; set; }

		/// <summary>
		/// Get or set the input channel number 
		/// </summary>
		[DisplayName("Input channel"), Category(Categories.Data)]
		[Description("The input channel number associated with the component.")]
		public string InCnlNumCustom { get; set; }

		/// <summary>
		/// Get or set the control channel number
		/// </summary>
		[Browsable(false)]
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
			Rotation = xmlNode.GetChildAsInt("Rotation");
			InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
			CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
			Width = xmlNode.GetChildAsInt("Width");
			Height = xmlNode.GetChildAsInt("Height");
			InCnlNumCustom = xmlNode.GetChildAsString("InCnlNumCustom");
			CtrlCnlNumCustom = xmlNode.GetChildAsString("CtrlCnlNumCustom");
			XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");

			if (conditionsNode != null)
			{
				Conditions = new List<AdvancedCondition>();
				XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
				foreach (XmlNode conditionNode in conditionNodes)
				{
					AdvancedCondition condition = new AdvancedCondition { SchemeView = SchemeView };
					condition.LoadFromXml(conditionNode);
					Conditions.Add(condition);
				}
			}
			ShapeType = xmlNode.GetChildAsString("ShapeType");
		}

		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
			foreach (AdvancedCondition condition in Conditions)
			{
				XmlElement conditionElem = conditionsElem.AppendElem("Condition");
				condition.SaveToXml(conditionElem);
			}
			xmlElem.AppendElem("Rotation", Rotation);
			xmlElem.AppendElem("ShapeType", ShapeType);
			xmlElem.AppendElem("InCnlNum", InCnlNum);
			xmlElem.AppendElem("CtrlCnlNum", CtrlCnlNum);
			xmlElem.AppendElem("Width", Width);
			xmlElem.AppendElem("Height", Height);
			xmlElem.AppendElem("InCnlNumCustom", InCnlNumCustom);
			xmlElem.AppendElem("CtrlCnlNumCustom", CtrlCnlNumCustom);
			xmlElem.AppendElem("Action", Action.ToString());
		}


		public override ComponentBase Clone()
		{
			BasicShape clonedComponent = (BasicShape)base.Clone();

			foreach (Condition condition in clonedComponent.Conditions)
			{
				condition.SchemeView = SchemeView;
			}

			return clonedComponent;
		}
	}
}