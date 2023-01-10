// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvDbImport.Config;
using Scada.Comm.Lang;
using Scada.Data.Models;
using Scada.Lang;
using Scada.MultiDb;

namespace Scada.Comm.Drivers.DrvDbImport.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevDbImportLogic : DeviceLogic
    {
        /// <summary>
        /// Contains data common to a communication line.
        /// </summary>
        private class DbImportLineData
        {
            public bool FatalError { get; set; } = false;
            public DataSource DataSource { get; init; }
            public override string ToString() => CommPhrases.SharedObject;
        }

        /// <summary>
        /// Represents metadata about a device tag.
        /// </summary>
        private class DeviceTagMeta
        {
            public Type ActualDataType { get; set; }
        }

        private readonly DbDeviceConfig config;              // the device configuration
        private bool configError;                            // indicates that that device configuration is not loaded
        private DbImportLineData lineData;                   // data common to the communication line
        private Dictionary<string, CommandConfig> cmdByCode; // the commands accessed by code


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevDbImportLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = new DbDeviceConfig();
            configError = false;
            lineData = null;
            cmdByCode = null;

            ConnectionRequired = false;
        }


        /// <summary>
        /// Initializes data common to the communication line.
        /// </summary>
        private void InitLineData()
        {
            if (LineContext.SharedData.TryGetValueOfType(nameof(DbImportLineData), out DbImportLineData data))
            {
                lineData = data;
            }
            else
            {
                DbLineConfig lineConfig = new();
                bool lineConfigError = false;
                DataSource dataSource = null;

                if (!lineConfig.Load(Storage, DbLineConfig.GetFileName(LineContext.CommLineNum), out string errMsg))
                {
                    Log.WriteLine(errMsg);
                    Log.WriteLine(Locale.IsRussian ?
                        "Взаимодействие с базой данных невозможно, т.к. конфигурация линии не загружена" :
                        "Interaction with database is impossible because line configuration is not loaded");
                    lineConfigError = true;
                }
                else
                {
                    try
                    {
                        dataSource = DataSourceFactory.GetDataSource(lineConfig.ConnectionOptions);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLine(Locale.IsRussian ?
                            "Ошибка при создании источника данных: {0}" :
                            "Error creating data source: {0}", ex.Message);
                        lineConfigError = true;
                    }
                }

                lineData = new DbImportLineData()
                {
                    FatalError = lineConfigError,
                    DataSource = dataSource
                };

                LineContext.SharedData[nameof(DbImportLineData)] = lineData;
            }
        }

        /// <summary>
        /// Initializes a command map.
        /// </summary>
        private void InitCommandMap()
        {
            cmdByCode = new Dictionary<string, CommandConfig>();

            foreach (CommandConfig commandConfig in config.Commands)
            {
                if (!string.IsNullOrEmpty(commandConfig.CmdCode) && !cmdByCode.ContainsKey(commandConfig.CmdCode))
                    cmdByCode.Add(commandConfig.CmdCode, commandConfig);
            }
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            InitLineData();

            if (config.Load(Storage, DbDeviceConfig.GetFileName(DeviceNum), out string errMsg))
            {
                InitCommandMap();
                CanSendCommands = cmdByCode.Count > 0;
            }
            else
            {
                Log.WriteLine(CommPhrases.DeviceMessage, Title, errMsg);
                configError = true;
            }
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            if (configError)
                return;

            foreach (QueryConfig queryConfig in config.Queries)
            {
                TagGroup tagGroup = new(queryConfig.Name);

                foreach (string tagCode in queryConfig.TagCodes)
                {
                    DeviceTag deviceTag = tagGroup.AddTag(tagCode, tagCode);
                    deviceTag.Aux = new DeviceTagMeta();
                }

                DeviceTags.AddGroup(tagGroup);
            }
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            if (lineData.FatalError || configError)
            {
                DeviceStatus = DeviceStatus.Error;
            }
            else
            {

            }

            SleepPollingDelay();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {

        }
    }
}
