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
 * Module   : Webstation Application
 * Summary  : Provides information to identify application statistics
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2024
 */

using Scada.Lang;
using Scada.Log;
using Scada.Storages;
using System;

namespace Scada.Web.Code
{
    /// <summary>
    /// Provides information to identify application statistics.
    /// <para>Предоставляет информацию для идентификации статистики приложения.</para>
    /// </summary>
    internal class Stats
    {
        /// <summary>
        /// The name of the file containing the statistics ID.
        /// </summary>
        private const string StatsIdFileName = "StatsID.txt";

        private readonly IStorage storage; // the application storage
        private readonly ILog log;         // the application log
        private string statsID;            // the statistics ID

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Stats(IStorage storage, ILog log)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        /// <summary>
        /// Gets the statistics ID.
        /// </summary>
        public string StatsID
        {
            get
            {
                if (string.IsNullOrEmpty(statsID))
                {
                    try
                    {
                        if (storage.GetFileInfo(DataCategory.Storage, StatsIdFileName).Exists)
                            statsID = storage.ReadText(DataCategory.Storage, StatsIdFileName);

                        if (string.IsNullOrEmpty(statsID))
                        {
                            DateTime utcNow = DateTime.UtcNow;
                            statsID = AppUtils.AppVersion + "-" + utcNow.ToString("yyyyMMdd") + "-" +
                                ScadaUtils.GenerateUniqueID(utcNow);
                            storage.WriteText(DataCategory.Storage, StatsIdFileName, statsID);
                        }

                        return statsID;
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, Locale.IsRussian ?
                            "Ошибка при получении идентификатора статистики" :
                            "Error getting statistics ID");
                    }
                }

                return statsID;
            }
        }
    }
}
