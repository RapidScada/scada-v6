﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Lang;
using Scada.Web.Lang;
using Scada.Web.Services;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Represents a user profile page.
    /// <para>Представляет страницу профиля пользователя.</para>
    /// </summary>
    public class UserProfileModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IClientAccessor clientAccessor;

        public UserProfileModel(IWebContext webContext, IUserContext userContext, IClientAccessor clientAccessor)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.clientAccessor = clientAccessor;
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public string TimeZone { get; set; }

        public IActionResult OnGet(int? id)
        {
            // define user ID
            int currentUserID = userContext.UserEntity.UserID;
            UserID = id ?? currentUserID;
            Data.Entities.User userEntity;

            // find user
            if (UserID == currentUserID)
            {
                userEntity = userContext.UserEntity;
            }
            else if (!userContext.Rights.Full)
            {
                return Forbid();
            }
            else
            {
                userEntity = webContext.ConfigDatabase.UserTable.GetItem(UserID) ??
                    clientAccessor.ScadaClient.GetUserByID(UserID);
            }

            // get user properties
            if (userEntity == null)
            {
                Username = WebPhrases.UnknownUsername;
                RoleName = "";
                TimeZone = "";
            }
            else
            {
                Username = userEntity.Name;
                Data.Entities.Role roleEntity = webContext.ConfigDatabase.RoleTable.GetItem(userEntity.RoleID);
                RoleName = roleEntity == null ? "" : roleEntity.Name;
                TimeZone = UserID == currentUserID ? userContext.TimeZone.DisplayName : CommonPhrases.UndefinedSign;
            }

            return Page();
        }
    }
}
