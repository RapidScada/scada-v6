﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Report
{
    /// <summary>
    /// Specifies the report output formats.
    /// <para>Задаёт выходные форматы отчётов.</para>
    /// </summary>
    public enum OutputFormat
    {
        Default,
        Pdf,
        Xml2003,
        Xlsx,
        Html
    }
}
