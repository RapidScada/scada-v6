// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;

namespace Scada.Comm.Drivers.DrvOpcUa.View
{
    /// <summary>
    /// Represents an object associated with a node of the server tree.
    /// <para>Представляет объект, связанный с узлом дерева сервера.</para>
    /// </summary>
    internal class ServerNodeTag
    {
        public ServerNodeTag(ReferenceDescription rd, NamespaceTable namespaceTable)
        {
            ArgumentNullException.ThrowIfNull(rd, nameof(rd));
            ArgumentNullException.ThrowIfNull(namespaceTable, nameof(namespaceTable));

            DisplayName = rd.DisplayName.Text;
            NodeId = ExpandedNodeId.ToNodeId(rd.NodeId, namespaceTable);
            NodeClass = rd.NodeClass;
            IsFilled = false;
        }


        public string DisplayName { get; }

        public NodeId NodeId { get; }

        public string NodeIdStr => NodeId == null ? "" : NodeId.ToString();

        public NodeClass NodeClass { get; }

        public bool IsFilled { get; set; }


        public bool ClassIs(NodeClass nodeClass) => NodeClass == nodeClass;

        public bool ClassIs(params NodeClass[] nodeClasses) => nodeClasses.Any(x => x == NodeClass);
    }
}
