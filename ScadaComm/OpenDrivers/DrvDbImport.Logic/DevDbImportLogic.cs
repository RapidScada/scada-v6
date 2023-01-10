// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvDbImport.Config;
using Scada.Comm.Lang;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.MultiDb;
using System.Data;
using System.Data.Common;

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

        private readonly DbDeviceConfig config;                       // the device configuration
        private readonly Dictionary<string, CommandConfig> cmdByCode; // the commands accessed by code

        private bool configError;          // indicates that that device configuration is not loaded
        private DbImportLineData lineData; // data common to the communication line


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevDbImportLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            config = new DbDeviceConfig();
            cmdByCode = new Dictionary<string, CommandConfig>();

            configError = false;
            lineData = null;

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
        /// Fills the command map.
        /// </summary>
        private void FillCommandMap()
        {
            foreach (CommandConfig commandConfig in config.Commands)
            {
                if (!string.IsNullOrEmpty(commandConfig.CmdCode) && !cmdByCode.ContainsKey(commandConfig.CmdCode))
                    cmdByCode.Add(commandConfig.CmdCode, commandConfig);
            }
        }

        /// <summary>
        /// Connects to the database.
        /// </summary>
        private bool Connect()
        {
            try
            {
                lineData.DataSource.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка при соединении с БД: {0}" :
                    "Error connecting to DB: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Disconnects from the database.
        /// </summary>
        private void Disconnect()
        {
            try
            {
                lineData.DataSource.Disconnect();
            }
            catch (Exception ex)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка при разъединении с БД: {0}" :
                    "Error disconnecting from DB: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Creates a database command according to the command configuration.
        /// </summary>
        private DbCommand CreateCommand(TeleCommand teleCommand, CommandConfig commandConfig)
        {
            DataSource dataSource = lineData.DataSource;
            DbCommand dbCommand = dataSource.CreateCommand();
            dbCommand.CommandText = commandConfig.Sql;
            dataSource.SetParam(dbCommand, "cmdVal",
                double.IsNaN(teleCommand.CmdVal) ? DBNull.Value : teleCommand.CmdVal);
            dataSource.SetParam(dbCommand, "cmdData",
                teleCommand.CmdData == null ? DBNull.Value : teleCommand.CmdData);
            return dbCommand;
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        private bool ExecuteCommand(DbCommand dbCommand, CommandConfig commandConfig)
        {
            try
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Выполнение команды \"{0}\"" :
                    "Execute command \"{0}\"", commandConfig.Name);
                dbCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                return false;
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
                FillCommandMap();
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
            base.SendCommand(cmd);
            LastRequestOK = false;

            if (lineData.DataSource == null)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка: источник данных не определён" :
                    "Error: data source is undefined");
            }
            else if (string.IsNullOrEmpty(cmd.CmdCode) ||
                !cmdByCode.TryGetValue(cmd.CmdCode, out CommandConfig commandConfig))
            {
                Log.WriteLine(CommPhrases.InvalidCommand);
            }
            else if (Connect())
            {
                // create and execute command
                DbCommand dbCommand = CreateCommand(cmd, commandConfig);
                int tryNum = 0;

                while (RequestNeeded(ref tryNum))
                {
                    if (ExecuteCommand(dbCommand, commandConfig))
                        LastRequestOK = true;

                    FinishRequest();
                    tryNum++;
                }

                Disconnect();
            }

            FinishCommand();
        }
    }
}
