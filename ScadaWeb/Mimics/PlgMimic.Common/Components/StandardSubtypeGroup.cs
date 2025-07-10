// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.Components
{
    /// <summary>
    /// Represents a group of standard subtypes.
    /// <para>Представляет группу стандартных подтипов.</para>
    /// </summary>
    public class StandardSubtypeGroup : SubtypeGroup
    {
        public StandardSubtypeGroup()
        {
            DictionaryPrefix = MimicConst.MimicModelPrefix;

            EnumNames.AddRange([
                "ActionType",
                "ComparisonOperator",
                "DataMember",
                "ImageSizeMode",
                "LogicalOperator",
                "LinkTarget",
                "ModalWidth",
                "ContentAlignment",
                "TextDirection"
            ]);

            StructNames.AddRange([
                "Action",
                "Border",
                "CommandArgs",
                "Condition",
                "CornerRadius",
                "Font",
                "ImageCondition",
                "LinkArgs",
                "Padding",
                "Point",
                "PropertyBinding",
                "PropertyExport",
                "Size",
                "VisualState"
            ]);
        }
    }
}
