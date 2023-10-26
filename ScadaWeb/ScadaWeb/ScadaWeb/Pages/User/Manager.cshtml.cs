// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Const;
using Scada.Web.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Represents an about page.
    /// </summary>
    public class UserManagerModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        public Dictionary<int, string> RoleMapperDic = new Dictionary<int, string>();
        public string RoleMapper = string.Empty;
        public UserManagerModel(IWebContext webContext,IUserContext userContext)
        {
            this.userContext = userContext;
            this.webContext = webContext;
        }

        public IActionResult OnGet()
        {
            if(userContext.UserEntity.RoleID != RoleID.Administrator)
            {
                return RedirectToPage(WebPath.AccessDeniedPage);
            }
            foreach (var roleItem in this.webContext.ConfigDatabase.RoleTable.Items)
            {
                if (roleItem.Key == 1) this.RoleMapperDic.Add(1, "Admin");
                else this.RoleMapperDic.Add(roleItem.Key, roleItem.Value.Name);
            }
            this.RoleMapper = JsonSerializer.Serialize(this.RoleMapperDic);

            return Page();
        }
    }
}
