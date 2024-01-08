using System.Xml;
using System.ComponentModel;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Log;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using Scada.Web.Plugins.PlgSchShapeComp.Code.PropertyGrid;


namespace Scada.Web.Plugins.PlgSchShapeComp.Code
{
	public class DynamicPicture : StaticPicture, IDynamicComponent
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public DynamicPicture()
			: base()
		{
			BackColorOnHover = "";
			BorderColorOnHover = "";
			ImageOnHoverName = "";
			Action = Actions.None;
			Conditions = new List<PictureConditions>();
			InCnlNum = 0;
			CtrlCnlNum = 0;
		}

		[DisplayName("Rotation"), Category(Categories.Appearance)]
		[Description("The rotation angle of the SVG shape in degrees.")]
		[DefaultValue(0)]
		public int Rotation { get; set; }

		/// <summary>
		/// Получить или установить цвет фона при наведении указателя мыши
		/// </summary>
		#region Attributes
		[DisplayName("Back color on hover"), Category(Categories.Behavior)]
		[Description("The background color of the component when user rests the pointer on it.")]
		//[CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
		#endregion
		public string BackColorOnHover { get; set; }

		/// <summary>
		/// Получить или установить цвет рамки при наведении указателя мыши
		/// </summary>
		#region Attributes
		[DisplayName("Border color on hover"), Category(Categories.Behavior)]
		[Description("The border color of the component when user rests the pointer on it.")]
		//[CM.Editor(typeof(ColorEditor), typeof(UITypeEditor))]
		#endregion
		public string BorderColorOnHover { get; set; }

		/// <summary>
		/// Получить или установить наименование изображения, отображаемого при наведении указателя мыши
		/// </summary>
		#region Attributes
		[DisplayName("Image on hover"), Category(Categories.Behavior)]
		[Description("The image shown when user rests the pointer on the component.")]
		//[TypeConverter(typeof(ImageConverter)), Editor(typeof(ImageEditor), typeof(UITypeEditor))]
		[DefaultValue("")]
		#endregion
		public string ImageOnHoverName { get; set; }

		/// <summary>
		/// Получить или установить действие
		/// </summary>
		#region Attributes
		[DisplayName("Action"), Category(Categories.Behavior)]
		[Description("The action executed by clicking the left mouse button on the component.")]
		[DefaultValue(Actions.None)]
		#endregion
		public Actions Action { get; set; }

		/// <summary>
		/// Получить условия вывода изображений
		/// </summary>
		#region Attributes
		[DisplayName("Conditions"), Category(Categories.Behavior)]
		[Description("The conditions for image output depending on the value of the input channel.")]
		[DefaultValue(null), TypeConverter(typeof(CollectionConverter))]
		//[CM.Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		#endregion
		public List<PictureConditions> Conditions { get; protected set; }

		/// <summary>
		/// Получить или установить номер входного канала
		/// </summary>
		#region Attributes
		//[CM.Browsable(false)]
		[DisplayName("Input channel"), Category(Categories.Data)]
		[Description("The input channel number associated with the component.")]
		[DefaultValue(0)]
		#endregion
		public int InCnlNum { get; set; }



		/// <summary>
		/// Получить или установить номер канала управления
		/// </summary>
		#region Attributes
		//[CM.Browsable(false)]
		[DisplayName("Output channel"), Category(Categories.Data)]
		[Description("The output channel number associated with the component.")]
		[DefaultValue(0)]
		#endregion
		public int CtrlCnlNum { get; set; }


		/// <summary>
		/// Загрузить конфигурацию компонента из XML-узла
		/// </summary>
		public override void LoadFromXml(XmlNode xmlNode)
		{
			base.LoadFromXml(xmlNode);

			BackColorOnHover = xmlNode.GetChildAsString("BackColorOnHover");
			BorderColorOnHover = xmlNode.GetChildAsString("BorderColorOnHover");
			ImageOnHoverName = xmlNode.GetChildAsString("ImageOnHoverName");
			Action = xmlNode.GetChildAsEnum<Actions>("Action");
			Rotation = xmlNode.GetChildAsInt("Rotation");

			XmlNode conditionsNode = xmlNode.SelectSingleNode("Conditions");
			if (conditionsNode != null)
			{
				Conditions = new List<PictureConditions>();
				XmlNodeList conditionNodes = conditionsNode.SelectNodes("Condition");
				foreach (XmlNode conditionNode in conditionNodes)
				{
					PictureConditions condition = new PictureConditions { SchemeView = SchemeView };
					condition.LoadFromXml(conditionNode);
					Conditions.Add(condition);
				}
			}

			InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
			CtrlCnlNum = xmlNode.GetChildAsInt("CtrlCnlNum");
		}

		/// <summary>
		/// Сохранить конфигурацию компонента в XML-узле
		/// </summary>
		public override void SaveToXml(XmlElement xmlElem)
		{
			base.SaveToXml(xmlElem);
			xmlElem.AppendElem("Rotation", Rotation);

			xmlElem.AppendElem("BackColorOnHover", BackColorOnHover);
			xmlElem.AppendElem("BorderColorOnHover", BorderColorOnHover);
			xmlElem.AppendElem("ImageOnHoverName", ImageOnHoverName);
			xmlElem.AppendElem("Action", Action);

			XmlElement conditionsElem = xmlElem.AppendElem("Conditions");
			foreach (Condition condition in Conditions)
			{
				XmlElement conditionElem = conditionsElem.AppendElem("Condition");
				condition.SaveToXml(conditionElem);
			}

			xmlElem.AppendElem("InCnlNum", InCnlNum);
			xmlElem.AppendElem("CtrlCnlNum", CtrlCnlNum);
		}
	}
}
