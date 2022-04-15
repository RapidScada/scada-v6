// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Server;
using Scada.Comm.Devices;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;
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
            public int DeviceNum { get; set; }
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

        private Dictionary<int, DeviceVars> varByDevice; // variables accessed by device number
        private Dictionary<string, VarItem> varByPath;   // variables accessed by path


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

            varByDevice = null;
            varByPath = null;
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

            switch (deviceTag.DataType)
            {
                case TagDataType.Double:
                    defaultValue = deviceTag.IsNumericArray ? (object)new double[deviceTag.DataLength] : 0.0;
                    break;

                case TagDataType.Int64:
                    opcDataType = DataTypeIds.Int64;
                    defaultValue = deviceTag.IsNumericArray ? (object)new long[deviceTag.DataLength] : 0;
                    break;

                case TagDataType.ASCII:
                case TagDataType.Unicode:
                    opcDataType = DataTypeIds.String;
                    defaultValue = "";
                    break;
            }

            BaseDataVariableState variable = CreateVariable(parent, pathPrefix + deviceTag.Code, 
                deviceTag.Code, deviceTag.Name, opcDataType, deviceTag.IsNumericArray, defaultValue);
            return variable;
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private BaseDataVariableState CreateVariable(NodeState parent, string path, 
            string name, string displayName, NodeId dataType, bool isArray, object defaultValue)
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
                AccessLevel = AccessLevels.CurrentReadOrWrite,
                UserAccessLevel = AccessLevels.CurrentReadOrWrite,
                Historizing = false,
                Value = defaultValue,
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
                        string deviceFolderName = CommUtils.GetDeviceLogFileName(deviceLogic.DeviceNum, "");
                        FolderState deviceFolder = CreateFolder(lineFolder, deviceFolderName, deviceLogic.Title);
                        DeviceVars deviceVars = new DeviceVars();
                        varByDevice[deviceLogic.DeviceNum] = deviceVars;

                        if (deviceLogic.DeviceTags.FlattenGroups)
                        {
                            foreach (TagGroup tagGroup in deviceLogic.DeviceTags.TagGroups)
                            {
                                CreateDeviceTagNodes(deviceFolder, deviceLogic, tagGroup, deviceVars);
                            }
                        }
                        else
                        {
                            int groupNum = 1;

                            foreach (TagGroup tagGroup in deviceLogic.DeviceTags.TagGroups)
                            {
                                FolderState groupFolder = CreateFolder(deviceFolder,
                                    deviceFolderName + ".group" + groupNum.ToString("D3"), tagGroup.Name);
                                CreateDeviceTagNodes(groupFolder, deviceLogic, tagGroup, deviceVars);
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
        private void CreateDeviceTagNodes(FolderState folder, DeviceLogic deviceLogic, TagGroup tagGroup, 
            DeviceVars deviceVars)
        {
            string pathPrefix = CommUtils.GetDeviceLogFileName(deviceLogic.DeviceNum, ".");

            foreach (DeviceTag deviceTag in tagGroup.DeviceTags)
            {
                if (!string.IsNullOrEmpty(deviceTag.Code))
                {
                    BaseDataVariableState variable = CreateVariable(folder, pathPrefix, deviceTag);
                    variable.OnSimpleWriteValue = OnSimpleWriteValue;

                    VarItem varItem = new VarItem
                    {
                        Variable = variable,
                        DeviceTag = deviceTag,
                        DeviceNum = deviceLogic.DeviceNum
                    };

                    deviceVars[deviceTag.Code] = varItem;
                    varByPath[variable.NodeId.ToString()] = varItem;
                }
            }
        }

        /// <summary>
        /// Sets the variable value.
        /// </summary>
        private void SetVariable(VarItem varItem, CnlData[] cnlData, int dataIndex, DateTime timestamp)
        {
            try
            {
                DeviceTag deviceTag = varItem.DeviceTag;
                int dataLength = deviceTag.DataLength;
                object value = 0.0;

                switch (deviceTag.DataType)
                {
                    case TagDataType.Double:
                        value = deviceTag.IsNumericArray 
                            ? (object)CnlDataConverter.GetDoubleArray(cnlData, dataIndex, dataLength) 
                            : CnlDataConverter.GetDouble(cnlData, dataIndex);
                        break;

                    case TagDataType.Int64:
                        value = deviceTag.IsNumericArray 
                            ? (object)CnlDataConverter.GetInt64Array(cnlData, dataIndex, dataLength) 
                            : CnlDataConverter.GetInt64(cnlData, dataIndex);
                        break;

                    case TagDataType.ASCII:
                        value = CnlDataConverter.GetAscii(cnlData, dataIndex, dataLength);
                        break;

                    case TagDataType.Unicode:
                        value = CnlDataConverter.GetUnicode(cnlData, dataIndex, dataLength);
                        break;
                }

                BaseDataVariableState variable = varItem.Variable;
                variable.Value = value;
                variable.StatusCode = CnlDataConverter.GetStatus(cnlData, dataIndex, dataLength) > CnlStatusID.Undefined ?
                    StatusCodes.Good : StatusCodes.Bad;
                variable.Timestamp = timestamp;
                variable.ClearChangeMasks(SystemContext, false);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при установке значения переменной {0}" :
                    "Error setting the variable {0}", varItem.Variable.NodeId);
            }
        }

        /// <summary>
        /// Raised when the Value attribute is written.
        /// </summary>
        private ServiceResult OnSimpleWriteValue(ISystemContext context, NodeState node, ref object value)
        {
            string varPath = node.NodeId.ToString();

            try
            {
                log.WriteAction(Locale.IsRussian ?
                    "Запись переменной {0} = {1}" :
                    "Write variable {0} = {1}", varPath, value?.ToString() ?? "");

                if (varByPath.TryGetValue(varPath, out VarItem varItem))
                {
                    DeviceTag deviceTag = varItem.DeviceTag;
                    double cmdVal = 0.0;
                    byte[] cmdData = null;

                    switch (deviceTag.DataType)
                    {
                        case TagDataType.Double:
                            if (deviceTag.IsNumericArray)
                                cmdData = DoubleArrayToCmdData(value as Array, deviceTag.DataLength);
                            else
                                cmdVal = Convert.ToDouble(value);
                            break;

                        case TagDataType.Int64:
                            if (deviceTag.IsNumericArray)
                                cmdData = Int64ArrayToCmdData(value as Array, deviceTag.DataLength);
                            else
                                cmdVal = Convert.ToInt64(value);
                            break;

                        case TagDataType.ASCII:
                        case TagDataType.Unicode:
                            cmdData = TeleCommand.StringToCmdData(value?.ToString());
                            break;
                    }

                    commContext.SendCommand(new TeleCommand
                    {
                        CommandID = ScadaUtils.GenerateUniqueID(),
                        CreationTime = DateTime.UtcNow,
                        DeviceNum = varItem.DeviceNum,
                        CmdCode = varItem.DeviceTag.Code,
                        CmdVal = cmdVal,
                        CmdData = cmdData
                    }, DriverUtils.DriverCode);
                }

                return ServiceResult.Good;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при записи переменной {0}" :
                    "Error writing the variable {0}", varPath);
                return new ServiceResult(StatusCodes.Bad);
            }
        }

        /// <summary>
        /// Converts the specified array of floating point numbers to command data.
        /// </summary>
        private byte[] DoubleArrayToCmdData(Array array, int count)
        {
            if (array == null)
            {
                return null;
            }
            else
            {
                byte[] cmdData = new byte[count * 8];

                for (int i = 0, j = 0, len1 = count, len2 = array.Length; i < len1; i++, j += 8)
                {
                    double itemVal = i < len2 ? Convert.ToDouble(array.GetValue(i)) : 0.0;
                    BinaryConverter.CopyDouble(itemVal, cmdData, j);
                }

                return cmdData;
            }
        }

        /// <summary>
        /// Converts the specified array of integers to command data.
        /// </summary>
        private byte[] Int64ArrayToCmdData(Array array, int count)
        {
            if (array == null)
            {
                return null;
            }
            else
            {
                byte[] cmdData = new byte[count * 8];

                for (int i = 0, j = 0, len1 = count, len2 = array.Length; i < len1; i++, j += 8)
                {
                    long itemVal = i < len2 ? Convert.ToInt64(array.GetValue(i)) : 0;
                    BinaryConverter.CopyInt64(itemVal, cmdData, j);
                }

                return cmdData;
            }
        }


        /// <summary>
        /// Writes the slice of the current data.
        /// </summary>
        public void WriteCurrentData(DeviceSlice deviceSlice)
        {
            if (deviceSlice == null)
                throw new ArgumentNullException(nameof(deviceSlice));

            if (varByDevice != null && varByDevice.TryGetValue(deviceSlice.DeviceNum, out DeviceVars deviceVars))
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
                varByDevice = new Dictionary<int, DeviceVars>();
                varByPath = new Dictionary<string, VarItem>();
                CreateChildNodes(rootFolder);

                // recursively index the node
                AddPredefinedNode(SystemContext, rootFolder);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
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
                varByDevice = null;
                varByPath = null;
            }
        }
    }
}
