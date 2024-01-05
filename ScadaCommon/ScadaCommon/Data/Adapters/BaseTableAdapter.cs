/*
 * Copyright 2024 Rapid Software LLC
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
 * Summary  : Represents a mechanism to read and write the configuration database tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Tables;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using static Scada.BinaryConverter;

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
        protected class FieldDef
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public FieldDef(string name, byte dataType, bool allowNull)
            {
                if (!Enum.IsDefined(typeof(ColumnDataType), dataType))
                    throw new ArgumentException(string.Format("Data type {0} is not defined.", dataType));

                Name = name ?? throw new ArgumentNullException(nameof(name));
                DataType = (ColumnDataType)dataType;
                AllowNull = allowNull;

                switch (DataType)
                {
                    case ColumnDataType.Integer:
                        DataSize = sizeof(int);
                        DefaultValue = 0;
                        break;
                    case ColumnDataType.Double:
                        DataSize = sizeof(double);
                        DefaultValue = 0.0;
                        break;
                    case ColumnDataType.Boolean:
                        DataSize = 1;
                        DefaultValue = false;
                        break;
                    case ColumnDataType.DateTime:
                        DataSize = sizeof(long);
                        DefaultValue = (long)0;
                        break;
                    case ColumnDataType.String:
                        DataSize = 0;
                        DefaultValue = "";
                        break;
                    default:
                        throw new ArgumentException(string.Format("Data type {0} is not supported.", DataType));
                }
            }
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public FieldDef(string name, Type type, bool allowNull)
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                if (name.Length > MaxFieldNameLength)
                    throw new ArgumentException("Name length exceeded.");
                if (type == null)
                    throw new ArgumentNullException(nameof(type));

                Name = name;
                AllowNull = allowNull;

                if (type == typeof(int))
                {
                    DataType = ColumnDataType.Integer;
                    DataSize = sizeof(int);
                    DefaultValue = 0;
                }
                else if (type == typeof(double))
                {
                    DataType = ColumnDataType.Double;
                    DataSize = sizeof(double);
                    DefaultValue = 0.0;
                }
                else if (type == typeof(bool))
                {
                    DataType = ColumnDataType.Boolean;
                    DataSize = 1;
                    DefaultValue = false;
                }
                else if (type == typeof(DateTime))
                {
                    DataType = ColumnDataType.DateTime;
                    DataSize = sizeof(long);
                    DefaultValue = (long)0;
                }
                else if (type == typeof(string))
                {
                    DataType = ColumnDataType.String;
                    DataSize = 0;
                    DefaultValue = "";
                }
                else
                {
                    throw new ArgumentException(string.Format("Data type {0} is not supported.", type.FullName));
                }
            }

            /// <summary>
            /// Gets or sets the field name.
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Gets or sets the data type.
            /// </summary>
            public ColumnDataType DataType { get; set; }
            /// <summary>
            /// Gets or sets the data size if it is fixed.
            /// </summary>
            public int DataSize { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether null values are possible.
            /// </summary>
            public bool AllowNull { get; set; }
            /// <summary>
            /// Gets the default field value.
            /// </summary>
            public object DefaultValue { get; protected set; }
        }


        /// <summary>
        /// The major version number.
        /// </summary>
        protected const ushort MajorVersion = 4;
        /// <summary>
        /// The minor version number.
        /// </summary>
        protected const ushort MinorVersion = 0;
        /// <summary>
        /// The header size in a file.
        /// </summary>
        protected const int HeaderSize = 20;
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
        /// Reads and validates the table header.
        /// </summary>
        protected bool ReadHeader(BinaryReader reader, byte[] buffer, out int fieldCount)
        {
            int bytesRead = reader.Read(buffer, 0, HeaderSize);

            if (bytesRead == 0) // table is empty
            {
                fieldCount = 0;
                return false;
            }

            if (bytesRead < HeaderSize)
                throw new ScadaException("Unexpected end of stream.");

            int index = 0;
            if (GetUInt16(buffer, ref index) != TableType.BaseTable)
                throw new ScadaException("Invalid table type.");

            if (GetUInt16(buffer, ref index) != MajorVersion)
                throw new ScadaException("Incompatible format version.");

            index += 2; // skip minor version
            fieldCount = GetUInt16(buffer, ref index);
            return fieldCount > 0;
        }

        /// <summary>
        /// Reads field definitions.
        /// </summary>
        protected FieldDef[] ReadFieldDefs(BinaryReader reader, byte[] buffer, int fieldCount)
        {
            FieldDef[] fieldDefs = new FieldDef[fieldCount];

            for (int i = 0; i < fieldCount; i++)
            {
                ReadData(reader, buffer, 0, FieldDefSize, true);

                if (ScadaUtils.CRC16(buffer, 0, FieldDefSize - 2) != BitConverter.ToUInt16(buffer, FieldDefSize - 2))
                    throw new ScadaException("Field definition CRC error.");

                int index = 0;
                int nameLength = buffer[index++];
                string fieldName = Encoding.ASCII.GetString(buffer, index, nameLength);
                index += MaxFieldNameLength;

                fieldDefs[i] = new FieldDef(
                    fieldName,
                    GetByte(buffer, ref index),
                    GetBool(buffer, ref index));
            }

            return fieldDefs;
        }

        /// <summary>
        /// Reads a row to the buffer.
        /// </summary>
        protected void ReadRowToBuffer(BinaryReader reader, ref byte[] buffer)
        {
            if (reader.ReadUInt16() != BlockMarker)
                throw new ScadaException("Row marker not found.");

            int rowDataSize = reader.ReadInt32();
            int fullRowSize = rowDataSize + 6;
            ResizeBuffer(ref buffer, fullRowSize, 2);
            ReadData(reader, buffer, 6, rowDataSize, true);

            // copy values to the buffer to calculate CRC
            CopyUInt16(BlockMarker, buffer, 0);
            CopyInt32(rowDataSize, buffer, 2);

            if (ScadaUtils.CRC16(buffer, 0, fullRowSize - 2) != BitConverter.ToUInt16(buffer, fullRowSize - 2))
                throw new ScadaException("Row CRC error.");
        }

        /// <summary>
        /// Writes the field definintion using the specified writer.
        /// </summary>
        protected void WriteFieldDef(BinaryWriter writer, FieldDef fieldDef, byte[] buffer)
        {
            Array.Clear(buffer, 0, FieldDefSize);
            int nameLength = fieldDef.Name.Length;
            buffer[0] = (byte)nameLength;
            Encoding.ASCII.GetBytes(fieldDef.Name).CopyTo(buffer, 1);
            buffer[MaxFieldNameLength + 1] = (byte)fieldDef.DataType;
            buffer[MaxFieldNameLength + 2] = (byte)(fieldDef.AllowNull ? 1 : 0);
            ushort crc = ScadaUtils.CRC16(buffer, 0, FieldDefSize - 2);
            CopyUInt16(crc, buffer, FieldDefSize - 2);
            writer.Write(buffer, 0, FieldDefSize);
        }

        /// <summary>
        /// Gets the field data to write.
        /// </summary>
        protected byte[] GetFieldData(FieldDef fieldDef, object value, byte[] buffer)
        {
            if (fieldDef.DataSize > 0 && (buffer == null || buffer.Length != fieldDef.DataSize))
                buffer = new byte[fieldDef.DataSize];

            switch (fieldDef.DataType)
            {
                case ColumnDataType.Integer:
                    CopyInt32((int)value, buffer, 0);
                    break;
                case ColumnDataType.Double:
                    CopyDouble((double)value, buffer, 0);
                    break;
                case ColumnDataType.Boolean:
                    CopyBool((bool)value, buffer, 0);
                    break;
                case ColumnDataType.DateTime:
                    CopyTime((DateTime)value, buffer, 0);
                    break;
                case ColumnDataType.String:
                    buffer = Encoding.UTF8.GetBytes((string)value);
                    if (buffer.Length > MaxFieldLenght)
                        throw new ArgumentException("String length exceeded.");
                    break;
                default:
                    buffer = EmptyBuffer;
                    break;
            }

            return buffer;
        }

        /// <summary>
        /// Gets the field value from the buffer.
        /// </summary>
        protected object GetFieldValue(ColumnDataType dataType, int dataSize, byte[] buffer, ref int index)
        {
            switch (dataType)
            {
                case ColumnDataType.Integer:
                    return GetInt32(buffer, ref index);
                case ColumnDataType.Double:
                    return GetDouble(buffer, ref index);
                case ColumnDataType.Boolean:
                    return GetBool(buffer, ref index);
                case ColumnDataType.DateTime:
                    return GetTime(buffer, ref index);
                case ColumnDataType.String:
                    string s = dataSize > 0 ? Encoding.UTF8.GetString(buffer, index, dataSize) : "";
                    index += dataSize;
                    return s;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the field value if it exists, or null.
        /// </summary>
        protected object GetFieldValueIfExists(FieldDef fieldDef, byte[] buffer, ref int index)
        {
            if (GetBool(buffer, ref index)) // value is null
            {
                return null;
            }
            else
            {
                int dataSize = fieldDef.DataSize > 0 ? fieldDef.DataSize : GetUInt16(buffer, ref index);
                return GetFieldValue(fieldDef.DataType, dataSize, buffer, ref index);
            }
        }


        /// <summary>
        /// Fills the specified table by reading data from the configuration database.
        /// </summary>
        public void Fill(IBaseTable baseTable)
        {
            if (baseTable == null)
                throw new ArgumentNullException(nameof(baseTable));

            Stream stream;
            BinaryReader reader = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new BinaryReader(stream, Encoding.UTF8, Stream != null);

                // prepare table
                baseTable.ClearItems();
                baseTable.IndexesEnabled = false;

                // read header
                byte[] buffer = new byte[Math.Max(HeaderSize, FieldDefSize)];
                if (!ReadHeader(reader, buffer, out int fieldCount))
                    return;

                // read field definitions
                FieldDef[] fieldDefs = ReadFieldDefs(reader, buffer, fieldCount);

                // map the field definitions and entity properties
                PropertyDescriptorCollection allProps = TypeDescriptor.GetProperties(baseTable.ItemType);
                PropertyDescriptor[] props = new PropertyDescriptor[fieldCount];

                for (int i = 0; i < fieldCount; i++)
                {
                    props[i] = allProps[fieldDefs[i].Name];
                }

                // read rows
                while (true)
                {
                    // read row data
                    ReadRowToBuffer(reader, ref buffer);

                    // read fields
                    object item = baseTable.NewItem();
                    int index = 6;

                    for (int i = 0; i < fieldCount; i++)
                    {
                        FieldDef fieldDef = fieldDefs[i];
                        PropertyDescriptor prop = props[i];
                        object value = GetFieldValueIfExists(fieldDef, buffer, ref index);

                        if (prop != null)
                            prop.SetValue(item, value ?? (fieldDef.AllowNull ? null : fieldDef.DefaultValue));
                    }

                    baseTable.AddObject(item);
                }
            }
            catch (EndOfStreamException)
            {
                // normal file ending case
            }
            finally
            {
                reader?.Close();
                baseTable.IndexesEnabled = true;
            }
        }

        /// <summary>
        /// Fills the specified table by reading data from the configuration database.
        /// </summary>
        public void Fill(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));

            Stream stream;
            BinaryReader reader = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new BinaryReader(stream, Encoding.UTF8, Stream != null);

                // prepare table
                dataTable.Rows.Clear();
                dataTable.BeginLoadData();
                dataTable.DefaultView.Sort = "";

                // read header
                byte[] buffer = new byte[Math.Max(HeaderSize, FieldDefSize)];
                if (!ReadHeader(reader, buffer, out int fieldCount))
                    return;

                // read field definitions
                FieldDef[] fieldDefs = ReadFieldDefs(reader, buffer, fieldCount);

                // create table columns
                if (dataTable.Columns.Count == 0)
                {
                    foreach (FieldDef fieldDef in fieldDefs)
                    {
                        DataColumn column = new DataColumn(fieldDef.Name) { AllowDBNull = fieldDef.AllowNull };
                        dataTable.Columns.Add(column);

                        switch (fieldDef.DataType)
                        {
                            case ColumnDataType.Integer:
                                column.DataType = typeof(int);
                                break;
                            case ColumnDataType.Double:
                                column.DataType = typeof(double);
                                break;
                            case ColumnDataType.Boolean:
                                column.DataType = typeof(bool);
                                break;
                            case ColumnDataType.DateTime:
                                column.DataType = typeof(DateTime);
                                break;
                            case ColumnDataType.String:
                                column.DataType = typeof(string);
                                break;
                        }
                    }
                }

                // read rows
                while (true)
                {
                    // read row data
                    ReadRowToBuffer(reader, ref buffer);

                    // read fields
                    DataRow row = dataTable.NewRow();
                    int index = 6;

                    foreach (FieldDef fieldDef in fieldDefs)
                    {
                        int columnIndex = dataTable.Columns.IndexOf(fieldDef.Name);
                        object value = GetFieldValueIfExists(fieldDef, buffer, ref index);

                        if (columnIndex >= 0)
                            row[columnIndex] = value ?? (fieldDef.AllowNull ? DBNull.Value : fieldDef.DefaultValue);
                    }

                    dataTable.Rows.Add(row);
                }
            }
            catch (EndOfStreamException)
            {
                // normal file ending case
            }
            finally
            {
                reader?.Close();
                dataTable.EndLoadData();
                dataTable.AcceptChanges();

                if (dataTable.Columns.Count > 0)
                    dataTable.DefaultView.Sort = dataTable.Columns[0].ColumnName;
            }
        }

        /// <summary>
        /// Updates the configuration database by writing data of the specified table.
        /// </summary>
        public void Update(IBaseTable baseTable)
        {
            if (baseTable == null)
                throw new ArgumentNullException(nameof(baseTable));

            Stream stream;
            BinaryWriter writer = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                writer = new BinaryWriter(stream, Encoding.UTF8, Stream != null);

                // write header
                writer.Write(TableType.BaseTable);
                writer.Write(MajorVersion);
                writer.Write(MinorVersion);

                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);
                int fieldCount = Math.Min(props.Count, ushort.MaxValue);
                writer.Write((ushort)fieldCount);
                writer.Write(ReserveBuffer, 0, 12);

                if (fieldCount > 0)
                {
                    // create and write field definitions
                    FieldDef[] fieldDefs = new FieldDef[fieldCount];
                    byte[] buffer = new byte[FieldDefSize];

                    for (int i = 0; i < fieldCount; i++)
                    {
                        PropertyDescriptor prop = props[i];
                        bool isNullable = prop.PropertyType.IsNullable();

                        FieldDef fieldDef = new FieldDef(
                            prop.Name,
                            isNullable ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType,
                            isNullable || prop.PropertyType.IsClass);

                        fieldDefs[i] = fieldDef;
                        WriteFieldDef(writer, fieldDef, buffer);
                    }

                    // write rows
                    byte[][] rowData = new byte[fieldCount][];
                    bool[] isNullArr = new bool[fieldCount];

                    foreach (object item in baseTable.EnumerateItems())
                    {
                        // get row data and size
                        int rowDataSize = 2; // CRC

                        for (int i = 0; i < fieldCount; i++)
                        {
                            object value = props[i].GetValue(item);

                            if (value == null)
                            {
                                isNullArr[i] = true;
                            }
                            else
                            {
                                FieldDef fieldDef = fieldDefs[i];
                                byte[] fieldData = GetFieldData(fieldDef, value, rowData[i]);
                                rowData[i] = fieldData;
                                isNullArr[i] = false;
                                rowDataSize += (fieldDef.DataSize <= 0 ? 2 : 0) + fieldData.Length;
                            }

                            rowDataSize++; // null flag
                        }

                        // copy row data to the buffer
                        int fullRowSize = rowDataSize + 6;
                        int copyIndex = 0;
                        ResizeBuffer(ref buffer, fullRowSize, 2);
                        CopyUInt16(BlockMarker, buffer, ref copyIndex);
                        CopyInt32(rowDataSize, buffer, ref copyIndex);

                        for (int i = 0; i < fieldCount; i++)
                        {
                            if (isNullArr[i])
                            {
                                buffer[copyIndex++] = 1;
                            }
                            else
                            {
                                buffer[copyIndex++] = 0;
                                FieldDef fieldDef = fieldDefs[i];
                                byte[] fieldData = rowData[i];

                                if (fieldDef.DataSize <= 0)
                                    CopyUInt16((ushort)fieldData.Length, buffer, ref copyIndex);

                                fieldData.CopyTo(buffer, copyIndex);
                                copyIndex += fieldData.Length;
                            }
                        }

                        ushort crc = ScadaUtils.CRC16(buffer, 0, fullRowSize - 2);
                        CopyUInt16(crc, buffer, fullRowSize - 2);

                        // write row data
                        writer.Write(buffer, 0, fullRowSize);
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
