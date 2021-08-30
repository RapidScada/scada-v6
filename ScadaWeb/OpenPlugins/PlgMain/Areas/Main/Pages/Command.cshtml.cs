// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;
using System;
using System.Globalization;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    /// <summary>
    /// Represents a page for sending a telecontrol command.
    /// <para>Представляет страницу для отправки команды ТУ.</para>
    /// </summary>
    public class CommandModel : PageModel
    {
        public enum InputType { Dec, Hex, Enum, Date, Str }

        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IClientAccessor clientAccessor;
        private readonly PluginContext pluginContext;
        private readonly dynamic dict;


        public CommandModel(IWebContext webContext, IUserContext userContext, IClientAccessor clientAccessor, 
            PluginContext pluginContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.clientAccessor = clientAccessor;
            this.pluginContext = pluginContext;
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Areas.Main.Pages.Command");
        }


        public bool HasError { get; private set; } = false;
        public string Message { get; private set; } = "";
        public OutCnl OutCnl { get; private set; } = null;
        public Obj Obj { get; private set; } = null;
        public Device Device { get; private set; } = null;
        public Format Format { get; private set; } = null;
        public InputType Input { get; private set; } = InputType.Dec;
        public EnumFormat EnumFormat { get; private set; } = null;
        public bool CloseModal { get; private set; } = false;
        public bool PwdIsInvalid { get; private set; } = false;
        public bool CmdIsInvalid { get; private set; } = false;

        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string CmdDec { get; set; }
        [BindProperty]
        public string CmdHex { get; set; }
        [BindProperty]
        public string CmdEnum { get; set; }
        [BindProperty]
        public string CmdDate { get; set; }
        [BindProperty]
        public string CmdData { get; set; }
        [BindProperty]
        public string CmdDataFormat { get; set; }


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

        private bool CheckPassword()
        {
            try
            {
                if (pluginContext.Options.CommandPassword &&
                    !clientAccessor.ScadaClient.ValidateUser(User.GetUsername(), Password,
                        out _, out _, out string errMsg))
                {
                    HasError = true;
                    Message = errMsg;
                    PwdIsInvalid = true;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                HasError = true;
                Message = WebPhrases.ClientError;
                webContext.Log.WriteError(ex, Message);
                return false;
            }
        }

        private bool CreateCommand(int outCnlNum, out TeleCommand command)
        {
            command = new()
            {
                UserID = User.GetUserID(),
                OutCnlNum = outCnlNum,
            };

            try
            {
                switch (Input)
                {
                    case InputType.Dec:
                        command.CmdVal = ScadaUtils.ParseDouble(CmdDec);
                        break;

                    case InputType.Hex:
                        command.CmdVal = int.Parse(CmdHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                        break;

                    case InputType.Enum:
                        command.CmdVal = int.Parse(CmdEnum);
                        break;

                    case InputType.Date:
                        command.CmdData = TeleCommand.StringToCmdData(CmdDate);
                        break;

                    case InputType.Str:
                        command.CmdData = CmdDataFormat == "str"
                            ? TeleCommand.StringToCmdData(CmdData)
                            : ScadaUtils.HexToBytes(CmdData, true, true);
                        break;
                }
            }
            catch
            {
                HasError = true;
                Message = dict.CmdParseError;
                CmdIsInvalid = true;
            }

            return !HasError;
        }

        private void SendCommand(TeleCommand command)
        {
            try
            {
                clientAccessor.ScadaClient.SendCommand(command, out CommandResult result);

                if (result.IsSuccessful)
                {
                    Message = dict.CommandSent;
                    CloseModal = true;
                    OutCnl = null; // hide fields
                }
                else
                {
                    HasError = true;
                    Message = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                Message = WebPhrases.ClientError;
                webContext.Log.WriteError(ex, Message);
            }
        }

        private IActionResult OnLoad(int outCnlNum, bool isPostback)
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

            if (OutCnl.FormatID != null)
            {
                Format = webContext.BaseDataSet.FormatTable.GetItem(OutCnl.FormatID.Value);

                if (Format != null)
                {
                    if (Format.IsEnum)
                    {
                        Input = InputType.Enum;
                        EnumFormat = EnumFormat.Parse(Format.Frmt);
                    }
                    else if (Format.IsDate)
                    {
                        Input = InputType.Date;
                    }
                    else if (Format.IsString)
                    {
                        Input = InputType.Str;
                    }
                    else if (Format.IsNumber && Format.Frmt != null &&
                        (Format.Frmt.StartsWith('x') || Format.Frmt.StartsWith('X')))
                    {
                        Input = InputType.Hex;
                    }
                }
            }

            // validate and send command
            if (isPostback &&
                CheckPassword() &&
                CreateCommand(outCnlNum, out TeleCommand command))
            {
                SendCommand(command);
            }

            return Page();
        }

        public IActionResult OnGet(int outCnlNum)
        {
            return OnLoad(outCnlNum, false);
        }

        public IActionResult OnPost(int outCnlNum)
        {
            return OnLoad(outCnlNum, true);
        }
    }
}
