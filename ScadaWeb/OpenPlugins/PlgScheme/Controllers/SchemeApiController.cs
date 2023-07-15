// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Protocol;
using Scada.Web.Api;
using Scada.Web.Audit;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.DataTypes;
using Scada.Web.Plugins.PlgScheme.Models;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgScheme.Controllers
{
    /// <summary>
    /// Represents a web API for accessing schemes.
    /// <para>Представляет веб API для доступа к схемам.</para>
    /// </summary>
    [ApiController]
    [Route("Api/Scheme/[action]")]
    public class SchemeApiController : ControllerBase
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IAuditLog auditLog;
        private readonly IClientAccessor clientAccessor;
        private readonly IViewLoader viewLoader;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SchemeApiController(IWebContext webContext, IUserContext userContext, IAuditLog auditLog,
            IClientAccessor clientAccessor, IViewLoader viewLoader)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.auditLog = auditLog;
            this.clientAccessor = clientAccessor;
            this.viewLoader = viewLoader;
        }


        /// <summary>
        /// Получить свойства документа схемы.
        /// </summary>
        public Dto<DocumentPacket> GetSchemeDoc(int viewID, long viewStamp)
        {
            try
            {
                if (viewLoader.GetView(viewID, true, out SchemeView schemeView, out string errMsg))
                {
                    DocumentPacket documentPacket = new(schemeView);
                    documentPacket.FillCnlProps(webContext.ConfigDatabase);
                    return Dto<DocumentPacket>.Success(documentPacket);
                }
                else
                {
                    return Dto<DocumentPacket>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetSchemeDoc)));
                return Dto<DocumentPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Получить компоненты схемы.
        /// </summary>
        public Dto<ComponentPacket> GetComponents(int viewID, long viewStamp, int startIndex, int count)
        {
            try
            {
                if (viewLoader.GetView(viewID, out SchemeView schemeView, out string errMsg))
                {
                    ComponentPacket componentsPacket = new(schemeView.ViewStamp, count);
                    componentsPacket.CopyComponents(schemeView.Components.Values, startIndex, count);
                    return Dto<ComponentPacket>.Success(componentsPacket);
                }
                else
                {
                    return Dto<ComponentPacket>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetComponents)));
                return Dto<ComponentPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Получить изображения схемы.
        /// </summary>
        public Dto<ImagePacket> GetImages(int viewID, long viewStamp, int startIndex, int totalDataSize)
        {
            try
            {
                if (viewLoader.GetView(viewID, out SchemeView schemeView, out string errMsg))
                {
                    ImagePacket imagePacket = new(schemeView.ViewStamp);
                    imagePacket.CopyImages(schemeView.SchemeDoc.Images.Values, startIndex, totalDataSize);
                    return Dto<ImagePacket>.Success(imagePacket);
                }
                else
                {
                    return Dto<ImagePacket>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(GetImages)));
                return Dto<ImagePacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Отправить команду ТУ со схемы.
        /// </summary>
        public Dto<bool> SendCommand(int ctrlCnlNum, double cmdVal, int viewID, int componentID)
        {
            bool success = false;
            string message = "";

            try
            {
                if (viewLoader.GetView(viewID, out SchemeView schemeView, out message))
                {
                    if (!webContext.AppConfig.GeneralOptions.EnableCommands)
                    {
                        message = WebPhrases.CommandsDisabled;
                    }
                    else if (webContext.ConfigDatabase.CnlTable.GetItem(ctrlCnlNum) is not Cnl cnl)
                    {
                        message = string.Format(WebPhrases.CnlNotFound, ctrlCnlNum);
                    }
                    else if (!cnl.IsOutput())
                    {
                        message = string.Format(WebPhrases.CnlNotOutput, ctrlCnlNum);
                    }
                    else if (!(userContext.Rights.GetRightByObj(cnl.ObjNum).Control &&
                        userContext.Rights.GetRightByView(schemeView.ViewEntity).Control &&
                        schemeView.Components.TryGetValue(componentID, out ComponentBase component) &&
                        component is IDynamicComponent dynamicComponent &&
                        dynamicComponent.Action == Actions.SendCommandNow &&
                        dynamicComponent.CtrlCnlNum == ctrlCnlNum))
                    {
                        message = WebPhrases.AccessDenied;
                    }
                    else
                    {
                        webContext.Log.WriteAction(WebPhrases.SendCommand, ctrlCnlNum, User.GetUsername());
                        CommandResult result = clientAccessor.ScadaClient.SendCommand(new TeleCommand
                        {
                            UserID = User.GetUserID(),
                            CnlNum = ctrlCnlNum,
                            CmdVal = cmdVal
                        }, WriteCommandFlags.Default);

                        success = result.IsSuccessful;
                        message = result.ErrorMessage;
                    }
                }

                return success ? Dto<bool>.Success(true) : Dto<bool>.Fail(message);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                webContext.Log.WriteError(ex.BuildErrorMessage(WebPhrases.ErrorInWebApi, nameof(SendCommand)));
                return Dto<bool>.Fail(message);
            }
            finally
            {
                auditLog.Write(new AuditLogEntry(userContext.UserEntity)
                {
                    ActionType = AuditActionType.SendCommand,
                    ActionArgs = AuditActionArgs.FromObject(new { CnlNum = ctrlCnlNum, CmdVal = cmdVal }),
                    ActionResult = AuditActionResult.FromBool(success),
                    Severity = Severity.Minor,
                    Message = message
                });
            }
        }
    }
}
