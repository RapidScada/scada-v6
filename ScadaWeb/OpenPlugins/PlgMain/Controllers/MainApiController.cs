// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Web.Api;
using Scada.Web.Authorization;
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
            if (cnlNums == null || userContext.Rights.ViewAll)
                return;

            foreach (int cnlNum in cnlNums)
            {
                InCnl inCnl = webContext.BaseDataSet.InCnlTable.GetItem(cnlNum);

                if (inCnl == null || inCnl.ObjNum == null)
                    throw new AccessDeniedException(); // no rights on undefined channel or object

                if (!userContext.Rights.GetRightByObj(inCnl.ObjNum.Value).View)
                    throw new AccessDeniedException();
            }
        }

        /// <summary>
        /// Checks if the user can view all data, otherwise throws an exception.
        /// </summary>
        private void RequireViewAll()
        {
            if (!userContext.Rights.ViewAll)
                throw new AccessDeniedException();
        }

        /// <summary>
        /// Converts the specified timestamp to UTC depending on its kind.
        /// </summary>
        private DateTime ConvertTimeToUtc(DateTime timestamp)
        {
            return timestamp.Kind == DateTimeKind.Utc
                ? timestamp
                : userContext.ConvertTimeToUtc(timestamp);
        }

        /// <summary>
        /// Creates a time range with UTC timestamps.
        /// </summary>
        private TimeRange CreateTimeRange(DateTime startTime, DateTime endTime, bool endInclusive)
        {
            return new TimeRange(ConvertTimeToUtc(startTime), ConvertTimeToUtc(endTime), endInclusive);
        }

        /// <summary>
        /// Creates a time range with the specified number of days ending in the current time.
        /// </summary>
        private static TimeRange CreateTimeRange(int period)
        {
            DateTime utcNow = DateTime.UtcNow;
            return new TimeRange(utcNow.AddDays(-period), utcNow, true);
        }

        /// <summary>
        /// Gets the filter to request events by view.
        /// </summary>
        private static DataFilter GetEventFilter(BaseView view, int limit)
        {
            DataFilter dataFilter = new(typeof(Event)) { Limit = limit };
            // TODO: OR operator

            foreach (int cnlNum in view.CnlNumList)
            {
                dataFilter.AddCondition("CnlNum", FilterOperator.Equals, cnlNum);
            }

            foreach (int outCnlNum in view.OutCnlNumList)
            {
                dataFilter.AddCondition("OutCnlNum", FilterOperator.Equals, outCnlNum);
            }

            return dataFilter;
        }

        /// <summary>
        /// Gets the filter to request limited number of events.
        /// </summary>
        private static DataFilter GetEventFilter(int limit)
        {
            return new DataFilter(typeof(Event)) { Limit = limit };
        }

        /// <summary>
        /// Requests the current data from the server.
        /// </summary>
        private CurData RequestCurData(IList<int> cnlNums, long cnlListID, bool useCache)
        {
            int cnlCnt = cnlNums == null ? 0 : cnlNums.Count;
            CurDataRecord[] records = new CurDataRecord[cnlCnt];
            CurData curData = new() 
            { 
                ServerTime = TimeRecord.Create(DateTime.UtcNow, userContext.TimeZone),
                Records = records, 
                CnlListID = 0 
            };

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
        private HistData RequestHistData(IList<int> cnlNums, TimeRange timeRange, int archiveBit)
        {
            if (cnlNums == null)
                cnlNums = Array.Empty<int>();

            int cnlCnt = cnlNums.Count;
            HistData.RecordList[] trends = new HistData.RecordList[cnlCnt];

            HistData histData = new()
            {
                CnlNums = cnlNums,
                Timestamps = Array.Empty<TimeRecord>(),
                Trends = trends
            };

            if (cnlCnt > 0)
            {
                // request trends
                TrendBundle trendBundle = clientAccessor.ScadaClient.GetTrends(
                    cnlNums.ToArray(), timeRange, archiveBit);

                // copy timestamps
                int pointCount = trendBundle.Timestamps.Count;
                TimeRecord[] timestamps = new TimeRecord[pointCount];
                histData.Timestamps = timestamps;

                for (int i = 0; i < pointCount; i++)
                {
                    timestamps[i] = TimeRecord.Create(trendBundle.Timestamps[i], userContext.TimeZone);
                }

                // copy channel data
                for (int cnlIdx = 0; cnlIdx < cnlCnt; cnlIdx++)
                {
                    int cnlNum = cnlNums[cnlIdx];
                    InCnl inCnl = webContext.BaseDataSet.InCnlTable.GetItem(cnlNum);
                    HistData.RecordList records = trends[cnlIdx] = new(pointCount);
                    TrendBundle.CnlDataList cnlDataList = trendBundle.Trends[cnlIdx];

                    for (int ptIdx = 0; ptIdx < pointCount; ptIdx++)
                    {
                        CnlData cnlData = cnlDataList[ptIdx];
                        records.Add(new HistDataRecord
                        {
                            D = cnlData,
                            Df = webContext.DataFormatter.FormatCnlData(cnlData, inCnl)
                        });
                    }
                }
            }

            return histData;
        }

        /// <summary>
        /// Requests events from the server.
        /// </summary>
        private EventPacket RequestEvents(DataFilter filter, long filterID, bool useCache,
            TimeRange timeRange, int archiveBit)
        {
            List<Event> events = filterID > 0
                ? clientAccessor.ScadaClient.GetEvents(timeRange, ref filterID, archiveBit)
                : clientAccessor.ScadaClient.GetEvents(timeRange, filter, archiveBit, useCache, out filterID);

            int eventCnt = events.Count;
            EventRecord[] records = new EventRecord[eventCnt];

            for (int i = 0; i < eventCnt; i++)
            {
                records[i] = new EventRecord();
            }

            return new EventPacket
            {
                Records = records,
                FilterID = filterID
            };
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
        /// Gets the current data of the specified channels.
        /// </summary>
        public Dto<CurData> GetCurDataStep1(IdList cnlNums, bool useCache)
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
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetCurDataStep1));
                return Dto<CurData>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets the current data by the channel list ID returned in step 1.
        /// </summary>
        public Dto<CurData> GetCurDataStep2(long cnlListID)
        {
            try
            {
                IdList cnlNums = memoryCache.Get<IdList>(PluginUtils.GetCacheKey("CnlList", cnlListID));
                CurData curData = RequestCurData(cnlNums, cnlListID, true);
                return Dto<CurData>.Success(curData);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetCurDataStep2));
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
                HistData histData = RequestHistData(cnlNums,
                    CreateTimeRange(startTime, endTime, endInclusive), archiveBit);
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
                    TimeRange timeRange = CreateTimeRange(startTime, endTime, endInclusive);
                    HistData histData = memoryCache.GetOrCreate(
                        PluginUtils.GetCacheKey("HistDataByView", viewID, timeRange.Key, archiveBit),
                        entry =>
                        {
                            entry.SetAbsoluteExpiration(DataCacheExpiration);
                            return RequestHistData(view.CnlNumList, timeRange, archiveBit);
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
        /// Gets all events for the period.
        /// </summary>
        public Dto<EventPacket> GetEvents(DateTime startTime, DateTime endTime, bool endInclusive, int archiveBit)
        {
            try
            {
                RequireViewAll();
                TimeRange timeRange = CreateTimeRange(startTime, endTime, endInclusive);
                EventPacket eventPacket = memoryCache.GetOrCreate(
                    PluginUtils.GetCacheKey("Events", timeRange.Key, archiveBit),
                    entry =>
                    {
                        entry.SetAbsoluteExpiration(DataCacheExpiration);
                        return RequestEvents(null, 0, false, timeRange, archiveBit);
                    });

                return Dto<EventPacket>.Success(eventPacket);
            }
            catch (AccessDeniedException ex)
            {
                return Dto<EventPacket>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetEvents));
                return Dto<EventPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets the last events.
        /// </summary>
        public Dto<EventPacket> GetLastEvents(int limit, int period, int archiveBit)
        {
            try
            {
                RequireViewAll();
                EventPacket eventPacket = memoryCache.GetOrCreate(
                    PluginUtils.GetCacheKey("LastEvents", limit, period, archiveBit),
                    entry =>
                    {
                        entry.SetAbsoluteExpiration(DataCacheExpiration);
                        return RequestEvents(GetEventFilter(limit), 0, false, CreateTimeRange(period), archiveBit);
                    });

                return Dto<EventPacket>.Success(eventPacket);
            }
            catch (AccessDeniedException ex)
            {
                return Dto<EventPacket>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetEvents));
                return Dto<EventPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets the last events by view.
        /// </summary>
        public Dto<EventPacket> GetLastEventsByView(int viewID, int limit, long filterID, int period, int archiveBit)
        {
            try
            {
                if (viewLoader.GetViewFromCache(viewID, out BaseView view, out string errMsg))
                {
                    EventPacket eventPacket = memoryCache.GetOrCreate(
                        PluginUtils.GetCacheKey("LastEventsByView", viewID, limit, period),
                        entry =>
                        {
                            entry.SetAbsoluteExpiration(DataCacheExpiration);
                            DataFilter filter = filterID > 0 ? null : GetEventFilter(view, limit);
                            return RequestEvents(filter, filterID, true, CreateTimeRange(period), archiveBit);
                        });

                    return Dto<EventPacket>.Success(eventPacket);
                }
                else
                {
                    return Dto<EventPacket>.Fail(errMsg);
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteException(ex, WebPhrases.ErrorInWebApi, nameof(GetLastEventsByView));
                return Dto<EventPacket>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets the Unix time in milliseconds when the archive was last written to.
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
