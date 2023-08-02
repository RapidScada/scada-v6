using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;


namespace Scada.Web.Plugins.PlgSchShapeComp.Code
{
	internal class PopupSizeConverter : TypeConverter
	{
		private readonly EnumConverter enumConverter = new(typeof(PopupWidth));

		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return new PopupSize((PopupWidth)propertyValues["Width"], (int)propertyValues["Height"]);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(value, attributes);
			return props.Sort(new string[] { "Width", "Height" });
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return destinationType == typeof(string) && value is PopupSize popupSize ?
				enumConverter.ConvertToString(popupSize.Width) + "; " + popupSize.Height :
				base.ConvertTo(context, culture, value, destinationType);
		}
	}
}