/*
 * Copyright 2021 Rapid Software LLC
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
 * Summary  : The phrases used by the Administrator application and its extensions
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Scada.Admin
{
    /// <summary>
    /// The phrases used by the Administrator application and its extensions.
    /// <para>Фразы, используемые приложением Администратор и его расширениями.</para>
    /// </summary>
    public static class AdminPhrases
    {
        // Scada.Admin.Project.ConfigBase
        public static string LoadConfigBaseError { get; private set; }
        public static string SaveConfigBaseError { get; private set; }

        // Scada.Admin.Project.ScadaProject
        public static string CreateProjectError { get; private set; }
        public static string LoadProjectError { get; private set; }
        public static string SaveProjectError { get; private set; }
    }
}
