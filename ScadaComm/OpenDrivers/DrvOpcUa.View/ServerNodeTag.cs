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
            DisplayName = rd.DisplayName.Text;
            OpcNodeId = ExpandedNodeId.ToNodeId(rd.NodeId, namespaceTable);
            NodeClass = rd.NodeClass;
            DataType = null;
            IsFilled = false;
        }


        public string DisplayName { get; private set; }

        public NodeId OpcNodeId { get; private set; }

        public NodeClass NodeClass { get; private set; }

        public Type DataType { get; private set; }

        public bool IsFilled { get; set; }
    }
}
