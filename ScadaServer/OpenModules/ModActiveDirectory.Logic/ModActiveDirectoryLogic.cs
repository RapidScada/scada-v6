// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
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
        /// <summary>
        /// The identifier shared between Active Directory users as a temporary solution.
        /// </summary>
        private const int AdUserID = 1001;

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
                        UserValidationResult result = new()
                        {
                            UserID = AdUserID,
                            RoleID = role.RoleID
                        };

                        if (role.RoleID > RoleID.Disabled)
                        {
                            result.IsValid = true;
                            return result;
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
                    users[user.Name.ToLowerInvariant()] = user;
            }
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
                    if (users.TryGetValue(username.ToLowerInvariant(), out User user))
                    {
                        return ValidateByConfigDatabase(user);
                    }
                    else if (moduleConfig.EnableSearch)
                    {
                        return ValidateByActiveDirectory(connection, username);
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
            return moduleConfig.EnableSearch && userID == AdUserID
                ? new User
                {
                    UserID = userID,
                    Enabled = false,
                    Name = "Active Directory user",
                    RoleID = RoleID.Disabled
                }
                : null;
        }
    }
}
