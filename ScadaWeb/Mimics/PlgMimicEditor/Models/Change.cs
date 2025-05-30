// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;
using System.Dynamic;
using System.Text.Json;

namespace Scada.Web.Plugins.PlgMimicEditor.Models
{
    /// <summary>
    /// Represents a change in a mimic.
    /// <para>Представляет изменение мнемосхемы.</para>
    /// </summary>
    public class Change
    {
        /// <summary>
        /// Gets or sets the change type.
        /// </summary>
        public string ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the ID of the affected object.
        /// </summary>
        public int ObjectID { get; set; }

        /// <summary>
        /// Gets or sets the IDs of the affected objects.
        /// </summary>
        public int[] ObjectIDs { get; set; }

        /// <summary>
        /// Gets or sets the name of the affected object.
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Gets or sets the properties of the object being added or updated.
        /// </summary>
        public ExpandoObject Properties { get; set; }

        /// <summary>
        /// Gets or sets the new parent ID of the components.
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// Gets or sets the number of steps by which the components are shifted.
        /// </summary>
        public int Shift { get; set; }

        /// <summary>
        /// Gets or sets the ID of the sibling component to arrange the components relative to it.
        /// </summary>
        public int SiblingID { get; set; }


        /// <summary>
        /// Gets the IDs of the affected objects as an array.
        /// </summary>
        public int[] GetObjectIDs()
        {
            return ObjectID > 0
                ? [ObjectID]
                : (ObjectIDs ?? []);
        }

        /// <summary>
        /// Gets the property value as a string.
        /// </summary>
        public string GetString(string propertyName)
        {
            return Properties?.GetValue(propertyName)?.ToString();
        }

        /// <summary>
        /// Gets the property value as an integer.
        /// </summary>
        public int GetInt32(string propertyName)
        {
            return Properties == null ? 0 : Properties.GetValue<JsonElement>(propertyName).GetInt32();
        }
    }
}
