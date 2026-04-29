// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Specifies the component properties that are loaded explicitly.
    /// <para>Задаёт свойства компонента, которые загружаются явно.</para>
    /// </summary>
    public static class KnownProperty
    {
        public const string ID = nameof(ID);
        public const string TypeName = nameof(TypeName);
        public const string ParentID = nameof(ParentID);
        public const string Components = nameof(Components);
        public static readonly HashSet<string> All = [ID, TypeName, ParentID, Components];
    }
}
