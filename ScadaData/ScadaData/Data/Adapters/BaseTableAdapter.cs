/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : Represents a mechanism to read and write the configuration database tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Tables;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Scada.Data.Adapters
{
    /// <summary>
    /// Represents a mechanism to read and write the configuration database tables.
    /// <para>Представляет механизм для чтения и записи таблиц базы конфигурации.</para>
    /// </summary>
    public class BaseTableAdapter : Adapter
    {
        /// <summary>
        /// Represents a field definition.
        /// </summary>
        protected struct FieldDef
        {
            /// <summary>
            /// Initializes a new instance of the structure.
            /// </summary>
            public FieldDef(string name, Type type, bool allowNull)
            {
                if (name == null)
                    throw new ArgumentNullException("name");
                if (name.Length > MaxFieldNameLength)
                    throw new ArgumentException("Name length exceeded.");

                Name = name;
                AllowNull = allowNull;

                if (type == typeof(int))
                {
                    DataType = DataTypeID.Integer;
                    DataSize = sizeof(int);
                }
                else if (type == typeof(double))
                {
                    DataType = DataTypeID.Double;
                    DataSize = sizeof(double);
                }
                else if (type == typeof(bool))
                {
                    DataType = DataTypeID.Boolean;
                    DataSize = 1;
                }
                else if (type == typeof(DateTime))
                {
                    DataType = DataTypeID.DateTime;
                    DataSize = sizeof(long);
                }
                else if (type == typeof(string))
                {
                    DataType = DataTypeID.String;
                    DataSize = 0;
                }
                else
                {
                    throw new ArgumentException("Data type is not supported.");
                }
            }

            /// <summary>
            /// Gets or sets the field name.
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Gets or sets the data type.
            /// </summary>
            public byte DataType { get; set; }
            /// <summary>
            /// Gets or sets the data size if it is fixed.
            /// </summary>
            public int DataSize { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether null values are possible.
            /// </summary>
            public bool AllowNull { get; set; }
        }

        /// <summary>
        /// Specifies the data type IDs.
        /// </summary>
        protected static class DataTypeID
        {
            /// <summary>
            /// Integer data type.
            /// </summary>
            public const byte Integer = 0;
            /// <summary>
            /// Floating point data type.
            /// </summary>
            public const byte Double = 1;
            /// <summary>
            /// Logical data type.
            /// </summary>
            public const byte Boolean = 2;
            /// <summary>
            /// Date and time.
            /// </summary>
            public const byte DateTime = 3;
            /// <summary>
            /// String data type.
            /// </summary>
            public const byte String = 4;
        }


        /// <summary>
        /// The table type.
        /// </summary>
        protected const ushort TableType = 2;
        /// <summary>
        /// The major version number.
        /// </summary>
        protected const ushort MajorVersion = 4;
        /// <summary>
        /// The minor version number.
        /// </summary>
        protected const ushort MinorVersion = 0;
        /// <summary>
        /// The field definition size in a file.
        /// </summary>
        protected const int FieldDefSize = 60;
        /// <summary>
        /// The maximum length allowed for a field name.
        /// </summary>
        protected const int MaxFieldNameLength = 50;
        /// <summary>
        /// The maximum length allowed for a field value.
        /// </summary>
        protected const int MaxFieldLenght = ushort.MaxValue;
        /// <summary>
        /// Indicates the beginning of a row.
        /// </summary>
        protected const ushort RowMarker = 0xAAFF;


        /// <summary>
        /// Writes the field definintion using the specified writer.
        /// </summary>
        protected void WriteFieldDef(FieldDef fieldDef, BinaryWriter writer, byte[] buffer)
        {
            Array.Clear(buffer, 0, FieldDefSize);
            int nameLength = fieldDef.Name.Length;
            buffer[0] = (byte)nameLength;
            Encoding.ASCII.GetBytes(fieldDef.Name).CopyTo(buffer, 1);
            buffer[nameLength + 2] = fieldDef.DataType;
            buffer[nameLength + 3] = (byte)(fieldDef.AllowNull ? 1 : 0);
            ushort crc = CalcCRC16(buffer, 0, FieldDefSize - 2);
            BitConverter.GetBytes(crc).CopyTo(buffer, FieldDefSize - 2);
            writer.Write(buffer);
        }

        /// <summary>
        /// Gets the field data to write.
        /// </summary>
        protected byte[] GetFieldData(int dataTypeID, object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            switch (dataTypeID)
            {
                case DataTypeID.Integer:
                    return BitConverter.GetBytes((int)value);
                case DataTypeID.Double:
                    return BitConverter.GetBytes((double)value);
                case DataTypeID.Boolean:
                    return BitConverter.GetBytes((bool)value);
                case DataTypeID.DateTime:
                    return BitConverter.GetBytes(((DateTime)value).Ticks);
                case DataTypeID.String:
                    string s = (string)value;
                    if (s.Length > MaxFieldLenght)
                        throw new ArgumentException("String length exceeded.");
                    return Encoding.UTF8.GetBytes(s);
                default:
                    return EmptyBuffer;
            }
        }

        /// <summary>
        /// Fills the specified table by reading data from the configuration database.
        /// </summary>
        public void Fill(IBaseTable baseTable)
        {
        }

        /// <summary>
        /// Updates the configuration database by writing data of the specified table.
        /// </summary>
        public void Update(IBaseTable baseTable)
        {
            if (baseTable == null)
                throw new ArgumentNullException("baseTable");

            Stream stream = null;
            BinaryWriter writer = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                writer = new BinaryWriter(stream, Encoding.UTF8, Stream != null);

                // write header
                writer.Write(TableType);
                writer.Write(MajorVersion);
                writer.Write(MinorVersion);

                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);
                ushort fieldCount = (ushort)Math.Min(props.Count, ushort.MaxValue);
                writer.Write(fieldCount);
                writer.Write(ReserveBuffer, 0, 12);

                if (fieldCount > 0)
                {
                    // create and write field definitions
                    FieldDef[] fieldDefs = new FieldDef[fieldCount];
                    byte[] fieldDefBuf = new byte[FieldDefSize];

                    for (int i = 0; i < fieldCount; i++)
                    {
                        PropertyDescriptor prop = props[i];
                        bool isNullable = prop.PropertyType.IsNullable();

                        FieldDef fieldDef = new FieldDef(
                            prop.Name,
                            isNullable ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType,
                            isNullable || prop.PropertyType.IsClass);

                        fieldDefs[i] = fieldDef;
                        WriteFieldDef(fieldDef, writer, fieldDefBuf);
                    }

                    // write rows
                    byte[] rowBuf = EmptyBuffer;
                    byte[][] rowData = new byte[fieldCount][];
                    bool[] isNullArr = new bool[fieldCount];

                    foreach (object item in baseTable.EnumerateItems())
                    {
                        // get row data and size
                        int rowDataSize = 0;

                        for (int i = 0; i < fieldCount; i++)
                        {
                            object value = props[i].GetValue(item);

                            if (value == null)
                            {
                                rowData[i] = EmptyBuffer;
                                isNullArr[i] = true;
                            }
                            else
                            {
                                FieldDef fieldDef = fieldDefs[i];
                                byte[] fieldData = GetFieldData(fieldDef.DataType, value);
                                rowData[i] = fieldData;
                                isNullArr[i] = false;
                                rowDataSize += (fieldDef.DataSize <= 0 ? 2 : 0) + fieldData.Length;
                            }

                            rowDataSize++; // null flag
                        }

                        // copy row data to the buffer
                        int fullRowSize = rowDataSize + 8;

                        if (rowBuf.Length < fullRowSize)
                            rowBuf = new byte[fullRowSize * 2];

                        BitConverter.GetBytes(RowMarker).CopyTo(rowBuf, 0);
                        BitConverter.GetBytes(rowDataSize).CopyTo(rowBuf, 2);
                        int copyIndex = 6;

                        for (int i = 0; i < fieldCount; i++)
                        {
                            if (isNullArr[i])
                            {
                                rowBuf[copyIndex++] = 1;
                            }
                            else
                            {
                                rowBuf[copyIndex++] = 0;
                                FieldDef fieldDef = fieldDefs[i];
                                byte[] fieldData = rowData[i];

                                if (fieldDef.DataSize <= 0)
                                {
                                    BitConverter.GetBytes((ushort)fieldData.Length).CopyTo(rowBuf, copyIndex);
                                    copyIndex += 2;
                                }

                                fieldData.CopyTo(rowBuf, copyIndex);
                                copyIndex += fieldData.Length;
                            }
                        }

                        ushort crc = CalcCRC16(rowBuf, 0, fullRowSize - 2);
                        BitConverter.GetBytes(crc).CopyTo(rowBuf, fullRowSize - 2);

                        // write row data
                        writer.Write(rowBuf, 0, fullRowSize);
                    }
                }
            }
            finally
            {
                writer?.Close();
            }
        }
    }
}
