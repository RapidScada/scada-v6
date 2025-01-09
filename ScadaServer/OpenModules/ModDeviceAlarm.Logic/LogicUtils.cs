// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModDeviceAlarm.Logic
{
    /// <summary>
    /// The class provides helper methods for the module logic.
    /// </summary>
    internal static class LogicUtils
    {
        /// <summary>
        /// The connection status names in English.
        /// </summary>
        private static readonly string[] ConnStatusNamesEn = { "Undefined", "Normal", "Error" };


        /// <summary>
        /// Calculates the next timer firing.
        /// </summary>
        public static DateTime CalcNextTimer(DateTime nowDT, int period)
        {
            return period > 0
                ? nowDT.Date.AddSeconds(((int)nowDT.TimeOfDay.TotalSeconds / period + 1) * period)
                : nowDT;
        }
    }
}
