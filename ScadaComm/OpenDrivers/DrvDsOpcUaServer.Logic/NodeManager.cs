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
using Scada.Comm.Devices;
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
        /// <summary>
        /// The name of the root folder of the OPC server nodes.
        /// </summary>
        private const string RootFolderName = "Communicator";

        private readonly ICommContext commContext;  // the application context
        private readonly OpcUaServerDSO options;    // the data source options
        private readonly HashSet<int> deviceFilter; // the device IDs to filter data
        private readonly ILog log;                  // the data source log

        private List<BaseDataVariableState> variables;
        private readonly Random random = new Random();


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public NodeManager(IServerInternal server, ApplicationConfiguration configuration,
            ICommContext commContext, OpcUaServerDSO options, ILog log)
            : base(server, configuration, NamespaceUri)
        {
            this.commContext = commContext ?? throw new ArgumentNullException(nameof(commContext));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            deviceFilter = options.DeviceFilter.Count > 0 ? new HashSet<int>(options.DeviceFilter) : null;
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }


        /// <summary>
        /// Creates a new folder.
        /// </summary>
        private FolderState CreateFolder(NodeState parent, string name, string displayName)
        {
            string path = name;

            FolderState folder = new FolderState(parent)
            {
                SymbolicName = name,
                ReferenceTypeId = ReferenceTypes.Organizes,
                TypeDefinitionId = ObjectTypeIds.FolderType,
                NodeId = new NodeId(path, NamespaceIndex),
                BrowseName = new QualifiedName(path, NamespaceIndex),
                DisplayName = new LocalizedText(displayName),
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
        private BaseDataVariableState CreateVariable(NodeState parent, string path, string name, string displayName, 
            NodeId dataType, int valueRank)
        {
            BaseDataVariableState variable = new BaseDataVariableState(parent)
            {
                SymbolicName = name,
                ReferenceTypeId = ReferenceTypes.Organizes,
                TypeDefinitionId = VariableTypeIds.BaseDataVariableType,
                NodeId = new NodeId(path, NamespaceIndex),
                BrowseName = new QualifiedName(path, NamespaceIndex),
                DisplayName = new LocalizedText(displayName),
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

        /// <summary>
        /// Creates OPC server child nodes.
        /// </summary>
        private void CreateChildNodes(FolderState rootFolder)
        {
            foreach (ILineContext lineContext in commContext.GetCommLines())
            {
                List<DeviceLogic> devices = new List<DeviceLogic>();
                devices.AddRange(deviceFilter == null ? 
                    lineContext.SelectDevices() :
                    lineContext.SelectDevices(d => deviceFilter.Contains(d.DeviceNum)));

                if (devices.Count > 0)
                {
                    FolderState lineFolder = CreateFolder(rootFolder,
                        CommUtils.GetLineLogFileName(lineContext.CommLineNum, ""), lineContext.Title);

                    foreach (DeviceLogic deviceLogic in devices)
                    {
                        FolderState deviceFolder = CreateFolder(lineFolder,
                            CommUtils.GetDeviceLogFileName(deviceLogic.DeviceNum, ""), deviceLogic.Title);

                        if (deviceLogic.DeviceTags.FlattenGroups)
                        {
                            foreach (TagGroup tagGroup in deviceLogic.DeviceTags.TagGroups)
                            {
                                CreateDeviceTagNodes(deviceFolder, deviceLogic, tagGroup);
                            }
                        }
                        else
                        {
                            int groupNum = 1;

                            foreach (TagGroup tagGroup in deviceLogic.DeviceTags.TagGroups)
                            {
                                FolderState groupFolder = CreateFolder(deviceFolder, 
                                    "group" + groupNum.ToString("D3"), tagGroup.Name);
                                CreateDeviceTagNodes(groupFolder, deviceLogic, tagGroup);
                                groupNum++;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates OPC server nodes that correspond to device tags.
        /// </summary>
        private void CreateDeviceTagNodes(FolderState folder, DeviceLogic deviceLogic, TagGroup tagGroup)
        {
            variables = new List<BaseDataVariableState>();
            string pathPrefix = CommUtils.GetDeviceLogFileName(deviceLogic.DeviceNum, ".");

            foreach (DeviceTag deviceTag in tagGroup.DeviceTags)
            {
                if (!string.IsNullOrEmpty(deviceTag.Code))
                {
                    variables.Add(CreateVariable(folder, pathPrefix + deviceTag.Code, deviceTag.Code, deviceTag.Name,
                        DataTypeIds.Double, ValueRanks.Scalar));
                }
            }

            variables.ForEach(v => v.OnSimpleWriteValue = OnSimpleWriteValue);
        }

        /// <summary>
        /// Raised when the Value attribute is written.
        /// </summary>
        private ServiceResult OnSimpleWriteValue(ISystemContext context, NodeState node, ref object value)
        {
            log.WriteAction("SimpleWriteValue {0} = {1}", node.NodeId, value?.ToString() ?? "");
            return ServiceResult.Good;
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
        /// Writes the slice of the current data.
        /// </summary>
        public void WriteCurrentData(DeviceSlice deviceSlice)
        {
            if (deviceSlice == null)
                throw new ArgumentNullException(nameof(deviceSlice));

        }

        /// <summary>
        /// Does any initialization required before the address space can be used.
        /// </summary>
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            try
            {
                // create the root folder
                if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out IList<IReference> references))
                {
                    references = new List<IReference>();
                    externalReferences[ObjectIds.ObjectsFolder] = references;
                }

                FolderState rootFolder = CreateFolder(null, RootFolderName, RootFolderName);
                rootFolder.AddReference(ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
                references.Add(new NodeStateReference(ReferenceTypes.Organizes, false, rootFolder.NodeId));
                rootFolder.EventNotifier = EventNotifiers.SubscribeToEvents;
                AddRootNotifier(rootFolder);

                // create child nodes
                CreateChildNodes(rootFolder);

                // recursively index the node
                AddPredefinedNode(SystemContext, rootFolder);
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при создании адресного пространства" :
                    "Error creating address space");
                throw;
            }
        }

        /// <summary>
        /// Frees any resources allocated for the address space.
        /// </summary>
        public override void DeleteAddressSpace()
        {
            base.DeleteAddressSpace();
        }
    }
}
