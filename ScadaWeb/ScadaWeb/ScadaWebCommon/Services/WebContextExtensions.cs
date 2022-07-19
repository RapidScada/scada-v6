/*
 * Copyright 2022 Rapid Software LLC
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
 * Module   : ScadaWebCommon
 * Summary  : Provides extensions to the IWebContext interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Tables;

namespace Scada.Web.Services
{
    /// <summary>
    /// Provides extensions to the IWebContext interface.
    /// <para>Предоставляет расширения интерфейса IWebContext.</para>
    /// </summary>
    public static class WebContextExtensions
    {
        /// <summary>
        /// Gets the archive bit from the configuration database.
        /// </summary>
        public static int FindArchiveBit(this IWebContext webContext, string archiveCode, int defaultArchiveBit)
        {
            if (string.IsNullOrEmpty(archiveCode))
            {
                return defaultArchiveBit;
            }
            else if (webContext.ConfigDatabase.ArchiveTable.SelectFirst(new TableFilter("Code", archiveCode))
                is Archive archive)
            {
                return archive.Bit;
            }
            else
            {
                return ArchiveBit.Unknown;
            }
        }
    }
}
