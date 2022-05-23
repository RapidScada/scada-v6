// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scada.Admin.Extensions.ExtProjectTools.Code
{
    /// <summary>
    /// Generates a channel map of the configuration database.
    /// <para>Генерирует карту каналов базы конфигурации.</para>
    /// </summary>
    internal class ChannelMap
    {
        /// <summary>
        /// The file name of newly created maps.
        /// </summary>
        public const string MapFileName = "ScadaAdmin_ChannelMap.txt";

        private readonly ILog log;                      // the application log
        private readonly ConfigDatabase configDatabase; // the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelMap(ILog log, ConfigDatabase configDatabase)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
            GroupByDevices = true;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to group channels by devices. Otherwise group by objects.
        /// </summary>
        public bool GroupByDevices { get; set; }


        /// <summary>
        /// Writes channels having the specified index key.
        /// </summary>
        private static void WriteCnls(StreamWriter writer, ITableIndex index, int indexKey)
        {
            List<int> keys = new(index.SelectItemKeys(indexKey));
            writer.WriteLine(keys.Count > 0
                ? ExtensionPhrases.ChannelsCaption + keys.ToRangeString() 
                : ExtensionPhrases.NoChannels);
        }

        /// <summary>
        /// Generates a channel map.
        /// </summary>
        public void Generate(string mapFileName)
        {
            try
            {
                using (StreamWriter writer = new(mapFileName, false, Encoding.UTF8))
                {
                    string indexedColumn = GroupByDevices ? "DeviceNum" : "ObjNum";
                    string title = GroupByDevices ? 
                        ExtensionPhrases.MapByDeviceTitle : ExtensionPhrases.MapByObjectTitle;
                    writer.WriteLine(title);
                    writer.WriteLine(new string('-', title.Length));

                    if (configDatabase.CnlTable.TryGetIndex(indexedColumn, out ITableIndex tableIndex))
                    {
                        if (GroupByDevices)
                        {
                            foreach (Device device in configDatabase.DeviceTable.EnumerateItems())
                            {
                                writer.WriteLine(string.Format(CommonPhrases.EntityCaption, 
                                    device.DeviceNum, device.Name));
                                WriteCnls(writer, tableIndex, device.DeviceNum);
                                writer.WriteLine();
                            }

                            writer.WriteLine(AdminPhrases.EmptyDevice);
                        }
                        else
                        {
                            foreach (Obj obj in configDatabase.ObjTable.EnumerateItems())
                            {
                                writer.WriteLine(string.Format(CommonPhrases.EntityCaption, obj.ObjNum, obj.Name));
                                WriteCnls(writer, tableIndex, obj.ObjNum);
                                writer.WriteLine();
                            }

                            writer.WriteLine(AdminPhrases.EmptyObject);
                        }

                        // channels with unspecified device or object
                        WriteCnls(writer, tableIndex, 0);
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
                log.HandleError(ex, ExtensionPhrases.GenerateChannelMapError);
            }
        }
    }
}
