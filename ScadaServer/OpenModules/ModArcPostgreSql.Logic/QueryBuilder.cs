// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Builds requests for creating and using the archive.
    /// <para>Формирует запросы для создания и использования архива.</para>
    /// </summary>
    internal class QueryBuilder
    {
        /// <summary>
        /// Defines the correspondence between the event object properties and the database column names.
        /// </summary>
        public static readonly Dictionary<string, string> EventColumnMap;


        /// <summary>
        /// Initializes the class.
        /// </summary>
        static QueryBuilder()
        {
            EventColumnMap = new Dictionary<string, string>()
            {
                { "EventID", "event_id" },
                { "Timestamp", "time_stamp" },
                { "Hidden", "hidden" },
                { "CnlNum", "cnl_num" },
                { "ObjNum", "obj_num" },
                { "DeviceNum", "device_num" },
                { "PrevCnlVal", "prev_cnl_val" },
                { "PrevCnlStat", "prev_cnl_stat" },
                { "CnlVal", "cnl_val" },
                { "CnlStat", "cnl_stat" },
                { "Severity", "severity" },
                { "AckRequired", "ack_required" },
                { "Ack", "ack" },
                { "AckTimestamp", "ack_timestamp" },
                { "AckUserID", "ack_user_id" },
                { "TextFormat", "text_format" },
                { "Text", "event_text" },
                { "Data", "event_data" }
            };
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QueryBuilder(string archiveCode)
        {
            string tablePrefix = string.IsNullOrEmpty(archiveCode) ? "" : archiveCode.ToLowerInvariant() + "_";
            CurrentTableShort = tablePrefix + "current";
            HistoricalTableShort = tablePrefix + "historical";
            EventTableShort = tablePrefix + "event";

            tablePrefix = DbUtils.Schema + ".";
            CurrentTable = tablePrefix + CurrentTableShort;
            HistoricalTable = tablePrefix + HistoricalTableShort;
            EventTable = tablePrefix + EventTableShort;
        }


        /// <summary>
        /// Gets the short name of the current data table.
        /// </summary>
        private string CurrentTableShort { get; }

        /// <summary>
        /// Gets the short name of the historical data table.
        /// </summary>
        private string HistoricalTableShort { get; }

        /// <summary>
        /// Gets the short name of the event table.
        /// </summary>
        private string EventTableShort { get; }

        /// <summary>
        /// Gets the name of the current data table.
        /// </summary>
        public string CurrentTable { get; }

        /// <summary>
        /// Gets the name of the historical data table.
        /// </summary>
        public string HistoricalTable { get; }

        /// <summary>
        /// Gets the name of the event table.
        /// </summary>
        public string EventTable { get; }

        /// <summary>
        /// Gets an SQL query to create a schema for the archives.
        /// </summary>
        public string CreateSchemaQuery
        {
            get
            {
                return "CREATE SCHEMA IF NOT EXISTS " + DbUtils.Schema;
            }
        }

        /// <summary>
        /// Gets an SQL query to create the current data table.
        /// </summary>
        public string CreateCurrentTableQuery
        {
            get
            {
                return
                    $"CREATE TABLE IF NOT EXISTS {CurrentTable} (" +
                    "cnl_num integer NOT NULL, " +
                    "time_stamp timestamp with time zone NOT NULL, " +
                    "val double precision NOT NULL, " +
                    "stat integer NOT NULL, " +
                    "PRIMARY KEY (cnl_num))";
            }
        }

        /// <summary>
        /// Gets an SQL query to create the historical data table.
        /// </summary>
        public string CreateHistoricalTableQuery
        {
            get
            {
                return
                    $"CREATE TABLE IF NOT EXISTS {HistoricalTable} (" +
                    "cnl_num integer NOT NULL, " +
                    "time_stamp timestamp with time zone NOT NULL, " +
                    "val double precision NOT NULL, " +
                    "stat integer NOT NULL, " +
                    "PRIMARY KEY (cnl_num, time_stamp)) " +
                    "PARTITION BY RANGE (time_stamp); " +
                    $"CREATE INDEX IF NOT EXISTS idx_{HistoricalTableShort}_time_stamp ON {HistoricalTable} (time_stamp);";
            }
        }

        /// <summary>
        /// Gets an SQL query to create the event table.
        /// </summary>
        public string CreateEventTableQuery
        {
            get
            {
                return
                    $"CREATE TABLE IF NOT EXISTS {EventTable} (" +
                    "event_id bigint NOT NULL, " +
                    "time_stamp timestamp with time zone NOT NULL, " +
                    "hidden boolean NOT NULL, " +
                    "cnl_num integer NOT NULL, " +
                    "obj_num integer NOT NULL, " +
                    "device_num integer NOT NULL, " +
                    "prev_cnl_val double precision NOT NULL, " +
                    "prev_cnl_stat integer NOT NULL, " +
                    "cnl_val double precision NOT NULL, " +
                    "cnl_stat integer NOT NULL, " +
                    "severity integer NOT NULL, " +
                    "ack_required boolean NOT NULL, " +
                    "ack boolean NOT NULL, " +
                    "ack_timestamp timestamp with time zone NOT NULL, " +
                    "ack_user_id integer NOT NULL, " +
                    "text_format integer NOT NULL, " +
                    "event_text character varying, " +
                    "event_data bytea, " +
                    "PRIMARY KEY (event_id, time_stamp)) " +
                    "PARTITION BY RANGE (time_stamp); " +
                    $"CREATE INDEX IF NOT EXISTS idx_{EventTableShort}_time_stamp ON {EventTable} (time_stamp); " +
                    $"CREATE INDEX IF NOT EXISTS idx_{EventTableShort}_cnl_num ON {EventTable} (cnl_num); " +
                    $"CREATE INDEX IF NOT EXISTS idx_{EventTableShort}_obj_num ON {EventTable} (obj_num); " +
                    $"CREATE INDEX IF NOT EXISTS idx_{EventTableShort}_device_num ON {EventTable} (device_num);";
            }
        }

        /// <summary>
        /// Gets an SQL query to insert or update current data.
        /// </summary>
        public string InsertCurrentDataQuery
        {
            get
            {
                return
                    $"INSERT INTO {CurrentTable} (cnl_num, time_stamp, val, stat) " +
                    "VALUES (@cnlNum, @timestamp, @val, @stat) " +
                    "ON CONFLICT (cnl_num) DO UPDATE " +
                    "SET time_stamp = EXCLUDED.time_stamp, val = EXCLUDED.val, stat = EXCLUDED.stat";
            }
        }

        /// <summary>
        /// Gets an SQL query to insert or update historical data.
        /// </summary>
        public string InsertHistoricalDataQuery
        {
            get
            {
                return
                    $"INSERT INTO {HistoricalTable} (cnl_num, time_stamp, val, stat) " +
                    "VALUES (@cnlNum, @timestamp, @val, @stat) " +
                    "ON CONFLICT (cnl_num, time_stamp) DO UPDATE " +
                    "SET val = EXCLUDED.val, stat = EXCLUDED.stat";
            }
        }

        /// <summary>
        /// Gets an SQL query to insert or update an event.
        /// </summary>
        public string InsertEventQuery
        {
            get
            {
                return
                    $"INSERT INTO {EventTable} (event_id, time_stamp, hidden, cnl_num, obj_num, device_num, " +
                    "prev_cnl_val, prev_cnl_stat, cnl_val, cnl_stat, severity, " +
                    "ack_required, ack, ack_timestamp, ack_user_id, text_format, event_text, event_data) " +
                    "VALUES (@eventID, @timestamp, @hidden, @cnlNum, " +
                    "@objNum, @deviceNum, @prevCnlVal, @prevCnlStat, @cnlVal, @cnlStat, @severity, " +
                    "@ackRequired, @ack, @ackTimestamp, @ackUserID, @textFormat, @eventText, @eventData) " +
                    "ON CONFLICT (event_id, time_stamp) DO UPDATE " +
                    "SET hidden = EXCLUDED.hidden, cnl_num = EXCLUDED.cnl_num, " +
                    "obj_num = EXCLUDED.obj_num, device_num = EXCLUDED.device_num, " + 
                    "prev_cnl_val = EXCLUDED.prev_cnl_val, prev_cnl_stat = EXCLUDED.prev_cnl_stat, " +
                    "cnl_val = EXCLUDED.cnl_val, cnl_stat = EXCLUDED.cnl_stat, severity = EXCLUDED.severity, " +
                    "ack_required = EXCLUDED.ack_required, ack = EXCLUDED.ack, ack_timestamp = EXCLUDED.ack_timestamp, " +
                    "ack_user_id = EXCLUDED.ack_user_id, text_format = EXCLUDED.text_format, " + 
                    "event_text = EXCLUDED.event_text, event_data = EXCLUDED.event_data";
            }
        }

        /// <summary>
        /// Gets an SQL query to select events. The WHERE clause must be added to the query.
        /// </summary>
        public string SelectEventQuery
        {
            get
            {
                return
                    "SELECT event_id, time_stamp, hidden, cnl_num, obj_num, device_num, " +
                    "prev_cnl_val, prev_cnl_stat, cnl_val, cnl_stat, severity, " +
                    "ack_required, ack, ack_timestamp, ack_user_id, text_format, event_text, event_data " +
                    $"FROM {EventTable} ";
            }
        }
    }
}
