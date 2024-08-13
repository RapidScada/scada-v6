// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents access options of a component.
    /// <para>Представляет настройки доступа компонента.</para>
    /// </summary>
    public class ComponentAccess
    {
        /// <summary>
        /// Gets or sets the object number.
        /// To see and use the component, a user must have access rights to the specified object.
        /// </summary>
        public int ObjNum { get; set; } = 0;
    }
}
