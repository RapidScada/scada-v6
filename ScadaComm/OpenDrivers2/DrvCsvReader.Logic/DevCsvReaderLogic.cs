// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvSms;
using Scada.Data.Const;
using Scada.Lang;
using System.Globalization;
using static System.Runtime.CompilerServices.RuntimeHelpers;

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
        /// The string that identifies a header row.
        /// </summary>
        private const string HeaderText = "Timestamp";

        private readonly CsvReaderOptions options; // the CSV reader options
        private readonly NumberFormatInfo nfi;     // the value format
        private FileStream fileStream;             // the stream of the data file
        private TextReader textReader;             // reads the data file


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

                    Log.WriteAction(Locale.IsRussian ?
                        "{0}: Открыт файл данных {1}" :
                        "{0}: Data file {1} opened", Title, filePath);
                }
                catch (Exception ex)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Ошибка {0}: Не удалось открыть файл данных: {1}" :
                        "Error {0}: Unable to open data file: {1}", Title, ex.Message);
                }
            }
            else
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка {0}: Не найден файл данных {1}" :
                    "Error {0}: Data file {0} not found", Title, filePath);
            }
        }

        /// <summary>
        /// Reads data from the file.
        /// </summary>
        private void ReadData()
        {
            try
            {
                if (textReader != null)
                {
                    if (options.ReadMode == ReadMode.RealTime)
                        ReadDataRealTime();
                    else
                        ReadDataDemo();
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
            string line = textReader.ReadLine();

            if (line == null)
            {

            }
            else
            {
                string[] parts = line.Split(options.FieldDelimiter);

                if (parts[0] == HeaderText)
                {
                    // get tag names from header
                }
                else
                {
                    DateTime utcNow = DateTime.UtcNow;

                    if (DateTime.TryParse(parts[0], out DateTime timestamp))
                    {
                        timestamp = DateTime.SpecifyKind(timestamp, DateTimeKind.Utc);
                        DeviceData.SetDateTime(TagCode.Timestamp, timestamp, CnlStatusID.Defined);
                    }
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
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            OpenDataFile();
        }

        /// <summary>
        /// Performs actions when terminating a communication line.
        /// </summary>
        public override void OnCommLineTerminate()
        {
            fileStream?.Dispose();
            textReader?.Dispose();
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
