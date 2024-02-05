﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Protocol;
using Scada.Web.Audit;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;
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
        private readonly IAuditLog auditLog;
        private readonly IClientAccessor clientAccessor;
        private readonly PluginContext pluginContext;
        private readonly dynamic dict;


        public CommandModel(IWebContext webContext, IUserContext userContext, IAuditLog auditLog,
            IClientAccessor clientAccessor, PluginContext pluginContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.auditLog = auditLog;
            this.clientAccessor = clientAccessor;
            this.pluginContext = pluginContext;
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Areas.Main.Pages.Command");
        }


        public bool HasError { get; private set; } = false;
        public string Message { get; private set; } = "";
        public Cnl Cnl { get; private set; } = null;
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


        private Cnl GetCnl(int cnlNum, out Right right)
        {
            if (!webContext.AppConfig.GeneralOptions.EnableCommands)
            {
                Message = WebPhrases.CommandsDisabled;
            }
            else if (webContext.ConfigDatabase.CnlTable.GetItem(cnlNum) is not Cnl cnl)
            {
                Message = string.Format(WebPhrases.CnlNotFound, cnlNum);
            }
            else if (!cnl.IsOutput())
            {
                Message = string.Format(WebPhrases.CnlNotOutput, cnlNum);
            }
            else
            {
                right = userContext.Rights.GetRightByObj(cnl.ObjNum);
                return cnl;
            }

            HasError = true;
            right = Right.Empty;
            return null;
        }

        private bool CheckPassword()
        {
            if (!pluginContext.Options.CommandPassword)
                return true;

            try
            {
                UserValidationResult result = clientAccessor.ScadaClient.ValidateUser(User.GetUsername(), Password);

                if (result.IsValid)
                {
                    return true;
                }
                else
                {
                    HasError = true;
                    Message = result.ErrorMessage;
                    PwdIsInvalid = true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                Message = WebPhrases.ClientError;
                webContext.Log.WriteError(ex, Message);
                return false;
            }
        }

        private bool CreateCommand(int cnlNum, out TeleCommand command)
        {
            command = new TeleCommand
            {
                UserID = User.GetUserID(),
                CnlNum = cnlNum,
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
                webContext.Log.WriteAction(WebPhrases.SendCommand, command.CnlNum, User.GetUsername());
                CommandResult result = clientAccessor.ScadaClient.SendCommand(command, WriteCommandFlags.Default);

                if (result.IsSuccessful)
                {
                    Message = dict.CommandSent;
                    CloseModal = true;
                    Cnl = null; // hide fields
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
            finally
            {
                auditLog.Write(new AuditLogEntry(userContext.UserEntity)
                {
                    ActionType = AuditActionType.SendCommand,
                    ActionArgs = AuditActionArgs.FromObject(new { command.CnlNum, command.CmdVal, command.CmdData }),
                    ActionResult = AuditActionResult.FromBool(!HasError),
                    Severity = Severity.Minor,
                    Message = Message
                });
            }
        }

        private IActionResult OnLoad(int cnlNum, bool isPostback)
        {
            Cnl = GetCnl(cnlNum, out Right right);

            if (Cnl == null)
                return Page();

            if (!right.Control)
                return Forbid();

            if (Cnl.ObjNum != null)
                Obj = webContext.ConfigDatabase.ObjTable.GetItem(Cnl.ObjNum.Value);

            if (Cnl.DeviceNum != null)
                Device = webContext.ConfigDatabase.DeviceTable.GetItem(Cnl.DeviceNum.Value);

            int? formatID = Cnl.OutFormatID ?? Cnl.FormatID;

            if (formatID != null)
            {
                Format = webContext.ConfigDatabase.FormatTable.GetItem(formatID.Value);

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
                CreateCommand(cnlNum, out TeleCommand command))
            {
                SendCommand(command);
            }

            return Page();
        }

        public IActionResult OnGet(int cnlNum)
        {
            return OnLoad(cnlNum, false);
        }

        public IActionResult OnPost(int cnlNum)
        {
            return OnLoad(cnlNum, true);
        }
    }
}
