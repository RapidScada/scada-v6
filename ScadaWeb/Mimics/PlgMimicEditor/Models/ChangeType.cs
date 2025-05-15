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

        // Dependencies
        AddDependency = 1,
        DeleteDependency = 2,

        // Document
        UpdateDocument = 3,

        // Components
        AddComponent = 4,
        UpdateComponent = 5,
        UpdateComponentParent = 6,
        UpdateComponentBindings = 7,
        UpdateComponentAccess = 8,
        RemoveComponent = 9,

        // Images
        AddImage = 10,
        RemoveImage = 11
    }
}
