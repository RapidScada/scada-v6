// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Api;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a response containing formatted current data.
    /// </summary>
    public class CurDataResponse
    {
        public List<CurDataRecord> Records { get; set; }

        public int CnlListID { get; set; }
    }
}
