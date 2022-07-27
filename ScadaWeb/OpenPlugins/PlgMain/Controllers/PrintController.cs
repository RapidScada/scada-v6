// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Scada.Web.Plugins.PlgMain.Controllers
{
    /// <summary>
    /// Represents a controller for printing table views and events.
    /// <para>Представляет контроллер для печати табличных представлений и событий.</para>
    /// </summary>
    [Route("Main/Print/[action]")]
    public class PrintController : Controller
    {
        /// <summary>
        /// Prints the specified table view to an Excel workbook.
        /// </summary>
        public IActionResult PrintTableView(int viewID, DateTime startTime, DateTime endTime)
        {
            string filePath = @"C:\SCADA\ScadaWeb\wwwroot\images\gear.png";
            return PhysicalFile(filePath, "application/octet-stream", Path.GetFileName(filePath));
        }

        /// <summary>
        /// Prints events filtered by the specified view ID to an Excel workbook.
        /// </summary>
        public IActionResult PrintEvents(int viewID)
        {
            string filePath = @"C:\SCADA\ScadaWeb\wwwroot\images\gear.png";
            return PhysicalFile(filePath, "application/octet-stream", Path.GetFileName(filePath));
        }
    }
}
