// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvHttpNotif.Config
{
    /// <summary>
    /// Specifies the methods for escaping request parameters.
    /// <para>Определяет методы кодирования параметров запросов.</para>
    /// </summary>
    internal enum EscapingMethod
    {
        None,
        EncodeUrl,
        EncodeJson
    }
}
