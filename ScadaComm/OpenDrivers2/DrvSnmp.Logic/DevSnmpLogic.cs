// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvSnmp.Config;
using Scada.Comm.Lang;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;
using System.Globalization;
using System.Net;
using System.Text;

namespace Scada.Comm.Drivers.DrvSnmp.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    /// <remarks>
    /// https://github.com/lextudio/sharpsnmplib
    /// https://docs.sharpsnmp.com/
    /// </remarks>
    internal class DevSnmpLogic : DeviceLogic
    {
        /// <summary>
        /// Represents a variable group.
        /// </summary>
        private class VarGroup
        {
            public VarGroupConfig Config { get; init; }
            public List<Variable> Variables { get; } = new();
            public int StartTagIndex { get; init; }
        }

        /// <summary>
        /// The default SNMP port.
        /// </summary>
        private const int DefaultPort = 161;

        private readonly SnmpDeviceConfig config;  // the device configuration
        private readonly List<VarGroup> varGroups; // the active variable groups

        private bool fatalError;                   // normal operation is impossible
        private IPEndPoint endPoint;               // the IP address and port of the device
        private OctetString readCommunity;         // the password for reading data
        private OctetString writeCommunity;        // the password for writing data
        private VersionCode snmpVersion;           // the protocol version


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevSnmpLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            CanSendCommands = true;
            ConnectionRequired = false;

            config = new SnmpDeviceConfig();
            varGroups = new List<VarGroup>();

            fatalError = false;
            endPoint = null;
            readCommunity = null;
            writeCommunity = null;
            snmpVersion = VersionCode.V2;
        }


        /// <summary>
        /// Retrieves an endpoint from the device address.
        /// </summary>
        private void RetrieveEndPoint()
        {
            try
            {
                ScadaUtils.RetrieveHostAndPort(DeviceConfig.StrAddress, DefaultPort, out string host, out int port);
                endPoint = new IPEndPoint(IPAddress.Parse(host), port);
            }
            catch (Exception ex)
            {
                fatalError = true;
                endPoint = null;
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка при извлечении конечной точки для {0}: {1}" :
                    "Error retrieving endpoint for {0}: {1}", Title, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a protocol version from the device configuration.
        /// </summary>
        private void RetrieveSnmpVersion()
        {
            snmpVersion = config.DeviceOptions.SnmpVersion switch
            {
                1 => VersionCode.V1,
                3 => VersionCode.V3,
                _ => VersionCode.V2
            };

            if (snmpVersion == VersionCode.V3)
            {
                fatalError = true;
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка в конфигурации {0}: SNMP v3 не поддерживается" :
                    "Error in {0} configuration: SNMP v3 is not supported", Title);
            }
        }

        /// <summary>
        /// Requests the group of variables.
        /// </summary>
        private bool RequestVarGroup(VarGroup varGroup)
        {
            try
            {
                VarGroupConfig varGroupConfig = varGroup.Config;
                int varCnt = varGroupConfig.Variables.Count;

                Log.WriteLine(Locale.IsRussian ?
                    "Запрос группы переменных \"{0}\"" :
                    "Request variable group \"{0}\"", varGroupConfig.Name);

                IList<Variable> receivedVars = 
                    Messenger.Get(snmpVersion, endPoint, readCommunity, varGroup.Variables, PollingOptions.Timeout);

                if (receivedVars == null || receivedVars.Count != varCnt)
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Несоответствие количества запрошенных и принятых переменных." :
                        "Mismatch between the number of requested and received variables.");
                }

                for (int i = 0; i < varCnt; i++)
                {
                    Variable receivedVar = receivedVars[i];

                    if (receivedVar.Id != varGroup.Variables[i].Id)
                    {
                        throw new ScadaException(Locale.IsRussian ?
                            "Несоответствие запрошенной и принятой переменной." :
                            "Mismatch between requested and received variable.");
                    }

                    VariableConfig variableConfig = varGroupConfig.Variables[i];
                    Log.WriteLine("{0} {1} = {2}", CommPhrases.ReceiveNotation, variableConfig.Name, 
                        SnmpDataToString(receivedVar.Data, variableConfig));
                    SetTagData(varGroup.StartTagIndex + i, receivedVar.Data, variableConfig);
                }

                Log.WriteLine(CommPhrases.ResponseOK);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Sets value and status of the specified tag.
        /// </summary>
        private void SetTagData(int tagIndex, ISnmpData snmpData, VariableConfig variableConfig)
        {
            try
            {
                if (snmpData == null)
                {
                    DeviceData.Invalidate(tagIndex);
                }
                else
                {
                    switch (variableConfig.DataType)
                    {
                        case TagDataType.Double:
                            if (GetDouble(snmpData, out double val1))
                                DeviceData.Set(tagIndex, val1);
                            else
                                DeviceData.Invalidate(tagIndex);
                            break;

                        case TagDataType.Int64:
                            if (snmpData is OctetString octetString) // BITS string
                                DeviceData.SetByteArray(tagIndex, octetString.GetRaw(), CnlStatusID.Defined);
                            else if (GetInt64(snmpData, out long val2))
                                DeviceData.SetInt64(tagIndex, val2, CnlStatusID.Defined);
                            else
                                DeviceData.Invalidate(tagIndex);
                            break;

                        case TagDataType.ASCII:
                            DeviceData.SetAscii(tagIndex, snmpData.ToString(), CnlStatusID.Defined);
                            break;

                        case TagDataType.Unicode:
                            DeviceData.SetUnicode(tagIndex, snmpData.ToString(), CnlStatusID.Defined);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteInfo(ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при установке данных тега" :
                    "Error setting tag data"));
            }
        }

        /// <summary>
        /// Finds a variable to set by the specified command.
        /// </summary>
        private bool FindVariable(TeleCommand cmd, out ObjectIdentifier oid)
        {
            oid = null;
            return false;
        }

        /// <summary>
        /// Creates an SNMP variable according to the configuration.
        /// </summary>
        private static Variable CreateVariable(VariableConfig variableConfig)
        {
            try
            {
                return new Variable(new ObjectIdentifier(variableConfig.OID));
            }
            catch (Exception ex)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Ошибка при создании переменной \"{0}\" с идентификатором {1}: {2}" :
                    "Error creating variable \"{0}\" with identifier {1}: {2}",
                    variableConfig.Name, variableConfig.OID, ex.Message);
            }
        }

        /// <summary>
        /// Converts the variable data to a string representation.
        /// </summary>
        private static string SnmpDataToString(ISnmpData snmpData, VariableConfig variableConfig)
        {
            if (snmpData == null)
            {
                return "null";
            }
            else
            {
                StringBuilder sb = new();

                if (snmpData is OctetString octetString && variableConfig.DataType == TagDataType.Int64)
                    sb.Append(octetString.ToHexString());
                else
                    sb.Append(snmpData.ToString());

                sb.Append(" (").Append(snmpData.TypeCode.ToString()).Append(')');
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the variable data as a double.
        /// </summary>
        private static bool GetDouble(ISnmpData snmpData, out double val)
        {
            switch (snmpData.TypeCode)
            {
                case SnmpType.Integer32:
                    val = ((Integer32)snmpData).ToInt32();
                    return true;

                case SnmpType.Counter32:
                    val = ((Counter32)snmpData).ToUInt32();
                    return true;

                case SnmpType.Counter64:
                    val = ((Counter64)snmpData).ToUInt64();
                    return true;

                case SnmpType.Gauge32:
                    val = ((Gauge32)snmpData).ToUInt32();
                    return true;

                case SnmpType.TimeTicks:
                    val = ((TimeTicks)snmpData).ToUInt32();
                    return true;

                case SnmpType.OctetString:
                    string s = snmpData.ToString().Trim().ToLower();

                    if (s == "true")
                    {
                        val = 1.0;
                        return true;
                    }
                    else if (s == "false")
                    {
                        val = 0.0;
                        return true;
                    }
                    else if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
                    {
                        return true;
                    }
                    else
                    {
                        val = 0.0;
                        return false;
                    }

                default:
                    val = 0.0;
                    return false;
            }
        }

        /// <summary>
        /// Gets the variable data as an integer.
        /// </summary>
        private static bool GetInt64(ISnmpData snmpData, out long val)
        {
            switch (snmpData.TypeCode)
            {
                case SnmpType.Integer32:
                    val = ((Integer32)snmpData).ToInt32();
                    return true;

                case SnmpType.Counter32:
                    val = ((Counter32)snmpData).ToUInt32();
                    return true;

                case SnmpType.Counter64:
                    val = (long)((Counter64)snmpData).ToUInt64();
                    return true;

                case SnmpType.Gauge32:
                    val = ((Gauge32)snmpData).ToUInt32();
                    return true;

                case SnmpType.TimeTicks:
                    val = ((TimeTicks)snmpData).ToUInt32();
                    return true;

                default:
                    val = 0;
                    return false;
            }
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            RetrieveEndPoint();

            if (config.Load(Storage, SnmpDeviceConfig.GetFileName(DeviceNum), out string errMsg))
            {
                readCommunity = new OctetString(config.DeviceOptions.ReadCommunity);
                writeCommunity = new OctetString(config.DeviceOptions.WriteCommunity);
                RetrieveSnmpVersion();
            }
            else
            {
                Log.WriteLine(CommPhrases.DeviceMessage, Title, errMsg);
                fatalError = true;
            }
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            if (fatalError)
                return;

            foreach (VarGroupConfig varGroupConfig in config.VarGroups)
            {
                if (varGroupConfig.Variables.Count == 0)
                    continue;

                // create tag group
                TagGroup tagGroup = new(varGroupConfig.Name);
                int startTagIdx = DeviceTags.Count;

                foreach (VariableConfig variableConfig in varGroupConfig.Variables)
                {
                    DeviceTag deviceTag = tagGroup.AddTag(variableConfig.TagCode, variableConfig.Name);
                    deviceTag.DataType = variableConfig.DataType;
                    deviceTag.DataLen = DeviceTag.CalcDataLength(variableConfig.DataLen, variableConfig.DataType);
                    deviceTag.Format = TagFormat.GetDefault(variableConfig.DataType);
                }

                DeviceTags.AddGroup(tagGroup);

                // create variable group
                if (varGroupConfig.Active)
                {
                    VarGroup varGroup = new()
                    {
                        Config = varGroupConfig,
                        StartTagIndex = startTagIdx
                    };

                    foreach (VariableConfig variableConfig in varGroupConfig.Variables)
                    {
                        varGroup.Variables.Add(CreateVariable(variableConfig));
                    }

                    varGroups.Add(varGroup);
                }
            }
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();

            if (fatalError)
            {
                Log.WriteLine(CommPhrases.UnablePollDevice);
                SleepPollingDelay();
                LastRequestOK = false;
            }
            else if (varGroups.Count == 0)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Отсутствуют активные группы переменных" :
                    "No active variable groups");
                SleepPollingDelay();
            }
            else
            {
                foreach (VarGroup varGroup in varGroups)
                {
                    if (LastRequestOK)
                    {
                        LastRequestOK = false;
                        int tryNum = 0;

                        while (RequestNeeded(ref tryNum))
                        {
                            if (RequestVarGroup(varGroup))
                                LastRequestOK = true;

                            FinishRequest();
                            tryNum++;
                        }
                    }

                    if (IsTerminated)
                        break;

                    if (!LastRequestOK)
                        DeviceData.Invalidate(varGroup.StartTagIndex, varGroup.Variables.Count);
                }
            }

            FinishSession();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {
            base.SendCommand(cmd);
            LastRequestOK = false;

            if (fatalError)
            {
                Log.WriteLine(CommPhrases.UnablePollDevice);
            }
            else if (!FindVariable(cmd, out ObjectIdentifier oid))
            {
                Log.WriteLine(CommPhrases.InvalidCommand);
            }
            else
            {

            }

            FinishCommand();
        }
    }
}
