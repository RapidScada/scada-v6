// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.MultiDb;
using Scada.Server.Modules.ModDbExport.Config;
using System.Data.Common;

namespace Scada.Server.Modules.ModDbExport.Logic.Queries
{
    /// <summary>
    /// Represents a query for exporting commands.
    /// <para>Представляет запрос для экспорта команд.</para>
    /// </summary>
    internal class CmdQuery : Query
    {
        /// <summary>
        /// Represents query parameters.
        /// </summary>
        public class QueryParameters
        {
            public DbParameter CommandID { get; init; }
            public DbParameter CreationTime { get; init; }
            public DbParameter ClientName { get; init; }
            public DbParameter UserID { get; init; }
            public DbParameter CnlNum { get; init; }
            public DbParameter ObjNum { get; init; }
            public DbParameter DeviceNum { get; init; }
            public DbParameter CmdNum { get; init; }
            public DbParameter CmdCode { get; init; }
            public DbParameter CmdVal { get; init; }
            public DbParameter CmdData { get; init; }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CmdQuery(int queryID, QueryOptions queryOptions, DataSource dataSource)
            : base(queryID, queryOptions, dataSource)
        {
            Parameters = new()
            {
                CommandID = DataSource.SetParam(Command, "commandID", 0L),
                CreationTime = DataSource.SetParam(Command, "creationTime", DateTime.MinValue),
                ClientName = DataSource.SetParam(Command, "clientName", ""),
                UserID = DataSource.SetParam(Command, "userID", 0),
                CnlNum = DataSource.SetParam(Command, "cnlNum", 0),
                ObjNum = DataSource.SetParam(Command, "objNum", 0),
                DeviceNum = DataSource.SetParam(Command, "deviceNum", 0),
                CmdNum = DataSource.SetParam(Command, "cmdNum", 0),
                CmdCode = DataSource.SetParam(Command, "cmdCode", ""),
                CmdVal = DataSource.SetParam(Command, "cmdVal", 0.0),
                CmdData = DataSource.SetParam(Command, "cmdData", Array.Empty<byte>())
            };
        }

        /// <summary>
        /// Gets the query parameters.
        /// </summary>
        public QueryParameters Parameters { get; }

        /// <summary>
        /// Sets the query parameters according the specified command.
        /// </summary>
        public void SetParameters(TeleCommand command)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            Parameters.CommandID.Value = command.CommandID;
            Parameters.CreationTime.Value = command.CreationTime;
            Parameters.ClientName.Value = command.ClientName ?? "";
            Parameters.UserID.Value = command.UserID;
            Parameters.CnlNum.Value = command.CnlNum;
            Parameters.ObjNum.Value = command.ObjNum;
            Parameters.DeviceNum.Value = command.DeviceNum;
            Parameters.CmdNum.Value = command.CmdNum;
            Parameters.CmdCode.Value = command.CmdCode ?? "";
            Parameters.CmdVal.Value = double.IsNaN(command.CmdVal) ? 
                DBNull.Value : command.CmdVal;
            Parameters.CmdData.Value = command.CmdData == null || command.CmdData.Length == 0 ?
                DBNull.Value : command.CmdData;
        }
    }
}
