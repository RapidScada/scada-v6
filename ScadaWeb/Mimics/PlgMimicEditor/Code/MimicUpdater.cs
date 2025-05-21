// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;
using Scada.Web.Plugins.PlgMimicEditor.Models;
using System.Dynamic;
using System.Text.Json;

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
            DependenciesChanged = false;
        }


        /// <summary>
        /// Gets a value indicating whether the mimic dependencies were changed during the last update.
        /// </summary>
        public bool DependenciesChanged { get; private set; }


        /// <summary>
        /// Applies the change of the AddDependency type.
        /// </summary>
        private void ApplyAddDependency(Change change)
        {
            if (string.IsNullOrEmpty(change.ObjectName))
                return;

            FaceplateMeta faceplateMeta = new()
            {
                TypeName = change.ObjectName,
                Path = change.GetString("path"),
                IsTransitive = false
            };

            // search in sorted list
            int index = mimic.Dependencies.BinarySearch(faceplateMeta);

            if (index >= 0)
                mimic.Dependencies[index] = faceplateMeta;
            else
                mimic.Dependencies.Insert(~index, faceplateMeta);

            mimic.DependencyMap[change.ObjectName] = faceplateMeta;
        }

        /// <summary>
        /// Applies the change of the RemoveDependency type.
        /// </summary>
        private void ApplyRemoveDependency(Change change)
        {
            if (!string.IsNullOrEmpty(change.ObjectName) &&
                mimic.DependencyMap.Remove(change.ObjectName, out FaceplateMeta faceplateMeta))
            {
                mimic.Dependencies.Remove(faceplateMeta);
            }
        }

        /// <summary>
        /// Applies the change of the UpdateDocument type.
        /// </summary>
        private void ApplyUpdateDocument(Change change)
        {
            if (change.Properties != null)
            {
                IDictionary<string, object> document = mimic.Document;

                foreach (KeyValuePair<string, object> kvp in change.Properties)
                {
                    string propName = kvp.Key.ToPascalCase();
                    JsonElement propVal = (JsonElement)kvp.Value;
                    document[propName] = JsonElementToObject(propVal);
                }
            }
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
            foreach (int componentID in change.GetObjectIDs())
            {
                if (mimic.ComponentMap.TryGetValue(componentID, out Component component))
                    SetComponentProperties(component, change);
            }
        }

        /// <summary>
        /// Applies the change of the UpdateParent type.
        /// </summary>
        private void ApplyUpdateParent(Change change)
        {

        }

        /// <summary>
        /// Applies the change of the ArrangeComponent type.
        /// </summary>
        private void ApplyArrangeComponent(Change change)
        {

        }

        /// <summary>
        /// Applies the change of the RemoveComponent type.
        /// </summary>
        private void ApplyRemoveComponent(Change change)
        {
            if (change.ObjectID > 0)
            {
                RemoveSingleComponent(change.ObjectID);
            }
            else if (change.ObjectIDs != null)
            {
                if (change.ObjectIDs.Length == 1)
                    RemoveSingleComponent(change.ObjectIDs[0]);
                else if (change.ObjectIDs.Length > 1)
                    RemoveMultipleComponents(change.ObjectIDs);
            }
        }

        /// <summary>
        /// Applies the change of the AddImage type.
        /// </summary>
        private void ApplyAddImage(Change change)
        {
            if (string.IsNullOrEmpty(change.ObjectName))
                return;

            Image image = new()
            {
                Name = change.ObjectName,
                MediaType = change.GetString("mediaType"),
                Data = Convert.FromBase64String(change.GetString("data"))
            };

            // search in sorted list
            int index = mimic.Images.BinarySearch(image);

            if (index >= 0)
                mimic.Images[index] = image;
            else
                mimic.Images.Insert(~index, image);

            mimic.ImageMap[change.ObjectName] = image;
        }

        /// <summary>
        /// Applies the change of the RemoveImage type.
        /// </summary>
        private void ApplyRemoveImage(Change change)
        {
            if (!string.IsNullOrEmpty(change.ObjectName) &&
                mimic.ImageMap.Remove(change.ObjectName, out Image image))
            {
                mimic.Images.Remove(image);
            }
        }

        /// <summary>
        /// Gets the parent of the specified component.
        /// </summary>
        private IContainer GetComponentParent(Component component)
        {
            return component.ParentID > 0 && mimic.ComponentMap.TryGetValue(component.ParentID, out Component parent)
                ? parent
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
                    JsonElement propVal = (JsonElement)kvp.Value;

                    if (Component.KnownProperties.Contains(propName))
                    {
                        if (propName == "Name")
                            component.Name = propVal.ToString();
                    }
                    else
                    {
                        componentProps[propName] = JsonElementToObject(propVal);
                    }
                }
            }
        }

        /// <summary>
        /// Converts the specified JSON element to a component property.
        /// </summary>
        private static object JsonElementToObject(JsonElement jsonElement)
        {
            if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                ExpandoObject expando = new();

                foreach (JsonProperty jsonProperty in jsonElement.EnumerateObject())
                {
                    string propName = jsonProperty.Name.ToPascalCase();
                    JsonElement propVal = jsonProperty.Value;
                    expando.SetValue(propName, JsonElementToObject(propVal));
                }

                return expando;
            }
            else
            {
                return jsonElement.ToString();
            }
        }

        /// <summary>
        /// Removes the specified component.
        /// </summary>
        private void RemoveSingleComponent(int componentID)
        {
            if (mimic.ComponentMap.TryGetValue(componentID, out Component component))
            {
                HashSet<int> idsToRemove = [componentID];
                GetComponentParent(component).Components.Remove(component);

                foreach (Component childComponent in component.GetAllChildren())
                {
                    idsToRemove.Add(childComponent.ID);
                }

                mimic.ComponentMap.Remove(idsToRemove);
            }
        }

        /// <summary>
        /// Removes the specified components.
        /// </summary>
        private void RemoveMultipleComponents(int[] componentIDs)
        {
            // find components to remove
            HashSet<int> idsToRemove = [];
            HashSet<int> parentIDs = [];

            foreach (int componentID in componentIDs)
            {
                if (mimic.ComponentMap.TryGetValue(componentID, out Component component))
                {
                    idsToRemove.Add(componentID);
                    parentIDs.Add(component.ParentID);

                    foreach (Component childComponent in component.GetAllChildren())
                    {
                        idsToRemove.Add(childComponent.ID);
                    }
                }
            }

            // remove components
            foreach (int parentID in parentIDs)
            {
                IContainer parent = parentID > 0 && mimic.ComponentMap.TryGetValue(parentID, out Component component)
                    ? component
                    : mimic;
                parent.Components.RemoveAll(c => idsToRemove.Contains(c.ID)); // O(n) operation
            }

            mimic.ComponentMap.Remove(idsToRemove);
        }


        /// <summary>
        /// Applies the changes to the mimic.
        /// </summary>
        public void ApplyChanges(IEnumerable<Change> changes)
        {
            ArgumentNullException.ThrowIfNull(changes, nameof(changes));
            DependenciesChanged = false;

            foreach (Change change in changes)
            {
                switch (change.ChangeType)
                {
                    case ChangeType.AddDependency:
                        ApplyAddDependency(change);
                        DependenciesChanged = true;
                        break;

                    case ChangeType.RemoveDependency:
                        ApplyRemoveDependency(change);
                        DependenciesChanged = true;
                        break;

                    case ChangeType.UpdateDocument:
                        ApplyUpdateDocument(change);
                        break;

                    case ChangeType.AddComponent:
                        ApplyAddComponent(change);
                        break;

                    case ChangeType.UpdateComponent:
                        ApplyUpdateComponent(change);
                        break;

                    case ChangeType.UpdateParent:
                        ApplyUpdateParent(change);
                        break;

                    case ChangeType.ArrangeComponent:
                        ApplyArrangeComponent(change);
                        break;

                    case ChangeType.RemoveComponent:
                        ApplyRemoveComponent(change);
                        break;

                    case ChangeType.AddImage:
                        ApplyAddImage(change);
                        break;

                    case ChangeType.RemoveImage:
                        ApplyRemoveImage(change);
                        break;
                }
            }
        }
    }
}
