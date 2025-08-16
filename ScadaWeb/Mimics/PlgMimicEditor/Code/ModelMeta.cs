// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.Components;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Contains information associated with the mimic model.
    /// <para>Содержит информацию, связанную с моделью мнемосхемы.</para>
    /// </summary>
    public class ModelMeta
    {
        /// <summary>
        /// Gets the groups of components.
        /// </summary>
        public List<ComponentGroup> ComponentGroups { get; } = [];

        /// <summary>
        /// Gets the groups of subtypes.
        /// </summary>
        public List<SubtypeGroup> SubtypeGroups { get; } = [];


        /// <summary>
        /// Clears the information.
        /// </summary>
        public void Clear()
        {
            ComponentGroups.Clear();
            SubtypeGroups.Clear();
        }
    }
}
