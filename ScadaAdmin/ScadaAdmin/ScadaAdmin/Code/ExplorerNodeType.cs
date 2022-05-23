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
 * Module   : Administrator
 * Summary  : Specifies the types of the explorer tree nodes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Specifies the types of the explorer tree nodes.
    /// <para>Задаёт типы узлов дерева проводника.</para>
    /// </summary>
    internal static class ExplorerNodeType
    {
        public const string Project = nameof(Project);
        public const string ConfigDatabase = nameof(ConfigDatabase);
        public const string BaseTable = nameof(BaseTable);
        public const string Views = nameof(Views);
        public const string Instances = nameof(Instances);
        public const string Instance = nameof(Instance);
        public const string App = nameof(App);
        public const string AppConfig = nameof(AppConfig);
        public const string Directory = nameof(Directory);
        public const string File = nameof(File);
    }
}
