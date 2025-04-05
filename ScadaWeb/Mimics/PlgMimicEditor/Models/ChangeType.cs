// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimicEditor.Models
{
    /// <summary>
    /// Specifies the change types.
    /// <para>Задаёт типы изменений.</para>
    /// </summary>
    public enum ChangeType
    {
        None = 0,

        UpdateDocument = 1,
        AddDependency = 2,
        DeleteDependency = 3,

        AddComponent = 4,
        UpdateComponent = 5,
        UpdateComponentParent = 6,
        UpdateComponentBindings = 7,
        UpdateComponentAccess = 8,
        DeleteComponent = 9,

        AddImage = 10,
        RenameImage = 11,
        DeleteImage = 12
    }
}
