// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme;
using Scada.Web.Plugins.PlgScheme.Model;

namespace Scada.Web.Plugins.PlgSchBasicComp.Code
{
    /// <summary>
    /// Factory for creating basic scheme components.
    /// <para>Фабрика для создания основных компонентов схемы.</para>
    /// </summary>
    public class BasicCompFactory : CompFactory
    {
        /// <summary>
        /// Создать компонент схемы.
        /// </summary>
        public override ComponentBase CreateComponent(string typeName, bool nameIsShort)
        {
            if (NameEquals("Button", typeof(Button).FullName, typeName, nameIsShort))
                return new Button();
            else if (NameEquals("Led", typeof(Led).FullName, typeName, nameIsShort))
                return new Led();
            else if (NameEquals("Link", typeof(Link).FullName, typeName, nameIsShort))
                return new Link();
            else if (NameEquals("Toggle", typeof(Toggle).FullName, typeName, nameIsShort))
                return new Toggle();
            else
                return null;
        }
    }
}
