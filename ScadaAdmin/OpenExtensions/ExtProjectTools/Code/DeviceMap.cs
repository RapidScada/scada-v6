// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Admin.Extensions.ExtProjectTools.Code
{
    /// <summary>
    /// Generates a device map of the configuration database.
    /// <para>Генерирует карту устройств базы конфигурации.</para>
    /// </summary>
    internal class DeviceMap
    {
        /// <summary>
        /// The file name of newly created maps.
        /// </summary>
        public const string MapFileName = "ScadaAdmin_DeviceMap.txt";

        private readonly ILog log;              // the application log
        private readonly ConfigBase configBase; // the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceMap(ILog log, ConfigBase configBase)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.configBase = configBase ?? throw new ArgumentNullException(nameof(configBase));
        }


        /// <summary>
        /// Writes channels having the specified index key.
        /// </summary>
        private static void WriteDevices(StreamWriter writer, ITableIndex index, int indexKey)
        {
            List<int> keys = new(index.SelectItemKeys(indexKey));
            writer.WriteLine(keys.Count > 0
                ? ExtensionPhrases.DevicesCaption + keys.ToRangeString()
                : ExtensionPhrases.NoDevices);
        }

        /// <summary>
        /// Generates a device map.
        /// </summary>
        public void Generate(string mapFileName)
        {
            try
            {
                using (StreamWriter writer = new(mapFileName, false, Encoding.UTF8))
                {
                    writer.WriteLine(ExtensionPhrases.DeviceMapTitle);
                    writer.WriteLine(new string('-', ExtensionPhrases.DeviceMapTitle.Length));

                    if (configBase.DeviceTable.TryGetIndex("CommLineNum", out ITableIndex tableIndex))
                    {
                        foreach (CommLine commLine in configBase.CommLineTable.EnumerateItems())
                        {
                            writer.WriteLine(string.Format(CommonPhrases.EntityCaption,
                                commLine.CommLineNum, commLine.Name));
                            WriteDevices(writer, tableIndex, commLine.CommLineNum);
                            writer.WriteLine();
                        }

                        // devices with unspecified communication line
                        writer.WriteLine(ExtensionPhrases.EmptyCommLine);
                        WriteDevices(writer, tableIndex, 0);
                    }
                    else
                    {
                        throw new ScadaException(CommonPhrases.IndexNotFound);
                    }
                }

                ScadaUiUtils.StartProcess(mapFileName);
            }
            catch (Exception ex)
            {
                log.HandleError(ex, ExtensionPhrases.GenerateDeviceMapError);
            }
        }
    }
}
