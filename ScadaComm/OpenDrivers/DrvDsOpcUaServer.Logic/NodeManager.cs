/*
 * Copyright 2021 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : DrvDsOpcUaServer
 * Summary  : Represents nodes of OPC UA server
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Opc.Ua;
using Opc.Ua.Server;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Scada.Comm.Drivers.DrvDsOpcUaServer.Logic
{
    /// <summary>
    /// Represents nodes of OPC UA server.
    /// <para>Представляет узлы сервера OPC UA.</para>
    /// </summary>
    internal class NodeManager : CustomNodeManager2
    {
        /// <summary>
        /// The namespace for the nodes provided by the server.
        /// </summary>
        private const string NamespaceUri = "http://rapidscada.org/RapidScada/DrvDsOpcUaServer";

        private readonly ICommContext commContext; // the application context
        private readonly OpcUaServerDSO options;   // the data source options
        private readonly ILog log; // the data source log

        private List<BaseDataVariableState> variables;
        private Random random = new Random();
        private Timer timer;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public NodeManager(IServerInternal server, ApplicationConfiguration configuration,
            ICommContext commContext, OpcUaServerDSO options, ILog log)
            : base(server, configuration, NamespaceUri)
        {
            this.commContext = commContext ?? throw new ArgumentNullException(nameof(commContext));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }


        /// <summary>
        /// Creates a new folder.
        /// </summary>
        private FolderState CreateFolder(NodeState parent, string path, string name)
        {
            FolderState folder = new FolderState(parent)
            {
                SymbolicName = name,
                ReferenceTypeId = ReferenceTypes.Organizes,
                TypeDefinitionId = ObjectTypeIds.FolderType,
                NodeId = new NodeId(path, NamespaceIndex),
                BrowseName = new QualifiedName(path, NamespaceIndex),
                DisplayName = new LocalizedText("en", name),
                WriteMask = AttributeWriteMask.None,
                UserWriteMask = AttributeWriteMask.None,
                EventNotifier = EventNotifiers.None
            };

            parent?.AddChild(folder);
            return folder;
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private BaseDataVariableState CreateVariable(NodeState parent, string path, string name, NodeId dataType, int valueRank)
        {
            BaseDataVariableState variable = new BaseDataVariableState(parent)
            {
                SymbolicName = name,
                ReferenceTypeId = ReferenceTypes.Organizes,
                TypeDefinitionId = VariableTypeIds.BaseDataVariableType,
                NodeId = new NodeId(path, NamespaceIndex),
                BrowseName = new QualifiedName(path, NamespaceIndex),
                DisplayName = new LocalizedText("en", name),
                WriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description,
                UserWriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description,
                DataType = dataType,
                ValueRank = valueRank,
                AccessLevel = AccessLevels.CurrentReadOrWrite,
                UserAccessLevel = AccessLevels.CurrentReadOrWrite,
                Historizing = false,
                StatusCode = StatusCodes.Good,
                Timestamp = DateTime.UtcNow
            };

            variable.Value = GetDefaultValue(variable);

            if (valueRank == ValueRanks.OneDimension)
            {
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0 });
            }
            else if (valueRank == ValueRanks.TwoDimensions)
            {
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0, 0 });
            }

            parent?.AddChild(variable);
            return variable;
        }

        /// <summary>
        /// Gets the default variable value.
        /// </summary>
        private object GetDefaultValue(BaseVariableState variable)
        {
            return 0.0;
        }

        private void DoSimulation(object state)
        {
            foreach (BaseDataVariableState variable in variables)
            {
                variable.Value = random.NextDouble() * 100;
                variable.Timestamp = DateTime.UtcNow;
                variable.ClearChangeMasks(SystemContext, false);
            }
        }

        /// <summary>
        /// Does any initialization required before the address space can be used.
        /// </summary>
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            // create the root folder
            if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out IList<IReference> references))
            {
                references = new List<IReference>();
                externalReferences[ObjectIds.ObjectsFolder] = references;
            }

            FolderState rootFolder = CreateFolder(null, "Communicator", "Communicator");
            rootFolder.AddReference(ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
            references.Add(new NodeStateReference(ReferenceTypes.Organizes, false, rootFolder.NodeId));
            rootFolder.EventNotifier = EventNotifiers.SubscribeToEvents;
            AddRootNotifier(rootFolder);

            // create subfolders and variables
            FolderState lineFolder = CreateFolder(rootFolder, "Line001", "Line001");
            FolderState deviceFolder = CreateFolder(lineFolder, "Device001", "Device001");

            variables = new List<BaseDataVariableState>();
            string pathPrefix = "Device001_";
            variables.Add(CreateVariable(deviceFolder, pathPrefix + "Var1", "Var1", DataTypeIds.Double, ValueRanks.Scalar));
            variables.Add(CreateVariable(deviceFolder, pathPrefix + "Var2", "Var2", DataTypeIds.Double, ValueRanks.Scalar));            

            foreach (BaseDataVariableState variable in variables)
            {
                /*variable.OnWriteValue = (ISystemContext context, NodeState node, NumericRange indexRange, QualifiedName dataEncoding, 
                    ref object value, ref StatusCode statusCode, ref DateTime timestamp) =>
                {
                    return ServiceResult.Good;
                };*/

                variable.OnSimpleWriteValue = (ISystemContext context, NodeState node, ref object value) =>
                {
                    log.WriteAction("SimpleWriteValue {0} = {1}", node.NodeId, value?.ToString() ?? "");
                    return ServiceResult.Good;
                };
            }

            // recursively index the node
            AddPredefinedNode(SystemContext, rootFolder);

            // simulate values
            timer = new Timer(DoSimulation, null, 1000, 1000);
        }

        public override void DeleteAddressSpace()
        {
            timer.Dispose();
            base.DeleteAddressSpace();
        }
    }
}
