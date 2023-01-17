// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModDbExport.Logic
{
    /// <summary>
    /// The class provides helper methods for the module logic.
    /// <para>Класс, предоставляющий вспомогательные методы для логики модуля.</para>
    /// </summary>
    internal static class LogicUtils
    {
        /// <summary>
        /// The connection status names in English.
        /// </summary>
        private static readonly string[] ConnStatusNamesEn = { "Undefined", "Normal", "Error" };
        /// <summary>
        /// The connection status names in Russian.
        /// </summary>
        private static readonly string[] ConnStatusNamesRu = { "не определено", "норма", "ошибка" };


        /// <summary>
        /// Calculates the next timer firing.
        /// </summary>
        public static DateTime CalcNextTimer(DateTime nowDT, int period)
        {
            return period > 0
                ? nowDT.Date.AddSeconds(((int)nowDT.TimeOfDay.TotalSeconds / period + 1) * period)
                : nowDT;
        }

        /// <summary>
        /// Converts the connection status to a string.
        /// </summary>
        public static string ToString(this ConnectionStatus status, bool isRussian)
        {
            return isRussian ?
                ConnStatusNamesRu[(int)status] :
                ConnStatusNamesEn[(int)status];
        }
    }
}
