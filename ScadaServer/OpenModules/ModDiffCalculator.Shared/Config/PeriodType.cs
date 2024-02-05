// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using NCM = System.ComponentModel;

namespace Scada.Server.Modules.ModDiffCalculator.Config
{
    /// <summary>
    /// Specifies the calculation period types.
    /// <para>Задаёт типы периодов вычислений.</para>
    /// </summary>
    [NCM.TypeConverter(typeof(EnumConverter))]
    internal enum PeriodType
    {
        [Description("Custom")]
        Custom,

        [Description("Minute")]
        Minute,

        [Description("Hour")]
        Hour,

        [Description("Day")]
        Day,

        [Description("Month")]
        Month
    }
}
