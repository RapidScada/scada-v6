// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using NpgsqlTypes;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using static Scada.Storages.PostgreSqlStorage.PostgreSqlStorageShared;

namespace Scada.Storages.PostgreSqlStorage
{
    /// <summary>
    /// Represents a storage logic.
    /// <para>Представляет логику хранилища.</para>
    /// </summary>
    /// <remarks>The class is thread-safe.</remarks>
    public class PostgreSqlStorageLogic : StorageLogic
    {
        /// <summary>
        /// Represents a reader that reads data from a view.
        /// </summary>
        private class ViewReader : BinaryReader
        {
            private readonly Action closeAction;

            public ViewReader(Stream stream, Action closeAction)
                : base(stream, Encoding.UTF8, false)
            {
                this.closeAction = closeAction;
            }

            protected override void Dispose(bool disposing)
            {
                try { closeAction?.Invoke(); }
                finally { base.Dispose(disposing); }
            }
        }

        /// <summary>
        /// The period of attempts to connect to the database.
        /// </summary>
        private static readonly TimeSpan ConnectAttemptPeriod = TimeSpan.FromSeconds(10);

        private TimeSpan waitTimeout;  // how long to wait for connection
        private NpgsqlConnection conn; // the database connection


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PostgreSqlStorageLogic(StorageContext storageContext)
            : base(storageContext)
        {
            waitTimeout = TimeSpan.Zero;
            conn = null;
        }


        /// <summary>
        /// Attempts to connect to the database.
        /// </summary>
        private bool CheckConnection(out string errMsg)
        {
            try
            {
                conn.Open();
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format(Locale.IsRussian ?
                    "Ошибка при соединении с БД хранилища: {0}" :
                    "Error connecting to storage database: {0}", ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Reads contents of the varchar type.
        /// </summary>
        private string ReadVarcharContents(string tableName, ServiceApp app, string path)
        {
            string sql = $"SELECT contents FROM {tableName} WHERE app_id = @appID AND path = @path LIMIT 1";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("appID", (int)app);
            cmd.Parameters.AddWithValue("path", NormalizePath(path));

            try
            {
                Monitor.Enter(conn);
                conn.Open();

                using (NpgsqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                        return reader.IsDBNull(0) ? "" : reader.GetString(0);
                }
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }

            throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, path));
        }

        /// <summary>
        /// Reads contents of the bytea type.
        /// </summary>
        private byte[] ReadByteaContents(string tableName, string path)
        {
            string sql = $"SELECT contents FROM {tableName} WHERE path = @path LIMIT 1";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("path", NormalizePath(path));

            try
            {
                Monitor.Enter(conn);
                conn.Open();

                using (NpgsqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                        return reader.IsDBNull(0) ? Array.Empty<byte>() : (byte[])reader[0];
                }
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }

            throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, path));
        }

