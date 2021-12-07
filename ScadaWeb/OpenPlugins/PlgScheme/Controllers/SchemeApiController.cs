// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Scada.Web.Api;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgScheme.Models;
using Scada.Web.Services;
using System;

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
        private readonly IViewLoader viewLoader;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SchemeApiController(IWebContext webContext, IViewLoader viewLoader)
        {
            this.webContext = webContext;
            this.viewLoader = viewLoader;
        }


        /// <summary>
        /// Получить свойства документа схемы.
        /// </summary>
        public Dto<DocumentPacket> GetSchemeDoc(int viewID, long viewStamp)
        {
            try
            {
                return viewLoader.GetView(viewID, out SchemeView schemeView, out string errMsg)
                    ? Dto<DocumentPacket>.Success(new DocumentPacket(schemeView))
                    : Dto<DocumentPacket>.Fail(errMsg);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex, WebPhrases.ErrorInWebApi, nameof(GetSchemeDoc));
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
                webContext.Log.WriteError(ex, WebPhrases.ErrorInWebApi, nameof(GetComponents));
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
                webContext.Log.WriteError(ex, WebPhrases.ErrorInWebApi, nameof(GetImages));
                return Dto<ImagePacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Получить ошибки при загрузке схемы.
        /// </summary>
        public Dto<ErrorPacket> GetLoadErrors(int viewID, long viewStamp)
        {
            try
            {
                if (viewLoader.GetView(viewID, out SchemeView schemeView, out string errMsg))
                {
                    return Dto<ErrorPacket>.Success(new ErrorPacket(schemeView));
                }
                else
                {
                    return Dto<ErrorPacket>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex, WebPhrases.ErrorInWebApi, nameof(GetLoadErrors));
                return Dto<ErrorPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Отправить команду ТУ со схемы.
        /// </summary>
        public Dto<bool> SendCommand(int ctrlCnlNum, double cmdVal, int viewID, int componentID)
        {
            return Dto<bool>.Fail("Not implemented.");
        }
    }
}
