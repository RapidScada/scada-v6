using Scada.Web.Plugins.PlgScheme;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    public class PipeSizeConverter : TypeConverter
    {
        private static readonly char[] FieldSep = new char[] { ';', ',', ' ' };


        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new PipeSize((int)propertyValues["SolidWidth"], (int)propertyValues["FlowWidth"], (int)propertyValues["DashWidth"]);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(value, attributes);
            return props.Sort(new string[] { "SolidWidth","FlowWidth", "DashWidth" });
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            PipeSize size = (PipeSize)value;
            return destinationType == typeof(string) ?
                size.SolidWidth + "; " + size.FlowWidth + "; " + size.DashWidth :
                base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            try
            {
                if (value is string)
                {
                    string[] parts = ((string)value).Split(FieldSep, StringSplitOptions.RemoveEmptyEntries);
                    int backWidth = Convert.ToInt32(parts[0]);
                    int flowWidth = Convert.ToInt32(parts[1]);
                    int dashWidth = Convert.ToInt32(parts[2]);
                    return new PipeSize(backWidth, flowWidth, dashWidth);
                }
                else
                {
                    return PipeSize.Default;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ConvertError: " + ex.Message);
            }
        }
    }
}