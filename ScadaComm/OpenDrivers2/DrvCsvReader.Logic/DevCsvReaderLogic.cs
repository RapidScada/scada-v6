// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvCsvReader.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevCsvReaderLogic : DeviceLogic
    {
        private readonly CsvReaderOptions options; // the CSV reader options
        private FileStream fileStream;             // the stream of the data file
        private TextReader textReader;             // reads the data file


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevCsvReaderLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            options = new CsvReaderOptions(deviceConfig.PollingOptions.CustomOptions);
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
            TagGroup tagGroup = new();

            for (int tagNum = 1; tagNum <= options.TagCount; tagNum++)
            {
                string tagCode = DriverUtils.GetTagCode(tagNum);
                tagGroup.AddTag(tagCode, tagCode);
            }

            DeviceTags.AddGroup(tagGroup);
            DeviceTags.FlattenGroups = true;
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();

            if (textReader != null)
            {

            }

            FinishRequest();
            FinishSession();
        }
    }
}
