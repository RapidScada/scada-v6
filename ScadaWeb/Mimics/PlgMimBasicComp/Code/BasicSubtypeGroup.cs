// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.Components;

namespace Scada.Web.Plugins.PlgMimBasicComp.Code
{
    /// <summary>
    /// Represents a group of basic subtypes.
    /// <para>Представляет группу основных подтипов.</para>
    /// </summary>
    public class BasicSubtypeGroup : SubtypeGroup
    {
        public BasicSubtypeGroup()
        {
            DictionaryPrefix = PluginConst.ComponentModelPrefix;
            EnumNames.Add("BasicTogglePosition");
            StructNames.Add("BasicColorCondition");
        }
    }
}
