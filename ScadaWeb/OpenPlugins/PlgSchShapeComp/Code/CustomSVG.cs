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
			Conditions = new List<AdvancedCondition>();

			InCnlNum = 0;
			CtrlCnlNum = 0;
			InCnlNumCustom = "NA (0)";
			CtrlCnlNumCustom = "NA (0)";
			BackColor = "black";
			ViewBoxHeight = 100;
			ViewBoxWidth = 100;
			ViewBoxX = 0;
			ViewBoxY = 0;
			Width = 100;
			Height = 100;
			SvgCode = "";
		}
	
		private string _svgCode;

		[DisplayName("SVG Code"), Category(Categories.Design)]
		[Description("SVG code .")]
		[DefaultValue("")]
		public string SvgCode
		{
			get => _svgCode;
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					InitializeFromSvgCode(value);
				}
				_svgCode = value;
			}
		}


		[DisplayName("Conditions"), Category(Categories.Behavior)]
		[Description("The conditions for polygon output depending on the value of the input channel.")]
		[DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
		public List<AdvancedCondition> Conditions { get; protected set; }


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

		[DisplayName("SVG Width"), Category(Categories.Appearance)]
		[Description("The width of the SVG image as specified in the SVG code.")]
		[DefaultValue(100)]
		public int Width { get; set; }


		[DisplayName("SVG Height"), Category(Categories.Appearance)]
		[Description("The height of the SVG image as specified in the SVG code.")]
		[DefaultValue(100)]
		public int Height { get; set; }


		[DisplayName("ViewBox X"), Category(Categories.Appearance)]
		[Description("The X coordinate of the SVG viewBox.")]
		[DefaultValue(0)]
		public int ViewBoxX { get; set; }

		[DisplayName("ViewBox Y"), Category(Categories.Appearance)]
		[Description("The Y coordinate of the SVG viewBox.")]
		[DefaultValue(0)]
		public int ViewBoxY { get; set; }

		[DisplayName("ViewBox Width"), Category(Categories.Appearance)]
		[Description("The width of the SVG viewBox.")]
		[DefaultValue(100)]
		public int ViewBoxWidth { get; set; }

		[DisplayName("ViewBox Height"), Category(Categories.Appearance)]
		[Description("The height of the SVG viewBox.")]
		[DefaultValue(100)]
		public int ViewBoxHeight { get; set; }


		public void InitializeFromSvgCode(string svgCode)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(svgCode);

			var svgElement = xmlDocument.DocumentElement;

			if (svgElement.Name != "svg")
			{
				throw new Exception("Invalid SVG code. The root element is not 'svg'.");
			}

			// Extract width and height attributes

			if (svgElement.Attributes["width"] != null && int.TryParse(svgElement.Attributes["width"].Value, out int width))
			{
				Width = width;
			}

			if (svgElement.Attributes["height"] != null && int.TryParse(svgElement.Attributes["height"].Value, out int height))
			{
				Height = height;
			}

			// Extract viewBox attribute
			var viewBoxAttribute = svgElement.Attributes["viewBox"];
			if (viewBoxAttribute != null)
			{
				var viewBoxValues = viewBoxAttribute.Value.Split(' ');

				try
				{
					if (viewBoxValues.Length == 4)
					{
						ViewBoxX = int.Parse(viewBoxValues[0]);
						ViewBoxY = int.Parse(viewBoxValues[1]);
						ViewBoxWidth = int.Parse(viewBoxValues[2]);
						ViewBoxHeight = int.Parse(viewBoxValues[3]);
					}
				}
				catch (FormatException ex)
				{
					Console.WriteLine("Une erreur s'est produite lors de la conversion des valeurs de viewBox en entiers. Vérifiez que les valeurs de viewBox sont bien des entiers. Exception :"+ ex);

					
				}
				catch (Exception ex)
				{
					Console.WriteLine("Une erreur inattendue s'est produite. Veuillez réessayer plus tard. Exception :" + ex);
				}

			}
			foreach (XmlNode childNode in svgElement.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Element)
				{
					var element = (XmlElement)childNode;

					// Extract fill, stroke, and stroke-width attributes
					var fillAttribute = element.Attributes["fill"];
					var strokeAttribute = element.Attributes["stroke"];
					var strokeWidthAttribute = element.Attributes["stroke-width"];

					if (fillAttribute != null)
					{
						BackColor = fillAttribute.Value;  // Set BackColor to fill
					}
					if (strokeAttribute != null) BorderColor = strokeAttribute.Value;
					if (strokeWidthAttribute != null) BorderWidth = int.Parse(strokeWidthAttribute.Value);
				}
			}

		}


		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);

			Action = xmlNode.GetChildAsEnum<Actions>("Action");

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
			InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
			CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
			InCnlNumCustom = xmlNode.GetChildAsString("InCnlNumCustom");
			CtrlCnlNumCustom = xmlNode.GetChildAsString("CtrlCnlNumCustom");
			Width = xmlNode.GetChildAsInt("Width");
			Height = xmlNode.GetChildAsInt("Height");
			ViewBoxX = xmlNode.GetChildAsInt("ViewBoxX");
			ViewBoxY = xmlNode.GetChildAsInt("ViewBoxY");
			ViewBoxWidth = xmlNode.GetChildAsInt("ViewBoxWidth");
			ViewBoxHeight = xmlNode.GetChildAsInt("ViewBoxHeight");
			SvgCode = xmlNode.GetChildAsString("SVGCode");

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

			xmlElem.AppendElem("InCnlNum", InCnlNum);
			xmlElem.AppendElem("CtrlCnlNum", CtrlCnlNum);
			xmlElem.AppendElem("InCnlNumCustom", InCnlNumCustom);
			xmlElem.AppendElem("CtrlCnlNumCustom", CtrlCnlNumCustom);
			xmlElem.AppendElem("ViewBoxX", ViewBoxX);
			xmlElem.AppendElem("ViewBoxY", ViewBoxY);
			xmlElem.AppendElem("ViewBoxWidth", ViewBoxWidth);
			xmlElem.AppendElem("Action", Action.ToString());
			xmlElem.AppendElem("ViewBoxHeight", ViewBoxHeight);
			xmlElem.AppendElem("Width", Width);
			xmlElem.AppendElem("Height", Height);
			xmlElem.AppendElem("SVGCode", SvgCode);

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