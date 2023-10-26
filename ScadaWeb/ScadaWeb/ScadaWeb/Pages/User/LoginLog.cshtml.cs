// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Const;
using Scada.Web.Code;
using Scada.Web.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Represents an about page.
    /// </summary>
    public class UserLoginLogModel : PageModel
    {
        public Dictionary<int, string> MapperDic = new Dictionary<int, string>();
        public string UserMapper = string.Empty;

        private readonly IUserContext userContext;
        private readonly IWebContext webContext;
        public UserLoginLogModel(IWebContext webContext, IUserContext userContext)
        {
            this.userContext = userContext;
            this.webContext = webContext;
        }

        public IActionResult OnGet()
        {
            if (userContext.UserEntity.RoleID != RoleID.Administrator)
            {
                return RedirectToPage(WebPath.AccessDeniedPage);
            }

            foreach (var roleItem in this.webContext.ConfigDatabase.UserTable.Items)
            {
                this.MapperDic.Add(roleItem.Key, roleItem.Value.Name);
            }
            this.UserMapper = JsonSerializer.Serialize(this.MapperDic);
            return Page();
        }
    }
}
