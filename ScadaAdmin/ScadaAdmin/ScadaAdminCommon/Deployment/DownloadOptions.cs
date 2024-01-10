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
 * Module   : ScadaAdminCommon
 * Summary  : Represents configuration download options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2021
 */

using Scada.Agent;
using System;

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Represents configuration download options.
    /// <para>Представляет параметры скачивания конфигурации с сервера.</para>
    /// </summary>
    [Serializable]
    public class DownloadOptions : TransferOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DownloadOptions()
            : base()
        {
        }
    }
}
