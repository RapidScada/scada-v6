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
 * Module   : ScadaCommon
 * Summary  : Specifies the common constants used by the Agent service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Agent
{
    /// <summary>
    /// Specifies the common constants used by the Agent service.
    /// <para>Задаёт общие константы, используемые службой Агента.</para>
    /// </summary>
    public static class AgentConst
    {
        /// <summary>
        /// The name of the archive entry that contains project information.
        /// </summary>
        public const string ProjectInfoEntry = "Project.txt";

        /// <summary>
        /// The name of the archive entry that contains upload options.
        /// </summary>
        public const string UploadOptionsEntry = "UploadOptions.xml";

        /// <summary>
        /// The beginning of an uploaded configuration file.
        /// </summary>
        public const string UploadConfigPrefix = "upload_config_";

        /// <summary>
        /// The beginning of a downloaded configuration file.
        /// </summary>
        public const string DownloadConfigPrefix = "download_config_";
    }
}
