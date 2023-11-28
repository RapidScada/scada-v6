﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvGooglePubSub
{
    /// <summary>
    /// The class provides helper methods for the driver.
    /// <para>Класс, предоставляющий вспомогательные методы для драйвера.</para>
    /// </summary>
    public static class DriverUtils
    {
        /// <summary>
        /// The driver code.
        /// </summary>
        public const string DriverCode = "DrvGooglePubSub";

        /// <summary>
        /// 获取测点的时间测点
        /// </summary>
        public static string GetPointTime(string pointName)
        {
            return $"{pointName}_time";
        }
    }
}
