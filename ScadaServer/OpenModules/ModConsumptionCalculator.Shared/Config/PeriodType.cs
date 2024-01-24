// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModConsumptionCalculator.Config
{
    /// <summary>
    /// Specifies the calculation period types.
    /// <para>Задаёт типы периодов вычислений.</para>
    /// </summary>
    internal enum PeriodType
    {
        Custom,
        Minute,
        Hour,
        Day,
        Month
    }
}
