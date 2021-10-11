// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using NpgsqlTypes;
using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.ComponentModel;
using System.Text;
using static Scada.Storages.PostgreSqlStorage.PostgreSqlStorageShared;

namespace Scada.Admin.Extensions.ExtDepPostgreSql
{
    /// <summary>
    /// Uploads the configuration.
    /// <para>Передаёт конфигурацию.</para>
    /// </summary>
    internal class Uploader
    {
        private readonly ScadaProject project;
        private readonly ProjectInstance instance;
        private readonly DeploymentProfile profile;
        private readonly ITransferControl transferControl;
        private NpgsqlConnection conn;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Uploader(ScadaProject project, ProjectInstance instance, DeploymentProfile profile, 
            ITransferControl transferControl)
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.profile = profile ?? throw new ArgumentNullException(nameof(profile));
            this.transferControl = transferControl ?? throw new ArgumentNullException(nameof(transferControl));
            conn = null;
        }


        /// <summary>
        /// Removes all tables from the configuration database.
        /// </summary>
        private void ClearBase()
        {
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                trans.Commit();

                foreach (IBaseTable baseTable in project.ConfigBase.AllTables)
                {
                    transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                        "Удаление таблицы {0}" :
                        "Delete table {0}", baseTable.Name));

                    string sql = $"DROP TABLE IF EXISTS {GetBaseTableName(baseTable)} CASCADE";
                    new NpgsqlCommand(sql, conn, trans).ExecuteNonQuery();
                }

                transferControl.WriteLine();
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Creates and fills the configuration database tables.
        /// </summary>
        private void CreateBase()
        {
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                trans.Commit();

                foreach (IBaseTable baseTable in project.ConfigBase.AllTables)
                {
                    transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                        "Создание таблицы {0}" :
                        "Create table {0}", baseTable.Name));

                    string sql = GetTableDDL(baseTable);
                    new NpgsqlCommand(sql, conn, trans).ExecuteNonQuery();
                    InsertRows(baseTable, trans);
                }

                transferControl.WriteLine();
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Inserts rows in the configuration database table.
        /// </summary>
        private void InsertRows(IBaseTable baseTable, NpgsqlTransaction trans)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);
            int propCnt = props.Count;

            if (propCnt == 0 || baseTable.ItemCount == 0)
                return;

            // create INSERT script like
            // INSERT INTO tablename (col1, col2) VALUES (@Col1, @Col2);
            StringBuilder sbSql1 = new();
            StringBuilder sbSql2 = new();
            sbSql1.Append("INSERT INTO ").Append(GetBaseTableName(baseTable)).Append(" (");
            sbSql2.Append("VALUES (");

            for (int i = 0; i < propCnt; i++)
            {
                if (i > 0)
                {
                    sbSql1.Append(", ");
                    sbSql2.Append(", ");
                }

                PropertyDescriptor prop = props[i];
                sbSql1.Append(GetBaseColumnName(prop));
                sbSql2.Append('@').Append(prop.Name);
            }

            sbSql1.Append(") ");
            sbSql2.Append(");");

            // create INSERT command
            string sql = sbSql1.ToString() + sbSql2.ToString();
            NpgsqlCommand cmd = new(sql, conn, trans);

            foreach (PropertyDescriptor prop in props)
            {
                cmd.Parameters.Add(prop.Name, GetDbType(prop.PropertyType));
            }

            // execute command for each table item
            foreach (object item in baseTable.EnumerateItems())
            {
                for (int i = 0; i < propCnt; i++)
                {
                    cmd.Parameters[i].Value = props[i].GetValue(item) ?? DBNull.Value;
                }

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Creates foreign keys for the configuration database tables.
        /// </summary>
        private void CreateForeignKeys()
        {
            NpgsqlTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                trans.Commit();

                foreach (IBaseTable baseTable in project.ConfigBase.AllTables)
                {
                    transferControl.WriteMessage(string.Format(Locale.IsRussian ?
                        "Создание внешних ключей таблицы {0}" :
                        "Create foreign keys for the {0} table", baseTable.Name));

                    foreach (TableRelation relation in baseTable.DependsOn)
                    {
                        string sql = GetForeignKeyDDL(relation);
                        new NpgsqlCommand(sql, conn, trans).ExecuteNonQuery();
                    }
                }

                transferControl.WriteLine();
            }
            catch
            {
                trans?.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Gets a SQL script to create the table.
        /// </summary>
        private static string GetTableDDL(IBaseTable baseTable)
        {
            StringBuilder sbSql = new();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);

            if (props.Count > 0)
            {
                sbSql.AppendLine($"CREATE TABLE {GetBaseTableName(baseTable)} (");

                foreach (PropertyDescriptor prop in props)
                {
                    sbSql
                        .Append(GetBaseColumnName(prop)).Append(' ')
                        .Append(GetDbTypeName(prop.PropertyType))
                        .Append(prop.PropertyType.IsNullable() || prop.PropertyType.IsClass ? "" : " NOT NULL")
                        .AppendLine(",");
                }

                sbSql.AppendLine(
                    $"CONSTRAINT pk_{baseTable.Name.ToLowerInvariant()} " +
                    $"PRIMARY KEY ({GetBaseColumnName(baseTable.PrimaryKey)}))");
            }

            return sbSql.ToString();
        }

        /// <summary>
        /// Gets a SQL script to create the table.
        /// </summary>
        private static string GetForeignKeyDDL(TableRelation relation)
        {
            // use child colmun name instead of parent table name,
            // because the RoleRef table has 2 FKs referencing the Role table
            string fkName = "fk_" +
                relation.ChildTable.Name.ToLowerInvariant() + "_" +
                relation.ChildColumn.ToLowerInvariant();

            return
                $"ALTER TABLE {GetBaseTableName(relation.ChildTable)} " +
                $"ADD CONSTRAINT {fkName} FOREIGN KEY ({GetBaseColumnName(relation.ChildColumn)}) " +
                $"REFERENCES {GetBaseTableName(relation.ParentTable)} ({GetBaseColumnName(relation.ParentTable.PrimaryKey)})";
        }

        /// <summary>
        /// Gets the database type name corresponding to the specified property type.
        /// </summary>
        private static string GetDbTypeName(Type propType)
        {
            Type type = propType.IsNullable() ? Nullable.GetUnderlyingType(propType) : propType;

            if (type == typeof(int))
                return "integer";
            else if (type == typeof(double))
                return "double precision";
            else if (type == typeof(bool))
                return "boolean";
            else if (type == typeof(DateTime))
                return "timestamp with time zone";
            else if (type == typeof(string))
                return "character varying";
            else
                throw new ScadaException("Data type {0} is not supported.", type.FullName);
        }

        /// <summary>
        /// Gets the database type corresponding to the specified property type.
        /// </summary>
        private static NpgsqlDbType GetDbType(Type propType)
        {
            Type type = propType.IsNullable() ? Nullable.GetUnderlyingType(propType) : propType;

            if (type == typeof(int))
                return NpgsqlDbType.Integer;
            else if (type == typeof(double))
                return NpgsqlDbType.Double;
            else if (type == typeof(bool))
                return NpgsqlDbType.Boolean;
            else if (type == typeof(DateTime))
                return NpgsqlDbType.TimestampTz;
            else if (type == typeof(string))
                return NpgsqlDbType.Varchar;
            else
                throw new ScadaException("Data type {0} is not supported.", type.FullName);
        }


        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        public bool Upload()
        {
            if (!profile.DbEnabled)
            {
                throw new ScadaException(Locale.IsRussian ?
                    "База данных не включена в профиле развёртывания." :
                    "Database is not enabled in the deployment profile.");
            }

            transferControl.SetCancelEnabled(true);

            try
            {
                conn = CreateDbConnection(profile.DbConnectionOptions);
                conn.Open();
                ClearBase();
                CreateBase();
                CreateForeignKeys();
            }
            finally
            {
                conn?.Close();
                conn = null;
            }

            return true;
        }
    }
}
