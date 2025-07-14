// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System.Globalization;

namespace Scada.Comm.Drivers.DrvOpcUa
{
    /// <summary>
    /// Represents data types supported by OPC commands.
    /// <para>Представляет типы данных, поддерживаемые командами OPC.</para>
    /// </summary>
    public static class KnownTypes
    {
        /// <summary>
        /// The known type names.
        /// </summary>
        public static readonly string[] TypeNames =
        [
            "System.Boolean",
            "System.Byte",
            "System.DateTime",
            "System.Decimal",
            "System.Double",
            "System.Int16",
            "System.Int32",
            "System.Int64",
            "System.SByte",
            "System.Single",
            "System.String",
            "System.UInt16",
            "System.UInt32",
            "System.UInt64"
        ];

        /// <summary>
        /// Converts the string to an object depending on the type name.
        /// </summary>
        public static object Parse(string typeName, string s)
        {
            typeName = (typeName ?? "").Trim().ToLowerInvariant();
            s = (s ?? "").Trim();

            if (typeName == "boolean" || typeName == "bool")
                return bool.Parse(s);
            else if (typeName == "byte")
                return byte.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "datetime")
                return DateTime.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "decimal")
                return decimal.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "double")
                return double.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "int16" || typeName == "short")
                return short.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "int32" || typeName == "int")
                return int.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "int64" || typeName == "long")
                return long.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "sbyte")
                return sbyte.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "string")
                return s;
            else if (typeName == "single" || typeName == "float")
                return float.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "uint16" || typeName == "ushort")
                return ushort.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "uint32" || typeName == "uint")
                return uint.Parse(s, CultureInfo.InvariantCulture);
            else if (typeName == "uint64" || typeName == "ulong")
                return ulong.Parse(s, CultureInfo.InvariantCulture);
            else
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Неизвестный тип данных \"{0}\"" :
                    "Unknown data type \"{0}\"", typeName);
            }
        }
    }
}
