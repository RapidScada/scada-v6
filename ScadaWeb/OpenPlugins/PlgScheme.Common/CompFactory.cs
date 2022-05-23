// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Model;
using System;

namespace Scada.Web.Plugins.PlgScheme
{
    /// <summary>
    /// The base class for creating scheme components
    /// <para>Базовый класс для создания компонентов схемы</para>
    /// </summary>
    public abstract class CompFactory
    {
        /// <summary>
        /// Определить, что имена типов равны
        /// </summary>
        protected static bool NameEquals(string expectedShortName, string expectedFullName, 
            string actualName, bool nameIsShort)
        {
            return string.Equals(nameIsShort ? expectedShortName : expectedFullName, 
                actualName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Создать компонент схемы
        /// </summary>
        public abstract ComponentBase CreateComponent(string typeName, bool nameIsShort);
    }
}
