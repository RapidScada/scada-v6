// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Scada.Web.Plugins.PlgSchBasicComp.Code
{
    /// <summary>
    /// Converter of PopupSize structures for PropertyGrid
    /// <para>Преобразователь структур типа PopupSize для PropertyGrid</para>
    /// </summary>
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
