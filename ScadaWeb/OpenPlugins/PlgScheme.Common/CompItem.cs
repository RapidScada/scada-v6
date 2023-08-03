// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgScheme
{
    /// <summary>
    /// Item of the component list in the editor
    /// <para>Элемент списка компонентов в редакторе</para>
    /// </summary>
    public class CompItem
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CompItem(object icon, Type compType)
            : this(icon, GetDisplayName(compType), compType)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CompItem(object icon, string displayName, Type compType)
        {
            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));

            Icon = icon;
            DisplayName = displayName;
            CompType = compType ?? throw new ArgumentNullException(nameof(compType));
        }


        /// <summary>
        /// Получить иконку, отображаемую в редакторе
        /// </summary>
        public object Icon { get; }

        /// <summary>
        /// Получить наименование, отображаемое в редакторе
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Получить тип компонента
        /// </summary>
        public Type CompType { get; }


        /// <summary>
        /// Получить отображаемое наименование из словарей
        /// </summary>
        private static string GetDisplayName(Type compType)
        {
            LocaleDict dict = Locale.GetDictionary(compType.FullName);
            return dict.Phrases.ContainsKey("DisplayName") ? dict["DisplayName"] : compType.Name;
        }
    }
}
