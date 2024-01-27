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
 * Summary  : Specifies the data type IDs for columns in the application tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Data.Tables
{
    /// <summary>
    /// Specifies the data type IDs for columns in the application tables.
    /// <para>Задаёт идентификаторы типов данных для столбцов в таблицах приложений.</para>
    /// </summary>
    public enum ColumnDataType : byte
    {
        Undefined = 0,
        Integer = 1,
        Double = 2,
        Boolean = 3,
        DateTime = 4,
        String = 5
    }
}
