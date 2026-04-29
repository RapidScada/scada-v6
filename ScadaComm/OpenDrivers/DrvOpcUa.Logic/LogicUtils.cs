// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Scada.Data.Const;

namespace Scada.Comm.Drivers.DrvOpcUa.Logic
{
    /// <summary>
    /// The class provides helper methods for the driver logic.
    /// <para>Класс, предоставляющий вспомогательные методы для логики драйвера.</para>
    /// </summary>
    internal static class LogicUtils
    {
        /// <summary>
        /// Gets the device tag status according to the OPC status code.
        /// </summary>
        public static int GetDeviceTagStatus(StatusCode statusCode)
        {
            if (StatusCode.IsGood(statusCode))
                return CnlStatusID.Defined;
            else if (StatusCode.IsUncertain(statusCode))
                return CnlStatusID.Unreliable;
            else
                return CnlStatusID.Undefined;
        }
    }
}
