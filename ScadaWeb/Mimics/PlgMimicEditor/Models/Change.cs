﻿// Copyright (c) Rapid Software LLC. All rights reserved.
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
        /// Initializes a new instance of the class.
        /// </summary>
        public Change()
        {
            ChangeType = ChangeType.None;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Change(ChangeType changeType)
        {
            ChangeType = changeType;
        }


        /// <summary>
        /// Gets or sets the change type.
        /// </summary>
        public ChangeType ChangeType { get; set; }

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
