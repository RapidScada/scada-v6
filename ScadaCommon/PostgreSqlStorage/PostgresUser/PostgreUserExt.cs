using Scada.Storages.PostgreSqlStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Storages
{
    public class PostgreUserExt
    {

        private string UserTable = PostgreSqlStorageShared.Schema + ".user";
        private string UserLoginLogTable = PostgreSqlStorageShared.Schema + ".userloginlog";
        private string UserMachineCodeTable = PostgreSqlStorageShared.Schema + ".usermachinecode";
        private string UserUsedPwdTable = PostgreSqlStorageShared.Schema + ".userusedpwd";


        /// <summary>
        /// Gets an SQL query to insert or update usermachinecode
        /// </summary>
        public string InsertUserTableQuery
        {
            get
            {
                return
                    $"INSERT INTO {UserTable} (userid, enabled, name, password, roleid, descr, userrealname, gender, phone, email, userpwdenabled, faenabled, googleenabled, fasecret, faverifysuccess, pwdperiodmodify, pwdperiodlimit, pwdlenlimit, pwdcomplicatedrequire, pwdcomplicatedformat, pwduseddifferent, pwdusedtimes, pwdupdatetime) " +
                    "VALUES (@userid, @enabled, @name, @password, @roleid, @descr, @userrealname, @gender, @phone, @email, @userpwdenabled, @faenabled, @googleenabled, @fasecret, @faverifysuccess, @pwdperiodmodify, @pwdperiodlimit, @pwdlenlimit, @pwdcomplicatedrequire, @pwdcomplicatedformat, @pwduseddifferent, @pwdusedtimes, @pwdupdatetime) " +
                    "ON CONFLICT (userid) DO UPDATE " +
                    "SET enabled = EXCLUDED.enabled,name = EXCLUDED.name,password = EXCLUDED.password,roleid = EXCLUDED.roleid,descr = EXCLUDED.descr,userrealname = EXCLUDED.userrealname,gender = EXCLUDED.gender,phone = EXCLUDED.phone,email = EXCLUDED.email,userpwdenabled = EXCLUDED.userpwdenabled,faenabled = EXCLUDED.faenabled," +
                    "googleenabled = EXCLUDED.googleenabled,fasecret = EXCLUDED.fasecret,faverifysuccess = EXCLUDED.faverifysuccess,pwdperiodmodify = EXCLUDED.pwdperiodmodify,pwdperiodlimit = EXCLUDED.pwdperiodlimit,pwdlenlimit = EXCLUDED.pwdlenlimit,pwdcomplicatedrequire = EXCLUDED.pwdcomplicatedrequire," +
                    "pwdcomplicatedformat = EXCLUDED.pwdcomplicatedformat,pwduseddifferent = EXCLUDED.pwduseddifferent,pwdusedtimes = EXCLUDED.pwdusedtimes,pwdupdatetime = EXCLUDED.pwdupdatetime";
            }
        }
        /// <summary>
        /// Gets an SQL query to delete usermachinecode
        /// </summary>
        public string DeleteUserTableQuery
        {
            get
            {
                return
                    $"DELETE {UserTable} where userid = @userid;";
            }
        }

        /// <summary>
        /// Gets an SQL query to insert userloginlog
        /// </summary>
        public string InsertUserLoginLogQuery
        {
            get
            {
                return
                    $"INSERT INTO {UserLoginLogTable} (id, userid, loginip, logintime, loginstatus, logindesc) " +
                    "VALUES (@id, @userid, @loginip, @logintime, @loginstatus, @logindesc) ";
            }
            /*
             INSERT INTO "project"."userloginlog"("id", "userid", "loginip", "logintime", "loginstatus", "logindesc") VALUES (1, 11, '127.0.0.1', '2023-04-20 17:50:16.579013+08', 1, 'WebLogin');
             */
        }

        /// <summary>
        /// Gets an SQL query to insert or update usermachinecode
        /// </summary>
        public string InsertUserMachineCodeTableQuery
        {
            get
            {
                return
                    $"INSERT INTO {UserMachineCodeTable} (id, userid, machinecode, isexpired, createtime, lastlogintime) " +
                    "VALUES (@id, @userid, @machinecode, @isexpired, @createtime, @lastlogintime) " +
                    "ON CONFLICT (id) DO UPDATE " +
                    "SET isexpired = EXCLUDED.isexpired, lastlogintime = EXCLUDED.lastlogintime";
            }
        }

        /// <summary>
        /// Gets an SQL query to insert userusedpwd
        /// </summary>
        public string InsertUserUsedPwdTableTableQuery
        {
            get
            {
                return
                    $"INSERT INTO {UserUsedPwdTable} (id, userid, password, createtime) " +
                    "VALUES (@id, @userid, @password, @createtime) ";
            }
        }
    }
}
