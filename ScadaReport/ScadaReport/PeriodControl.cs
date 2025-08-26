﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Report
{
    /// <summary>
    /// Specifies the types of controls for entering a period.
    /// <para>Задаёт типы элементов управления для ввода периода.</para>
    /// </summary>
    public enum PeriodControl
    {
        SingleDate,
        SingleMonth,
        DatePeriod,
        DateTimePeriod,
        MonthPeriod
    }
}
