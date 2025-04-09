// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;
using Scada.Web.Plugins.PlgMimicEditor.Models;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Provides a mechanism for updating a mimic.
    /// <para>Представляет механизм для обновления мнемосхемы.</para>
    /// </summary>
    public class MimicUpdater
    {
        private readonly Mimic mimic; // the mimic to update


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MimicUpdater(Mimic mimic)
        {
            this.mimic = mimic ?? throw new ArgumentNullException(nameof(mimic));
        }
        
        
        /// <summary>
        /// Applies the change of the AddComponent type.
        /// </summary>
        private void ApplyAddComponent(Change change)
        {
            // validate component ID
            if (change.ObjectID <= 0 || mimic.ComponentMap.ContainsKey(change.ObjectID))
                return;

            // create component
            Component component = new()
            {
                ID = change.ObjectID,
                Name = change.GetString("name"),
                TypeName = change.GetString("typeName"),
                ParentID = change.GetInt32("parentID")
            };

            SetComponentProperties(component, change);

            // add component
            GetComponentParent(component).Components.Add(component);
            mimic.ComponentMap.Add(component.ID, component);
        }

        /// <summary>
        /// Applies the change of the UpdateComponent type.
        /// </summary>
        private void ApplyUpdateComponent(Change change)
        {
            if (mimic.ComponentMap.TryGetValue(change.ObjectID, out Component component))
                SetComponentProperties(component, change);
        }

        /// <summary>
        /// Applies the change of the RemoveComponent type.
        /// </summary>
        private void ApplyRemoveComponent(Change change)
        {
            if (mimic.ComponentMap.TryGetValue(change.ObjectID, out Component component))
            {
                HashSet<int> idsToRemove = [change.ObjectID];
                GetComponentParent(component).Components.Remove(component);

                if (component is Panel panel)
                {
                    foreach (Component childComponent in panel.GetAllChildren())
                    {
                        idsToRemove.Add(childComponent.ID);
                    }
                }

                mimic.ComponentMap.Remove(idsToRemove);
            }
        }

        /// <summary>
        /// Applies the change of the RemoveComponent type.
        /// </summary>
        private void ApplyRemoveComponents(Change change)
        {
            // find components to remove
            HashSet<int> idsToRemove = [];
            HashSet<int> parentIDs = [];

            foreach (int componentID in change.ObjectIDs)
            {
                if (mimic.ComponentMap.TryGetValue(componentID, out Component component))
                {
                    idsToRemove.Add(componentID);
                    parentIDs.Add(component.ParentID);

                    if (component is Panel panel)
                    {
                        foreach (Component childComponent in panel.GetAllChildren())
                        {
                            idsToRemove.Add(childComponent.ID);
                        }
                    }
                }
            }

            // remove components
            foreach (int parentID in parentIDs)
            {
                IContainer parent = parentID > 0 && mimic.ComponentMap.GetValueOrDefault(parentID) is Panel panel
                    ? panel
                    : mimic;
                parent.Components.RemoveAll(c => idsToRemove.Contains(c.ID)); // O(n) operation
            }

            mimic.ComponentMap.Remove(idsToRemove);
        }

        /// <summary>
        /// Gets the parent of the specified component.
        /// </summary>
        private IContainer GetComponentParent(Component component)
        {
            return component.ParentID > 0 && mimic.ComponentMap.GetValueOrDefault(component.ParentID) is Panel panel
                ? panel
                : mimic;
        }

        /// <summary>
        /// Sets the component properties according to the specified change.
        /// </summary>
        private static void SetComponentProperties(Component component, Change change)
        {
            if (change.Properties != null)
            {
                IDictionary<string, object> componentProps = component.Properties;

                foreach (KeyValuePair<string, object> kvp in change.Properties)
                {
                    string propName = kvp.Key.ToPascalCase();

                    if (component.KnownProperties.Contains(propName))
                    {
                        if (propName == "Name")
                            component.Name = kvp.Value == null ? "" : kvp.Value.ToString();
                    }
                    else
                    {
                        componentProps[propName] = kvp.Value;
                    }
                }
            }
        }


        /// <summary>
        /// Applies the changes to the mimic.
        /// </summary>
        public void ApplyChanges(IEnumerable<Change> changes)
        {
            ArgumentNullException.ThrowIfNull(changes, nameof(changes));

            foreach (Change change in changes)
            {
                switch (change.ChangeType)
                {
                    case ChangeType.AddComponent:
                        ApplyAddComponent(change);
                        break;

                    case ChangeType.UpdateComponent:
                        ApplyUpdateComponent(change);
                        break;

                    case ChangeType.RemoveComponent:
                        ApplyRemoveComponent(change);
                        break;

                    case ChangeType.RemoveComponents:
                        ApplyRemoveComponents(change);
                        break;
                }
            }
        }
    }
}