        /// <summary>
        /// Writes contents of the varchar type.
        /// </summary>
        private void WriteVarcharContents(string tableName, ServiceApp app, string path, string contents)
        {
            string sql = 
                $"INSERT INTO {tableName} (app_id, path, contents, write_time) " + 
                "VALUES (@appID, @path, @contents, @writeTime) " +
                "ON CONFLICT (app_id, path) DO UPDATE SET contents = @contents, write_time = @writeTime";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("appID", (int)app);
            cmd.Parameters.AddWithValue("path", NormalizePath(path));
            cmd.Parameters.AddWithValue("writeTime", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("contents", 
                string.IsNullOrEmpty(contents) ? (object)DBNull.Value : contents);

            try
            {
                Monitor.Enter(conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }
        }

        /// <summary>
        /// Writes contents of the bytea type.
        /// </summary>
        private void WriteByteaContents(string tableName, string path, byte[] bytes)
        {
            string sql =
                $"INSERT INTO {tableName} (path, contents, write_time) " +
                "VALUES (@path, @contents, @writeTime) " +
                "ON CONFLICT (path) DO UPDATE SET contents = @contents, write_time = @writeTime";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("path", NormalizePath(path));
            cmd.Parameters.AddWithValue("writeTime", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("contents", 
                bytes == null || bytes.Length == 0 ? (object)DBNull.Value : bytes);

            try
            {
                Monitor.Enter(conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }
        }

        /// <summary>
        /// Opens a view for reading.
        /// </summary>
        private BinaryReader GetViewReader(string path)
        {
            string sql = $"SELECT contents FROM {GetTableName(DataCategory.View)} WHERE path = @path LIMIT 1";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("path", NormalizePath(path));
            
            NpgsqlDataReader reader = null;
            bool postponeClose = false;

            void CloseAction()
            {
                reader?.Close();
                conn.Close();
                Monitor.Exit(conn);
            }

            try
            {
                Monitor.Enter(conn);
                conn.Open();
                reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                if (reader.Read())
                {
                    if (reader.IsDBNull(0))
                    {
                        return new BinaryReader(new MemoryStream(0));
                    }
                    else
                    {
                        postponeClose = true;
                        return new ViewReader(reader.GetStream(0), CloseAction);
                    }
                }
            }
            finally
            {
                if (!postponeClose)
                    CloseAction();
            }

            throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, path));
        }

        /// <summary>
        /// Gets the table name according to the specified category.
        /// </summary>
        private static string GetTableName(DataCategory category)
        {
            switch (category)
            {
                case DataCategory.Config:
                    return Schema + ".app_config";

                case DataCategory.Storage:
                    return Schema + ".app_storage";

                case DataCategory.View:
                    return Schema + ".view_file";

                default:
                    throw new ScadaException("Data category not supported.");
            }
        }

        /// <summary>
        /// Sets separators in the specified path to backslashes.
        /// </summary>
        private static string NormalizePath(string path)
        {
            return string.IsNullOrEmpty(path) ? "" : path.Replace('/', '\\');
        }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public override void LoadConfig(XmlElement xmlElement)
        {
            base.LoadConfig(xmlElement);
            waitTimeout = TimeSpan.FromSeconds(xmlElement.GetChildAsInt("WaitTimeout"));
        }

        /// <summary>
        /// Makes the storage ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            conn = CreateDbConnection(StorageContext.InstanceConfig.Connection);

            // wait for connection
            DateTime utcNow = DateTime.UtcNow;
            DateTime startDT = utcNow;
            DateTime attempDT = DateTime.MinValue;

            while (utcNow - startDT <= waitTimeout)
            {
                if (utcNow - attempDT >= ConnectAttemptPeriod)
                {
                    attempDT = utcNow;

                    if (CheckConnection(out string errMsg))
                        break;
                    else
                        StorageContext.Log.WriteError(errMsg);
                }

                Thread.Sleep(ScadaUtils.ThreadDelay);
                utcNow = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Closes the storage.
        /// </summary>
        public override void Close()
        {
            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }
        }

        /// <summary>
        /// Reads text from the file.
        /// </summary>
        public override string ReadText(DataCategory category, string path)
        {
            if (category == DataCategory.View)
            {
                byte[] contents = ReadByteaContents(GetTableName(category), path);
                return Encoding.UTF8.GetString(contents);
            }
            else
            {
                return ReadVarcharContents(GetTableName(category), App, path);
            }
        }

        /// <summary>
        /// Reads a byte array from the file.
        /// </summary>
        public override byte[] ReadBytes(DataCategory category, string path)
        {
            if (category == DataCategory.View)
            {
                return ReadByteaContents(GetTableName(category), path);
            }
            else
            {
                string contents = ReadVarcharContents(GetTableName(category), App, path);
                return Convert.FromBase64String(contents);
            }
        }

        /// <summary>
        /// Reads the table of the configuration database.
        /// </summary>
        public override void ReadBaseTable(IBaseTable baseTable)
        {
            try
            {
                Monitor.Enter(conn);
                conn.Open();
                PostgreSqlStorageShared.ReadBaseTable(baseTable, conn);
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }
        }

        /// <summary>
        /// 更新基础表
        /// </summary>

        public override void UpdateBaseTable(IBaseTable baseTable)
        {

        }

        /// <summary>
        /// 插入或更新用户
        /// </summary>
        public override void SaveUser(IBaseTable baseTable, User user)
        {
            try
            {
                Monitor.Enter(conn);
                conn.Open();
                SaveUser(baseTable, conn, user);
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        public override int DeleteUser(IBaseTable baseTable, User user)
        {
            try
            {
                Monitor.Enter(conn);
                conn.Open();
                DeleteUser(baseTable, conn, user);
                return 1;
            }
            catch
            {
                return 0;
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }
        }


        /// <summary>
        /// 插入用户登录日志
        /// </summary>
        public override void AddUserLoginLog(IBaseTable baseTable, UserLoginLog userLoginLog)
        {
            try
            {
                Monitor.Enter(conn);
                conn.Open();
                AddUserLoginLog(baseTable, conn, userLoginLog);
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }
        }

        /// <summary>
        /// 保存机器认证码
        /// </summary>
        public override void SaveUserMachineCode(IBaseTable baseTable, UserMachineCode userMachineCode)
        {
            try
            {
                Monitor.Enter(conn);
                conn.Open();
                SaveUserMachineCode(baseTable, conn, userMachineCode);
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }
        }

        /// <summary>
        /// 保存历史密码
        /// </summary>
        public override void AddUserUsedPwd(IBaseTable baseTable, UserUsedPwd userUsedPwd)
        {
            try
            {
                Monitor.Enter(conn);
                conn.Open();
                AddUserUsedPwd(baseTable, conn, userUsedPwd);
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }
        }

        /// <summary>
        /// Writes the text to the file.
        /// </summary>
        public override void WriteText(DataCategory category, string path, string contents)
        {
            if (category == DataCategory.View)
            {
                byte[] bytes = string.IsNullOrEmpty(contents) ? null : Encoding.UTF8.GetBytes(contents);
                WriteByteaContents(GetTableName(category), path, bytes);
            }
            else
            {
                WriteVarcharContents(GetTableName(category), App, path, contents);
            }
        }

        /// <summary>
        /// Writes the byte array to the file.
        /// </summary>
        public override void WriteBytes(DataCategory category, string path, byte[] bytes)
        {
            if (category == DataCategory.View)
            {
                WriteByteaContents(GetTableName(category), path, bytes);
            }
            else
            {
                string contents = bytes == null ? null : Convert.ToBase64String(bytes);
                WriteVarcharContents(GetTableName(category), App, path, contents);
            }
        }

        /// <summary>
        /// Opens a text file for reading.
        /// </summary>
        public override TextReader OpenText(DataCategory category, string path)
        {
            string contents = ReadText(category, path);
            return new StringReader(contents);
        }

        /// <summary>
        /// Opens a binary file for reading.
        /// </summary>
        public override BinaryReader OpenBinary(DataCategory category, string path)
        {
            if (category == DataCategory.View)
            {
                return GetViewReader(path);
            }
            else
            {
                byte[] bytes = ReadBytes(category, path);
                return new BinaryReader(new MemoryStream(bytes, false), Encoding.UTF8, false);
            }
        }

        /// <summary>
        /// Gets information associated with the file.
        /// </summary>
        public override ShortFileInfo GetFileInfo(DataCategory category, string path)
        {
            string sql =
                $"SELECT length(contents), write_time FROM {GetTableName(category)} " +
                "WHERE " + (category == DataCategory.View ? "" : "app_id = @appID AND ") +
                "path = @path LIMIT 1";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("path", NormalizePath(path));

            if (category != DataCategory.View)
                cmd.Parameters.AddWithValue("appID", (int)App);

            try
            {
                Monitor.Enter(conn);
                conn.Open();

                using (NpgsqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        return new ShortFileInfo
                        {
                            Exists = true,
                            LastWriteTime = reader.IsDBNull(1)
                                ? DateTime.MinValue
                                : reader.GetDateTime(1).ToUniversalTime(),
                            Length = reader.GetInt32(0)
                        };
                    }
                }
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }

            return ShortFileInfo.FileNotExists;
        }

        /// <summary>
        /// Gets a list of file paths that match the specified pattern.
        /// </summary>
        public override ICollection<string> GetFileList(DataCategory category, string path, string searchPattern)
        {
            List<string> fileList = new List<string>();
            string sql =
                $"SELECT path FROM {GetTableName(category)} " +
                "WHERE " + (category == DataCategory.View ? "" : "app_id = @appID AND ") +
                "starts_with(path, @path) AND substring(path from @pathLen) LIKE @searchPattern";
            // TODO: split_part(path, '\', -1) LIKE @searchPattern after porting on PostgreSQL 14

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("path", NormalizePath(path));
            cmd.Parameters.AddWithValue("pathLen", (path ?? "").Length + 1);
            cmd.Parameters.AddWithValue("searchPattern", string.IsNullOrEmpty(searchPattern) 
                ? "%" 
                : searchPattern.Replace('*', '%').Replace('?', '_'));

            if (category != DataCategory.View)
                cmd.Parameters.AddWithValue("appID", (int)App);

            try
            {
                Monitor.Enter(conn);
                conn.Open();

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fileList.Add(reader.GetString(0));
                    }
                }
            }
            finally
            {
                conn.Close();
                Monitor.Exit(conn);
            }

            return fileList;
        }

        /// <summary>
        /// 插入或更新用户
        /// </summary>
        private void SaveUser(IBaseTable baseTable, NpgsqlConnection conn, User user)
        {
            PostgreUserExt postgreUserExt = new PostgreUserExt();
            NpgsqlCommand cmd = new NpgsqlCommand(postgreUserExt.InsertUserTableQuery, conn);
            cmd.Parameters.AddWithValue("userid", NpgsqlDbType.Integer, user.UserID);
            cmd.Parameters.AddWithValue("enabled", NpgsqlDbType.Boolean, user.Enabled);
            cmd.Parameters.AddWithValue("name", NpgsqlDbType.Varchar, user.Name);
            cmd.Parameters.AddWithValue("password", NpgsqlDbType.Varchar, user.Password);
            cmd.Parameters.AddWithValue("roleid", NpgsqlDbType.Integer, user.RoleID);
            cmd.Parameters.AddWithValue("descr", NpgsqlDbType.Varchar, user.Descr.NormalText());
            cmd.Parameters.AddWithValue("userrealname", NpgsqlDbType.Varchar, user.UserRealName.NormalText());
            cmd.Parameters.AddWithValue("gender", NpgsqlDbType.Varchar, user.Gender.NormalText());
            cmd.Parameters.AddWithValue("phone", NpgsqlDbType.Varchar, user.Phone.NormalText());
            cmd.Parameters.AddWithValue("email", NpgsqlDbType.Varchar, user.Email.NormalText());
            cmd.Parameters.AddWithValue("userpwdenabled", NpgsqlDbType.Boolean, user.UserPwdEnabled);
            cmd.Parameters.AddWithValue("faenabled", NpgsqlDbType.Boolean, user.FaEnabled);
            cmd.Parameters.AddWithValue("googleenabled", NpgsqlDbType.Boolean, user.GoogleEnabled);
            cmd.Parameters.AddWithValue("fasecret", NpgsqlDbType.Varchar, user.FaSecret.NormalText());
            cmd.Parameters.AddWithValue("faverifysuccess", NpgsqlDbType.Boolean, user.FaVerifySuccess);
            cmd.Parameters.AddWithValue("pwdperiodmodify", NpgsqlDbType.Boolean, user.PwdPeriodModify);
            cmd.Parameters.AddWithValue("pwdperiodlimit", NpgsqlDbType.Integer, user.PwdPeriodLimit);
            cmd.Parameters.AddWithValue("pwdlenlimit", NpgsqlDbType.Integer, user.PwdLenLimit);
            cmd.Parameters.AddWithValue("pwdcomplicatedrequire", NpgsqlDbType.Boolean, user.PwdComplicatedRequire);
            cmd.Parameters.AddWithValue("pwdcomplicatedformat", NpgsqlDbType.Varchar, user.PwdComplicatedFormat.NormalText());
            cmd.Parameters.AddWithValue("pwduseddifferent", NpgsqlDbType.Boolean, user.PwdUsedDifferent);
            cmd.Parameters.AddWithValue("pwdusedtimes", NpgsqlDbType.Integer, user.PwdUsedTimes);
            cmd.Parameters.AddWithValue("pwdupdatetime", NpgsqlDbType.TimestampTz, user.PwdUpdateTime.ToUniversalTime());

            cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// 插入或更新用户
        /// </summary>
        private void DeleteUser(IBaseTable baseTable, NpgsqlConnection conn, User user)
        {
            PostgreUserExt postgreUserExt = new PostgreUserExt();
            NpgsqlCommand cmd = new NpgsqlCommand(postgreUserExt.InsertUserTableQuery, conn);
            cmd.Parameters.AddWithValue("userid", NpgsqlDbType.Integer, user.UserID);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 插入用户登录日志
        /// </summary>
        private void AddUserLoginLog(IBaseTable baseTable, NpgsqlConnection conn, UserLoginLog userLoginLog)
        {
            PostgreUserExt postgreUserExt = new PostgreUserExt();
            NpgsqlCommand cmd = new NpgsqlCommand(postgreUserExt.InsertUserLoginLogQuery, conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Integer, userLoginLog.Id);
            cmd.Parameters.AddWithValue("userid", NpgsqlDbType.Integer, userLoginLog.UserID);
            cmd.Parameters.AddWithValue("loginip", NpgsqlDbType.Varchar, userLoginLog.LoginIP.NormalText());
            cmd.Parameters.AddWithValue("logintime", NpgsqlDbType.TimestampTz, userLoginLog.LoginTime.ToUniversalTime());
            cmd.Parameters.AddWithValue("loginstatus", NpgsqlDbType.Integer, userLoginLog.LoginStatus);
            cmd.Parameters.AddWithValue("logindesc", NpgsqlDbType.Varchar, userLoginLog.LoginDesc.NormalText());

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 保存机器认证码
        /// </summary>
        private void SaveUserMachineCode(IBaseTable baseTable, NpgsqlConnection conn, UserMachineCode userMachineCode)
        {
            PostgreUserExt postgreUserExt = new PostgreUserExt();
            NpgsqlCommand cmd = new NpgsqlCommand(postgreUserExt.InsertUserMachineCodeTableQuery, conn);
            cmd.Parameters.AddWithValue("id", userMachineCode.Id);
            cmd.Parameters.AddWithValue("userid", userMachineCode.UserID);
            cmd.Parameters.AddWithValue("machinecode", NpgsqlDbType.Varchar, userMachineCode.MachineCode);
            cmd.Parameters.AddWithValue("isexpired", userMachineCode.IsExpired);
            cmd.Parameters.AddWithValue("createtime", NpgsqlDbType.TimestampTz, userMachineCode.CreateTime.ToUniversalTime());
            cmd.Parameters.AddWithValue("lastlogintime", NpgsqlDbType.TimestampTz, userMachineCode.LastLoginTime.ToUniversalTime());
            //id, userid, machinecode, isexpired, createtime, lastlogintime
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 保存历史密码
        /// </summary>
        private void AddUserUsedPwd(IBaseTable baseTable, NpgsqlConnection conn, UserUsedPwd userUsedPwd)
        {
            PostgreUserExt postgreUserExt = new PostgreUserExt();
            NpgsqlCommand cmd = new NpgsqlCommand(postgreUserExt.InsertUserUsedPwdTableTableQuery, conn);
            cmd.Parameters.AddWithValue("id", userUsedPwd.Id);
            cmd.Parameters.AddWithValue("userid", userUsedPwd.UserID);
            cmd.Parameters.AddWithValue("password", NpgsqlDbType.Varchar, userUsedPwd.Password);
            cmd.Parameters.AddWithValue("createtime", NpgsqlDbType.TimestampTz, userUsedPwd.CreateTime.ToUniversalTime());
            //id, userid, password, createtime
            cmd.ExecuteNonQuery();
        }
    }
}
