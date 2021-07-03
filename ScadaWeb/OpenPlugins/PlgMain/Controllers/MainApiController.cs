// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Log;
using Scada.Web.Api;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMain.Code;
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
        /// <summary>
        /// The cache expiration for archive data.
        /// </summary>
        private static readonly TimeSpan DataCacheExpiration = TimeSpan.FromMilliseconds(500);

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
        /// Requests the current data from the server.
        /// </summary>
        private CurData RequestCurData(IList<int> cnlNums, long cnlListID, bool useCache)
        {
            int cnlCnt = cnlNums == null ? 0 : cnlNums.Count;
            CurDataRecord[] records = new CurDataRecord[cnlCnt];
            CurData curData = new() { Records = records, CnlListID = 0 };

            if (cnlCnt > 0)
            {
                CnlData[] cnlDataArr = cnlListID > 0
                    ? clientAccessor.ScadaClient.GetCurrentData(ref cnlListID)
                    : clientAccessor.ScadaClient.GetCurrentData(cnlNums.ToArray(), useCache, out cnlListID);
                curData.CnlListID = cnlListID;

                for (int i = 0; i < cnlCnt; i++)
                {
                    int cnlNum = cnlNums[i];
                    InCnl inCnl = webContext.BaseDataSet.InCnlTable.GetItem(cnlNum);
                    CnlData cnlData = i < cnlDataArr.Length ? cnlDataArr[i] : CnlData.Empty;

                    records[i] = new CurDataRecord
                    {
                        D = new CurDataPoint(cnlNum, cnlData),
                        Df = webContext.DataFormatter.FormatCnlData(cnlData, inCnl)
                    };
                }
            }

            return curData;
        }

        /// <summary>
        /// Requests historical data from the server.
        /// </summary>
        private HistData RequestHistData(IList<int> cnlNums, DateTime startTime, DateTime endTime, bool endInclusive,
            int archiveBit)
        {
            if (cnlNums == null)
                cnlNums = Array.Empty<int>();

            int cnlCnt = cnlNums.Count;
            HistData.RecordList[] trends = new HistData.RecordList[cnlCnt];

            HistData histData = new()
            {
                CnlNums = cnlNums,
                Timestamps = Array.Empty<long>(),
                Trends = trends
            };

            if (cnlCnt > 0)
            {
                // request trends
                TrendBundle trendBundle = clientAccessor.ScadaClient.GetTrends(cnlNums.ToArray(), 
                    new TimeRange(startTime, endTime, endInclusive), archiveBit);

                // copy timestamps
                int pointCount = trendBundle.Timestamps.Count;
                long[] timestamps = new long[pointCount];
                histData.Timestamps = timestamps;

                for (int i = 0; i < pointCount; i++)
                {
                    timestamps[i] = new DateTimeOffset(trendBundle.Timestamps[i]).ToUnixTimeMilliseconds();
                }

                // copy channel data
                for (int i = 0; i < cnlCnt; i++)
                {
                    int cnlNum = cnlNums[i];
                    InCnl inCnl = webContext.BaseDataSet.InCnlTable.GetItem(cnlNum);
                    HistData.RecordList records = trends[i] = new(pointCount);
                    TrendBundle.CnlDataList cnlDataList = trendBundle.Trends[i];

                    for (int j = 0; j < pointCount; j++)
                    {
                        CnlData cnlData = cnlDataList[j];
                        records[i] = new HistDataRecord
                        {
                            D = cnlData,
                            Df = webContext.DataFormatter.FormatCnlData(cnlData, inCnl)
                        };
                    }
                }
            }

            return histData;
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
        public Dto<CurData> GetCurDataFormatted(IdList cnlNums, bool useCache)
        {
            try
            {
                // request data
                CheckAccessRights(cnlNums);
                CurData curData = RequestCurData(cnlNums, 0, useCache);

                // write channel list to the cache
                if (useCache && curData.CnlListID >= 0)
                {
                    memoryCache.Set(
                        PluginUtils.GetCacheKey("CnlList", curData.CnlListID),
                        cnlNums,
                        new MemoryCacheEntryOptions { SlidingExpiration = WebUtils.CacheExpiration });
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
        /// Gets the formatted current data by the channel list ID.
        /// </summary>
        public Dto<CurData> GetCurDataFormatted2(long cnlListID)
        {
            try
            {
                IdList cnlNums = memoryCache.Get<IdList>(PluginUtils.GetCacheKey("CnlList", cnlListID));
                CurData curData = RequestCurData(cnlNums, cnlListID, true);
                return Dto<CurData>.Success(curData);
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
                    CurData curData = memoryCache.GetOrCreate(PluginUtils.GetCacheKey("CurData", viewID), entry =>
                    {
                        entry.SetAbsoluteExpiration(DataCacheExpiration);
                        return RequestCurData(view.CnlNumList, cnlListID, true);
                    });

                    return Dto<CurData>.Success(curData);
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
        public Dto<HistData> GetHistData(IdList cnlNums, DateTime startTime, DateTime endTime, bool endInclusive, 
            int archiveBit)
        {
            try
            {
                CheckAccessRights(cnlNums);
                HistData histData = RequestHistData(cnlNums, startTime, endTime, endInclusive, archiveBit);
                return Dto<HistData>.Success(histData);
            }
            catch (AccessDeniedException ex)
            {
                return Dto<HistData>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetHistData));
                return Dto<HistData>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets the historical data by view.
        /// </summary>
        public Dto<HistData> GetHistDataByView(int viewID, DateTime startTime, DateTime endTime, bool endInclusive, 
            int archiveBit)
        {
            try
            {
                if (viewLoader.GetViewFromCache(viewID, out BaseView view, out string errMsg))
                {
                    HistData histData = memoryCache.GetOrCreate(PluginUtils.GetCacheKey("HistData", viewID), entry =>
                    {
                        entry.SetAbsoluteExpiration(DataCacheExpiration);
                        return RequestHistData(view.CnlNumList, startTime, endTime, endInclusive, archiveBit);
                    });

                    return Dto<HistData>.Success(histData);
                }
                else
                {
                    return Dto<HistData>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetHistDataByView));
                return Dto<HistData>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets the Unix time when the archive was last written to.
        /// </summary>
        public Dto<long> GetArcWriteTime(int archiveBit)
        {
            try
            {
                DateTime dt = memoryCache.GetOrCreate(PluginUtils.GetCacheKey("ArcWriteTime", archiveBit), entry =>
                {
                    entry.SetAbsoluteExpiration(DataCacheExpiration);
                    return clientAccessor.ScadaClient.GetLastWriteTime(archiveBit);
                });

                return Dto<long>.Success(new DateTimeOffset(dt).ToUnixTimeMilliseconds());
            }
            catch (Exception ex)
            {
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetArcWriteTime));
                return Dto<long>.Fail(ex.Message);
            }
        }
    }
}
