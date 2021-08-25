// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a page for sending a telecontrol command.
    /// <para>Представляет страницу для отправки команды ТУ.</para>
    /// </summary>
    public class CommandModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IClientAccessor clientAccessor;
        private readonly dynamic dict;


        public CommandModel(IWebContext webContext, IUserContext userContext, IClientAccessor clientAccessor)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.clientAccessor = clientAccessor;
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Areas.Main.Pages.Command");
        }


        public bool HasError { get; private set; } = false;
        public string Message { get; private set; } = "";
        public OutCnl OutCnl { get; private set; } = null;
        public Obj Obj { get; private set; } = null;
        public Device Device { get; private set; } = null;
        public bool CloseModal { get; private set; } = false;


        private OutCnl GetOutCnl(int outCnlNum, out Right right)
        {
            OutCnl outCnl = webContext.BaseDataSet.OutCnlTable.GetItem(outCnlNum);

            if (outCnl == null)
            {
                HasError = true;
                Message = string.Format(dict.OutCnlNotFound, outCnlNum);
                right = Right.Empty;
                return null;
            }
            else
            {
                right = userContext.Rights.GetRightByObj(outCnl.ObjNum ?? 0);
                return outCnl;
            }
        }

        public IActionResult OnGet(int outCnlNum)
        {
            OutCnl = GetOutCnl(outCnlNum, out Right right);

            if (OutCnl == null)
                return Page();

            if (!right.Control)
                return Forbid();

            if (OutCnl.ObjNum != null)
                Obj = webContext.BaseDataSet.ObjTable.GetItem(OutCnl.ObjNum.Value);

            if (OutCnl.DeviceNum != null)
                Device = webContext.BaseDataSet.DeviceTable.GetItem(OutCnl.DeviceNum.Value);

            return Page();
        }
    }
}
