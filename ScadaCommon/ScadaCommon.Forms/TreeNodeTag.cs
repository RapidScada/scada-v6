// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Forms
{
    /// <summary>
    /// Represents an object associated with a tree node.
    /// <para>Представляет объект, связанный с узлом дерева.</para>
    /// </summary>
    public class TreeNodeTag
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TreeNodeTag()
        {
            FormType = null;
            FormArgs = null;
            ExistingForm = null;
            RelatedObject = null;
            NodeType = "";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TreeNodeTag(object relatedObject, string nodeType)
            : base()
        {
            RelatedObject = relatedObject;
            NodeType = nodeType;
        }


        /// <summary>
        /// Gets or sets the type of form to create.
        /// </summary>
        public Type FormType { get; set; }

        /// <summary>
        /// Gets or sets the form creation arguments.
        /// </summary>
        public object[] FormArgs { get; set; }

        /// <summary>
        /// Gets or sets a form that already exists.
        /// </summary>
        public Form ExistingForm { get; set; }

        /// <summary>
        /// Gets or sets the object related to the node.
        /// </summary>
        public object RelatedObject { get; set; }

        /// <summary>
        /// Gets or sets the node type that determines possible actions with the node.
        /// </summary>
        public string NodeType { get; set; }
    }
}
