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
 * Module   : ScadaServerCommon
 * Summary  : Represents the base class for current data archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Server.Config;

namespace Scada.Server.Archives
{
    /// <summary>
    /// Represents the base class for current data archive logic.
    /// <para>Представляет базовый класс логики архива текщих данных.</para>
    /// </summary>
    /// <remarks>Descendants of this class must be thread-safe.</remarks>
    public abstract class CurrentArchiveLogic : ArchiveLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CurrentArchiveLogic(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums)
            : base(archiveContext, archiveConfig, cnlNums)
        {
        }


        /// <summary>
        /// Reads the current data.
        /// </summary>
        public abstract void ReadData(ICurrentData curData, out bool completed);

        /// <summary>
        /// Writes the current data.
        /// </summary>
        public abstract void WriteData(ICurrentData curData);

        /// <summary>
        /// Processes new data.
        /// </summary>
        /// <remarks>Returns true if the data has been written to the archive.</remarks>
        public abstract void ProcessData(ICurrentData curData);
    }
}
