// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Specifies the change types.
    /// <para>Задаёт типы изменений.</para>
    /// </summary>
    public enum ChangeType
    {
        None,

        UpdateDocument,
        AddDependency,
        DeleteDependency,

        AddComponent,
        UpdateComponent,
        UpdateComponentParent,
        UpdateComponentBindings,
        UpdateComponentAccess,
        DeleteComponent,

        AddImage,
        RenameImage,
        DeleteImage
    }
}
