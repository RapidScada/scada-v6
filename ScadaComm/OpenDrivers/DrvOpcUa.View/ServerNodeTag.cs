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
            if (rd == null)
                throw new ArgumentNullException(nameof(rd));
            if (namespaceTable == null)
                throw new ArgumentNullException(nameof(namespaceTable));

            DisplayName = rd.DisplayName.Text;
            NodeId = ExpandedNodeId.ToNodeId(rd.NodeId, namespaceTable);
            NodeClass = rd.NodeClass;
            DataType = null;
            IsFilled = false;
        }


        public string DisplayName { get; }

        public NodeId NodeId { get; }

        public NodeClass NodeClass { get; }

        public Type DataType { get; }

        public bool IsFilled { get; set; }
    }
}
