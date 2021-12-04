// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Scada.Comm.Drivers.DrvOpcUa
{
    /// <summary>
    /// Represents data types supported by OPC commands.
    /// <para>Представляет типы данных, поддерживаемые командами OPC.</para>
    /// </summary>
    public static class KnownTypes
    {
        private static readonly Dictionary<string, Type> TypesByName = new Dictionary<string, Type>
        {
            { "boolean", typeof(Boolean) },
            { "byte", typeof(Byte) },
            { "double", typeof(Double) },
            { "int16", typeof(Int16) },
            { "int32", typeof(Int32) },
            { "int64", typeof(Int64) },
            { "sbyte", typeof(SByte) },
            { "string", typeof(String) },
            { "float", typeof(Single) },
            { "uint16", typeof(UInt16) },
            { "uint32", typeof(UInt32) },
            { "uint64", typeof(UInt64) }
        };


        /// <summary>
        /// Gets the data type by name, case insensitive.
        /// </summary>
        public static bool GetType(string typeName, out Type type)
        {
            typeName = (typeName ?? "").Trim().ToLowerInvariant();

            if (TypesByName.TryGetValue(typeName, out type))
            {
                return true;
            }
            else
            {
                type = null;
                return false;
            }
        }

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
