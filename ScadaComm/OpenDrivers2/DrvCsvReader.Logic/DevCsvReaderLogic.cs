// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvSms;
using Scada.Data.Const;
using Scada.Lang;
using System.Globalization;

namespace Scada.Comm.Drivers.DrvCsvReader.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevCsvReaderLogic : DeviceLogic
    {
        /// <summary>
        /// Represents a data row.
        /// </summary>
        private class DataRow
        {
            public DataRow(int tagCount)
            {
                Timestamp = DateTime.MinValue;
                Values = new double[tagCount];
            }
            public DateTime Timestamp { get; set; }
            public double[] Values { get; }
        }

        /// <summary>
        /// The maximum line length in bytes.
        /// </summary>
        private const int MaxLineLength = 1024;
        /// <summary>
        /// The string that identifies a header row.
        /// </summary>
        private const string HeaderText = "Timestamp";
        /// <summary>
        /// Specifies the time interval when data is considered outdated.
        /// </summary>
        private static readonly TimeSpan DataLifetime = TimeSpan.FromMinutes(1);

        private readonly CsvReaderOptions options; // the CSV reader options
        private readonly NumberFormatInfo nfi;     // the value format
        private FileStream fileStream;             // the stream of the data file
        private TextReader textReader;             // reads the data file
        private bool justOpened;                   // indicates that the data file is just opened
        private long lastFileSize;                 // the file size measured after iteration
        private DateTime lastTimestamp;            // the last timestamp parsed


        /// <summary>
        /// Gets a value indicating whether the data file is open.
        /// </summary>
        private bool FileIsOpen => textReader != null;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevCsvReaderLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            options = new CsvReaderOptions(deviceConfig.PollingOptions.CustomOptions);
            nfi = new NumberFormatInfo { NumberDecimalSeparator = options.DecimalSeparator };
            fileStream = null;
            textReader = null;
            justOpened = false;
            lastFileSize = 0;
            lastTimestamp = DateTime.MinValue;

            ConnectionRequired = false;
        }


        /// <summary>
        /// Defines a full path and opens a data file.
        /// </summary>
        private void OpenDataFile()
        {
            string fileName = string.IsNullOrEmpty(options.FileName)
                ? $"{DriverUtils.DriverCode}_{DeviceNum:D3}.csv"
                : options.FileName;

            string filePath = Path.IsPathRooted(fileName)
                ? fileName
                : Path.Combine(AppDirs.ConfigDir, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    textReader = new StreamReader(fileStream);
                    justOpened = true;

                    Log.WriteLine(Locale.IsRussian ?
                        "Открыт файл данных {1}" :
                        "Data file {1} opened", filePath);
                }
                catch (Exception ex)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Ошибка: Не удалось открыть файл данных: {1}" :
                        "Error: Unable to open data file: {1}", ex.Message);
                }
            }
            else
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка: Не найден файл данных {1}" :
                    "Error: Data file {0} not found", filePath);
            }
        }

        /// <summary>
        /// Closes the data file if it is open.
        /// </summary>
        private void CloseDataFile()
        {
            fileStream?.Dispose();
            textReader?.Dispose();
            justOpened = false;
            lastFileSize = 0;
            lastTimestamp = DateTime.MinValue;
        }

        /// <summary>
        /// Reads data from the file.
        /// </summary>
        private void ReadData()
        {
            if (!FileIsOpen)
                OpenDataFile();

            try
            {
                if (FileIsOpen)
                {
                    if (options.ReadMode == ReadMode.RealTime)
                        ReadDataRealTime();
                    else
                        ReadDataDemo();

                    DeviceData.SetDateTime(TagCode.Timestamp, lastTimestamp, 
                        lastTimestamp > DateTime.MinValue ? CnlStatusID.Defined : CnlStatusID.Undefined);
                    DeviceData.Set(TagCode.Position, fileStream.Position);
                    lastFileSize = fileStream.Length;
                }
                else
                {
                    DeviceData.Invalidate(TagCode.Timestamp);
                    DeviceData.Invalidate(TagCode.Position);
                }
            }
            catch (Exception ex)
            {
                DeviceData.Invalidate(0, options.TagCount);
                LastRequestOK = false;
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка при чтении из файла: {0}" :
                    "Error reading from file: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Reads data in RealTime mode.
        /// </summary>
        private void ReadDataRealTime()
        {
            if (justOpened)
                ParseHeader(textReader.ReadLine());
            else if (fileStream.Length < lastFileSize)
                justOpened = true; // file was overwritten

            string line = justOpened
                ? ReadLastLine()
                : textReader.ReadLine();

            if (line == null)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Новых данных нет" :
                    "No new data available");
            }
            else
            {
                while (line != null)
                {
                    DateTime utcNow = DateTime.UtcNow;
                    string[] lineParts = line.Split(options.FieldDelimiter);

                    if (ParseDataRow(lineParts, out DataRow dataRow) &&
                        utcNow - DataLifetime <= dataRow.Timestamp && dataRow.Timestamp <= utcNow + DataLifetime)
                    {
                        lastTimestamp = dataRow.Timestamp;
                        CopyValues(dataRow);
                    }

                    line = textReader.ReadLine();
                }
            }
        }

        /// <summary>
        /// Reads data in Demo mode.
        /// </summary>
        private void ReadDataDemo()
        {

        }

        /// <summary>
        /// Parses the header to obtain tag names.
        /// </summary>
        private void ParseHeader(string header)
        {
            if (header != null)
            {
                string[] lineParts = header.Split(options.FieldDelimiter);
                
                if (lineParts[0] == HeaderText)
                {
                    RetrieveTagNames(lineParts);
                }
            }
        }

        /// <summary>
        /// Retrieves the tag names from the line parts.
        /// </summary>
        private void RetrieveTagNames(string[] lineParts)
        {
            for (int i = 0, len = Math.Min(lineParts.Length - 1, options.TagCount); i < len; i++)
            {
                DeviceTags[i].Name = lineParts[i + 1];
            }
        }

        /// <summary>
        /// Reads the last line in the data file.
        /// </summary>
        private string ReadLastLine()
        {
            if (fileStream.Length <= MaxLineLength)
                fileStream.Seek(0, SeekOrigin.Begin);
            else
                fileStream.Seek(-MaxLineLength, SeekOrigin.End);

            // skip first line
            if (textReader.ReadLine() == null)
                return null;

            // search for last line
            string currentLine;
            string lastLine = null;

            do
            {
                currentLine = textReader.ReadLine();

                if (currentLine != null)
                    lastLine = currentLine;
            } while (currentLine != null);

            return lastLine;
        }

        /// <summary>
        /// Converts the line parts to a data row.
        /// </summary>
        private bool ParseDataRow(string[] lineParts, out DataRow dataRow)
        {
            if (DateTime.TryParse(lineParts[0], out DateTime timestamp))
            {
                timestamp = DateTime.SpecifyKind(timestamp, DateTimeKind.Utc);
                dataRow = new DataRow(options.TagCount) { Timestamp = timestamp };
                RetrieveValues(lineParts, dataRow);
                return true;
            }
            else
            {
                dataRow = null;
                return false;
            }
        }

        /// <summary>
        /// Retrieves data values from the line parts.
        /// </summary>
        private void RetrieveValues(string[] lineParts, DataRow dataRow)
        {
            for (int valIdx = 0; valIdx < dataRow.Values.Length; valIdx++)
            {
                int partIdx = valIdx + 1;
                dataRow.Values[valIdx] = partIdx <= lineParts.Length && 
                    double.TryParse(lineParts[partIdx], NumberStyles.Float, nfi, out double value)
                    ? value 
                    : double.NaN;
            }
        }

        /// <summary>
        /// Copies the values of the data row to the device data.
        /// </summary>
        private void CopyValues(DataRow dataRow)
        {
            for (int i = 0; i < dataRow.Values.Length; i++)
            {
                DeviceData.Set(i, dataRow.Values[i]);
            }
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
        }

        /// <summary>
        /// Performs actions when terminating a communication line.
        /// </summary>
        public override void OnCommLineTerminate()
        {
            CloseDataFile();
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            // Main Data group
            TagGroup tagGroup = new("Main Data");

            for (int tagNum = 1; tagNum <= options.TagCount; tagNum++)
            {
                string tagCode = TagCode.GetMainTagCode(tagNum);
                tagGroup.AddTag(tagCode, tagCode);
            }

            DeviceTags.AddGroup(tagGroup);

            // Reading Status group
            tagGroup = new("Reading Status");
            tagGroup.AddTag(TagCode.Timestamp, TagCode.Timestamp).SetFormat(TagFormat.DateTime);
            tagGroup.AddTag(TagCode.Position, TagCode.Position).SetFormat(TagFormat.IntNumber);
            DeviceTags.AddGroup(tagGroup);
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();
            ReadData();
            FinishRequest();
            FinishSession();
        }
    }
}
