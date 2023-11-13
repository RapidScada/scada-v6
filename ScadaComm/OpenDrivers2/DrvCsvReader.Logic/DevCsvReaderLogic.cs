// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvSms;
using Scada.Comm.Lang;
using Scada.Data.Const;
using Scada.Lang;
using System.Data;
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
        /// The initial year in Demo mode.
        /// </summary>
        private const int InitialYearDemo = 2001;
        /// <summary>
        /// Specifies the time interval when data is considered outdated.
        /// </summary>
        private static readonly TimeSpan DataLifetime = TimeSpan.FromMinutes(1);

        private readonly CsvReaderOptions options; // the CSV reader options
        private readonly NumberFormatInfo nfi;     // the value format
        private FileStream fileStream;             // the stream of the data file
        private TextReader textReader;             // reads the data file
        private string[] tagNames;                 // the tag names from file
        private bool justOpened;                   // indicates that the data file is just opened
        private long lastFileSize;                 // the file size measured after iteration
        private DateTime lastTimestamp;            // the last timestamp parsed
        private DateTime prevVirtualTime;          // the previous time in Demo mode
        private DataRow prevDataRow;               // the previously read data row


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
            tagNames = Array.Empty<string>();
            justOpened = false;
            lastFileSize = 0;
            lastTimestamp = DateTime.MinValue;
            prevVirtualTime = DateTime.MinValue;
            prevDataRow = null;

            ConnectionRequired = false;
        }


        /// <summary>
        /// Defines a full path and opens a data file.
        /// </summary>
        private bool OpenDataFile()
        {
            string fileName = string.IsNullOrEmpty(options.FileName)
                ? $"{DriverUtils.DriverCode}_{DeviceNum:D3}.csv"
                : options.FileName;

            string filePath = Path.IsPathRooted(fileName)
                ? fileName
                : Path.Combine(AppDirs.StorageDir, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    textReader = new StreamReader(fileStream);
                    justOpened = true;

                    Log.WriteLine(Locale.IsRussian ?
                        "Открыт файл данных {0}" :
                        "Data file {0} opened", filePath);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Ошибка: Не удалось открыть файл данных: {0}" :
                        "Error: Unable to open data file: {0}", ex.Message);
                    return false;
                }
            }
            else
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка: Не найден файл данных {0}" :
                    "Error: Data file {0} not found", filePath);
                return false;
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
        /// Reads tag names from the file.
        /// </summary>
        private void ReadTagNames()
        {
            Log.WriteLine();
            Log.WriteAction(Locale.IsRussian ?
                "Чтение наименований тегов для устройства {0}" :
                "Read tag names for the device {0}", Title);

            if (OpenDataFile())
            {
                try
                {
                    string header = textReader.ReadLine();
                    string[] lineParts = header.Split(options.FieldDelimiter);

                    if (lineParts[0] == HeaderText && lineParts.Length > 1)
                        tagNames = lineParts.AsSpan(1).ToArray();
                }
                catch (Exception ex)
                {
                    Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
                }
            }
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
                    InvalidateData();
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
            bool newDataFound = false;
            string line;

            if (fileStream.Length < lastFileSize)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Файл был перезаписан" :
                    "File was overwritten");
                justOpened = true;
            }

            if (justOpened)
            {
                line = ReadLastLine();
                justOpened = false;
            }
            else
            {
                line = textReader.ReadLine();
            }

            while (line != null)
            {
                DateTime utcNow = DateTime.UtcNow;

                if (ParseDataRow(line, out DataRow dataRow) && DataRowIsCurrent(dataRow, utcNow))
                {
                    newDataFound = true;
                    lastTimestamp = dataRow.Timestamp;
                    CopyValues(dataRow);
                }

                line = textReader.ReadLine();
            }

            LogNewData(newDataFound);
        }

        /// <summary>
        /// Reads data in Demo mode.
        /// </summary>
        private void ReadDataDemo()
        {
            DateTime virtualTime = GetVirtualTime();
            bool newDataFound = false;
            DataRow dataRow;

            // start over
            if (prevVirtualTime > virtualTime)
            {
                prevDataRow = null;
                fileStream.Seek(0, SeekOrigin.Begin);
            }

            // read data
            string line = "";
            dataRow = prevDataRow;

            while (line != null && (dataRow == null || dataRow.Timestamp < virtualTime))
            {
                line = textReader.ReadLine();
                dataRow = ParseDataRow(line);
            }

            // get tag values
            if (DataRowIsCurrent(dataRow, virtualTime))
            {
                newDataFound = prevDataRow != dataRow;
                lastTimestamp = dataRow.Timestamp;
                CopyValues(dataRow);
            }

            LogNewData(newDataFound);
            prevVirtualTime = virtualTime;
            prevDataRow = dataRow;
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
        private bool ParseDataRow(string line, out DataRow dataRow)
        {
            if (!string.IsNullOrEmpty(line))
            {
                string[] lineParts = line.Split(options.FieldDelimiter);

                if (DateTime.TryParse(lineParts[0], out DateTime timestamp))
                {
                    timestamp = DateTime.SpecifyKind(timestamp, DateTimeKind.Utc);
                    dataRow = new DataRow(options.TagCount) { Timestamp = timestamp };
                    RetrieveValues(lineParts, dataRow);
                    return true;
                }
            }

            dataRow = null;
            return false;
        }

        /// <summary>
        /// Converts the line parts to a data row.
        /// </summary>
        private DataRow ParseDataRow(string line)
        {
            ParseDataRow(line, out DataRow dataRow);
            return dataRow;
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
        /// Checks whether the timestamp of the data row is close to the current time.
        /// </summary>
        private static bool DataRowIsCurrent(DataRow dataRow, DateTime currentTime)
        {
            return dataRow != null &&
                currentTime - DataLifetime <= dataRow.Timestamp && dataRow.Timestamp <= currentTime + DataLifetime;
        }

        /// <summary>
        /// Logs a message indicating whether new data was found or not.
        /// </summary>
        private void LogNewData(bool newDataFound)
        {
            if (newDataFound)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Считаны новые данные" :
                    "New data has been read");
            }
            else
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Новых данных нет" :
                    "No new data available");
            }
        }

        /// <summary>
        /// Получить виртуальное время на основе текущего времени и установленного периода.
        /// </summary>
        private DateTime GetVirtualTime()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime startDate = new(InitialYearDemo, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return options.DemoPeriod switch
            {
                DemoPeriod.OneHour => startDate.Add(utcNow.TimeOfDay).AddHours(-utcNow.Hour),
                DemoPeriod.OneDay => startDate.Add(utcNow.TimeOfDay),
                DemoPeriod.OneMonth => startDate.Add(utcNow.TimeOfDay).AddDays(utcNow.Day - 1),
                _ => throw new ScadaException("Uknonwn demo period.")
            }; ;
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            ReadTagNames();
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
                tagGroup.AddTag(
                    tagCode, 
                    tagNum <= tagNames.Length ? tagNames[tagNum - 1] : tagCode);
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
