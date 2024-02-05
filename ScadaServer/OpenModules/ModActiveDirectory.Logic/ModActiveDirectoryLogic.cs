// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Dbms;
using Scada.Lang;
using Scada.Server.Lang;
using Scada.Server.Modules.ModActiveDirectory.Config;
using System.Data;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Scada.Server.Modules.ModActiveDirectory.Logic
{
    /// <summary>
    /// Implements the server module logic.
    /// <para>Реализует логику серверного модуля.</para>
    /// </summary>
    public class ModActiveDirectoryLogic : ModuleLogic
    {
        private readonly ModuleConfig moduleConfig;      // the module configuration
        private readonly Dictionary<string, User> users; // the users accessed by username


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModActiveDirectoryLogic(IServerContext serverContext)
            : base(serverContext)
        {
            moduleConfig = new ModuleConfig();
            users = new Dictionary<string, User>();
        }


        /// <summary>
        /// Gets the module code.
        /// </summary>
        public override string Code
        {
            get
            {
                return ModuleUtils.ModuleCode;
            }
        }

        /// <summary>
        /// Gets the module purposes.
        /// </summary>
        public override ModulePurposes ModulePurposes
        {
            get
            {
                return ModulePurposes.Auth;
            }
        }


        /// <summary>
        /// Validates user credentials.
        /// </summary>
        private static bool ValidateCredentials(LdapConnection connection)
        {
            try
            {
                connection.Bind();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates the user against the configuration database.
        /// </summary>
        private static UserValidationResult ValidateByConfigDatabase(User user)
        {
            UserValidationResult result = new()
            {
                UserID = user.UserID,
                RoleID = user.RoleID
            };

            if (user.Enabled && user.RoleID > RoleID.Disabled)
                result.IsValid = true;
            else
                result.ErrorMessage = ServerPhrases.AccountDisabled;

            return result;
        }

        /// <summary>
        /// Validates the user in Active Directory.
        /// </summary>
        private UserValidationResult ValidateByActiveDirectory(LdapConnection connection, string username)
        {
            if (FindUserEntry(connection, username, out SearchResultEntry userEntry))
            {
                List<string> userGroups = FindUserGroups(connection, userEntry);

                foreach (Role role in ServerContext.ConfigDatabase.RoleTable)
                {
                    if (GroupsContain(userGroups, role.Code))
                    {
                        if (role.RoleID > RoleID.Disabled)
                        {
                            return new UserValidationResult
                            {
                                IsValid = true,
                                RoleID = role.RoleID
                            };
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                return UserValidationResult.Fail(ServerPhrases.AccountDisabled);
            }
            else
            {
                return UserValidationResult.Fail(Locale.IsRussian ?
                    "Пользователь не найден в Active Directory" :
                    "User not found in Active Directory");
            }
        }

        /// <summary>
        /// Finds the specified user.
        /// </summary>
        private bool FindUserEntry(LdapConnection connection, string username, out SearchResultEntry userEntry)
        {
            SearchRequest request = new(moduleConfig.SearchRoot, "(sAMAccountName=" + username + ")", 
                SearchScope.Subtree, "memberOf")
            {
                SizeLimit = 1
            };

            if (connection.SendRequest(request) is SearchResponse searchResponse &&
                searchResponse.Entries.Count > 0)
            {
                userEntry = searchResponse.Entries[0];
                return true;
            }
            else
            {
                userEntry = null;
                return false;
            }
        }

        /// <summary>
        /// Finds security groups that the specified user is a member of.
        /// </summary>
        private List<string> FindUserGroups(LdapConnection connection, SearchResultEntry userEntry)
        {
            List<string> groups = new();

            if (userEntry.Attributes.Contains("memberOf"))
            {
                foreach (object attrVal in userEntry.Attributes["memberOf"].GetValues(typeof(string)))
                {
                    string group = attrVal == null ? "" : attrVal.ToString();

                    if (!string.IsNullOrEmpty(group))
                    {
                        groups.Add(group);
                        FindOwnerGroups(connection, group, groups);
                    }
                }
            }

            return groups;
        }

        /// <summary>
        /// Finds security groups that the specified group belongs to and adds them to the list.
        /// </summary>
        private void FindOwnerGroups(LdapConnection connection, string group, List<string> groups)
        {
            SearchRequest request = new(moduleConfig.SearchRoot, "(distinguishedName=" + group + ")", 
                SearchScope.Subtree, "memberOf")
            {
                SizeLimit = 1
            };

            if (connection.SendRequest(request) is SearchResponse searchResponse)
            {
                foreach (SearchResultEntry entry in searchResponse.Entries)
                {
                    if (entry.Attributes.Contains("memberOf"))
                    {
                        foreach (object attrVal in entry.Attributes["memberOf"].GetValues(typeof(string)))
                        {
                            string ownerGroup = attrVal == null ? "" : attrVal.ToString();

                            if (!string.IsNullOrEmpty(ownerGroup))
                            {
                                groups.Add(ownerGroup);
                                FindOwnerGroups(connection, ownerGroup, groups);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the list of security groups contains the specified role.
        /// </summary>
        private static bool GroupsContain(List<string> groups, string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode))
            {
                return false;
            }
            else
            {
                string groupPrefix = "CN=" + roleCode;
                return groups.Any(g => g.StartsWith(groupPrefix, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Creates a database connection.
        /// </summary>
        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(ServerContext.InstanceConfig.Connection
                .BuildConnectionString(KnownDBMS.PostgreSQL));
        }

        /// <summary>
        /// Creates database entities if they do not exist.
        /// </summary>
        private void CreateDbEntities()
        {
            try
            {
                using NpgsqlConnection conn = CreateConnection();
                conn.Open();

                NpgsqlCommand cmd1 = new(SqlScript.CreateSchema, conn);
                cmd1.ExecuteNonQuery();

                NpgsqlCommand cmd2 = new(SqlScript.CreateUserTable, conn);
                cmd2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.WriteError(ServerPhrases.ModuleMessage, Code,
                    ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при создании объектов БД" :
                    "Error creating database entities"));
            }
        }

        /// <summary>
        /// Updates user information in the database and sets the user ID in the validation result.
        /// </summary>
        private void UpdateUserInDB(string usernameLower, ref UserValidationResult userValidationResult)
        {
            try
            {
                using NpgsqlConnection conn = CreateConnection();
                conn.Open();

                NpgsqlCommand cmd1 = new(SqlScript.UpdateUser, conn);
                cmd1.Parameters.AddWithValue("username", usernameLower);
                cmd1.Parameters.AddWithValue("roleID", userValidationResult.RoleID);
                cmd1.Parameters.AddWithValue("updateTime", DateTime.UtcNow);
                cmd1.ExecuteNonQuery();

                NpgsqlCommand cmd2 = new(SqlScript.SelectUserID, conn);
                cmd2.Parameters.AddWithValue("username", usernameLower);
                userValidationResult.UserID = (int)cmd2.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string message = Locale.IsRussian ?
                    "Ошибка при обновлении информации о пользователе в БД" :
                    "Error updating user information in database";
                userValidationResult = UserValidationResult.Fail(message);
                Log.WriteError(ServerPhrases.ModuleMessage, Code, ex.BuildErrorMessage(message));
            }
        }

        /// <summary>
        /// Finds a user in the database.
        /// </summary>
        private User FindUserInDB(int userID)
        {
            try
            {
                using NpgsqlConnection conn = CreateConnection();
                conn.Open();

                NpgsqlCommand cmd = new(SqlScript.SelectUserByID, conn);
                cmd.Parameters.AddWithValue("adUserID", userID);
                using NpgsqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.Read())
                {
                    return new User
                    {
                        UserID = userID,
                        Enabled = true,
                        Name = reader.GetString("username"),
                        RoleID = reader.GetInt32("role_id")
                    };
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ServerPhrases.ModuleMessage, Code,
                    ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при поиске пользователя в БД" :
                    "Error searching for user in database"));
            }

            return null;
        }


        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public override void OnServiceStart()
        {
            // load module configuration
            if (!moduleConfig.Load(ServerContext.Storage, ModuleConfig.DefaultFileName, out string errMsg))
                Log.WriteError(ServerPhrases.ModuleMessage, Code, errMsg);

            // get users from the configuration database
            foreach (User user in ServerContext.ConfigDatabase.UserTable)
            {
                if (!string.IsNullOrEmpty(user.Name))
                    users[user.Name.ToLowerInvariant()] = user;
            }

            // create entities in the module database
            CreateDbEntities();
        }

        /// <summary>
        /// Validates the username and password.
        /// </summary>
        public override UserValidationResult ValidateUser(string username, string password)
        {
            try
            {
                using LdapConnection connection = new(
                    new LdapDirectoryIdentifier(moduleConfig.LdapServer),
                    new NetworkCredential(username, password));

                if (ValidateCredentials(connection))
                {
                    string usernameLower = username.ToLowerInvariant();

                    if (users.TryGetValue(usernameLower, out User user))
                    {
                        return ValidateByConfigDatabase(user);
                    }
                    else if (moduleConfig.EnableSearch)
                    {
                        UserValidationResult result = ValidateByActiveDirectory(connection, username);
                        UpdateUserInDB(usernameLower, ref result);
                        return result;
                    }
                    else
                    {
                        return UserValidationResult.Fail(Locale.IsRussian ?
                            "Пользователь не найден в базе конфигурации" :
                            "User not found in the configuration database");
                    }
                }
                else
                {
                    return UserValidationResult.Fail(ServerPhrases.InvalidCredentials);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.BuildErrorMessage(ServerPhrases.ModuleMessage, Code, Locale.IsRussian ?
                    "Ошибка при взаимодействии с Active Directory" :
                    "Error interacting with Active Directory"));
                return UserValidationResult.Empty;
            }
        }

        /// <summary>
        /// Finds an external user by ID.
        /// </summary>
        public override User FindUser(int userID)
        {
            return moduleConfig.EnableSearch ? FindUserInDB(userID) : null;
        }
    }
}
