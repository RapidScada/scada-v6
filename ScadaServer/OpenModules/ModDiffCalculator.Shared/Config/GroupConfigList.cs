// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;

namespace Scada.Server.Modules.ModDiffCalculator.Config
{
    /// <summary>
    /// Represents a list of group configurations.
    /// <para>Представляет список конфигураций групп.</para>
    /// </summary>
    [Serializable]
    internal class GroupConfigList : List<GroupConfig>, ITreeNode
    {
        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        ITreeNode ITreeNode.Parent
        {
            get => null;
            set => throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        IList ITreeNode.Children => this;
    }
}
