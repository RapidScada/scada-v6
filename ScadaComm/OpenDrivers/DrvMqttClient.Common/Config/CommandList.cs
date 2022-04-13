// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;

namespace Scada.Comm.Drivers.DrvMqttClient.Config
{
    /// <summary>
    /// Represents a list of commands.
    /// <para>Представляет список команд.</para>
    /// </summary>
    [Serializable]
    public class CommandList : List<CommandConfig>, ITreeNode
    {
        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        ITreeNode ITreeNode.Parent
        {
            get
            {
                return null;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        IList ITreeNode.Children
        {
            get
            {
                return this;
            }
        }
    }
}
