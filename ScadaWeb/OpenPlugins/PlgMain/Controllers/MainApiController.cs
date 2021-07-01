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
        public Dto<CurDataLightResponse> GetCurData(IdList cnlNums)
        {
            List<CurDataPoint> dataPoints = new();

            if (cnlNums != null)
            {
                foreach (int cnlNum in cnlNums)
                {
                    dataPoints.Add(new CurDataPoint(cnlNum));
                }
            }

            return Dto<CurDataLightResponse>.Success(new CurDataLightResponse { Points = dataPoints });
        }

        public Dto<CurDataResponse> GetCurDataFormatted(IdList cnlNums)
        {
            return null;
        }

        public Dto<CurDataResponse> GetCurDataByView(int viewID)
        {
            return null;
        }

        public Dto<HistDataResponse> GetHistData(IdList cnlNums, DateTime startTime, DateTime endTime, bool endInclusive, int archiveBit)
        {
            return null;
        }

        public Dto<HistDataResponse> GetHistDataByView(int viewID, DateTime startTime, DateTime endTime, bool endInclusive, int archiveBit)
        {
            return null;
        }
    }
}
