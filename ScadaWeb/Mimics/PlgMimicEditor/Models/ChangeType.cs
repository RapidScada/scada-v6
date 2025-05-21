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
        public const string AddDependency = "add-dependency";
        public const string RemoveDependency = "remove-dependency";

        // Document
        public const string UpdateDocument = "update-document";

        // Components
        public const string AddComponent = "add-component";
        public const string UpdateComponent = "update-component";
        public const string UpdateParent = "update-parent";
        public const string ArrangeComponent = "arrange-component";
        public const string RemoveComponent = "remove-component";

        // Images
        public const string AddImage = "add-image";
        public const string RemoveImage = "remove-image";
    }
}
