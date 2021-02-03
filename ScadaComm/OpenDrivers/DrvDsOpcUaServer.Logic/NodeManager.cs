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
using Scada.Data.Models;
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
        /// Represents information associated with a variable.
        /// </summary>
        private class VarItem
        {
            public BaseDataVariableState Variable { get; set; }
            public DeviceTag DeviceTag { get; set; }
        }

        /// <summary>
        /// Represents a dictionary that stores device variables.
        /// </summary>
        private class DeviceVars : Dictionary<string, VarItem>
        {
        }


        /// <summary>
        /// The namespace for the nodes provided by the server.
        /// </summary>
        private const string NamespaceUri = "http://rapidscada.org/RapidScada/DrvDsOpcUaServer";
        /// <summary>
        /// The name of the root folder of the OPC server nodes.
        /// </summary>
        private const string RootFolderName = "Communicator";

        private readonly ICommContext commContext;   // the application context
        private readonly HashSet<int> deviceFilter;  // the device IDs to filter data
        private readonly ILog log;                   // the data source log
        private readonly object dataLock;            // syncronizes access to variable data

        private Dictionary<int, DeviceVars> devices; // the device variables accessed by device number


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public NodeManager(IServerInternal server, ApplicationConfiguration configuration,
            ICommContext commContext, OpcUaServerDSO options, ILog log)
            : base(server, configuration, NamespaceUri)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.commContext = commContext ?? throw new ArgumentNullException(nameof(commContext));
            deviceFilter = options.DeviceFilter.Count > 0 ? new HashSet<int>(options.DeviceFilter) : null;
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            dataLock = new object();

            devices = null;
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
        /// Creates a new variable according to the device tag.
        /// </summary>
        private BaseDataVariableState CreateVariable(NodeState parent, string pathPrefix, DeviceTag deviceTag)
        {
            NodeId opcDataType = DataTypeIds.Double;
            object defaultValue = 0.0;
            bool isArray = deviceTag.IsArray;

            switch (deviceTag.DataType)
            {
                case TagDataType.Double:
                    defaultValue = isArray ? (object)new double[deviceTag.DataLength] : 0.0;
                    break;

                case TagDataType.Int64:
                    opcDataType = DataTypeIds.Int64;
                    defaultValue = isArray ? (object)new long[deviceTag.DataLength] : (long)0;
                    break;

                case TagDataType.ASCII:
                case TagDataType.Unicode:
                    opcDataType = DataTypeIds.String;
                    defaultValue = "";
                    break;
            }

            BaseDataVariableState variable = CreateVariable(parent, 
                pathPrefix + deviceTag.Code, deviceTag.Code, deviceTag.Name, opcDataType, isArray);
            variable.Value = defaultValue;
            return variable;
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private BaseDataVariableState CreateVariable(NodeState parent, string path, string name, string displayName, 
            NodeId dataType, bool isArray)
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
                ValueRank = isArray ? ValueRanks.OneDimension : ValueRanks.Scalar,
                AccessLevel = AccessLevels.CurrentRead,
                UserAccessLevel = AccessLevels.CurrentRead,
                Historizing = false,
                StatusCode = StatusCodes.Bad,
                Timestamp = DateTime.UtcNow
            };

            if (isArray)
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0 });

            parent?.AddChild(variable);
            return variable;
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
            string pathPrefix = CommUtils.GetDeviceLogFileName(deviceLogic.DeviceNum, ".");
            DeviceVars deviceVars = new DeviceVars();
            devices[deviceLogic.DeviceNum] = deviceVars;

            foreach (DeviceTag deviceTag in tagGroup.DeviceTags)
            {
                if (!string.IsNullOrEmpty(deviceTag.Code))
                {
                    BaseDataVariableState variable = CreateVariable(folder, pathPrefix, deviceTag);
                    variable.OnSimpleWriteValue = OnSimpleWriteValue;

                    deviceVars[deviceTag.Code] = new VarItem
                    {
                        Variable = variable,
                        DeviceTag = deviceTag
                    };
                }
            }
        }

        /// <summary>
        /// Sets the variable value.
        /// </summary>
        private void SetVariable(VarItem varItem, CnlData[] cnlData, int dataIndex, DateTime timestamp)
        {
            object value = 0.0;

            BaseDataVariableState variable = varItem.Variable;
            variable.Value = value;
            variable.Timestamp = timestamp;
            variable.ClearChangeMasks(SystemContext, false);
        }

        /// <summary>
        /// Raised when the Value attribute is written.
        /// </summary>
        private ServiceResult OnSimpleWriteValue(ISystemContext context, NodeState node, ref object value)
        {
            log.WriteAction("SimpleWriteValue {0} = {1}", node.NodeId, value?.ToString() ?? "");
            return ServiceResult.Good;
        }


        /// <summary>
        /// Writes the slice of the current data.
        /// </summary>
        public void WriteCurrentData(DeviceSlice deviceSlice)
        {
            if (deviceSlice == null)
                throw new ArgumentNullException(nameof(deviceSlice));

            if (devices != null && devices.TryGetValue(deviceSlice.DeviceNum, out DeviceVars deviceVars))
            {
                lock (dataLock)
                {
                    int dataIndex = 0;

                    foreach (DeviceTag deviceTag in deviceSlice.DeviceTags)
                    {
                        if (deviceVars.TryGetValue(deviceTag.Code, out VarItem varItem))
                            SetVariable(varItem, deviceSlice.CnlData, dataIndex, deviceSlice.Timestamp);

                        dataIndex += deviceTag.DataLength;
                    }
                }
            }
        }

        /// <summary>
        /// Does any initialization required before the address space can be used.
        /// </summary>
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            try
            {
                Monitor.Enter(dataLock);

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
                devices = new Dictionary<int, DeviceVars>();
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
            finally
            {
                Monitor.Exit(dataLock);
            }
        }

        /// <summary>
        /// Frees any resources allocated for the address space.
        /// </summary>
        public override void DeleteAddressSpace()
        {
            lock (dataLock)
            {
                base.DeleteAddressSpace();
                devices = null;
            }
        }
    }
}
