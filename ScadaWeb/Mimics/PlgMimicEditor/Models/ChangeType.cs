// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimicEditor.Models
{
    /// <summary>
    /// Specifies the change types.
    /// <para>Задаёт типы изменений.</para>
    /// </summary>
    public static class ChangeType
    {
        // Dependencies
        public const string AddDependency = nameof(AddDependency);
        public const string RemoveDependency = nameof(RemoveDependency);

        // Document
        public const string UpdateDocument = nameof(UpdateDocument);

        // Components
        public const string AddComponent = nameof(AddComponent);
        public const string UpdateComponent = nameof(UpdateComponent);
        public const string UpdateParent = nameof(UpdateParent);
        public const string ArrangeComponent = nameof(ArrangeComponent);
        public const string RemoveComponent = nameof(RemoveComponent);

        // Images
        public const string AddImage = nameof(AddImage);
        public const string RemoveImage = nameof(RemoveImage);
    }
}
