// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Lang;
using Scada.Server.Lang;
using Scada.Server.Modules.ModActiveDirectory.Config;
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
        /// Performs actions when starting the service.
        /// </summary>
        public override void OnServiceStart()
        {
            // load module configuration
            if (!moduleConfig.Load(ServerContext.Storage, ModuleConfig.DefaultFileName, out string errMsg))
                Log.WriteError(ServerPhrases.ModuleMessage, Code, errMsg);

            // get users
            foreach (User user in ServerContext.ConfigDatabase.UserTable)
            {
                if (!string.IsNullOrEmpty(user.Name))
                    users[user.Name.Trim().ToLowerInvariant()] = user;
            }
        }

        /// <summary>
        /// Validates the username and password.
        /// </summary>
        public override bool ValidateUser(string username, string password,
            out int userID, out int roleID, out string errMsg, out bool handled)
        {
            userID = 0;
            roleID = 0;
            errMsg = "";
            handled = false;

            if (users.TryGetValue(username.ToLowerInvariant(), out User user) &&
                !string.IsNullOrEmpty(user.Password))
            {
                return false; // use default validation
            }

            try
            {
                using LdapConnection connection = new(
                    new LdapDirectoryIdentifier(moduleConfig.LdapServer),
                    new NetworkCredential(username, password));

                if (ValidateCredentials(connection))
                {
                    if (user == null)
                    {
                        // user does not exist in the configuration database
                        if (FindUserEntry(connection, username, out SearchResultEntry userEntry))
                        {
                            List<string> userGroups = FindUserGroups(connection, userEntry);

                            foreach (Role role in ServerContext.ConfigDatabase.RoleTable)
                            {
                                if (GroupsContain(userGroups, role.Code))
                                {
                                    if (roleID > RoleID.Disabled)
                                    {
                                        roleID = role.RoleID;
                                        handled = true;
                                        return true;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            errMsg = Locale.IsRussian ?
                                "Пользователь не найден в Active Directory" :
                                "User not found in Active Directory";
                        }
                    }
                    else
                    {
                        // user is found in the configuration database
                        userID = user.UserID;
                        roleID = user.RoleID;

                        if (user.Enabled && roleID > RoleID.Disabled)
                        {
                            errMsg = "";
                            handled = true;
                            return true;
                        }
                    }
                }
                else
                {
                    errMsg = Locale.IsRussian ?
                        "Неверное имя пользователя или пароль" :
                        "Invalid username or password";
                }
            }
            catch (Exception ex)
            {
                errMsg = Locale.IsRussian ?
                    "Ошибка при взаимодействии с Active Directory" :
                    "Error interacting with Active Directory";
                Log.WriteError(ServerPhrases.ModuleMessage, Code, string.Format("{0}: {1}", errMsg, ex.Message));
            }

            if (roleID <= 0 && string.IsNullOrEmpty(errMsg))
            {
                errMsg = Locale.IsRussian ?
                    "Пользователь отключен" :
                    "Account is disabled";
            }

            handled = true;
            return false;
        }
    }
}
