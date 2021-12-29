// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Scada.Comm.Drivers.DrvOpcUa
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
        public const string DriverCode = "DrvOpcUa";


        /// <summary>
        /// Gets the length of the tag data required to store a string of the specified length.
        /// </summary>
        public static int GetTagDataLength(int stringLength)
        {
            return stringLength / 4 + ((stringLength % 4) == 0 ? 0 : 1);
        }

        /// <summary>
        /// Checks if the specified data type name matches the type.
        /// </summary>
        public static bool DataTypeEquals(string dataTypeName, Type type)
        {
            return type != null && string.Equals(dataTypeName, type.FullName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
