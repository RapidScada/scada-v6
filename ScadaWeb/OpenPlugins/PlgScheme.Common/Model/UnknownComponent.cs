// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;

namespace Scada.Web.Plugins.PlgScheme.Model
{
    /// <summary>
    /// Unknown scheme component
    /// <para>Неизвестный компонент схемы</para>
    /// </summary>
    /// <remarks>Stores the properties of the component that failed to load
    /// <para>Сохраняет свойства компонента, который не удалось загрузить</para></remarks>
    [Serializable]
    public sealed class UnknownComponent : ComponentBase
    {
        /// <summary>
        /// Получить или установить XML-узел, содержащий свойства компонента
        /// </summary>
        [Browsable(false), JsonIgnore]
        public XmlNode XmlNode { get; set; }

        /// <summary>
        /// Получить наименование XML-узла
        /// </summary>
        #region Attributes
        [DisplayName("XML node"), Category(Categories.Design)]
        [Description("The XML node that contains component properties in the scheme file.")]
        #endregion
        public string XmlNodeName
        {
            get
            {
                return XmlNode == null ? "" : XmlNode.Name;
            }
        }

        /// <summary>
        /// Вернуть строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return BuildDisplayName(XmlNodeName);
        }
    }
}
