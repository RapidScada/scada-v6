// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;
using System.Dynamic;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
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
        /// Gets or sets the properties of a mimic or component being updated.
        /// </summary>
        public ExpandoObject Properties { get; set; }

        /// <summary>
        /// Gets or sets the dependency being added or deleted.
        /// </summary>
        public FaceplateMeta Dependency { get; set; }


        /// <summary>
        /// Gets or sets the ID of the affected component.
        /// </summary>
        public int ComponentID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parent component.
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// Gets or sets the previous ID of the parent component.
        /// </summary>
        public int OldParentID { get; set; }


        /// <summary>
        /// Gets or sets the image being added.
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets the name of the image being renamed or deleted.
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Gets or sets the previous name of the image being renamed.
        /// </summary>
        public string OldImageName { get; set; }
    }
}
