// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Log;
using Scada.Web.Api;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMain.Models;
using Scada.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scada.Web.Plugins.PlgMain.Controllers
{
    /// <summary>
    /// Represents the plugin's web API.
    /// <para>Представляет веб API плагина.</para>
    /// </summary>
    [ApiController]
    [Route("Api/Main/[action]")]
    public class MainApiController : ControllerBase
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IClientAccessor clientAccessor;
        private readonly IViewLoader viewLoader;
        private readonly IMemoryCache memoryCache;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MainApiController(IWebContext webContext, IUserContext userContext, 
            IClientAccessor clientAccessor, IViewLoader viewLoader, IMemoryCache memoryCache)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.clientAccessor = clientAccessor;
            this.viewLoader = viewLoader;
            this.memoryCache = memoryCache;
        }


        /// <summary>
        /// Checks user permissions and throws an exception if access is denied.
        /// </summary>
        private void CheckAccessRights(IdList cnlNums)
        {
            if (cnlNums == null || userContext.Rights.Full)
                return;

            foreach (int cnlNum in cnlNums)
            {
                InCnl inCnl = webContext.BaseDataSet.InCnlTable.GetItem(cnlNum);

                if (inCnl != null)
                {
                    // no rights on undefined object
                    if (inCnl.ObjNum == null)
                        throw new AccessDeniedException();

                    if (!userContext.Rights.GetRightByObj(inCnl.ObjNum.Value).View)
                        throw new AccessDeniedException();
                }
            }
        }


        /// <summary>
        /// Gets the current data without formatting.
        /// </summary>
        public Dto<IEnumerable<CurDataPoint>> GetCurData(IdList cnlNums)
        {
            try
            {
                CheckAccessRights(cnlNums);
                int cnlCnt = cnlNums == null ? 0 : cnlNums.Count;
                CurDataPoint[] dataPoints = new CurDataPoint[cnlCnt];

                if (cnlCnt > 0)
                {
                    CnlData[] cnlData = clientAccessor.ScadaClient.GetCurrentData(cnlNums.ToArray(), false, out _);

                    for (int i = 0, cnt = cnlNums.Count; i < cnt; i++)
                    {
                        dataPoints[i] = new CurDataPoint(cnlNums[i], cnlData[i]);
                    }
                }

                return Dto<IEnumerable<CurDataPoint>>.Success(dataPoints);
            }
            catch (AccessDeniedException ex)
            {
                return Dto<IEnumerable<CurDataPoint>>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetCurData));
                return Dto<IEnumerable<CurDataPoint>>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets the formatted current data.
        /// </summary>
        public Dto<CurData> GetCurDataFormatted(IdList cnlNums, long cnlListID)
        {
            try
            {
                CheckAccessRights(cnlNums);
                int cnlCnt = cnlNums == null ? 0 : cnlNums.Count;
                CurDataRecord[] records = new CurDataRecord[cnlCnt];
                CurData curData = new() { Records = records, CnlListID = 0 };

                if (cnlCnt > 0)
                {
                    CnlData[] cnlDataArr = cnlListID > 0
                        ? clientAccessor.ScadaClient.GetCurrentData(ref cnlListID)
                        : clientAccessor.ScadaClient.GetCurrentData(cnlNums.ToArray(), true, out cnlListID);
                    curData.CnlListID = cnlListID;

                    for (int i = 0, cnt = cnlNums.Count; i < cnt; i++)
                    {
                        int cnlNum = cnlNums[i];
                        CnlData cnlData = cnlDataArr[i];
                        InCnl inCnl = webContext.BaseDataSet.InCnlTable.GetItem(cnlNum);

                        records[i] = new CurDataRecord 
                        {
                            Pt = new CurDataPoint(cnlNum, cnlData),
                            Fd = webContext.DataFormatter.FormatCnlData(cnlData, inCnl)
                        };
                    }
                }

                return Dto<CurData>.Success(curData);
            }
            catch (AccessDeniedException ex)
            {
                return Dto<CurData>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetCurDataFormatted));
                return Dto<CurData>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets the current data by view.
        /// </summary>
        public Dto<CurData> GetCurDataByView(int viewID, long cnlListID)
        {
            try
            {
                if (viewLoader.GetViewFromCache(viewID, out BaseView view, out string errMsg))
                {
                    return null;
                }
                else
                {
                    return Dto<CurData>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetCurDataByView));
                return Dto<CurData>.Fail(ex.Message);
            }
        }


        /// <summary>
        /// Gets the historical data.
        /// </summary>
        public Dto<HistData> GetHistData(IdList cnlNums, DateTime startTime, DateTime endTime, bool endInclusive, int archiveBit)
        {
            return null;
        }

        /// <summary>
        /// Gets the historical data by view.
        /// </summary>
        public Dto<HistData> GetHistDataByView(int viewID, DateTime startTime, DateTime endTime, bool endInclusive, int archiveBit)
        {
            return null;
        }

        /// <summary>
        /// Gets the Unix time when the archive was last written to.
        /// </summary>
        public Dto<long> GetArcWriteTime(int archiveBit)
        {
            return Dto<long>.Success(0);
        }
    }
}
