/*
 * Copyright 2022 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Webstation Application
 * Summary  : Represents a user profile page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public UserProfileModel(IWebContext webContext, IUserContext userContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }

        public IActionResult OnGet(int? id)
        {
            // define user ID
            int currentUserID = userContext.UserEntity.UserID;
            UserID = id ?? currentUserID;
            Data.Entities.User userEntity;

            // find user
            if (UserID == currentUserID)
                userEntity = userContext.UserEntity;
            else if (!userContext.Rights.Full)
                return Forbid();
            else
                userEntity = webContext.ConfigBase.UserTable.GetItem(UserID);

            // get user properties
            if (userEntity == null)
            {
                Username = WebPhrases.UnknownUsername;
                RoleName = "";
            }
            else
            {
                Username = userEntity.Name;
                Data.Entities.Role roleEntity = webContext.ConfigBase.RoleTable.GetItem(userEntity.RoleID);
                RoleName = roleEntity == null ? "" : roleEntity.Name;
            }

            return Page();
        }
    }
}
