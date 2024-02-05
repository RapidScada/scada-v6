﻿/*
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
 * Module   : Administrator
 * Summary  : Predefined lengths of string columns
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2023
 */

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Predefined lengths of string columns.
    /// <para>Предопределенные длины строковых столбцов.</para>
    /// </summary>
    internal static class ColumnLength
    {
        public const int Default = 100;
        public const int Long = 1000;
        public const int Name = 100;
        public const int Code = 100;
        public const int Password = 100;
        public const int Description = 1000;
        public const int SourceCode = 10000;
    }
}
