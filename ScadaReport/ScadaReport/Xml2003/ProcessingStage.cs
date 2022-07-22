// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Report.Xml2003
{
    /// <summary>
    /// Specifies the report processing stages.
    /// <para>Задаёт этапы обработки отчета.</para>
    /// </summary>
    public enum ProcessingStage
    {
        NotStarted,
        Preprocessing,
        Processing,
        Postprocessing,
        Completed
    }
}
