/*
 * Copyright 2022 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommon
 * Summary  : The class provides helper methods for the entire software package
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2007
 * Modified : 2022
 */

using Scada.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Scada
{
    /// <summary>
    /// The class provides helper methods for the entire software package.
    /// <para>Класс, предоставляющий вспомогательные методы для всего программного комплекса.</para>
    /// </summary>
    public static partial class ScadaUtils
    {
        /// <summary>
        /// The delay in thread iteration to save resources, ms.
        /// </summary>
        public const int ThreadDelay = 100;
        /// <summary>
        /// The waiting time to stop the thread, ms.
        /// </summary>
        public const int ThreadWait = 10000;
        /// <summary>
        /// The ending of a registration key file name.
        /// </summary>
        public const string RegFileSuffix = "_Reg.xml";
        /// <summary>
        /// Generates parts of unique IDs.
        /// </summary>
        private static readonly Random UniqueIDGenerator = new Random();
        /// <summary>
        /// The period of writing information about the service status.
        /// </summary>
        public static readonly TimeSpan WriteInfoPeriod = TimeSpan.FromSeconds(1);
        /// <summary>
        /// The time after which a command is ignored.
        /// </summary>
        public static readonly TimeSpan CommandLifetime = TimeSpan.FromSeconds(60);
        /// <summary>
        /// Determines that the application is running on Windows.
        /// </summary>
        public static readonly bool IsRunningOnWin = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        /// <summary>
        /// Determines that the application is running on .NET Core.
        /// </summary>
        public static readonly bool IsRunningOnCore = RuntimeInformation.FrameworkDescription.StartsWith(".NET Core");

        #region Tables for calculating CRC-16.
        /* Table of CRC values for high–order byte */
        private static readonly byte[] CRCHiTable = new byte[256] {
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
            0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40
        };

        /* Table of CRC values for low–order byte */
        private static readonly byte[] CRCLoTable = new byte[256] {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7, 0x05, 0xC5, 0xC4,
            0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
            0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD,
            0x1D, 0x1C, 0xDC, 0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32, 0x36, 0xF6, 0xF7,
            0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
            0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE,
            0x2E, 0x2F, 0xEF, 0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1, 0x63, 0xA3, 0xA2,
            0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
            0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB,
            0x7B, 0x7A, 0xBA, 0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0, 0x50, 0x90, 0x91,
            0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
            0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88,
            0x48, 0x49, 0x89, 0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83, 0x41, 0x81, 0x80,
            0x40
        };
        #endregion

        #region Table for calculating CRC-32.
        private static readonly uint[] Crc32Table = new uint[256] {
            0x00000000, 0xF26B8303, 0xE13B70F7, 0x1350F3F4,
            0xC79A971F, 0x35F1141C, 0x26A1E7E8, 0xD4CA64EB,
            0x8AD958CF, 0x78B2DBCC, 0x6BE22838, 0x9989AB3B,
            0x4D43CFD0, 0xBF284CD3, 0xAC78BF27, 0x5E133C24,
            0x105EC76F, 0xE235446C, 0xF165B798, 0x030E349B,
            0xD7C45070, 0x25AFD373, 0x36FF2087, 0xC494A384,
            0x9A879FA0, 0x68EC1CA3, 0x7BBCEF57, 0x89D76C54,
            0x5D1D08BF, 0xAF768BBC, 0xBC267848, 0x4E4DFB4B,
            0x20BD8EDE, 0xD2D60DDD, 0xC186FE29, 0x33ED7D2A,
            0xE72719C1, 0x154C9AC2, 0x061C6936, 0xF477EA35,
            0xAA64D611, 0x580F5512, 0x4B5FA6E6, 0xB93425E5,
            0x6DFE410E, 0x9F95C20D, 0x8CC531F9, 0x7EAEB2FA,
            0x30E349B1, 0xC288CAB2, 0xD1D83946, 0x23B3BA45,
            0xF779DEAE, 0x05125DAD, 0x1642AE59, 0xE4292D5A,
            0xBA3A117E, 0x4851927D, 0x5B016189, 0xA96AE28A,
            0x7DA08661, 0x8FCB0562, 0x9C9BF696, 0x6EF07595,
            0x417B1DBC, 0xB3109EBF, 0xA0406D4B, 0x522BEE48,
            0x86E18AA3, 0x748A09A0, 0x67DAFA54, 0x95B17957,
            0xCBA24573, 0x39C9C670, 0x2A993584, 0xD8F2B687,
            0x0C38D26C, 0xFE53516F, 0xED03A29B, 0x1F682198,
            0x5125DAD3, 0xA34E59D0, 0xB01EAA24, 0x42752927,
            0x96BF4DCC, 0x64D4CECF, 0x77843D3B, 0x85EFBE38,
            0xDBFC821C, 0x2997011F, 0x3AC7F2EB, 0xC8AC71E8,
            0x1C661503, 0xEE0D9600, 0xFD5D65F4, 0x0F36E6F7,
            0x61C69362, 0x93AD1061, 0x80FDE395, 0x72966096,
            0xA65C047D, 0x5437877E, 0x4767748A, 0xB50CF789,
            0xEB1FCBAD, 0x197448AE, 0x0A24BB5A, 0xF84F3859,
            0x2C855CB2, 0xDEEEDFB1, 0xCDBE2C45, 0x3FD5AF46,
            0x7198540D, 0x83F3D70E, 0x90A324FA, 0x62C8A7F9,
            0xB602C312, 0x44694011, 0x5739B3E5, 0xA55230E6,
            0xFB410CC2, 0x092A8FC1, 0x1A7A7C35, 0xE811FF36,
            0x3CDB9BDD, 0xCEB018DE, 0xDDE0EB2A, 0x2F8B6829,
            0x82F63B78, 0x709DB87B, 0x63CD4B8F, 0x91A6C88C,
            0x456CAC67, 0xB7072F64, 0xA457DC90, 0x563C5F93,
            0x082F63B7, 0xFA44E0B4, 0xE9141340, 0x1B7F9043,
            0xCFB5F4A8, 0x3DDE77AB, 0x2E8E845F, 0xDCE5075C,
            0x92A8FC17, 0x60C37F14, 0x73938CE0, 0x81F80FE3,
            0x55326B08, 0xA759E80B, 0xB4091BFF, 0x466298FC,
            0x1871A4D8, 0xEA1A27DB, 0xF94AD42F, 0x0B21572C,
            0xDFEB33C7, 0x2D80B0C4, 0x3ED04330, 0xCCBBC033,
            0xA24BB5A6, 0x502036A5, 0x4370C551, 0xB11B4652,
            0x65D122B9, 0x97BAA1BA, 0x84EA524E, 0x7681D14D,
            0x2892ED69, 0xDAF96E6A, 0xC9A99D9E, 0x3BC21E9D,
            0xEF087A76, 0x1D63F975, 0x0E330A81, 0xFC588982,
            0xB21572C9, 0x407EF1CA, 0x532E023E, 0xA145813D,
            0x758FE5D6, 0x87E466D5, 0x94B49521, 0x66DF1622,
            0x38CC2A06, 0xCAA7A905, 0xD9F75AF1, 0x2B9CD9F2,
            0xFF56BD19, 0x0D3D3E1A, 0x1E6DCDEE, 0xEC064EED,
            0xC38D26C4, 0x31E6A5C7, 0x22B65633, 0xD0DDD530,
            0x0417B1DB, 0xF67C32D8, 0xE52CC12C, 0x1747422F,
            0x49547E0B, 0xBB3FFD08, 0xA86F0EFC, 0x5A048DFF,
            0x8ECEE914, 0x7CA56A17, 0x6FF599E3, 0x9D9E1AE0,
            0xD3D3E1AB, 0x21B862A8, 0x32E8915C, 0xC083125F,
            0x144976B4, 0xE622F5B7, 0xF5720643, 0x07198540,
            0x590AB964, 0xAB613A67, 0xB831C993, 0x4A5A4A90,
            0x9E902E7B, 0x6CFBAD78, 0x7FAB5E8C, 0x8DC0DD8F,
            0xE330A81A, 0x115B2B19, 0x020BD8ED, 0xF0605BEE,
            0x24AA3F05, 0xD6C1BC06, 0xC5914FF2, 0x37FACCF1,
            0x69E9F0D5, 0x9B8273D6, 0x88D28022, 0x7AB90321,
            0xAE7367CA, 0x5C18E4C9, 0x4F48173D, 0xBD23943E,
            0xF36E6F75, 0x0105EC76, 0x12551F82, 0xE03E9C81,
            0x34F4F86A, 0xC69F7B69, 0xD5CF889D, 0x27A40B9E,
            0x79B737BA, 0x8BDCB4B9, 0x988C474D, 0x6AE7C44E,
            0xBE2DA0A5, 0x4C4623A6, 0x5F16D052, 0xAD7D5351
        };
        #endregion


        /// <summary>
        /// Creates a new FormatException for the specified parameter.
        /// </summary>
        private static FormatException NewFormatException(string paramName)
        {
            return new FormatException(string.Format(CommonPhrases.InvalidParamVal, paramName));
        }

        /// <summary>
        /// Adds a directory separator to the directory name if necessary.
        /// </summary>
        public static string NormalDir(string path)
        {
            path = path == null ? "" : path.Trim();
            int lastIndex = path.Length - 1;

            if (lastIndex >= 0 &&
                path[lastIndex] != Path.DirectorySeparatorChar &&
                path[lastIndex] != Path.AltDirectorySeparatorChar)
            {
                path += Path.DirectorySeparatorChar;
            }

            return path;
        }

        /// <summary>
        /// Normalizes path separators according to the operating system.
        /// </summary>
        public static string NormalPathSeparators(string path)
        {
            return path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Removes file name suffixes that follow the first dot.
        /// </summary>
        public static string RemoveFileNameSuffixes(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "";
            }
            else
            {
                int dotIndex = fileName.IndexOf('.');
                return dotIndex >= 0 ? fileName.Substring(0, dotIndex) : fileName;
            }
        }

        /// <summary>
        /// Reads all lines from the stream.
        /// </summary>
        public static List<string> ReadAllLines(this Stream stream)
        {
            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            return lines;
        }

        /// <summary>
        /// Identifies a nullable type.
        /// </summary>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Indicates whether the string is correct URL.
        /// </summary>
        public static bool IsValidUrl(string s)
        {
            return !string.IsNullOrEmpty(s) &&
                Uri.TryCreate(s, UriKind.Absolute, out Uri uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Makes a full copy of the specified object.
        /// </summary>
        /// <remarks>
        /// A cloned object and its children must have the Serializable attribute.
        /// BinaryFormatter is not recommended, see https://aka.ms/binaryformatter
        /// </remarks>
        public static object DeepClone(this object obj, SerializationBinder binder = null)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                
                if (binder != null)
                    formatter.Binder = binder;

                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Creates a full copy of the specified object.
        /// </summary>
        public static T DeepClone<T>(this T obj, SerializationBinder binder = null)
        {
            return (T)DeepClone((object)obj, binder);
        }

        /// <summary>
        /// Creates a full copy of the specified object using XmlSerializer.
        /// </summary>
        public static T SafeClone<T>(this T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    serializer.Serialize(writer, obj);
                }

                stream.Position = 0;

                using (XmlReader reader = XmlReader.Create(stream))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }

        /// <summary>
        /// Creates a shallow copy of the specified object.
        /// </summary>
        public static T ShallowCopy<T>(this T obj)
        {
            object newObj = Activator.CreateInstance(typeof(T));

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(typeof(T)))
            {
                object val = prop.GetValue(obj);
                prop.SetValue(newObj, val);
            }

            return (T)newObj;
        }

        /// <summary>
        /// Generates a positive globally unique ID.
        /// </summary>
        public static long GenerateUniqueID()
        {
            return GenerateUniqueID(DateTime.UtcNow);
        }

        /// <summary>
        /// Generates a positive globally unique ID.
        /// </summary>
        public static long GenerateUniqueID(DateTime nowDT)
        {
            lock (UniqueIDGenerator)
            {
                long unixTime = new DateTimeOffset(nowDT).ToUnixTimeSeconds();
                return ((unixTime << 32) + UniqueIDGenerator.Next(1, int.MaxValue)) & 0x7FFFFFFFFFFFFFFF;
            }
        }

        /// <summary>
        /// Retrieves the time encoded in the ID.
        /// </summary>
        public static DateTime RetrieveTimeFromID(long id)
        {
            return DateTimeOffset.FromUnixTimeSeconds(id >> 32).UtcDateTime;
        }

        /// <summary>
        /// Checks whether the specified bit of the integer number is set.
        /// </summary>
        public static bool BitIsSet(this int number, int bit)
        {
            return ((number >> bit) & 0x01) > 0;
        }

        /// <summary>
        /// Sets the specified bit of the integer number.
        /// </summary>
        public static int SetBit(this int number, int bit, bool value)
        {
            return value ?
                number | (1 << bit) :
                number & ~(1 << bit);
        }

        /// <summary>
        /// Calculates a 16-bit CRC.
        /// </summary>
        /// <remarks>The method is taken from the Modbus specification.</remarks>
        public static ushort CRC16(byte[] buffer, int offset, int length)
        {
            byte crcHi = 0xFF;   // high byte of CRC initialized
            byte crcLo = 0xFF;   // low byte of CRC initialized
            int index;           // will index into CRC lookup table

            while (length-- > 0) // pass through message buffer
            {
                index = crcLo ^ buffer[offset++]; // calculate the CRC
                crcLo = (byte)(crcHi ^ CRCHiTable[index]);
                crcHi = CRCLoTable[index];
            }

            return (ushort)((crcHi << 8) | crcLo);
        }

        /// <summary>
        /// Calculates a 32-bit CRC.
        /// </summary>
        /// <remarks>CRC-32C algorithm with 0x1EDC6F41 polynom.</remarks>
        public static uint CRC32(byte[] buffer, int offset, int length)
        {
            uint crc = 0xFFFFFFFF;

            while (length-- > 0)
            {
                crc = Crc32Table[(crc ^ buffer[offset++]) & 0xFF] ^ (crc >> 8);
            }

            return crc ^ 0xFFFFFFFF;
        }

        /// <summary>
        /// Retrieves the hostname and port number from the specified address string.
        /// </summary>
        public static void RetrieveHostAndPort(string address, int defaultPort, out string host, out int port)
        {
            int ind = address.IndexOf(':');

            if (ind >= 0)
            {
                host = address.Substring(0, ind);
                if (!int.TryParse(address.Substring(ind + 1), out port))
                    port = defaultPort;
            }
            else
            {
                host = address;
                port = defaultPort;
            }
        }

        /// <summary>
        /// Formats the specified text with the specified arguments.
        /// </summary>
        public static string FormatText(string text, object[] args)
        {
            return string.IsNullOrEmpty(text)
                ? ""
                : args == null || args.Length == 0 ? text : string.Format(text, args);
        }

        /// <summary>
        /// Builds an error message combining the exception message, the specified text and arguments.
        /// </summary>
        public static string BuildErrorMessage(this Exception ex, string text = "", params object[] args)
        {
            string message = ex?.Message ?? "";

            if (string.IsNullOrEmpty(text))
                return message;
            else if (string.IsNullOrEmpty(message))
                return FormatText(text, args);
            else
                return FormatText(text, args) + ":" + Environment.NewLine + message;
        }

        /// <summary>
        /// Returns the first non-empty string or null.
        /// </summary>
        public static string FirstNonEmpty(params string[] args)
        {
            return args.FirstOrDefault(s => !string.IsNullOrEmpty(s));
        }

        /// <summary>
        /// Gets the beginning of the string that does not exceed the specified length.
        /// </summary>
        public static string GetPreview(this string s, int maxLength)
        {
            if (s == null)
                return "";
            else if (s.Length <= maxLength)
                return s;
            else
                return s.Substring(0, maxLength) + "...";
        }

        /// <summary>
        /// Recursively sets the parent for each child object.
        /// </summary>
        public static void RestoreHierarchy(this ITreeNode treeNode)
        {
            if (treeNode.Children != null)
            {
                foreach (object child in treeNode.Children)
                {
                    if (child is ITreeNode childNode)
                    {
                        childNode.Parent = treeNode;
                        RestoreHierarchy(childNode);
                    }
                }
            }
        }
    }
}
