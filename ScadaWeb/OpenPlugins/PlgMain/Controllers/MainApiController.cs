// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Scada.Data.Models;
using Scada.Web.Api;
using Scada.Web.Plugins.PlgMain.Models;
using System;
using System.Collections.Generic;

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
        public Dto<CurDataLight> GetCurData(IdList cnlNums)
        {
            List<CurDataPoint> dataPoints = new();

            if (cnlNums != null)
            {
                foreach (int cnlNum in cnlNums)
                {
                    dataPoints.Add(new CurDataPoint(cnlNum));
                }
            }

            return Dto<CurDataLight>.Success(new CurDataLight { Points = dataPoints });
        }

        public Dto<CurData> GetCurDataFormatted(IdList cnlNums)
        {
            return null;
        }

        public Dto<CurData> GetCurDataByView(int viewID)
        {
            return null;
        }

        public Dto<HistData> GetHistData(IdList cnlNums, DateTime startTime, DateTime endTime, bool endInclusive, int archiveBit)
        {
            return null;
        }

        public Dto<HistData> GetHistDataByView(int viewID, DateTime startTime, DateTime endTime, bool endInclusive, int archiveBit)
        {
            return null;
        }

        public Dto<DateTime> GetArcWriteTime(int archiveBit)
        {
            return Dto<DateTime>.Success(DateTime.UtcNow);
        }
    }
}
