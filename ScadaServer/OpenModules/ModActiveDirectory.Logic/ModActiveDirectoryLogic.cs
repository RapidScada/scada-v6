// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
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


        /*
        /// <summary>
        /// Finds the security groups in Active Directory that the specified group belongs to and adds them to the list.
        /// </summary>
        private static void FindOwnerGroups(DirectoryEntry entry, string group, List<string> groups)
        {
            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(distinguishedName=" + group + ")";
            search.PropertiesToLoad.Add("memberOf");
            SearchResult searchRes = search.FindOne();

            if (searchRes != null)
            {
                foreach (object result in searchRes.Properties["memberOf"])
                {
                    string gr = result.ToString();
                    groups.Add(gr);
                    FindOwnerGroups(entry, gr, groups);
                }
            }
        }
        */
        /// <summary>
        /// Checks if the list of security groups contains the specified user role.
        /// </summary>
        private static bool GroupsContain(List<string> groups, string roleName)
        {
            roleName = "CN=" + roleName;

            foreach (string group in groups)
            {
                if (group.StartsWith(roleName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
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
            // https://github.com/dotnet/runtime/tree/main/src/libraries/System.DirectoryServices/src/System/DirectoryServices
            // https://github.com/dotnet/runtime/issues/36888
            // https://github.com/dotnet/runtime/issues/64900
            // https://learn.microsoft.com/en-us/previous-versions/windows/desktop/dacx/how-to-enumerate-active-directory-definitions-for-user-and-device-claims-and-resource-properties

            try
            {
                Log.WriteLine("!!! ValidateUser");
                using LdapConnection connection = new(
                    new LdapDirectoryIdentifier(moduleConfig.LdapPath),
                    new NetworkCredential(username, password));

                connection.Bind();
                Log.WriteLine("!!! Bind OK");

                SearchRequest request = new("", "(sAMAccountName=" + username + ")", SearchScope.Subtree, "memberOf")
                {
                    SizeLimit = 1
                };

                if (connection.SendRequest(request) is SearchResponse searchResponse)
                {
                    Log.WriteLine("!!! Search OK");

                    foreach (SearchResultEntry entry in searchResponse.Entries)
                    {
                        Log.WriteLine("!!! Entry=" + entry.ToString());

                        if (entry.Attributes.Contains("memberOf"))
                        {
                            foreach (DirectoryAttribute attr in entry.Attributes["memberOf"])
                            {
                                Log.WriteLine("!!! Attr=" + attr.ToString());
                            };
                        }
                    }
                }

                //string filter = $"(|(mail=joao.silva@brunobrito.net)(sAMAccountName={username}))";
                //SearchRequest request = new("DC=brunobrito,DC=net", filter, SearchScope.Subtree, "displayName", "cn", "mail");
                //DirectoryResponse response = connection.SendRequest(request); // response is SearchResponse
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                //Log.WriteError(ServerPhrases.ModuleMessage, Code, ex.Message);
            }

            /*DirectoryEntry entry = null;

            try
            {
                // check password
                bool pwdOK = false;

                if (string.IsNullOrEmpty(password))
                {
                    entry = new DirectoryEntry(moduleConfig.LdapPath);
                    pwdOK = true;
                }
                else
                {
                    entry = new DirectoryEntry(moduleConfig.LdapPath, username, password);

                    // user authentication
                    try
                    {
                        object native = entry.NativeObject;
                        pwdOK = true;
                    }
                    catch { }
                }

                if (pwdOK)
                {
                    if (users.TryGetValue(username.Trim().ToLowerInvariant(), out User user))
                    {
                        roleID = user.RoleID;
                        handled = true;
                        return true;
                    }
                    else
                    {
                        // get user security groups
                        DirectorySearcher search = new DirectorySearcher(entry);
                        search.Filter = "(sAMAccountName=" + username + ")";
                        search.PropertiesToLoad.Add("memberOf");
                        SearchResult searchRes = search.FindOne();

                        if (searchRes != null)
                        {
                            List<string> groups = new List<string>();
                            foreach (object result in searchRes.Properties["memberOf"])
                            {
                                string group = result.ToString();
                                groups.Add(group);
                                FindOwnerGroups(entry, group, groups);
                            }

                            // define user role
                            if (GroupsContain(groups, "ScadaDisabled"))
                                roleID = BaseValues.Roles.Disabled;
                            else if (GroupsContain(groups, "ScadaGuest"))
                                roleID = BaseValues.Roles.Guest;
                            else if (GroupsContain(groups, "ScadaDispatcher"))
                                roleID = BaseValues.Roles.Dispatcher;
                            else if (GroupsContain(groups, "ScadaAdmin"))
                                roleID = BaseValues.Roles.Admin;
                            else if (GroupsContain(groups, "ScadaApp"))
                                roleID = BaseValues.Roles.App;
                            else
                                roleID = BaseValues.Roles.Err;

                            // return successful result
                            if (roleID != BaseValues.Roles.Err)
                            {
                                handled = true;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0}. Ошибка при работе с Active Directory: {1}" :
                    "{0}. Error working with Active Directory: {1}", Name, ex.Message),
                    Log.ActTypes.Exception);
            }
            finally
            {
                entry?.Close();
            }*/

            return base.ValidateUser(username, password, out userID, out roleID, out errMsg, out handled);
        }
    }
}
