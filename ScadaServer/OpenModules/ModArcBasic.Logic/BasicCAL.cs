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
 * Module   : ModArcBasic
 * Summary  : Implements the current data archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Config;
using Scada.Data.Adapters;
using Scada.Data.Models;
using Scada.Server.Archives;
using Scada.Server.Config;
using System;
using System.IO;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// Implements the current data archive logic.
    /// <para>Реализует логику архива текщих данных.</para>
    /// </summary>
    internal class BasicCAL : CurrentArchiveLogic
    {
        /// <summary>
        /// Represents archive options.
        /// </summary>
        private class ArchiveOptions
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public ArchiveOptions(CustomOptions options)
            {
                IsCopy = options.GetValueAsBool("IsCopy");
                WritingPeriod = options.GetValueAsInt("WritingPeriod", 10);
            }

            /// <summary>
            /// Gets or sets a value indicating whether the archive stores a copy of the data.
            /// </summary>
            public bool IsCopy { get; set; }
            /// <summary>
            /// Gets the period of writing data to a file, sec.
            /// </summary>
            public int WritingPeriod { get; set; }
        }

        /// <summary>
        /// The current data file name.
        /// </summary>
        private const string CurDataFileName = "current.dat";

        private readonly ArchiveOptions options;    // the archive options
        private readonly SliceTableAdapter adapter; // reads and writes current data
        private readonly Slice slice;   // the slice for writing
        private DateTime nextWriteTime; // the next time to write the current data
        private int[] cnlIndices;       // the indices that map the input channels


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicCAL(ArchiveConfig archiveConfig, int[] cnlNums, PathOptions pathOptions)
            : base(archiveConfig, cnlNums)
        {
            options = new ArchiveOptions(archiveConfig.CustomOptions);
            adapter = new SliceTableAdapter { FileName = GetCurDataPath(pathOptions) };
            slice = new Slice(DateTime.MinValue, cnlNums, new CnlData[cnlNums.Length]);
            nextWriteTime = GetNextWriteTime(DateTime.UtcNow, options.WritingPeriod);
            cnlIndices = null;
        }


        /// <summary>
        /// Gets the full file name of the current data file.
        /// </summary>
        private string GetCurDataPath(PathOptions pathOptions)
        {
            string arcDir = options.IsCopy ? pathOptions.ArcCopyDir : pathOptions.ArcDir;
            return Path.Combine(arcDir, Code, CurDataFileName);
        }

        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(adapter.FileName));
        }

        /// <summary>
        /// Reads the current data.
        /// </summary>
        public override void ReadData(ICurrentData curData, out bool completed)
        {
            if (File.Exists(adapter.FileName))
            {
                Slice slice = adapter.ReadSingleSlice();

                for (int i = 0, cnlCnt = slice.CnlNums.Length; i < cnlCnt; i++)
                {
                    int cnlNum = slice.CnlNums[i];
                    int cnlIndex = curData.GetCnlIndex(cnlNum);

                    if (cnlIndex >= 0)
                    {
                        curData.Timestamps[cnlIndex] = slice.Timestamp;
                        curData.CnlData[cnlIndex] = slice.CnlData[i];
                    }
                }

                completed = true;
            }
            else
            {
                completed = false;
            }
        }

        /// <summary>
        /// Writes the current data.
        /// </summary>
        public override void WriteData(ICurrentData curData)
        {
            InitCnlIndices(curData, ref cnlIndices);
            CopyCnlData(curData, slice, cnlIndices);
            adapter.WriteSingleSlice(slice);
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        public override bool ProcessData(ICurrentData curData)
        {
            if (nextWriteTime <= curData.Timestamp)
            {
                nextWriteTime = GetNextWriteTime(curData.Timestamp, options.WritingPeriod);
                WriteData(curData);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
