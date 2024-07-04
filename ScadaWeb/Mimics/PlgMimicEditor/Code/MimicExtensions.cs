// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Provides extensions for a mimic diagram.
    /// <para>Предоставляет расширения для мнемосхемы.</para>
    /// </summary>
    internal static class MimicExtensions
    {
        /// <summary>
        /// Applies the change of the UpdateComponent type.
        /// </summary>
        private static void ApplyUpdateComponent(Mimic mimic, Change change)
        {
            if (mimic.ComponentMap.TryGetValue(change.ComponentID, out Component component))
            {
                IDictionary<string, object> componentProps = component.Properties;

                foreach (KeyValuePair<string, object> kvp in change.Properties)
                {
                    if (!component.KnownNodes.Contains(kvp.Key))
                        componentProps[kvp.Key] = kvp.Value;
                }
            }
        }

        /// <summary>
        /// Applies the changes to the mimic.
        /// </summary>
        public static void ApplyChanges(this Mimic mimic, IEnumerable<Change> changes)
        {
            ArgumentNullException.ThrowIfNull(mimic, nameof(mimic));
            ArgumentNullException.ThrowIfNull(changes, nameof(changes));

            foreach (Change change in changes)
            {
                switch (change.ChangeType)
                {
                    case ChangeType.UpdateComponent:
                        ApplyUpdateComponent(mimic, change);
                        break;
                }
            }
        }
    }
}
