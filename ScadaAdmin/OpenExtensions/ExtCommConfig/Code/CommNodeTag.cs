// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Forms;
using System;

namespace Scada.Admin.Extensions.ExtCommConfig.Code
{
    /// <summary>
    /// Represents an object associated with a tree node.
    /// <para>Представляет объект, связанный с узлом дерева.</para>
    /// </summary>
    internal class CommNodeTag : TreeNodeTag
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommNodeTag(CommApp commApp, object relatedObject, string nodeType)
            : base(relatedObject, nodeType)
        {
            CommApp = commApp ?? throw new ArgumentNullException(nameof(commApp));
        }

        /// <summary>
        /// Gets the Communicator application in the project.
        /// </summary>
        public CommApp CommApp { get; }
    }
}
