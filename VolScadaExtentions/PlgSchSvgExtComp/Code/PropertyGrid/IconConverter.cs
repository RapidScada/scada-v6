using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    public class IconConverter : TypeConverter
    {
        private static readonly char[] FieldSep = new char[] { ';' };


        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Icon icon = value as Icon;

                if (icon == null)
                {
                    return "";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(icon.Name);
                   //if (font.Bold)
                   //    sb.Append("; ").Append(SchemePhrases.BoldSymbol);
                   //if (font.Italic)
                   //    sb.Append("; ").Append(SchemePhrases.ItalicSymbol);
                   //if (font.Underline)
                   //    sb.Append("; ").Append(SchemePhrases.UnderlineSymbol);
                    return sb.ToString();
                }
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] parts = ((string)value).Split(FieldSep, StringSplitOptions.RemoveEmptyEntries);
                int partsLen = parts.Length;

                if (partsLen > 0)
                {
                    Icon icon = new Icon();
                    icon.Name = parts[0];

                    for (int i = 1; i < partsLen; i++)
                    {
                        string part = parts[i].Trim();
                        int size;

                       //if (part.Equals(SchemePhrases.BoldSymbol, StringComparison.OrdinalIgnoreCase))
                       //    icon.Bold = true;
                       //else if (part.Equals(SchemePhrases.ItalicSymbol, StringComparison.OrdinalIgnoreCase))
                       //    icon.Italic = true;
                       //else if (part.Equals(SchemePhrases.UnderlineSymbol, StringComparison.OrdinalIgnoreCase))
                       //    icon.Underline = true;
                       //else if (int.TryParse(part, out size))
                       //    icon.Size = size;
                    }

                    return icon;
                }
            }

            return null;
        }
    }
}