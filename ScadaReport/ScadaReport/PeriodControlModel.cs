// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Report
{
    /// <summary>
    /// Represents a data model for a period control.
    /// <para>Представляет модель данных для элемента управления периода.</para>
    /// </summary>
    public class PeriodControlModel
    {
        public PeriodControlModel()
        {
            PeriodControl = PeriodControl.None;
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
        }

        public PeriodControlModel(PeriodControl periodControl, DateTime todayLocal)
        {
            switch (periodControl)
            {
                case PeriodControl.SingleDate:
                case PeriodControl.DatePeriod:
                    PeriodControl = periodControl;
                    StartTime = todayLocal;
                    EndTime = todayLocal;
                    break;

                case PeriodControl.SingleMonth:
                case PeriodControl.MonthPeriod:
                    PeriodControl = periodControl;
                    StartTime = todayLocal.GetMonthStart();
                    EndTime = todayLocal.GetMonthEnd();
                    break;

                case PeriodControl.DateTimePeriod:
                    PeriodControl = periodControl;
                    StartTime = todayLocal;
                    EndTime = todayLocal.AddDays(1.0);
                    break;

                default:
                    PeriodControl = PeriodControl.None;
                    StartTime = DateTime.MinValue;
                    EndTime = DateTime.MinValue;
                    break;
            }
        }


        public PeriodControl PeriodControl { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
