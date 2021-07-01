// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a response containing current data without formatting.
    /// </summary>
    public class CurDataLightResponse
    {
        public List<CurDataPoint> Points { get; set; }

        public int CnlListID { get; set; }
    }
}
