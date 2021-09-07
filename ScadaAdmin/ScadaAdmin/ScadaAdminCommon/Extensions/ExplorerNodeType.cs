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
 * Summary  : Specifies the types of the explorer tree nodes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Admin.Extensions
{
    /// <summary>
    /// Specifies the types of the explorer tree nodes.
    /// <para>Задаёт типы узлов дерева проводника.</para>
    /// </summary>
    public static class ExplorerNodeType
    {
        public const string Project = "Project";
        public const string Base = "Base";
        public const string BaseTable = "BaseTable";
        public const string Views = "Views";
        public const string Instances = "Instances";
        public const string Instance = "Instance";
        public const string App = "App";
        public const string AppConfig = "AppConfig";
        public const string Directory = "Directory";
        public const string File = "File";
    }
}
